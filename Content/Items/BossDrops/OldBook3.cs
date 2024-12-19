using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables;

namespace PurringTale.Content.Items.BossDrops;

public class OldBook3 : ModItem
{


    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.maxStack = 1;
        Item.material = true;
        Item.value = Item.sellPrice(gold: 5);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;

    }

}



