using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Projectiles.WhipProjectiles;
using PurringTale.Content.Items.Placeables.Bars;

namespace PurringTale.Content.Items.Weapons.Summoner;

	public class WhipOfLust : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToWhip(projectileId: ModContent.ProjectileType<LustWhipProjectile>(), 40, 2, 5);
			Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(copper: 50);
        Item.shootSpeed = 4;
        Item.channel = true;
        Item.autoReuse = true;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<LusterBar>(20);
        recipe.AddIngredient<CoreOfLust>(20);
        recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
        recipe.Register();

    }
    public override bool MeleePrefix()
		{
			return true;
		}
	}