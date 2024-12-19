using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics;
using Microsoft.CodeAnalysis;
using Terraria.Localization;
using Terraria.GameContent.Events;
using Terraria.UI;

namespace PurringTale.CatBoss
{
    public class Consumed : ModBuff
    {
        public override string Texture => "PurringTale/CatBoss/Assets/Consumed";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            return base.PreDraw(spriteBatch, buffIndex, ref drawParams);
        }
        public override void Update(Player player, ref int buffIndex)
        {
            
        }


    }
    /*public class ConsumedPlayer : ModPlayer
    {
        public int BuffIndex
        {
            get { return Player.FindBuffIndex(ModContent.BuffType<Consumed>()); }
        }
        public bool HasBuff
        {
            get { return BuffIndex >= 0; }
        }
        
    }*/
    [Autoload(Side = ModSide.Client)] 
    public class ScreenEffectUI : ModSystem {

        private UserInterface screenoverface;
        internal ScreenOverlayState screenOverlayState;

        public override void Load()
        {
            screenoverface = new UserInterface();
            screenOverlayState = new ScreenOverlayState();
            screenOverlayState.Activate();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.LocalPlayer.FindBuffIndex(ModContent.BuffType<Consumed>()) == -1)
            {
                screenoverface?.SetState(null);
            } else
            {
                screenoverface?.SetState(screenOverlayState);
            }
            if (screenoverface?.CurrentState != null)
            {
                screenoverface?.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new LegacyGameInterfaceLayer(
                "hi",
                delegate
                {
                    if (screenoverface?.CurrentState != null)
                    {
                        screenoverface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.Game)
            );
        }
    }
    internal class ScreenOverlayState : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            int buffIndex = Main.LocalPlayer.FindBuffIndex(ModContent.BuffType<Consumed>());
            if (buffIndex > -1)
            {
                int time = Main.LocalPlayer.buffTime[buffIndex];
                float opacity()
                {
                    if (time >= 60 * 5 - 10)
                    {
                        int num2 = time - (60 * 5 - 10);
                        return (10 - (float)num2) * 0.9f / 10f;
                    }
                    if (time <= 10)
                    {
                        return (float)time * 0.9f / 10f;
                    }
                    return 0.9f;
                }
                Color color = Color.Black * opacity();
                int num1 = TextureAssets.Extra[49].Width();
                int num2 = 10;
                Rectangle rect = Main.player[Main.myPlayer].getRect();
                rect.Y += 10;
                rect.Inflate((num1 - rect.Width) / 2, (num1 - rect.Height) / 2 + num2 / 2);
                rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);
                Rectangle destinationRectangle1 = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
                Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
                Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
                Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle1, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle3, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle4, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
                spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, color);
            }
        }
    }
}

