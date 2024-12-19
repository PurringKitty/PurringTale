using PurringTale.Content.Dusts;
using PurringTale.Content.Items;
using PurringTale.Content.Items.Accessories;
using PurringTale.Content.Items.Armor;
using PurringTale.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
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
using ReLogic.Content;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;
using PurringTale.Common;
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.Vanity;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Consumables;
using PurringTale.Common.Systems;
using PurringTale.Content.NPCs.BossNPCs.Envy;
using Terraria.GameContent.Achievements;
using PurringTale.Content.Items.MobLoot;
using PurringTale.CatBoss;

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
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
			NPC.Happiness
			.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
			.SetBiomeAffection<SnowBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CorruptionBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love)
			.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate)
			.SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
			.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
			.SetBiomeAffection<HallowBiome>(AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Nurse, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Angler, AffectionLevel.Like)
			.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Princess, AffectionLevel.Love)
			.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
			.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Stylist, AffectionLevel.Love)
			.SetNPCAffection(NPCID.Guide, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Painter, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Golfer, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Clothier, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Wizard, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Clothier, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Truffle, AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Merchant, AffectionLevel.Hate)
			.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate)
            .SetNPCAffection<EnvyNPC>(AffectionLevel.Love);
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
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Sparkle>());
			}
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				string variant = "";
				if (NPC.IsShimmerVariant) variant += "_Shimmer";
				if (NPC.altTexture == 1) variant += "_Party";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;
				if (hatGore > 0)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
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
				.Add(new Item(ItemID.SlimeCrown) { shopCustomPrice = Item.buyPrice(gold: 1) },Condition.DownedKingSlime)
				.Add(new Item(ItemID.KingSlimeBossBag) { shopCustomPrice = Item.buyPrice(gold: 1) },Condition.DownedKingSlime)
				.Add(new Item(ItemID.RecallPotion) { shopCustomPrice = Item.buyPrice(silver: 1) })
				.Add(new Item(ItemID.WormholePotion) { shopCustomPrice = Item.buyPrice(silver: 1) })
				.Add<WeakValhallaOre>(Condition.DownedKingSlime, Condition.InMasterMode)
                .Add(new Item(ItemID.IronOre) { shopCustomPrice = Item.buyPrice(silver: 10) }, Condition.DownedKingSlime)
                .Add(new Item(ItemID.LeadOre) { shopCustomPrice = Item.buyPrice(silver: 10) }, Condition.DownedKingSlime)
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
		
	
