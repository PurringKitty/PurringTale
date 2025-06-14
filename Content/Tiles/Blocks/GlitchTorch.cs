﻿using PurringTale.Content.Biomes;
using PurringTale.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PurringTale.Content.Tiles.Blocks
{

    public class GlitchTorch : ModTile
    {
        private Asset<Texture2D> flameTexture;

        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileWaterDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.DisableSmartInteract[Type] = true;
            TileID.Sets.Torch[Type] = true;

            DustType = ModContent.DustType<GlitchSolution>();
            AdjTiles = new int[] { TileID.Torches };

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Torches, 0));


            TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            TileObjectData.newSubTile.LinkedAlternates = true;
            TileObjectData.newSubTile.WaterDeath = false;
            TileObjectData.newSubTile.LavaDeath = false;
            TileObjectData.newSubTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newSubTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.addSubTile(1);

            TileObjectData.addTile(Type);


            AddMapEntry(new Color(200, 200, 200), Language.GetText("ItemName.Torch"));

            flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;

            int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
            player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
        }

        public override float GetTorchLuck(Player player)
        {


            bool inGlitchSurfaceBiome = player.InModBiome<GlitchSurfaceBiome>();
            return inGlitchSurfaceBiome ? 1f : -0.1f; 
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = Main.rand.Next(1, 3);

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];

            if (tile.TileFrameX < 66)
            {
                int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
                if (style == 0)
                {
                    r = 0.9f;
                    g = 0.9f;
                    b = 0.9f;
                }
                else if (style == 1)
                {
                    r = 0.5f;
                    g = 1.5f;
                    b = 0.5f;
                }
            }
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {

            offsetY = 0;

            if (WorldGen.SolidTile(i, j - 1))
            {
                offsetY = 4;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];

            if (!TileDrawing.IsVisible(tile))
            {
                return;
            }


            int offsetY = 0;

            if (WorldGen.SolidTile(i, j - 1))
            {
                offsetY = 4;
            }

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i); 
            Color color = new Color(100, 100, 100, 0);
            int width = 20;
            int height = 20;
            int frameX = tile.TileFrameX;
            int frameY = tile.TileFrameY;
            int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
            if (style == 1)
            { 
                color.G = 255;
            }

            for (int k = 0; k < 7; k++)
            {
                float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

                spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + offsetY + yy) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}