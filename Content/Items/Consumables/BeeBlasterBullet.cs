using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles;

namespace PurringTale.Content.Items.Consumables;

public class BeeBlasterBullet : ModItem
{
	public override void SetStaticDefaults() 
	{
	}

	public override void SetDefaults() 
	{
		Item.damage = 20;
		Item.DamageType = DamageClass.Ranged;
		Item.width = 40;
		Item.height = 40;
		Item.knockBack = 2f;
		Item.value = 5;
		Item.rare = ItemRarityID.Yellow;
		Item.consumable = true;
		Item.shoot = Mod.Find<ModProjectile>("BeeBlasterProj").Type;
		Item.ammo = ModContent.ItemType<BeeBlasterBullet>();
		Item.maxStack = 9999;
		Item.shootSpeed = 3.5f;
	}

	public override void AddRecipes() 
	{
		Recipe recipe = CreateRecipe(25);
		recipe.AddIngredient(ItemID.EmptyBullet, 1);
		recipe.AddIngredient(ItemID.Beenade, 1);
		recipe.AddTile(TileID.AmmoBox);
		recipe.Register();
	}
}