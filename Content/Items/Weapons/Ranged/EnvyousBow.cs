using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class EnvyousBow : ModItem
	{
		public override void SetDefaults() {
        Item.damage = 16;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 18;
        Item.height = 38;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 0.5f;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Lime;
        Item.UseSound = SoundID.Item5;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.useAmmo = AmmoID.Arrow;
        Item.shootSpeed = 7f;
		}
    public override Vector2? HoldoutOffset()
    {
       Vector2 offset = new(0, 2);
       return offset;

    }
}