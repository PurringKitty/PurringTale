using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Consumables;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class BeeBlaster : ModItem
{
    // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

    public override void SetDefaults()
    {
        Item.damage = 50;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 5f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Red;
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