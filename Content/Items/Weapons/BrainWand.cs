using Microsoft.Xna.Framework;
using PurringTale.Content;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class BrainWand : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 32;
        Item.DamageType = DamageClass.Magic;
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 2.5f;
        Item.value = Item.sellPrice(silver: 8);
        Item.rare = ItemRarityID.Master;
        Item.UseSound = SoundID.Item8;
        Item.autoReuse = true;
        Item.mana = 10;
        Item.shootSpeed = 10f;
        Item.shoot = ModContent.ProjectileType<BrainMagicProj>();
    }


    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-2, 0);
        return offset;

    }
}

 