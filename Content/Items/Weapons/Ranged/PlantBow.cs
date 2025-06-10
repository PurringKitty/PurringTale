using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class PlantBow : ModItem
	{
		public override void SetDefaults() {
        Item.damage = 55;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 13;
        Item.useAnimation = 13;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 1f;
        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Lime;
        Item.UseSound = SoundID.Item5;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.useAmmo = AmmoID.Arrow;
        Item.shootSpeed = 8f;
		}
    public override Vector2? HoldoutOffset()
    {
       Vector2 offset = new(0, 2);
       return offset;
    }
}