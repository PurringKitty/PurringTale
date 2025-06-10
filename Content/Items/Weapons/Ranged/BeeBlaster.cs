using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles.BulletProjectiles;
using PurringTale.Content.Items.Consumables.Ammo;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class BeeBlaster : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 40;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 5f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Yellow;
        Item.UseSound = SoundID.Item20;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<BeeBlasterProj>();
        Item.useAmmo = ModContent.ItemType<BeeBlasterBullet>();
        Item.shootSpeed = 30f;
    }



    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-7, -1);
        return offset;

    }
}