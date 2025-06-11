using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Projectiles.MagicProjectiles;
using PurringTale.Content.Items.Placeables.Bars;

namespace PurringTale.Content.Items.Weapons.Magic;

    public class LastAeigis : ModItem
{
	public override string Texture => "Terraria/Images/Item_" + ItemID.LastPrism;
	public static Color OverrideColor = new(122, 173, 255);

	public override void SetDefaults() 
	{
		Item.CloneDefaults(ItemID.LastPrism);
		Item.mana = 5;
		Item.damage = 500;
            Item.value = Item.sellPrice(platinum: 5);
            Item.rare = ItemRarityID.Master;
            Item.shoot = ModContent.ProjectileType<LastAeigisHoldout>();
		Item.shootSpeed = 50f;
		Item.color = OverrideColor;
	}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.CrystalBall);
            recipe.AddIngredient<TopiumBar>(2000);
            recipe.AddIngredient<ValhallaBar>(2000);
            recipe.AddIngredient<CoreOfValhalla>(200);
            recipe.AddIngredient(ItemID.LunarBar, 1000);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
	}
}