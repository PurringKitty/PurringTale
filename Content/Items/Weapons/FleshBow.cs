using PurringTale.Content.Projectiles;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
#pragma warning disable CS0105 // Using directive appeared previously in this namespace
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.MobLoot;
#pragma warning restore CS0105 // Using directive appeared previously in this namespace

namespace PurringTale.Content.Items.Weapons
{
    public class FleshBow : ModItem
	{
		

		public override void SetDefaults() {
            Item.damage = 125;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(silver: 25);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 4.50f;
		}
        public override Vector2? HoldoutOffset()
        {
           Vector2 offset = new(0, 2);
           return offset;

        }
    }
}