using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class DukeSword : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 66;
        Item.knockBack = 6f;
        Item.crit = 6;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Cyan;
        Item.UseSound = SoundID.Item1;
    }
}