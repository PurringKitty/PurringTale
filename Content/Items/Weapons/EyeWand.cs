using Microsoft.Xna.Framework;
using PurringTale.Content;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class EyeWand : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 15;
        Item.DamageType = DamageClass.Magic;
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 2f;
        Item.value = Item.sellPrice(silver: 5);
        Item.rare = ItemRarityID.Master;
        Item.UseSound = SoundID.Item8;
        Item.autoReuse = true;
        Item.mana = 5;
        Item.shootSpeed = 15f;
        Item.shoot = ModContent.ProjectileType<EyeMagicProj>();
    }


    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-5, 0);
        return offset;

    }
}

 
