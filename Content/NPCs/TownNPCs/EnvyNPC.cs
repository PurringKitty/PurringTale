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
using PurringTale.CatBoss;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.Placeables.MusicBoxes;

namespace PurringTale.Content.NPCs.TownNPCs
{
	[AutoloadHead]
	public class EnvyNPC : ModNPC
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


			NPCProfile = new Profiles.StackedNPCProfile
				(
					new Profiles.DefaultNPCProfile(Texture, -1),
					new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
				);
			Main.npcFrameCount[Type] = 26;
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 200;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.AttackType[Type] = -1;

			NPC.Happiness
			.SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike)
			.SetBiomeAffection<SnowBiome>(AffectionLevel.Like)
			.SetBiomeAffection<CorruptionBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love)
			.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
			.SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike)
			.SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
			.SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
			.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Nurse, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Princess, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Stylist, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
			.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Love)
			.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Painter, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Pirate, AffectionLevel.Love)
			.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Clothier, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Wizard, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Truffle, AffectionLevel.Like)
			.SetNPCAffection(NPCID.Merchant, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Like)
            .SetNPCAffection<GluttonyNPC>(AffectionLevel.Dislike)
            .SetNPCAffection<GreedNPC>(AffectionLevel.Hate)
            .SetNPCAffection<LustNPC>(AffectionLevel.Love)
            .SetNPCAffection<PrideNPC>(AffectionLevel.Dislike)
            .SetNPCAffection<SlothNPC>(AffectionLevel.Dislike)
            .SetNPCAffection<WrathNPC>(AffectionLevel.Like)
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				new FlavorTextBestiaryInfoElement("Invidia in her Terrarian form, she is known to get rather jealous at times but she is a good god deep down! - Rukuka"),
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
				string variant = "";
				if (NPC.IsShimmerVariant)
					variant += "_Shimmer";
				int headgore = Mod.Find<ModGore>($"EnvyNPC_Gore_Head").Type;
				int armgore = Mod.Find<ModGore>($"EnvyNPC_Gore_Arm").Type;


				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headgore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armgore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armgore);
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
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Envy>()))
				{
					return true;
				}
			}

			return false;
		}


		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
                "Invidia"
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
                chat.Add(Language.GetTextValue("Why is SHE Always So Happy?", Main.npc[partyGirl].GivenName));
            }
            chat.Add(Language.GetTextValue("Anymore Of Me That Gets Summoned Is A Fake... trust Me..."));
			chat.Add(Language.GetTextValue("I Have My EYE On You Pal!"));
			chat.Add(Language.GetTextValue("Dirt Gets In My Eye Alot"));
			chat.Add(Language.GetTextValue("I Sell Some Of My Stuff"));
			chat.Add(Language.GetTextValue("Hello..."));
			chat.Add(Language.GetTextValue("I Am Always Envyous Of Everything I Try Not To Sometimes!"), 5.0);
			chat.Add(Language.GetTextValue("Hey King Slime's Crown Is Making Me So Envyous... Go Get It!"), 5.0);
			chat.Add(Language.GetTextValue("My House Is Smaller Than The Others... I Think..."), 5.0);
			chat.Add(Language.GetTextValue("I Am Envyous Of Your Two Eyes..."), 0.1);
			chat.Add(Language.GetTextValue("All Of Us Got Turned Into Eyeballs..."), 0.1);
			chat.Add(Language.GetTextValue("Please... Stop Talking To Me..."), 0.001);

			var TopiumBarDialogue = Language.GetTextValue("My Sin Specialized In Magic...");
			chat.Add(TopiumBarDialogue);


			string dialogueLine = chat;
			if (TopiumBarDialogue.Equals(dialogueLine))

				Main.npcChatCornerItem = ModContent.ItemType<Envy>();


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
                .Add(new Item(ItemID.ManaCrystal) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ItemID.LesserManaPotion) { shopCustomPrice = Item.buyPrice(silver: 1) })
                .Add(new Item(ItemID.ManaPotion) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)
                .Add(new Item(ItemID.GreaterManaPotion) { shopCustomPrice = Item.buyPrice(gold: 1) }, Condition.DownedPlantera)
                .Add(new Item(ItemID.SuperManaPotion) { shopCustomPrice = Item.buyPrice(gold: 10) }, Condition.DownedMoonLord)
                .Add<EnvyBossBag>()
				.Add<EnvyMusicBox>();
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Envy>()));
		}

		public override bool CanGoToStatue(bool toQueenStatue) => true;
	}
}
		
	
