using PurringTale.Content.Tiles.MusicBoxes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.MusicBoxes
{
	public class MenuThemeMusicBox : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/MainMenu"), ModContent.ItemType<MenuThemeMusicBox>(), ModContent.TileType<MenuThemeMusicBoxTile>());
		}

    public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<MenuThemeMusicBoxTile>(), 0);
		}
	}
}
