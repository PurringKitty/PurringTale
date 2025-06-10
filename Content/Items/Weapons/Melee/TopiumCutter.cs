using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class TopiumCutter : ModItem
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
        Item.damage = 250;
        Item.knockBack = 6f;
        Item.crit = 10;
        Item.value = Item.sellPrice(gold: 50);
        Item.rare = ItemRarityID.Cyan;
        Item.UseSound = SoundID.Item1;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<TopiumBar>(19)
            .AddTile(TileID.Anvils)
            .Register();
    }
}