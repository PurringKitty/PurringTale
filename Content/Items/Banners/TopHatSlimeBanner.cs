using PurringTale.Content.Tiles;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using PurringTale.Content.Items.Vanity;

namespace PurringTale.Content.Items.Banners
{
	public class TopHatSlimeBanner : ModItem
	{
		// The tooltip for this item is automatically assigned from .lang files
		public override void SetDefaults() {
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = TileType<MonsterBanner>();
			Item.placeStyle = 0;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Gel, 10);
            recipe.AddIngredient<THCTopHat>(1);
            recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
            recipe.Register();
        }
    }
}