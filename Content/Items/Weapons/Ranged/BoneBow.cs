using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class BoneBow : ModItem
	{
		public override void SetDefaults() {
        Item.damage = 33;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 1.5f;
        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.White;
        Item.UseSound = SoundID.Item5;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.useAmmo = AmmoID.Arrow;
        Item.shootSpeed = 5f;
		}
    public override Vector2? HoldoutOffset()
    {
       Vector2 offset = new(0, 2);
       return offset;

    }
}