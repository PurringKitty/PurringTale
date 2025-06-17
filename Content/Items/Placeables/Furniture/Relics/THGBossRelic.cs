using PurringTale.Content.Tiles.Furniture.Relics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Furniture.Relics
{
	public class THGBossRelic : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<THGBossRelicTile>(), 0);
            Item.width = 30;
            Item.height = 48;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(silver: 25);
        }
	}
}
