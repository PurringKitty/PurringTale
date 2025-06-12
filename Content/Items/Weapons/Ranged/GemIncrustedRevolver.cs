using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

public class GemIncrustedRevolver : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Revolver);
        Item.damage = 35;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.knockBack = 6;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Yellow;
        Item.shootSpeed = 30f;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(1, 2);
        return offset;
    }
}