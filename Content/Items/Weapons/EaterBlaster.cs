using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Consumables;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class EaterBlaster : ModItem
{
    // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

    public override void SetDefaults()
    {
        Item.damage = 30;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 1f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Red;
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