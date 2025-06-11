using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class GluttonsGreatsword : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 68;
        Item.height = 68;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 18;
        Item.useAnimation = 18;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 25;
        Item.knockBack = 2;
        Item.crit = 1;
        Item.value = Item.sellPrice(copper: 75);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
    }
}