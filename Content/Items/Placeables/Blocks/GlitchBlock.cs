using PurringTale.Content.Items.MobLoot;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Blocks
{
    public class GlitchBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;

 
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Blocks.GlitchBlock>());
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.Green;
        }




    }
        }
    
