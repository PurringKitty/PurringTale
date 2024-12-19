using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.Weapons
{
	public class WhipOfLust : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToWhip(projectileId: ModContent.ProjectileType<LustWhipProjectile>(), 40, 2, 10);
			Item.width = 28;
			Item.height = 25;
			Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(copper: 50);
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
}