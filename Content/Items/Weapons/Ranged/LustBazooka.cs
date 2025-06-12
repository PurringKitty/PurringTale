using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class LustBazooka : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.RocketLauncher);
        Item.damage = 50;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 28;
        Item.useAnimation = 28;
        Item.knockBack = 20;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Pink;
        Item.shootSpeed = 5f;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(1, 2);
        return offset;
    }
}