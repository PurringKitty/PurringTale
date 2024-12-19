using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using PurringTale.Content.Items.Vanity;
using PurringTale.Content.Items.Consumables;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.BossDrops;

namespace PurringTale.Content.NPCs.BossNPCs.Wrath
{

    [AutoloadBossHead]
    public class EyeOfWrathBody : ModNPC

    {
        public static int secondStageHeadSlot = -1;
        public override void Load()
        {
            string texture = BossHeadTexture + "_SecondStage";
            secondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1);
        }

        public override void BossHeadSlot(ref int index)
        {
            int slot = secondStageHeadSlot;
            if (SecondStage && slot != -1)
            {
                index = slot;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public bool SecondStage
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }




        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 110;
            NPC.damage = 100;
            NPC.defense = 0;
            NPC.lifeMax = 60000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 50);
            NPC.boss = true;
            NPC.npcSlots = 5f;
            NPC.aiStyle = 31;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath3;
            AIType = NPCID.EyeofCthulhu;
            AnimationType = NPCID.EyeofCthulhu;
            NPC.BossBar = ModContent.GetInstance<BossBar>();
            NPC.TargetClosest();

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Wrath");
            }
        }



        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("The Eye Of The Sin Of Wrath, Crazy Attack No Defence"),
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<EyeOfWrathBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SinsBossTrophy>(), 4, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<SinsBossRelic>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VanityVoucher>(), 5, 0, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfWrath>(), 1, 50, 80));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
        }

    }
}