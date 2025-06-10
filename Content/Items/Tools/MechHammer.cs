using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Tools;

public class MechHammer : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 100;
        Item.DamageType = DamageClass.Melee;
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 10f;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Expert;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.hammer = 1000;
        Item.useTurn = true;
    }
}
