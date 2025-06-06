using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons
{
    public class CultBow : ModItem
	{
		

		public override void SetDefaults() {
            Item.damage = 135;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 13f;
		}
        public override Vector2? HoldoutOffset()
        {
           Vector2 offset = new(0, 2);
           return offset;

        }
    }
}