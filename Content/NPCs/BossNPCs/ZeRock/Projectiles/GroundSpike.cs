using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles
{
    public class GroundSpike : ModProjectile
    {
        private ref float DelayTimer => ref Projectile.ai[0];
        private ref float GrowthTimer => ref Projectile.ai[1];

        private bool hasEmerged = false;
        private float maxHeight = 64f;
        private Vector2 groundPosition;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 8;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
        }

        public override void AI()
        {
            if (DelayTimer > 0)
            {
                DelayTimer--;

                if (DelayTimer % 15 == 0)
                {
                    Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(-12f, 12f), 0);
                    Dust.NewDust(dustPos, 0, 0, DustID.Stone, 0, -1.5f, 100, Color.Red, 0.6f);
                }
                return;
            }

            if (!hasEmerged)
            {
                hasEmerged = true;
                groundPosition = Projectile.Center;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);

                for (int i = 0; i < 30; i++)
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

            if (GrowthTimer < 30f)
            {
                GrowthTimer++;
                float growthProgress = GrowthTimer / 30f;

                float easedProgress = (float)(1 - Math.Pow(1 - growthProgress, 3));

                Projectile.height = (int)(maxHeight * easedProgress);
                Projectile.Center = new Vector2(Projectile.Center.X, groundPosition.Y - Projectile.height / 2);

                if (GrowthTimer % 8 == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 dustPos = Projectile.Bottom + new Vector2(Main.rand.NextFloat(-8f, 8f), 0);
                        Dust.NewDust(dustPos, 0, 0, DustID.Stone, 0, -0.8f, 100, default, 1.0f);
                    }
                }
            }
            else if (GrowthTimer < 120f)
            {
                GrowthTimer++;
            }
            else
            {
                GrowthTimer++;
                float retractProgress = (GrowthTimer - 120f) / 30f;
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

            return DelayTimer <= 0 && GrowthTimer > 0 && GrowthTimer < 120f;
        }

        public override bool CanHitPlayer(Player target)
        {
            return DelayTimer <= 0 && GrowthTimer > 0 && GrowthTimer < 120f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (DelayTimer > 0) return false;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Rectangle sourceRect = new Rectangle(0, texture.Height - Projectile.height, texture.Width, Projectile.height);

            Main.spriteBatch.Draw(texture, drawPos, sourceRect, lightColor, Projectile.rotation,
                new Vector2(texture.Width / 2f, Projectile.height / 2f), Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
