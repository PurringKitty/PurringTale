using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;
using PurringTale.Content.Tiles;

namespace PurringTale.Content.Tiles
{
	public class TopiumOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;

			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine[Type] = 1500;
			Main.tileShine2[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 5000;
            DustType = 84;
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 225;
            AddMapEntry(new Color(94, 147, 150), Language.GetText("Topium"));
        }
    }
}

public class TopiumOreSystem : ModSystem
{
    public static LocalizedText TopiumOrePassMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        TopiumOrePassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(TopiumOrePassMessage)}"));
    }
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

        if (ShiniesIndex != -1)
        {
            tasks.Insert(ShiniesIndex + 1, new TopiumOrePass("PurringTail Mod Ores", 237.4299f));
        }
    }
}

public class TopiumOrePass : GenPass
{
    public TopiumOrePass(string name, float loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = TopiumOreSystem.TopiumOrePassMessage.Value;
        for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
        {
            int x = WorldGen.genRand.Next(0, Main.maxTilesX);
            int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
            WorldGen.TileRunner(x, y, WorldGen.genRand.Next(6, 6), WorldGen.genRand.Next(8, 8), ModContent.TileType<TopiumOreTile>());
        }
    }
}

