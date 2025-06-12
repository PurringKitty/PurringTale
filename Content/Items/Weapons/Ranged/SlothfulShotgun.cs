using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class SlothfulShotgun : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Shotgun);
        Item.damage = 90;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.knockBack = 4;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.LightPurple;
        Item.shootSpeed = 30f;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-7, 2);
        return offset;
    }
}