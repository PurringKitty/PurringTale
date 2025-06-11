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
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using PurringTale.Content.Items.MobLoot;
using PurringTale.CatBoss;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.Weapons.Summoner;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.Placeables.MusicBoxes;

namespace PurringTale.Content.NPCs.TownNPCs
{
	[AutoloadHead]
	public class LustNPC : ModNPC
	{
		public const string ShopName = "Shop";
		public int NumberOfTimesTalkedTo = 0;

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load()
		{
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;

			NPCID.Sets.ExtraFramesCount[Type] = 8;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.AttackType[Type] = 3; // Swings a weapon. This NPC attacks in roughly the same manner as Stylist
            NPCID.Sets.AttackTime[Type] = 12;
            NPCID.Sets.AttackAverageChance[Type] = 1;

			NPC.Happiness
			.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
			.SetBiomeAffection<SnowBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CorruptionBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love)
			.SetBiomeAffection<DesertBiome>(AffectionLevel.Love)
			.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
			.SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
			.SetBiomeAffection<HallowBiome>(AffectionLevel.Love)
			.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Nurse, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Angler, AffectionLevel.Love)
			.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Princess, AffectionLevel.Love)
			.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
			.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Stylist, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Guide, AffectionLevel.Love)
			.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Love)
			.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Painter, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Golfer, AffectionLevel.Love)
			.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Pirate, AffectionLevel.Love)
			.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Love)
			.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Clothier, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Wizard, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Truffle, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Merchant, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Love)
			.SetNPCAffection<EnvyNPC>(AffectionLevel.Love)
			.SetNPCAffection<GluttonyNPC>(AffectionLevel.Love)
			.SetNPCAffection<GreedNPC>(AffectionLevel.Love)
			.SetNPCAffection<PrideNPC>(AffectionLevel.Love)
			.SetNPCAffection<SlothNPC>(AffectionLevel.Love)
			.SetNPCAffection<WrathNPC>(AffectionLevel.Love)
			.SetNPCAffection<TopHatSlimeGood>(AffectionLevel.Love);
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				new FlavorTextBestiaryInfoElement("The Sin Of Lust But Y'know Smaller. Went Back To Normal After Getting Killed In Eye Form"),
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
				if (NPC.IsShimmerVariant) variant += "_Shimmer";
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
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Lust>()))
				{
					return true;
				}
			}

			return false;
		}


		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
                "Luxuria"
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
                chat.Add(Language.GetTextValue("She sure is cute <3", Main.npc[partyGirl].GivenName));
            }
            chat.Add(Language.GetTextValue("Hiya honey"));
			chat.Add(Language.GetTextValue("I love everyone equally!"));
			chat.Add(Language.GetTextValue("Everyone also LOVES me of course!"));
			chat.Add(Language.GetTextValue("Don't Run <3"));
			chat.Add(Language.GetTextValue("Owo"));
			chat.Add(Language.GetTextValue("Tee Hee"), 5.0);
			chat.Add(Language.GetTextValue("Go kill Plantera for me and Ill give you a good time sugur <3"), 5.0);
			chat.Add(Language.GetTextValue("Ugh my back hurts..."), 5.0);
			chat.Add(Language.GetTextValue("Sex... the joke is Sex"), 0.1);
			chat.Add(Language.GetTextValue("-w-"), 0.1);
			chat.Add(Language.GetTextValue("Sorry hun Im Tired right now <3"), 0.001);

			var TopiumBarDialogue = Language.GetTextValue("I like whips hehehe <3");
			chat.Add(TopiumBarDialogue);


			string dialogueLine = chat;
			if (TopiumBarDialogue.Equals(dialogueLine))

				Main.npcChatCornerItem = ModContent.ItemType<Lust>();


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
                .Add(new Item(ItemID.GenderChangePotion) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.LifeCrystal) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ItemID.LifeFruit) { shopCustomPrice = Item.buyPrice(platinum: 1) }, Condition.DownedPlantera)
                .Add<EyeOfLustBossBag>()
                .Add<LustMusicBox>();
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

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 200;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 8;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
        {
            itemWidth = itemHeight = 40;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            Main.GetItemDrawFrame(ModContent.ItemType<WhipOfLust>(), out item, out itemFrame);
            itemSize = 40;
            // This adjustment draws the swing the way town npcs usually do.
            if (NPC.ai[1] > NPCID.Sets.AttackTime[NPC.type] * 0.66f)
            {
                offset.Y = 12f;
            }
        }
    
    public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Lust>()));
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;
	}
}
		
	
