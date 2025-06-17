using PurringTale.Content.Dusts;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using PurringTale.CatBoss;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.Placeables.MusicBoxes;

namespace PurringTale.Content.NPCs.TownNPCs
{
	[AutoloadHead]
	public class GluttonyNPC : ModNPC
	{
		public const string ShopName = "Shop";
		public int NumberOfTimesTalkedTo = 0;

		public override void Load()
		{
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;

			NPCID.Sets.ExtraFramesCount[Type] = 8;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 10;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.AttackType[Type] = -1;

            NPC.Happiness
			.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
			.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
			.SetBiomeAffection<CorruptionBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love)
			.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate)
			.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
			.SetBiomeAffection<JungleBiome>(AffectionLevel.Hate)
			.SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Nurse, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Princess, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Stylist, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Guide, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
			.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Painter, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Pirate, AffectionLevel.Love)
			.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Clothier, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Truffle, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Love)
			.SetNPCAffection<EnvyNPC>(AffectionLevel.Dislike)
            .SetNPCAffection<GreedNPC>(AffectionLevel.Hate)
            .SetNPCAffection<LustNPC>(AffectionLevel.Love)
            .SetNPCAffection<PrideNPC>(AffectionLevel.Like)
            .SetNPCAffection<SlothNPC>(AffectionLevel.Like)
            .SetNPCAffection<WrathNPC>(AffectionLevel.Hate)
            .SetNPCAffection<TopHatSlimeGood>(AffectionLevel.Hate);
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 100;
			NPC.defense = 20;
			NPC.lifeMax = 200;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.shimmering = true;
			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				new FlavorTextBestiaryInfoElement("Gula in his Terrarian form, gave me some food once... then took some back because he was hungry. - Rukuka"),
			});
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers))
			{
				drawModifiers.Rotation += 0.001f;
			}

			return true;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Sparkle>());
			}
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				string variant = "Shimmer";
				if (NPC.IsShimmerVariant) variant += "GluttonyNPC_Shimmer";
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			for (int k = 0; k < Main.maxPlayers; k++)
			{
				Player player = Main.player[k];
				if (!player.active)
				{
					continue;
				}
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Gluttony>()))
				{
					return true;
				}
			}

			return false;
		}


		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
                "Gula"
            };
		}

		public override void FindFrame(int frameHeight)
		{
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (partyGirl >= 0 && Main.rand.NextBool(4))
            {
                chat.Add(Language.GetTextValue("Stupid Human...", Main.npc[partyGirl].GivenName));
            }
            chat.Add(Language.GetTextValue("Who Is This Patches?"));
			chat.Add(Language.GetTextValue("Why Do Some People Call Me The Onion Knight?"));
			chat.Add(Language.GetTextValue("What Is A Dark Souls? "));
			chat.Add(Language.GetTextValue("Got Any Food On You?"));
			chat.Add(Language.GetTextValue("GOD I AM SO HUNGRY"));
			chat.Add(Language.GetTextValue("That Eye of Cthulhu Makes Me So Hungry... Go Kill It..."), 5.0);
			chat.Add(Language.GetTextValue("I Have A Fast Matabolism So..."), 5.0);
			chat.Add(Language.GetTextValue("Home Sweet Home..."), 5.0);
			chat.Add(Language.GetTextValue("I Am Gonna Eat Another Person..."), 0.1);
			chat.Add(Language.GetTextValue("All Of Us Got Turned Into Eyeballs..."), 0.1);
			chat.Add(Language.GetTextValue("Your Annoying me I Will EAT YOU If You Don't Leave Me Alone!"), 0.001);

			var TopiumBarDialogue = Language.GetTextValue("My Sin Is Specialized In Ranged...");
			chat.Add(TopiumBarDialogue);


			string dialogueLine = chat;
			if (TopiumBarDialogue.Equals(dialogueLine))

				Main.npcChatCornerItem = ModContent.ItemType<Gluttony>();


			return dialogueLine;
		}

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "";
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<Debugger>()))
            {
                button = "" + Lang.GetItemNameValue(ModContent.ItemType<Debugger>());
            }
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = ShopName;
			}
		}
		public override void AddShops()
		{

			var npcShop = new NPCShop(Type, ShopName)
                .Add(new Item(ItemID.CookingPot) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.Bottle) { shopCustomPrice = Item.buyPrice(silver: 1) })
                .Add(new Item(ItemID.Bowl) { shopCustomPrice = Item.buyPrice(silver: 1) })
                .Add(new Item(ItemID.Apple) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.Apricot) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.Grapefruit) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.Lemon) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.Peach) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.Cherry) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InSnow)
                .Add(new Item(ItemID.Plum) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InSnow)
                .Add(new Item(ItemID.BlackCurrant) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InCorrupt)
                .Add(new Item(ItemID.Elderberry) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InCorrupt)
                .Add(new Item(ItemID.BloodOrange) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InCrimson)
                .Add(new Item(ItemID.Rambutan) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InCrimson)
                .Add(new Item(ItemID.Mango) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InJungle)
                .Add(new Item(ItemID.Pineapple) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InJungle)
                .Add(new Item(ItemID.Banana) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InBeach)
                .Add(new Item(ItemID.Coconut) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InBeach)
                .Add(new Item(ItemID.Dragonfruit) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InHallow)
                .Add(new Item(ItemID.Starfruit) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InHallow)
                .Add(new Item(ItemID.Pomegranate) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InUnderworld)
                .Add(new Item(ItemID.SpicyPepper) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.InUnderworld)
                .Add<GluttonyBossBag>()
				.Add<GluttonyMusicBox>();
			npcShop.Register();
		}

		public override void ModifyActiveShop(string shopName, Item[] items)
		{
			foreach (Item item in items)
			{
				if (item == null || item.type == ItemID.None)
				{
					continue;
				}
				if (NPC.IsShimmerVariant)
				{
					int value = item.shopCustomPrice ?? item.value;
					item.shopCustomPrice = value / 2;
				}
			}
		}
    
    public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Gluttony>()));
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;
	}
}
		
	
