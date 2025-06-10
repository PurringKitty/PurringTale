using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Consumables.Ammo;
using PurringTale.Content.Projectiles.BulletProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class EaterBlaster : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 24;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 1f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.LightPurple;
        Item.UseSound = SoundID.Item20;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<EaterBlasterProj>();
        Item.useAmmo = ModContent.ItemType<CursedFireBullet>();
        Item.shootSpeed = 20f;
    }



    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-7, -1);
        return offset;
    }
}