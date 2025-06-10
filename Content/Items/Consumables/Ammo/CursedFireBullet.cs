using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles;

namespace PurringTale.Content.Items.Consumables.Ammo;

public class CursedFireBullet : ModItem
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
		Item.rare = ItemRarityID.Lime;
		Item.consumable = true;
		Item.shoot = Mod.Find<ModProjectile>("EaterBlasterProj").Type;
		Item.ammo = ModContent.ItemType<CursedFireBullet>();
		Item.maxStack = 9999;
		Item.shootSpeed = 4.5f;
	}

	public override void AddRecipes() 
	{
		Recipe recipe = CreateRecipe(25);
		recipe.AddIngredient(ItemID.EmptyBullet, 1);
		recipe.AddIngredient(ItemID.CursedFlame, 1);
		recipe.AddTile(TileID.AmmoBox);
		recipe.Register();
	}
}