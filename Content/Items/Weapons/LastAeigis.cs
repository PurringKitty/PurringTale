using PurringTale.Content.Projectiles;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
#pragma warning disable CS0105 // Using directive appeared previously in this namespace
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.MobLoot;
#pragma warning restore CS0105 // Using directive appeared previously in this namespace

namespace PurringTale.Content.Items.Weapons
{
    public class LastAeigis : ModItem
	{
		// You can use a vanilla texture for your item by using the format: "Terraria/Item_<Item ID>".
		public override string Texture => "Terraria/Images/Item_" + ItemID.LastPrism;
		public static Color OverrideColor = new(122, 173, 255);

		public override void SetDefaults() {
			// Start by using CloneDefaults to clone all the basic item properties from the vanilla Last Prism.
			// For example, this copies sprite size, use style, sell price, and the item being a magic weapon.
			Item.CloneDefaults(ItemID.LastPrism);
			Item.mana = 5;
			Item.damage = 500;
            Item.value = Item.sellPrice(platinum: 5);
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<LastAeigisHoldout>();
			Item.shootSpeed = 50f;

			// Change the item's draw color so that it is visually distinct from the vanilla Last Prism.
			Item.color = OverrideColor;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalBall);
            recipe.AddIngredient<TopiumBar>(20);
            recipe.AddIngredient<WeakValhallaBar>(20);
            recipe.AddIngredient<CoreOfValhalla>(20);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();

		}
	}
}