﻿using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Vanity
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class DOLLeggings : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.LightPurple; // The rarity of the item
			Item.defense = 0; // The amount of defense the item will give when equipped
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<VanityVoucher>(1);
            recipe.AddTile<ValhallaWorkbench>();
            recipe.Register();
        }
	}
}
