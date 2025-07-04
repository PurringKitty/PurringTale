﻿using PurringTale.Content.Dusts;
using PurringTale.Content.Items.Armor;
using PurringTale.Content.Items.Tools;
using PurringTale.Content.Items.Weapons.Magic;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Weapons.Summoner;
using PurringTale.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using PurringTale.Content.Items.Vanity;
using System.IO;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Accessories.Wings;
using PurringTale.Content.Items.Accessories.Boots;
using PurringTale.Content.Items.Accessories.Necklaces;
using PurringTale.Content.Items.Consumables.Summons;
using PurringTale.Content.Items.Placeables.Ores;

namespace PurringTale.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    class TopHatCat : ModNPC
    {
        public const double despawnTime = 48600.0;
        public static double spawnTime = double.MaxValue;
        public static ExampleTravelingMerchantShop Shop;
        public readonly List<Item> shopItems = new();
        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        public override bool PreAI()
        {
            if ((!Main.dayTime || Main.time >= despawnTime) && !IsNpcOnscreen(NPC.Center)) // If it's past the despawn time and the NPC isn't onscreen
            {
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("LegacyMisc.35", NPC.FullName), 50, 125, 255);
                else ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.35", NPC.GetFullNetName()), new Color(50, 125, 255));
                NPC.active = false;
                NPC.netSkip = -1;
                NPC.life = 0;
                return false;
            }

            return true;
        }

        public override void AddShops()
        {
            Shop = new ExampleTravelingMerchantShop(NPC.type);

            Shop.AddPool("Tools", slots: 1)
                .Add<ValhallaAxe>(Condition.DownedKingSlime)
                .Add<ValhallaPick>(Condition.DownedKingSlime)
                .Add<DirtAxe>()
                .Add<DirtPickaxe>()
                .Add<MechHammer>(Condition.DownedMechBossAny);

            Shop.AddPool("Weapons", slots: 2)
                .Add<DirtSword>()
                .Add<ValhallaStabber>()
                .Add<ImposterTongue>(Condition.DownedDeerclops)
                .Add<SlimySword>(Condition.DownedKingSlime)
                .Add<SlimeK47>(Condition.DownedKingSlime)
                .Add<SlimeStaff>(Condition.DownedKingSlime)
                .Add<SlimyWhip>(Condition.DownedKingSlime)
                .Add<EyeDrop>(Condition.DownedEyeOfCthulhu)
                .Add<EyeShot>(Condition.DownedEyeOfCthulhu)
                .Add<EyeStaff>(Condition.DownedEyeOfCthulhu)
                .Add<EyeWhip>(Condition.DownedEyeOfCthulhu)
                .Add<EyeWand>(Condition.DownedEyeOfCthulhu)
                .Add<BrainStem>(Condition.DownedBrainOfCthulhu)
                .Add<BrainString>(Condition.DownedBrainOfCthulhu)
                .Add<BrainStaff>(Condition.DownedBrainOfCthulhu)
                .Add<BrainWand>(Condition.DownedBrainOfCthulhu)
                .Add<BrainWhip>(Condition.DownedBrainOfCthulhu)
                .Add<EaterSword>(Condition.DownedEaterOfWorlds)
                .Add<EaterBlaster>(Condition.DownedEaterOfWorlds)
                .Add<EaterWand>(Condition.DownedEaterOfWorlds)
                .Add<EaterWhip>(Condition.DownedEaterOfWorlds)
                .Add<FleshSword>(Condition.DownedDeerclops)
                .Add<FleshBow>(Condition.DownedDeerclops)
                .Add<FleshStaff>(Condition.DownedDeerclops)
                .Add<FleshWand>(Condition.DownedDeerclops)
                .Add<DukeSword>(Condition.DownedDukeFishron)
                .Add<DukeSpear>(Condition.DownedDukeFishron)
                .Add<DukeWhip>(Condition.DownedDukeFishron)
                .Add<LightSword>(Condition.DownedEmpressOfLight)
                .Add<LightSpear>(Condition.DownedEmpressOfLight)
                .Add<QSlimeSword>(Condition.DownedQueenSlime)
                .Add<QSlimeWhip>(Condition.DownedQueenSlime)
                .Add<PlantBow>(Condition.DownedPlantera)
                .Add<PlantSpear>(Condition.DownedPlantera)
                .Add<PlantWand>(Condition.DownedPlantera)
                .Add<PlantWhip>(Condition.DownedPlantera)
                .Add<MechBow>(Condition.DownedMechBossAny)
                .Add<MechWhip>(Condition.DownedMechBossAny)
                .Add<GolemBlaster>(Condition.DownedGolem)
                .Add<GolemSpear>(Condition.DownedGolem)
                .Add<GolemWand>(Condition.DownedGolem)
                .Add<GolemWhip>(Condition.DownedGolem)
                .Add<CultBow>(Condition.DownedCultist)
                .Add<CultSpear>(Condition.DownedCultist)
                .Add<CultWand>(Condition.DownedCultist)
                .Add<CultWhip>(Condition.DownedCultist)
                .Add<BoneBow>(Condition.DownedSkeletron)
                .Add<BoneSpear>(Condition.DownedSkeletron)
                .Add<BoneStaff>(Condition.DownedSkeletron)
                .Add<BoneWand>(Condition.DownedSkeletron)
                .Add<BeeBlaster>(Condition.DownedQueenBee)
                .Add<BeeStaff>(Condition.DownedQueenBee)
                .Add<BeeWhip>(Condition.DownedQueenBee)
                .Add<MoonBow>(Condition.DownedMoonLord)
                .Add<MoonSpear>(Condition.DownedMoonLord)
                .Add<MoonWhip>(Condition.DownedMoonLord)
                .Add<RustedDancersWhip>(Condition.Hardmode)
                .Add<BrokenStarShootingStaff>(Condition.Hardmode)
                .Add<BrokenDestroyerLaserCannon>(Condition.Hardmode)
                .Add<RuinedGodSlayerSummoningBook>(Condition.Hardmode)
                .Add<BrokenSerratedGreatsword>(Condition.Hardmode);

            Shop.AddPool("Accessories", slots: 1)
                .Add<BlueFlameWings>()
                .Add<GrubbbWings>()
                .Add<OviaWings>()
                .Add<BlobticWings>()
                .Add<DOLWings>()
                .Add<EndSlayerWings>()
                .Add<GasterBlasterW>()
                .Add<MothWings>()
                .Add<MungusBackpack>()
                .Add<NatashaWings>()
                .Add<WolfCape>()
                .Add<VanityVoucher>()
                .Add<BottleOfOJ>()
                .Add<MauriceWings>()
                .Add<PastaNecklace>(Condition.Hardmode)
                .Add<PastaWings>()
                .Add<THCWings>()
                .Add<MicroWings>()
                .Add<TreadBoots>(Condition.Hardmode)
                .Add<ForgedWings>(Condition.InMasterMode, Condition.Hardmode, Condition.DownedMoonLord);

            Shop.AddPool("Armor", slots: 2)
                .Add<DirtHelmet>()
                .Add<DirtBreastplate>()
                .Add<DirtLeggings>()
                .Add<OverlordVisage>()
                .Add<OverlordChestplate>()
                .Add<OverlordBoots>()
                .Add<WolfHelm>()
                .Add<WolfGear>()
                .Add<WolfBoots>()
                .Add<RustedHelmet>(Condition.InMasterMode, Condition.Hardmode, Condition.DownedPlantera)
                .Add<RustedBreastplate>(Condition.InMasterMode, Condition.Hardmode, Condition.DownedPlantera)
                .Add<RustedLeggings>(Condition.InMasterMode, Condition.Hardmode, Condition.DownedPlantera)
                .Add<ValhallaHelmet>(Condition.InExpertMode, Condition.Hardmode)
                .Add<ValhallaHood>(Condition.InExpertMode, Condition.Hardmode)
                .Add<ValhallaHat>(Condition.InExpertMode, Condition.Hardmode)
                .Add<ValhallaCap>(Condition.InExpertMode, Condition.Hardmode)
                .Add<ValhallaBreastplate>(Condition.InExpertMode, Condition.Hardmode)
                .Add<ValhallaLeggings>(Condition.InExpertMode, Condition.Hardmode);

            Shop.AddPool("Vanity", slots: 3)
                .Add<DOLHelmet>()
                .Add<DOLBreastplate>()
                .Add<DOLLeggings>()
                .Add<EndSlayerHead>()
                .Add<EndSlayerBreastplate>()
                .Add<EndSlayerLeggings>()
                .Add<BlobticHead>()
                .Add<BlobticBody>()
                .Add<BlobticLegs>()
                .Add<GrubbbHead>()
                .Add<GrubbbBody>()
                .Add<GrubbbLegs>()
                .Add<MothHead>()
                .Add<MothBody>()
                .Add<MothLegs>()
                .Add<MungusHead>()
                .Add<MungusBody>()
                .Add<MungusLegs>()
                .Add<NatashaHead>()
                .Add<NatashaTop>()
                .Add<NatashaLeggings>()
                .Add<SansHead>()
                .Add<SansBody>()
                .Add<SansLeggings>()
                .Add<MonoHead>()
                .Add<MonoHoodie>()
                .Add<MonoLegs>()
                .Add<PastasHoodie>()
                .Add<PastasHood>()
                .Add<PastasLeggings>()
                .Add<MauriceHead>()
                .Add<MauriceBody>()
                .Add<MauriceLegs>()
                .Add<OviaHead>()
                .Add<OviaSweater>()
                .Add<OviaLeggings>()
                .Add<MicroHead>()
                .Add<MicroBody>()
                .Add<MicroLegs>()
                .Add<THCTopHat>()
                .Add<THCTie>()
                .Add<THCTail>();

            Shop.AddPool("HIMSELF", slots: 1)
                .Add<ClumpOfTopiumOre>(Condition.InMasterMode, Condition.Hardmode);

            Shop.Register();
        }

        public static void UpdateTravelingMerchant()
        {
            bool travelerIsThere = NPC.FindFirstNPC(ModContent.NPCType<TopHatCat>()) != -1; // Find a Merchant if there's one spawned in the world

            // Main.time is set to 0 each morning, and only for one update. Sundialling will never skip past time 0 so this is the place for 'on new day' code
            if (Main.dayTime && Main.time == 0)
            {
                // insert code here to change the spawn chance based on other conditions (say, npcs which have arrived, or milestones the player has passed)
                // You can also add a day counter here to prevent the merchant from possibly spawning multiple days in a row.

                // NPC won't spawn today if it stayed all night
                if (!travelerIsThere && Main.rand.NextBool(4))
                { // 4 = 25% Chance
                  // Here we can make it so the NPC doesnt spawn at the EXACT same time every time it does spawn
                    spawnTime = GetRandomSpawnTime(0, 48600.0); // minTime = 6:00am, maxTime = 7:30am
                }
                else
                {
                    spawnTime = double.MaxValue; // no spawn today
                }
            }

            // Spawn the traveler if the spawn conditions are met (time of day, no events, no sundial)
            if (!travelerIsThere && CanSpawnNow())
            {
                int newTraveler = NPC.NewNPC(Terraria.Entity.GetSource_TownSpawn(), Main.spawnTileX * 16, Main.spawnTileY * 16, ModContent.NPCType<TopHatCat>(), 1); // Spawning at the world spawn
                NPC traveler = Main.npc[newTraveler];
                traveler.homeless = true;
                traveler.direction = Main.spawnTileX >= WorldGen.bestX ? -1 : 1;
                traveler.netUpdate = true;

                // Prevents the traveler from spawning again the same day
                spawnTime = double.MaxValue;

                // Annouce that the traveler has spawned in!
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("Announcement.HasArrived", traveler.FullName), 50, 125, 255);
                else ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasArrived", traveler.GetFullNetName()), new Color(50, 125, 255));
            }
        }

        private static bool CanSpawnNow()
        {
            // can't spawn if any events are running
            if (Main.eclipse || Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return false;

            // can't spawn if the sundial is active
            if (Main.IsFastForwardingTime())
                return false;

            // can spawn if daytime, and between the spawn and despawn times
            return Main.dayTime && Main.time >= spawnTime && Main.time < despawnTime;
        }

        private static bool IsNpcOnscreen(Vector2 center)
        {
            int w = NPC.sWidth + NPC.safeRangeX * 2;
            int h = NPC.sHeight + NPC.safeRangeY * 2;
            Rectangle npcScreenRect = new Rectangle((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
            foreach (Player player in Main.player)
            {
                // If any player is close enough to the traveling merchant, it will prevent the npc from despawning
                if (player.active && player.getRect().Intersects(npcScreenRect)) return true;
            }
            return false;
        }

        public static double GetRandomSpawnTime(double minTime, double maxTime)
        {
            // A simple formula to get a random time between two chosen times
            return (maxTime - minTime) * Main.rand.NextDouble() + minTime;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26;
            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 60;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.AttackType[Type] = 3; // Swings a weapon. This NPC attacks in roughly the same manner as Stylist
            NPCID.Sets.AttackTime[Type] = 5;
            NPCID.Sets.AttackAverageChance[Type] = 1;
            NPCID.Sets.HatOffsetY[Type] = 4;
            NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.NoTownNPCHappiness[Type] = true; // Prevents the happiness button

            // Influences how the NPC looks in the Bestiary
           

          
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 200;
            NPC.defense = 2000;
            NPC.lifeMax = 2500000;
            NPC.rarity = 4;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.Guide;
            TownNPCStayingHomeless = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            shopItems.Clear();
            shopItems.AddRange(Shop.GenerateNewInventoryList());
            if (Main.netMode == NetmodeID.Server)
            {
                NPC.netUpdate = true;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(shopItems.Count);
            foreach (Item item in shopItems)
            {
                ItemIO.Send(item, writer, true);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            shopItems.Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                shopItems.Add(ItemIO.Receive(reader, true));
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("This Is One Of The Great Gods... The God Of Top Hats So WHY IS HE SELLING STUFF?!?"),
            });
        }

        public override void SaveData(TagCompound tag)
        {
            tag["shopItems"] = shopItems;
        }

        public override void LoadData(TagCompound tag)
        {
            shopItems.Clear();
            shopItems.AddRange(tag.Get<List<Item>>("shopItems"));
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
                int headgore = Mod.Find<ModGore>($"TopHatCat_Gore_Head").Type;
                int armgore = Mod.Find<ModGore>($"TopHatCat_Gore_Arm").Type;


                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headgore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armgore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armgore);
            }
        }
        public override bool UsesPartyHat()
        {
            // ExampleTravelingMerchant likes to keep his hat on while shimmered.
            if (NPC.IsShimmerVariant)
            {
                return false;
            }
            return true;
        }




        public override bool CanTownNPCSpawn(int numTownNPCs)
        { // Requirements for the town NPC to spawn.
            for (int k = 1; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }

                // Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
                if (player.inventory.Any(item => item.type == ModContent.ItemType<TopiumOre>() || item.type == ModContent.ItemType<MoonlightGreatSword>()))
                {
                    return true;
                }
            }

            return false;
        }


        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Purring",
                "Natasha",
                "Moddingus",
                "Nyawul",
                "Ritual Moth",
                "Dark Or Light",
                "Sky Blue God",
                "Gato Tactico",
                "Mono",
                "Blu3",
                "Psuedofaux",
                "Lucifer Morningstar",
                "Maurice",
                "Honey Bear",
                "Mythic Fire",
                "Topper",
                "John",
                "Pasta Machine"
            };
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (partyGirl >= 0)
            {
                chat.Add(Language.GetTextValue("Goofy Ahh Human", Main.npc[partyGirl].GivenName));
            }

            chat.Add(Language.GetTextValue("Hello I Am The God Of Top Hats!"));
            chat.Add(Language.GetTextValue("I Sell Stuff I Find When Traveling!"));
            chat.Add(Language.GetTextValue("Yes Mortal?"));
            chat.Add(Language.GetTextValue("Buy My Jun.. I Mean Goods!"));
            chat.Add(Language.GetTextValue("That Slime With A Top Hat Is My Sla- Servent!"));
            chat.Add(Language.GetTextValue("Mhm Yes Indubadbly..."));
            chat.Add(Language.GetTextValue("The Gods Can be Unforgiving Friend..."), 0.1);
            chat.Add(Language.GetTextValue("My True Name Is Rukuka, I Just Like Having Nicknames HeHe!"), 0.1);
            chat.Add(Language.GetTextValue("Zeus Is Mean To Me Sometimes :("), 0.01);
            chat.Add(Language.GetTextValue("Hades Is In Hell Trust Me ;)"), 0.01);
            chat.Add(Language.GetTextValue("Posidion Is A Freak... Poor Fishes..."), 0.01);
            chat.Add(Language.GetTextValue("Bacchus Is Always Drunk Sheesh..."), 0.01);
            chat.Add(Language.GetTextValue("Odin Is Old... Very Old..."), 0.01);
            chat.Add(Language.GetTextValue("Thor Thinks He's The Strongest God... I'll Prove Him Wrong..."), 0.01);
            chat.Add(Language.GetTextValue("Hermes... He Is Fast... Too Fast..."), 0.01);
            chat.Add(Language.GetTextValue("Cronus..."), 0.001);
            chat.Add(Language.GetTextValue("Gaia..."), 0.001);
            chat.Add(Language.GetTextValue("Uranus..."), 0.001);
            chat.Add(Language.GetTextValue("I AM Tired Of All This TALKING!"), 0.001);

        var TopiumOreDialogue = Language.GetTextValue("If You Ever Need To Call On Me Use One Of These!");
            chat.Add(TopiumOreDialogue);


            string dialogueLine = chat; // chat is implicitly cast to a string.
            if (TopiumOreDialogue.Equals(dialogueLine))

                Main.npcChatCornerItem = ModContent.ItemType<ClumpOfTopiumOre>();


            return dialogueLine;

            

        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = Shop.Name; // Opens the shop
            }
        }

        public override void AI()
        {
            NPC.homeless = true; // Make sure it stays homeless
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THCTopHat>(), 1, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THCTie>(), 1, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THCTail>(), 1, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THCWings>(), 1, 1, 1));
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
            Main.GetItemDrawFrame(ModContent.ItemType<MoonlightGreatSword>(), out item, out itemFrame);
            itemSize = 40;
            // This adjustment draws the swing the way town npcs usually do.
            if (NPC.ai[1] > NPCID.Sets.AttackTime[NPC.type] * 0.66f)
            {
                offset.Y = 12f;
            }
        }
    }

    // You have the freedom to implement custom shops however you want
    // This example uses a 'pool' concept where items will be randomly selected from a pool with equal weight
    // We copy a bunch of code from NPCShop and NPCShop.Entry, allowing this shop to be easily adjusted by other mods.
    // 
    // This uses some fairly advanced C# to avoid being accessively long, so make sure you learn the language before trying to adapt it significantly
    public class ExampleTravelingMerchantShop : AbstractNPCShop
    {
        public new record Entry(Item Item, List<Condition> Conditions) : AbstractNPCShop.Entry
        {
            IEnumerable<Condition> AbstractNPCShop.Entry.Conditions => Conditions;

            public bool Disabled { get; private set; }

            public Entry Disable()
            {
                Disabled = true;
                return this;
            }

            public bool ConditionsMet() => Conditions.All(c => c.IsMet());
        }

        public record Pool(string Name, int Slots, List<Entry> Entries)
        {
            public Pool Add(Item item, params Condition[] conditions)
            {
                Entries.Add(new Entry(item, conditions.ToList()));
                return this;
            }

            public Pool Add<T>(params Condition[] conditions) where T : ModItem => Add(ModContent.ItemType<T>(), conditions);
            public Pool Add(int item, params Condition[] conditions) => Add(ContentSamples.ItemsByType[item], conditions);

            // Picks a number of items (up to Slots) from the entries list, provided conditions are met.
            public IEnumerable<Item> PickItems()
            {
                // This is not a fast way to pick items without replacement, but it's certainly easy. Be careful not to do this many many times per frame, or on huge lists of items.
                var list = Entries.Where(e => !e.Disabled && e.ConditionsMet()).ToList();
                for (int i = 0; i < Slots; i++)
                {
                    if (list.Count == 0)
                        break;

                    int k = Main.rand.Next(list.Count);
                    yield return list[k].Item;

                    // remove the entry from the list so it can't be selected again this pick
                    list.RemoveAt(k);
                }
            }
        }

        public List<Pool> Pools { get; } = new();

        public ExampleTravelingMerchantShop(int npcType) : base(npcType) { }

        public override IEnumerable<Entry> ActiveEntries => Pools.SelectMany(p => p.Entries).Where(e => !e.Disabled);

        public Pool AddPool(string name, int slots)
        {
            var pool = new Pool(name, slots, new List<Entry>());
            Pools.Add(pool);
            return pool;
        }

        // Some methods to add a pool with a single item
        public void Add(Item item, params Condition[] conditions) => AddPool(item.ModItem?.FullName ?? $"Terraria/{item.type}", slots: 1).Add(item, conditions);
        public void Add<T>(params Condition[] conditions) where T : ModItem => Add(ModContent.ItemType<T>(), conditions);
        public void Add(int item, params Condition[] conditions) => Add(ContentSamples.ItemsByType[item], conditions);

        // Here is where we actually 'roll' the contents of the shop
        public List<Item> GenerateNewInventoryList()
        {
            var items = new List<Item>();
            foreach (var pool in Pools)
            {
                items.AddRange(pool.PickItems());
            }
            return items;
        }

        public override void FillShop(ICollection<Item> items, NPC npc)
        {
            // use the items which were selected when the NPC spawned.
            foreach (var item in ((TopHatCat)npc.ModNPC).shopItems)
            {
                // make sure to add a clone of the item, in case any ModifyActiveShop hooks adjust the item when the shop is opened
                items.Add(item.Clone());
            }
        }

        public override void FillShop(Item[] items, NPC npc, out bool overflow)
        {
            overflow = false;
            int i = 0;
            // use the items which were selected when the NPC spawned.
            foreach (var item in ((TopHatCat)npc.ModNPC).shopItems)
            {

                if (i == items.Length - 1)
                {
                    // leave the last slot empty for selling
                    overflow = true;
                    return;
                }

                // make sure to add a clone of the item, in case any ModifyActiveShop hooks adjust the item when the shop is opened
                items[i++] = item.Clone();
            }
        }
    }
}
