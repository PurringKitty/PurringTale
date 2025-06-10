using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Ores;

public class WeakValhallaOre : ModItem
{

    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 7;
        Item.maxStack = 9999;
        Item.consumable = true;
        Item.value = Item.sellPrice(silver: 10);
        Item.createTile = ModContent.TileType<Tiles.WeakValhallaOreTile>();
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Expert;
        ItemID.Sets.OreDropsFromSlime[Type] = (5, 15);
        Item.ResearchUnlockCount = 100;
        ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
    }
}