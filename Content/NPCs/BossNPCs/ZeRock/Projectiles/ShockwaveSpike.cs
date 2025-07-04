﻿using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles
{
    public class ShockwaveSpike : ModProjectile
    {
        private ref float DelayTimer => ref Projectile.ai[0];
        private ref float GrowthTimer => ref Projectile.ai[1];

        private bool hasEmerged = false;
        private float maxHeight = 80f;
        private Vector2 groundPosition;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
        }

        public override void AI()
        {
            if (DelayTimer > 0)
            {
                DelayTimer--;

                if (DelayTimer % 12 == 0)
                {
                    Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(-16f, 16f), 0);
                    Dust.NewDust(dustPos, 0, 0, DustID.Stone, 0, -2f, 100, Color.DarkRed, 1.0f);
                }
                return;
            }

            if (!hasEmerged)
            {
                hasEmerged = true;
                groundPosition = Projectile.Center;
                SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

                for (int i = 0; i < 40; i++)
                {
                    int tileX = (int)(Projectile.Center.X / 16f);
                    int tileY = (int)((Projectile.Center.Y + i * 16) / 16f);

                    if (tileX >= 0 && tileX < Main.maxTilesX && tileY >= 0 && tileY < Main.maxTilesY)
                    {
                        Tile tile = Main.tile[tileX, tileY];
                        if (tile.HasTile && Main.tileSolid[tile.TileType])
                        {
                            groundPosition.Y = tileY * 16f;
                            break;
                        }
                    }
                }

                Projectile.Center = groundPosition;
            }

            if (GrowthTimer < 40f)
            {
                GrowthTimer++;
                float growthProgress = GrowthTimer / 40f;

                float easedProgress = (float)(1 - Math.Pow(1 - growthProgress, 2));

                Projectile.height = (int)(maxHeight * easedProgress);
                Projectile.Center = new Vector2(Projectile.Center.X, groundPosition.Y - Projectile.height / 2);

                if (GrowthTimer % 6 == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 dustPos = Projectile.Bottom + new Vector2(Main.rand.NextFloat(-12f, 12f), 0);
                        Dust.NewDust(dustPos, 0, 0, DustID.Stone, 0, -1.5f, 100, Color.DarkGray, 1.4f);
                    }
                }

                if (GrowthTimer % 10 == 0)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(Projectile.Center, 4f, 15);
                }
            }
            else if (GrowthTimer < 150f)
            {
                GrowthTimer++;
            }
            else
            {
                GrowthTimer++;
                float retractProgress = (GrowthTimer - 150f) / 40f;
                retractProgress = Math.Min(retractProgress, 1f);

                Projectile.height = (int)(maxHeight * (1f - retractProgress));
                Projectile.Center = new Vector2(Projectile.Center.X, groundPosition.Y - Projectile.height / 2);

                if (retractProgress >= 1f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == ModContent.NPCType<RockBoss>())
                return false;

            return DelayTimer <= 0 && GrowthTimer > 0 && GrowthTimer < 150f;
        }

        public override bool CanHitPlayer(Player target)
        {
            return DelayTimer <= 0 && GrowthTimer > 0 && GrowthTimer < 150f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (DelayTimer > 0) return false;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Rectangle sourceRect = new Rectangle(0, texture.Height - Projectile.height, texture.Width, Projectile.height);

            Color drawColor = Color.Lerp(lightColor, Color.DarkRed, 0.2f);

            Main.spriteBatch.Draw(texture, drawPos, sourceRect, drawColor, Projectile.rotation,
                new Vector2(texture.Width / 2f, Projectile.height / 2f), Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
