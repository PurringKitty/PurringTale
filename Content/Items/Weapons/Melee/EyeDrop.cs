using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class EyeDrop : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 18;
        Item.useAnimation = 18;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 25;
        Item.knockBack = 1.0f;
        Item.crit = 1;
        Item.value = Item.sellPrice(copper: 66);
        Item.rare = ItemRarityID.LightRed;
        Item.UseSound = SoundID.Item1;
    }
}