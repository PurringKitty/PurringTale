using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables;

namespace PurringTale.Content.Items.MobLoot;

public class CoreOfPride : ModItem
{


    public override void SetDefaults()
    {
        Item.width = 15;
        Item.height = 15;
        Item.maxStack = 9999;
        Item.material = true;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.sellPrice(silver: 25);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;
    }
}
