using Microsoft.Xna.Framework;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class GolemBlaster : ModItem
{
    // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

    public override void SetDefaults()
    {
        Item.damage = 90;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 5f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Red;
        Item.UseSound = SoundID.Item10;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.GolemFist;
        Item.useAmmo = AmmoID.Bullet;
        Item.shootSpeed = 50f;
    }



    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-7, -4);
        return offset;

    }
}