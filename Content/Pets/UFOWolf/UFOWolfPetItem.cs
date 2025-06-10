using Microsoft.Xna.Framework;
using PurringTale.Content.Items;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Pets.UFOWolf;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Pets.UFOWolf
{
	public class UFOWolfPetItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ZephyrFish);
			Item.shoot = ModContent.ProjectileType<UFOWolfPetProjectile>();
			Item.buffType = ModContent.BuffType<UFOWolfPetBuff>();
			Item.width = 20;
			Item.height = 20;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600);
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<VanityVoucher>(1);
			recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
			recipe.Register();

		}
	}
}

