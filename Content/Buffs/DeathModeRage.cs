using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;

namespace PurringTale.Content.Buffs
{
    public class DeathModeRage : ModBuff
    {
        public override string Texture => "PurringTale/Content/Buffs/DeathModeRage";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
    }

    [Autoload(Side = ModSide.Client)]
    public class DeathModeScreenEffect : ModSystem
    {
        private UserInterface deathModeInterface;
        internal DeathModeOverlayState deathModeOverlayState;

        public override void Load()
        {
            deathModeInterface = new UserInterface();
            deathModeOverlayState = new DeathModeOverlayState();
            deathModeOverlayState.Activate();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            bool hasDeathModeBuff = Main.LocalPlayer.FindBuffIndex(ModContent.BuffType<DeathModeRage>()) != -1;

            if (!hasDeathModeBuff)
            {
                deathModeInterface?.SetState(null);
            }
            else
            {
                deathModeInterface?.SetState(deathModeOverlayState);
            }

            if (deathModeInterface?.CurrentState != null)
            {
                deathModeInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new LegacyGameInterfaceLayer(
                "PurringTale:DeathModeOverlay",
                delegate
                {
                    if (deathModeInterface?.CurrentState != null)
                    {
                        deathModeInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.Game)
            );
        }
    }

    internal class DeathModeOverlayState : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            int buffIndex = Main.LocalPlayer.FindBuffIndex(ModContent.BuffType<DeathModeRage>());
            if (buffIndex > -1)
            {
                int timeLeft = Main.LocalPlayer.buffTime[buffIndex];
                float totalTime = 2700f;
                float progress = 1f - (timeLeft / totalTime);

                float pulseIntensity = 0.3f + 0.2f * (float)System.Math.Sin(Main.GameUpdateCount * 0.15f);

                float urgencyMultiplier = 1f + progress * 1.5f;
                pulseIntensity *= urgencyMultiplier;

                Color redOverlay = Color.Red * (0.15f + pulseIntensity * 0.1f);

                Rectangle screenRect = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, screenRect, redOverlay);

                DrawEdgeDarkening(spriteBatch, progress, pulseIntensity);

                if (progress > 0.5f)
                {
                    DrawScanlines(spriteBatch, progress);
                }
            }
        }

        private void DrawEdgeDarkening(SpriteBatch spriteBatch, float progress, float pulseIntensity)
        {
            float edgeWidth = 100f + progress * 50f;
            float alpha = (0.3f + progress * 0.4f + pulseIntensity * 0.1f);
            Color edgeColor = Color.Black * alpha;

            Rectangle topEdge = new Rectangle(0, 0, Main.screenWidth, (int)edgeWidth);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, topEdge, edgeColor);

            Rectangle bottomEdge = new Rectangle(0, Main.screenHeight - (int)edgeWidth, Main.screenWidth, (int)edgeWidth);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, bottomEdge, edgeColor);

            Rectangle leftEdge = new Rectangle(0, 0, (int)edgeWidth, Main.screenHeight);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, leftEdge, edgeColor);

            Rectangle rightEdge = new Rectangle(Main.screenWidth - (int)edgeWidth, 0, (int)edgeWidth, Main.screenHeight);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rightEdge, edgeColor);
        }

        private void DrawScanlines(SpriteBatch spriteBatch, float progress)
        {
            int lineSpacing = 4;
            float alpha = (progress - 0.5f) * 0.3f;

            for (int y = 0; y < Main.screenHeight; y += lineSpacing)
            {
                Rectangle scanline = new Rectangle(0, y, Main.screenWidth, 1);
                Color scanlineColor = Color.Red * alpha;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, scanline, scanlineColor);
            }
        }
    }
}