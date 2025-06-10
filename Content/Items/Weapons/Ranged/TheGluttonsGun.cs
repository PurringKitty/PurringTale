using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class TheGluttonsGun : ModItem
{
		public override void SetDefaults() {
        Item.damage = 30;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 2;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item12;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.useAmmo = AmmoID.Arrow;
        Item.shootSpeed = 50f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<GluttonusBar>(10);
            recipe.AddIngredient<CoreOfGluttony>(10);
            recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
            recipe.Register();

		}
        public override Vector2? HoldoutOffset()
        {
            Vector2 offset = new(6, 2);
            return offset;
        }
}