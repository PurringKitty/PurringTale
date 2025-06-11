using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Pets.Boots;
using PurringTale.Content.Pets.UFOWolf;
using PurringTale.Content.Items.Armor;
using PurringTale.Content.Items.Tools;
using PurringTale.Content.Pets.FedoraCat;
using PurringTale.Content.Items.Weapons.Melee;

namespace PurringTale.Content.Items.Consumables.Bags
{
	public class BoxOfStuff : ModItem
	{
		public override void SetStaticDefaults() 
        {
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() {
			Item.maxStack = 1;
			Item.consumable = true;
			Item.width = 28;
			Item.height = 22;
            Item.value = 0;
            Item.rare = ItemRarityID.Master;
		}
		public override bool CanRightClick() {
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BootsPetItem>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<UFOWolfPetItem>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FedoraCatPetItem>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtHelmet>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtBreastplate>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtLeggings>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtSword>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtPickaxe>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtAxe>(), 1, 1, 1));
        }
    }
}