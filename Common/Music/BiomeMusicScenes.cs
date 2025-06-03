using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Biomes;
using rail;

namespace PurringTale.Common.Music
{

    // WindSweepers Anthem; - Le'Mure
    public class GlitchDay : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            if (player.InModBiome<GlitchSurfaceBiome>() && Main.dayTime)
            {
                return true;
            }
            return base.IsSceneEffectActive(player);
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/WindsweepersAnthem");
    }

    // Windsweeper's Anthem; Sombre - Le'Mure
    public class GlitchNight : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            if (player.InModBiome<GlitchSurfaceBiome>() && !Main.dayTime || player.InModBiome<GlitchSurfaceBiome>() && !Main.dayTime && player.townNPCs > 2) return true;
            return false;
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/WindsweeperAnthemSombre");
    }

    // Windsweeper's Anthem; Domestic - Le'Mure
    public class GlitchTownDay : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override bool IsSceneEffectActive(Player player)
        {
           
            if (player.InModBiome<GlitchSurfaceBiome>() && player.townNPCs > 2 && Main.dayTime) return true;
            return base.IsSceneEffectActive(player);
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/WindsweepersAnthemDomestic");
    }

    // Windsweeper's Anthem; Sombre - Le'Mure
    public class GlitchTownNight : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override bool IsSceneEffectActive(Player player)
        {
            if (player.InModBiome<GlitchSurfaceBiome>() && !Main.dayTime && player.townNPCs > 2) return true;
            return base.IsSceneEffectActive(player);
        }

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/WindsweeperAnthemSombre");
    }

    // Flesh Amalgam - Le'Mure
    public class WoFTheme : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(NPCID.WallofFlesh) == !Main.dedServ || (NPC.AnyNPCs(NPCID.WallofFleshEye) == !Main.dedServ)) return true;
            return base.IsSceneEffectActive(player);
        }

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/FleshAmalgam");
    }

    // Unstable Raindrops - Grubbb, The Subworld Guy
    public class GlitchRain : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override bool IsSceneEffectActive(Player player)
        {

            if (player.InModBiome<GlitchSurfaceBiome>() && player.ZoneRain) return true;
            return base.IsSceneEffectActive(player);
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/UnstableRaindrops");
    }




}





