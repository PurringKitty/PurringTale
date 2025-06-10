using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using PurringTale.Content.Items.MobLoot;
using Terraria.GameContent.ItemDropRules;

namespace PurringTale.Content.NPCs.HostileNPCs
{
    public class SkeletalHorseman : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 11;
        }

        public override void SetDefaults()
        {
            NPC.width = 51;
            NPC.height = 41;
            NPC.damage = 20;
            NPC.defense = 5;
            NPC.lifeMax = 200;
            NPC.value = 50f;
            NPC.aiStyle = 26;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath5;
            AIType = NPCID.Unicorn;
            AnimationType = NPCID.HeadlessHorseman;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNight.Chance * 0.1f;
        }



        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 1, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.Skull, 5, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 1, 6));
        }

    }
}