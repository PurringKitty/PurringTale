using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Legs)]
	public class WolfBoots : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18; 
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<VanityVoucher>(1);
            recipe.AddTile<ValhallaWorkbench>();
            recipe.Register();
        }
	}
}
