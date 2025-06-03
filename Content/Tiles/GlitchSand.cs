using PurringTale.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;

namespace PurringTale.Content.Tiles
{

    public class GlitchSand : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            TileID.Sets.CanBeDugByShovel[Type] = true;

            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
            TileID.Sets.GeneralPlacementTiles[Type] = false;
            TileID.Sets.ChecksForMerge[Type] = true;

            MineResist = 0.5f; 
            DustType = DustID.Stone;
            AddMapEntry(new Color(150, 255, 150));
        }

        public override bool HasWalkDust()
        {
            return Main.rand.NextBool(3);
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.Sand;
        }


    }

    public class SandGenPass : GenPass
    {
        public SandGenPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = TopiumOreSystem.TopiumOrePassMessage.Value;
            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurface, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(100, 30), WorldGen.genRand.Next(200, 50), ModContent.TileType<GlitchSand>());
            }
        }
    }
}