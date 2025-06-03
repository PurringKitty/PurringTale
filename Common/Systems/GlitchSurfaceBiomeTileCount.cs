using System;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Tiles;

namespace PurringTale.Common.Systems
{
    public class GlitchSurfaceBiomeTileCount : ModSystem
    {
        public int glitchBlockCount;
        public int glitchBlockCount2;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            glitchBlockCount = tileCounts[ModContent.TileType<GlitchBlock>()] + tileCounts[ModContent.TileType<GlitchSand>()];
        }
    }
}