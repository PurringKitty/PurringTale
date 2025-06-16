using PurringTale.Content.Tiles.Furniture.Relics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Furniture.Relics
{
	public class SinsBossRelic : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<SinsBossRelicTile>(), 0);
			Item.width = 30;
			Item.height = 48;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.buyPrice(silver: 25);
		}
	}
}
