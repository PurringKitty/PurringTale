using PurringTale.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Numerics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Tiles.Blocks
{
    public class GlitchTree : ModTree
    {
        private Asset<Texture2D> texture;
        private Asset<Texture2D> branchesTexture;
        private Asset<Texture2D> topsTexture;

        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { ModContent.TileType<GlitchSand>() };
            texture = ModContent.Request<Texture2D>("PurringTale/Content/Tiles/Blocks/GlitchTree");
            branchesTexture = ModContent.Request<Texture2D>("PurringTale/Content/Tiles/Blocks/GlitchTree_Branches");
            topsTexture = ModContent.Request<Texture2D>("PurringTale/Content/Tiles/Blocks/GlitchTree_Tops");
        }

        public override Asset<Texture2D> GetTexture()
        {
            return texture;
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<GlitchSapling>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {

        }


        public override Asset<Texture2D> GetBranchTextures() => branchesTexture;


        public override Asset<Texture2D> GetTopTextures() => topsTexture;

        public override int DropWood()
        {
            return ModContent.ItemType<CoreOfGlitch>();
        }


        public override int TreeLeaf()
        {
            return ModContent.GoreType<GlitchTreeLeaf>();
        }
    }
}