using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;
using PurringTale;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Tiles;

namespace PurringTale.Content.Tiles
{
    public class WeakValhallaOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 900;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 2000;


            AddMapEntry(new Color(222, 155, 113), Language.GetText("Fools Valhalla"));

            DustType = 84;
            HitSound = SoundID.Tink;

            MineResist = 4f;
            MinPick = 35;
            // MineResist = 4f;
            // MinPick = 200;
        }

        // Example of how to enable the Biome Sight buff to highlight this tile. Biome Sight is technically intended to show "infected" tiles, so this example is purely for demonstration purposes.
        //public override bool IsTileBiomeSightable  {
        //TileID.sets.cr
    }
}

public class WeakValhallaOreSystem : ModSystem
{
    public static LocalizedText WeakValhallaOrePassMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        WeakValhallaOrePassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(WeakValhallaOrePassMessage)}"));
    }

    // World generation is explained more in https://github.com/tModLoader/tModLoader/wiki/World-Generation
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        // Because world generation is like layering several images on top of each other, we need to do some steps between the original world generation steps.

        // Most vanilla ores are generated in a step called "Shinies", so for maximum compatibility, we will also do this.
        // First, we find out which step "Shinies" is.
        int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

        if (ShiniesIndex != -1)
        {
            // Next, we insert our pass directly after the original "Shinies" pass.
            // ExampleOrePass is a class seen bellow
            tasks.Insert(ShiniesIndex + 1, new WeakValhallaOrePass("PurringTail Mod Ores", 237.4300f));
        }
    }
}

public class WeakValhallaOrePass : GenPass
{
    public WeakValhallaOrePass(string name, float loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        // progress.Message is the message shown to the user while the following code is running.
        // Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
        progress.Message = WeakValhallaOreSystem.WeakValhallaOrePassMessage.Value;

        // Ores are quite simple, we simply use a for loop and the WorldGen.TileRunner to place splotches of the specified Tile in the world.
        // "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
        for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
        {
            // The inside of this for loop corresponds to one single splotch of our Ore.
            // First, we randomly choose any coordinate in the world by choosing a random x and y value.
            int x = WorldGen.genRand.Next(0, Main.maxTilesX);

            // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.
            int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);

            // Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place.
            // Feel free to experiment with strength and step to see the shape they generate.
            WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(7, 15), ModContent.TileType<WeakValhallaOreTile>());

            // Alternately, we could check the tile already present in the coordinate we are interested.
            // Wrapping WorldGen.TileRunner in the following condition would make the ore only generate in Snow.
            // Tile tile = Framing.GetTileSafely(x, y);
            // if (tile.HasTile && tile.TileType == TileID.SnowBlock) {
            // 	WorldGen.TileRunner(.....);
            // }
        }
    }
}


