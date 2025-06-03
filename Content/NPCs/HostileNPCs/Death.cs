using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using PurringTale.Content.Items.MobLoot;
using Terraria.GameContent.ItemDropRules;

namespace PurringTale.Content.NPCs.HostileNPCs
{
    public class Death : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 11;
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 26;
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
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Skull, Main.rand.Next(1, 1));
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Bone, Main.rand.Next(4, 9));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 0, 20));
        }

    }
}