using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class EaterSword : ModItem
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
        Item.knockBack = 1.5f;
        Item.crit = 2;
        Item.value = Item.sellPrice(silver: 5);
        Item.rare = ItemRarityID.LightPurple;
        Item.UseSound = SoundID.Item1;
    }
}