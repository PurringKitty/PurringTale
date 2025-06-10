using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class LightSword : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 150;
        Item.knockBack = 5.5f;
        Item.crit = 5;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Expert;
        Item.UseSound = SoundID.Item1;
    }
}