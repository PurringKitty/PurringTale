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
	public class GreedNPC : ModNPC
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
			NPCID.Sets.DangerDetectRange[Type] = -1;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.AttackType[Type] = -1;

			NPC.Happiness
			//(Loves)
			.SetBiomeAffection<CorruptionBiome>(AffectionLevel.Love)
			.SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love)
			.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Love)
			//(Likes)
			.SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
			.SetNPCAffection(NPCID.Merchant, AffectionLevel.Like)
			//(Dislikes)
			.SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike)
			.SetNPCAffection(NPCID.Guide, AffectionLevel.Dislike)
			//(Hates)
			.SetBiomeAffection<JungleBiome>(AffectionLevel.Hate)
			.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Hate);
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
				new FlavorTextBestiaryInfoElement("Avaritia in his Terrarian form, likes money like A LOT too much even... freak... - Rukuka"),
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
                int headgore = Mod.Find<ModGore>($"GreedNPC_Gore_Head").Type;
                int armgore = Mod.Find<ModGore>($"GreedNPC_Gore_Arm").Type;


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
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Greed>()))
				{
					return true;
				}
			}

			return false;
		}


		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
                "Avaritia"
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
                chat.Add(Language.GetTextValue("Weirdo", Main.npc[partyGirl].GivenName));
            }
            chat.Add(Language.GetTextValue("Gimme Money"));
			chat.Add(Language.GetTextValue("I Like Money"));
			chat.Add(Language.GetTextValue("Yes My Drip Is Made Of Gold"));
			chat.Add(Language.GetTextValue("What Do You Want Lowly Peasant"));
			chat.Add(Language.GetTextValue("KYS So I Can Steal Your Cash"));
			chat.Add(Language.GetTextValue("Ive Been Told My Greed Will Be My Downfall..."), 5.0);
			chat.Add(Language.GetTextValue("How Much Can I Get If I Sell A Kidney?"), 5.0);
			chat.Add(Language.GetTextValue("Hey Can You Go Kill That Skeletron Fella?"), 5.0);
			chat.Add(Language.GetTextValue("I Will Steal From You When Your Not Looking"), 0.1);
			chat.Add(Language.GetTextValue("Even As An Eye I Was Golden"), 0.1);
			chat.Add(Language.GetTextValue("Bugger Off Will Ya?"), 0.001);

			var TopiumBarDialogue = Language.GetTextValue("HAH My Stuff Is All Purpose!");
			chat.Add(TopiumBarDialogue);


			string dialogueLine = chat;
			if (TopiumBarDialogue.Equals(dialogueLine))

				Main.npcChatCornerItem = ModContent.ItemType<Greed>();


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
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add<GreedBossBag>()
				.Add<GreedMusicBox>()
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) })
				.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = Item.buyPrice(gold: 99) });

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
            Main.GetItemDrawFrame(ModContent.ItemType<GoldOnAStick>(), out item, out itemFrame);
            itemSize = 40;
            // This adjustment draws the swing the way town npcs usually do.
            if (NPC.ai[1] > NPCID.Sets.AttackTime[NPC.type] * 0.66f)
            {
                offset.Y = 12f;
            }
        }
    
    public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Greed>()));
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;
	}
}
		
	
