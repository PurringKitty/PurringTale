using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class PridePistol : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Handgun);
        Item.damage = 80;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 18;
        Item.useAnimation = 18;
        Item.knockBack = 2;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Purple;
        Item.shootSpeed = 50f;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-1, 4);
        return offset;
    }
}