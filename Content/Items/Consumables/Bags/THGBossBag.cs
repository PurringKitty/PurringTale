using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Pets.Boots;
using PurringTale.CatBoss;
using PurringTale.Content.Items.Lore;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Weapons.Ranged;
using PurringTale.Content.Items.Accessories.Masks;
using PurringTale.Content.Items.Accessories.Necklaces;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.Consumables.Bags
{
	public class THGBossBag : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = false;
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
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TopHatDemonEye>(), 2, 1, 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TopHatDemonPendent>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonlightGreatSword>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EldritchBlaster>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook1>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook2>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook3>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook4>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TopiumOre>(), 2, 1, 100));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ValhallaOre>(), 2, 1, 100));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VanityVoucher>(), 1, 1, 4));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<TopHatCatBoss>()));
        }
	}
}
