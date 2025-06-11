using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class BladeOfEnvy : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 19;
        Item.useAnimation = 19;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 16;
        Item.knockBack = 0.5f;
        Item.crit = 1;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Lime;
        Item.UseSound = SoundID.Item1;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(0,-8);
        return offset;
    }
}