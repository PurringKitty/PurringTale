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
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.FeatherfallPotion, Main.rand.Next(0, 2));
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Feather, Main.rand.Next(5, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 0, 20));
        }

    }
}