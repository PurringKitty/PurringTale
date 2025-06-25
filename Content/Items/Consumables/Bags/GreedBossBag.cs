using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Armor;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Weapons.Ranged;
using PurringTale.Content.Items.Weapons.Summoner;
using PurringTale.Content.NPCs.BossNPCs.Greed;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Consumables.Bags
{
	public class GreedBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ValhallaOre>(), 1, 1, 20));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VanityVoucher>(), 5, 1, 2));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Greed>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GreedHelmet>(), 2, 0, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GreedBreastplate>(), 2, 0, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GreedLeggings>(), 2, 0, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoldOnAStick>(), 2, 0, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<WhipOfGreed>(), 2, 0, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GemIncrustedRevolver>(), 2, 0, 1));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<GreedBoss>()));
        }
    }
}
