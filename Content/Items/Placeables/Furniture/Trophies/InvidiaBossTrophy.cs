using PurringTale.Content.Tiles.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Furniture.Trophies
{
	public class InvidiaBossTrophy : ModItem
	{
		public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<InvidiaBossTrophyTile>());
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 50);
        }
	}
}
