using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;
public class SinsBane: ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 222;
        Item.DamageType = DamageClass.Melee;
        Item.width = 37;
        Item.height = 35;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.knockBack = 10;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Red;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }
}
