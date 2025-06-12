using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class WrathfulMachineGun : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.SniperRifle);
        Item.damage = 110;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 15;
        Item.useAnimation = 15;
        Item.knockBack = 4;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Red;
        Item.shootSpeed = 80f;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-6, 6);
        return offset;
    }
}