using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class BrainStem : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 40;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 32;
        Item.knockBack = 5f;
        Item.crit = 5;
        Item.value = Item.sellPrice(silver: 1);
        Item.rare = ItemRarityID.LightRed;
        Item.UseSound = SoundID.Item1;
    }
}