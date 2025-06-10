using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Projectiles.WhipProjectiles;
using PurringTale.Content.Items.Placeables.Bars;

namespace PurringTale.Content.Items.Weapons.Summoner;

public class DancersWhip : ModItem
{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<GodSlayerWhipProjectile>(), 1000, 2, 6);
			Item.rare = ItemRarityID.Quest;
            Item.value = Item.sellPrice(gold: 50);
            Item.shootSpeed = 4;
            Item.channel = true;
            Item.autoReuse = true;
        }
		public override bool MeleePrefix()
		{
			return true;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<RustedDancersWhip>(1);
            recipe.AddIngredient<TopiumBar>(20);
            recipe.AddIngredient<CoreOfValhalla>(20);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
}