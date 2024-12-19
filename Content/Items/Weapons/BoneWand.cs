using Microsoft.Xna.Framework;
using PurringTale.Content;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class BoneWand : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 80;
        Item.DamageType = DamageClass.Magic;
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 5f;
        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Master;
        Item.UseSound = SoundID.Item8;
        Item.autoReuse = true;
        Item.mana = 15;
        Item.shootSpeed = 10f;
        Item.shoot = ModContent.ProjectileType<BoneMagicProj>();
    }


    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-5, 0);
        return offset;

    }
}

 
