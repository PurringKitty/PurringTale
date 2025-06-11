using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.IO;
using PurringTale.Content.Tiles.Blocks;

namespace PurringTale.Content.Subworlds
{
    public class GlitchDimension : Subworld
    {
        public override int Width => 500;
        public override int Height => 500;

        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => false;


        public override List<GenPass> Tasks => new List<GenPass>()
    {
        new GlitchGenPass()
    };

        public override void OnLoad()
        {
            Main.dayTime = true;
            Main.time = 27000;
            SubworldSystem.hideUnderworld = true;
        }



        public class GlitchGenPass : GenPass
        {
            public GlitchGenPass() : base("Terrain", 1) { }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                progress.Message = "Generating terrain";
                Main.worldSurface = Main.maxTilesY - 42;
                Main.rockLayer = Main.maxTilesY;
                progress.Message = TopiumOreSystem.TopiumOrePassMessage.Value;
                Main.ServerSideCharacter = true;

                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY)); // Controls the progress bar, should only be set between 0f and 1f
                        Tile tile = Main.tile[i, j];
                        tile.HasTile = true;
                        tile.TileType = TileID.Dirt;
                    }
                    for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                    {
                        int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(6, 6), WorldGen.genRand.Next(8, 8), ModContent.TileType<GlitchSand>());
                    }

                    for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                    {
                        int x = WorldGen.genRand.Next(250, Main.maxTilesX);

                        int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);

                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(7, 15), ModContent.TileType<GlitchSand>());

                    }


                    for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                    {
                        int x = WorldGen.genRand.Next(250, Main.maxTilesX);

                        int y = WorldGen.genRand.Next((int)GenVars.worldSurface, Main.maxTilesY);

                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(7, 15), ModContent.TileType<GlitchSand>());

                    }
                    for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                    {
                        int x = WorldGen.genRand.Next(250, Main.maxTilesX);

                        int y = WorldGen.genRand.Next((int)GenVars.worldSurface, Main.maxTilesY);

                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(7, 15), ModContent.TileType<GlitchSand>());

                    }
                    for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                    {
                        int x = WorldGen.genRand.Next(250, Main.maxTilesX);

                        int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);

                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(7, 15), ModContent.TileType<GlitchSand>());

                    }
                    for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
                    {
                        int x = WorldGen.genRand.Next(250, Main.maxTilesX);

                        int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);

                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(7, 15), ModContent.TileType<GlitchSand>());

                    }
                }
            }

        }
    }
}

