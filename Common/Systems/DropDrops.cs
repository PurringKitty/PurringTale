using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using PurringTale.Content.Items.BossDrops;
using PurringTale.Content.Items.Consumables;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Common.Systems
{
    public class DropDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimeK47>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GelBullet>(), 2, 50, 150));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimySword>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimyWhip>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RedCrystalStaff>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<HeartOfSlime>()));
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeDrop>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeWand>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeShot>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeWhip>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeStaff>(), 6));
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainStem>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainWand>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainString>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainWhip>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainStaff>(), 6));
            }
            if (npc.type == NPCID.EaterofWorldsBody)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterSword>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterWand>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterBlaster>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterWhip>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedFireBullet>(), 1, 50, 500));
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneSpear>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneWand>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneBow>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneStaff>(), 6));
            }
            if (npc.type == NPCID.QueenBee)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeBlaster>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeWhip>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeStaff>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeBlasterBullet>(), 1, 50, 500));
            }
            if (npc.type == NPCID.Deerclops)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshSword>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshWand>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshBow>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshStaff>(), 6));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeSword>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeWhip>(), 6));
            }
            if (npc.type == NPCID.Spazmatism)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechBow>(), 9));
            }
            if (npc.type == NPCID.Retinazer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechBow>(), 9));
            }
            if (npc.type == NPCID.TheDestroyer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechWhip>(), 9));
            }
            if (npc.type == NPCID.SkeletronPrime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechHammer>(), 9));
            }
            if (npc.type == NPCID.Plantera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantBow>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantSpear>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantWhip>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantWand>(), 6));
            }
            if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemBlaster>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemSpear>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemWhip>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemWand>(), 6));
            }
            if (npc.type == NPCID.CultistBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultBow>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultSpear>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultWand>(), 7));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultWhip>(), 6));
            }
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeSpear>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeSword>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeWhip>(), 7));
            }
            if (npc.type == NPCID.HallowBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LightSpear>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LightSword>(), 8));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonBow>(), 9));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonSpear>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonWhip>(), 7));
            }
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeSword>(), 8));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeWhip>(), 6));
            }
        }
    }
}
