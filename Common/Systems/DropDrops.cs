using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Weapons.Magic;
using PurringTale.Content.Items.Weapons.Summoner;
using PurringTale.Content.Items.Weapons.Ranged;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Accessories.Others;
using PurringTale.Content.Items.Tools;
using PurringTale.Content.Items.Consumables.Ammo;

namespace PurringTale.Common.Systems
{
    public class VanillaBossBagDrops : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.KingSlimeBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimeK47>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GelBullet>(), 1, 50, 150));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimySword>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimyWhip>(), 6));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimeStaff>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartOfSlime>(), 3, 1, 1));
            }

            if (item.type == ItemID.EyeOfCthulhuBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeDrop>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeWand>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeShot>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeWhip>(), 6));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeStaff>(), 6));
            }
            if (item.type == ItemID.BrainOfCthulhuBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainStem>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainWand>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainString>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainWhip>(), 6));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainStaff>(), 6));
            }
            if (item.type == ItemID.EaterOfWorldsBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterSword>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterWand>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterBlaster>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EaterWhip>(), 6));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedFireBullet>(), 1, 50, 500));
            }
            if (item.type == ItemID.SkeletronBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneSpear>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneWand>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneBow>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneStaff>(), 6));
            }
            if (item.type == ItemID.QueenBeeBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeBlaster>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeWhip>(), 6));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeStaff>(), 6));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BeeBlasterBullet>(), 1, 50, 500));
            }
            if (item.type == ItemID.WallOfFleshBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshSword>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshWand>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshBow>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FleshStaff>(), 6));
            }
            if (item.type == ItemID.QueenSlimeBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeSword>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeWhip>(), 6));
            }
            if (item.type == ItemID.TwinsBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechBow>(), 9));
            }
            if (item.type == ItemID.DestroyerBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechWhip>(), 9));
            }
            if (item.type == ItemID.SkeletronPrimeBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MechHammer>(), 9));
            }
            if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantBow>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantSpear>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantWhip>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlantWand>(), 6));
            }
            if (item.type == ItemID.GolemBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemBlaster>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemSpear>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemWhip>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GolemWand>(), 6));
            }
            if (item.type == ItemID.CultistBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultBow>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultSpear>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultWand>(), 7));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CultWhip>(), 6));
            }
            if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeSpear>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeSword>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DukeWhip>(), 7));
            }
            if (item.type == ItemID.FairyQueenBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<LightSpear>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<LightSword>(), 8));
            }
            if (item.type == ItemID.MoonLordBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonBow>(), 9));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonSpear>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonWhip>(), 7));
            }
            if (item.type == ItemID.QueenSlimeBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeSword>(), 8));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<QSlimeWhip>(), 6));
            }
        }
    }
    public class BossBodyDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.EaterofWorldsBody)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 1, 5));
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.QueenBee)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.Deerclops)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.Spazmatism)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.Retinazer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.TheDestroyer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.SkeletronPrime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.Plantera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.CultistBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.HallowBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfGlitch>(), 1, 10, 30));
            }
        }
    }
}
