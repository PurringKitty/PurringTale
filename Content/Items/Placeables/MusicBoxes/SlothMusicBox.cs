using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Tiles.Furniture.Crafters;
using PurringTale.Content.Tiles.MusicBoxes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.MusicBoxes
{
	public class SlothMusicBox : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/FleshAmalgam"), ModContent.ItemType<SlothMusicBox>(), ModContent.TileType<SlothMusicBoxTile>());
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MusicBox, 1);
            recipe.AddIngredient<Sloth>(5);
            recipe.AddTile<ValhallaWorkbench>();
            recipe.Register();
        }
    
    public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<SlothMusicBoxTile>(), 0);
		}
	}
}
