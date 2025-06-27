using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.CatBoss;
using PurringTale.Common.Systems;
using PurringTale.Content.Dusts;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Consumables.Ammo;
using PurringTale.Content.Items.Consumables.Summons;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Items.Vanity;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace PurringTale.Content.NPCs.TownNPCs
{
	[AutoloadHead]
	public class TopHatSlimeGood : ModNPC
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
			Main.npcFrameCount[Type] = 26;

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = -1;
			NPCID.Sets.AttackType[Type] = -1;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
			NPC.Happiness
			//(Loves)
			.SetBiomeAffection<CorruptionBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love)
			.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
			//(Likes)
			.SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
			.SetNPCAffection(NPCID.Nurse, AffectionLevel.Like)
			//(Dislikes)
			.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Truffle, AffectionLevel.Dislike)
			//(Hates)
			.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)
			.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate);
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 25;
			NPC.lifeMax = 2500;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				new FlavorTextBestiaryInfoElement("A Slime With A Top Hat...? Wait... THAT IS ONE OF TOP HAT GODS MINIONS!!! Oh? It's Friendly... cool..."),
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
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Slush);
			}
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
                string variant = "";
                if (NPC.IsShimmerVariant)
                    variant += "_Shimmer";
                int headgore = Mod.Find<ModGore>($"TopHatSlimeGood_Gore_Head").Type;


                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headgore, 1f);
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
                if (player.inventory.Any(item => item.type == ModContent.ItemType<THCTopHat>()))
                {
					return true;
				}
			}

			return false;
		}


		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Minion",
				"Slurps",
				"Jelly",
				"EDP445",
				"FinnaTouchYou",
				"Toppy"
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
				chat.Add(Language.GetTextValue("Party Time!", Main.npc[partyGirl].GivenName));
			}
			chat.Add(Language.GetTextValue("Hello Human I Am A Slime... Do Not Ask Why I Can Talk..."));
			chat.Add(Language.GetTextValue("I Am A Good Slime Trust Me!"));
			chat.Add(Language.GetTextValue("Like My Hat? Well TOO BAD! You Would Have To Kill Me To Get It!"));
			chat.Add(Language.GetTextValue("I Sell Stuff!"));
			chat.Add(Language.GetTextValue("*Wobble*"));
			chat.Add(Language.GetTextValue("The Top Hat God Is A Weirdo Sometimes..."), 5.0);
			chat.Add(Language.GetTextValue("I Dislike Human Males... They Always Try To Kill Me..."), 0.1);
			chat.Add(Language.GetTextValue("Stop Talking To Me Please..."), 0.001);

			var TopiumBarDialogue = Language.GetTextValue("You'll Piss Of The Top hat God If You Summon Him With This!");
			chat.Add(TopiumBarDialogue);


			string dialogueLine = chat;
			if (TopiumBarDialogue.Equals(dialogueLine))

				Main.npcChatCornerItem = ModContent.ItemType<StackOfTopiumBars>();


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
				.Add<GelBullet>()
				.Add(new Item(ItemID.Gel) { shopCustomPrice = Item.buyPrice(copper: 1) })
				.Add(new Item(ItemID.RecallPotion) { shopCustomPrice = Item.buyPrice(silver: 1) })
				.Add(new Item(ItemID.WormholePotion) { shopCustomPrice = Item.buyPrice(silver: 1) })
				.Add<ValhallaOre>(Condition.DownedKingSlime, Condition.InMasterMode)
                .Add(new Item(ItemID.IronOre) { shopCustomPrice = Item.buyPrice(silver: 10) })
                .Add(new Item(ItemID.LeadOre) { shopCustomPrice = Item.buyPrice(silver: 10) })
                .Add<TopiumOre>(Condition.DownedMoonLord, Condition.InMasterMode);
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THCTopHat>()));
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;
	}
}
		
	
