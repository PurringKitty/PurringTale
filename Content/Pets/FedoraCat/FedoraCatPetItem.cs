using PurringTale.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Pets.Boots;

namespace PurringTale.Content.Pets.FedoraCat
{
	public class FedoraCatPetItem : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() 
		{
			Item.CloneDefaults(ItemID.CrimsonHeart);
			Item.shoot = ModContent.ProjectileType<FedoraCatPetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<FedoraCatPetBuff>(); // Apply buff upon usage of the Item.
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LightKey)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
