using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.Consumables.Bags;

namespace PurringTale.Content.NPCs.BossNPCs.Gluttony
{
    [AutoloadBossHead]
    public class EyeOfGluttonyBody : ModNPC
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
        set => NPC.ai[0] = value ? 1f : 2f;
    }
        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 110;
            NPC.damage = 13;
            NPC.defense = 12;
            NPC.lifeMax = 2500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 5, silver: 50);
            NPC.boss = true;
            NPC.npcSlots = 5f;
            NPC.aiStyle = 4;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath3;
            AIType = NPCID.EyeofCthulhu;
            AnimationType = NPCID.EyeofCthulhu;
            NPC.TargetClosest();
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Gluttony");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("The sin of Gluttony... they uhh don't like to admit to being bald... - Rukuka"),
            });
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedGluttony, -1);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<GluttonyBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
        }
    }
}