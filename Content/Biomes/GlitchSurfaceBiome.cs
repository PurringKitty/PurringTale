using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using PurringTale.Assets.Textures.Backgrounds;
using tModPorter;
using Terraria.WorldBuilding;
using Terraria.GameContent;
using PurringTale.Content.Tiles;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace PurringTale.Content.Biomes
{
    public class GlitchSurfaceBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<GlitchWaterStyle>(); 
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<GlitchBiomeBackgroundStyle>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;




        public override int BiomeTorchItemType => ModContent.ItemType<Items.Placeables.Blocks.GlitchTorch>();

        public override string BackgroundPath => "PurringTale/Assets/Textures/Background/GlitchBiomeSurfaceMid";
        public override string MapBackground => BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;
        public override string BestiaryIcon => "Content/Biomes/GlitchBesitery";
        
        

        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<Common.Systems.GlitchSurfaceBiomeTileCount>().glitchBlockCount >= 100;

            bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 4;


            bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            return b1 && b2 && b3;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;


        }
 

    }

