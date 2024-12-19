using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;

namespace PurringTale.Content.Items.Placeables;

public class LusterOre : ModItem
{

    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 7;
        Item.maxStack = 9999;
        Item.consumable = true;
        Item.value = Item.sellPrice(silver: 25);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Expert;
        Item.ResearchUnlockCount = 100;
        ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
    }
}