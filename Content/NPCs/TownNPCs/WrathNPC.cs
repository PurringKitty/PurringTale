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
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.Placeables.MusicBoxes;

namespace PurringTale.Content.NPCs.TownNPCs
{
	[AutoloadHead]
	public class WrathNPC : ModNPC
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
			.SetNPCAffection<TopHatSlimeGood>(AffectionLevel.Love);
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 200;
			NPC.defense = 20;
			NPC.lifeMax = 400;
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
				new FlavorTextBestiaryInfoElement("Ira in his Terrarian form, when it gets cold get close to him he generates a lot of heat. - Rukuka"),
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
				if (NPC.IsShimmerVariant) variant += "WrathNPC_Shimmer";
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
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Wrath>()))
				{
					return true;
				}
			}

			return false;
		}


		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
                "Ira"
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
                chat.Add(Language.GetTextValue("A Very Particular Creature", Main.npc[partyGirl].GivenName));
            }
            chat.Add(Language.GetTextValue("Don't Worry I Can Keep My Rage Contained"));
			chat.Add(Language.GetTextValue("Try Not To Piss Me Off Please"));
			chat.Add(Language.GetTextValue("I Can Be Uhmm Somewhat Reasonable!"));
			chat.Add(Language.GetTextValue("Hehe Pentagrams... Huh What?"));
			chat.Add(Language.GetTextValue("Hello There"));
			chat.Add(Language.GetTextValue("Sometimes My Rage Goes To Crazy..."), 5.0);
			chat.Add(Language.GetTextValue("If You Defeat The Empress of Light Ill Sell You My Stuff"), 5.0);
			chat.Add(Language.GetTextValue("Avaritia Is Insanly Greedy..."), 5.0);
			chat.Add(Language.GetTextValue("Ive Been Told I Have A Hot Aura Around Me Dunno What Thats About..."), 0.1);
			chat.Add(Language.GetTextValue("I Am The Strongest Of The Sins!"), 0.1);
			chat.Add(Language.GetTextValue("ok your talking to much go away please......."), 0.001);

			var TopiumBarDialogue = Language.GetTextValue("Ah Yes My Sin Is Good For Swords And The Like!");
			chat.Add(TopiumBarDialogue);


			string dialogueLine = chat;
			if (TopiumBarDialogue.Equals(dialogueLine))

				Main.npcChatCornerItem = ModContent.ItemType<Wrath>();


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
				.Add<WrathBossBag>()
				.Add<WrathMusicBox>();
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Wrath>()));
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;
	}
}
		
	
