using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using PurringTale.Content.Items.MobLoot;
using Terraria.GameContent.ItemDropRules;

namespace PurringTale.Content.NPCs.HostileNPCs
{
    public class ValHarpy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 26;
            NPC.damage = 20;
            NPC.defense = 5;
            NPC.lifeMax = 200;
            NPC.value = 500f;
            NPC.aiStyle = 14;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AIType = NPCID.Harpy;
            AnimationType = NPCID.Harpy;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.HardmodeJungle.Chance * 1f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Feather, 1, 1, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.GiantHarpyFeather, 15, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 0, 20));
        }
    }
}