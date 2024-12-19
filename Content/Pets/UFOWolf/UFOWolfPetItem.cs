using PurringTale.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Pets.UFOWolf;

namespace PurringTale.Content.Pets.UFOWolf
{
	public class UFOWolfPetItem : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.
			Item.shoot = ModContent.ProjectileType<UFOWolfPetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<UFOWolfPetBuff>(); // Apply buff upon usage of the Item.
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}


		}
	}

