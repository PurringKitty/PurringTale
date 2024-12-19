using Microsoft.Xna.Framework;
using PurringTale.Content;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class GolemWand : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 110;
        Item.DamageType = DamageClass.Magic;
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 13;
        Item.useAnimation = 13;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 10;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Master;
        Item.UseSound = SoundID.Item8;
        Item.autoReuse = true;
        Item.mana = 20;
        Item.shootSpeed = 10f;
        Item.shoot = ModContent.ProjectileType<GolemMagicProj>();
    }


    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-5, 0);
        return offset;

    }
}

 
