using Microsoft.Xna.Framework;
using PurringTale.Content.Items;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Pets.Boots;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Pets.FedoraCat
{
	public class FedoraCatPetItem : ModItem
	{
		public override void SetDefaults() 
		{
			Item.CloneDefaults(ItemID.CrimsonHeart);
			Item.shoot = ModContent.ProjectileType<FedoraCatPetProjectile>();
			Item.buffType = ModContent.BuffType<FedoraCatPetBuff>();
            Item.width = 20;
            Item.height = 20;
        }

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
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
