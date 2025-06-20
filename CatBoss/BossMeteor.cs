using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace PurringTale.CatBoss
{
    public class BossMeteor : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.Fireball];
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Fireball);

            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.5f;
        }

        public override bool PreAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            return true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;

            Projectile.velocity.Y += 0.3f;

            if (Projectile.velocity.Y > 20f)
                Projectile.velocity.Y = 20f;

            if (Main.rand.NextBool(1))
            {
                Color[] bossColors = {
                    Color.Purple,
                    Color.Red,
                    Color.DarkGray,
                    Color.DarkRed,
                    Color.Magenta
                };

                Color trailColor = bossColors[Main.rand.Next(bossColors.Length)];

                Vector2 dustPos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2);
                Vector2 dustVel = -Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(3f, 3f);

                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Shadowflame, dustVel);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1.5f, 3f);
                dust.color = trailColor;
                dust.alpha = Main.rand.Next(30, 120);
            }

            if (Main.rand.NextBool(2))
            {
                Vector2 firePos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height);
                Vector2 fireVel = -Projectile.velocity * 0.15f + Main.rand.NextVector2Circular(2f, 2f);

                Dust fire = Dust.NewDustPerfect(firePos, DustID.Torch, fireVel);
                fire.noGravity = true;
                fire.scale = Main.rand.NextFloat(2f, 4f);

                Color[] fireColors = { Color.DarkRed, Color.Purple, Color.Red, Color.Magenta };
                fire.color = fireColors[Main.rand.Next(fireColors.Length)];
            }

            if (Main.rand.NextBool(3))
            {
                Vector2 sparkPos = Projectile.Center;
                Vector2 sparkVel = Main.rand.NextVector2Circular(5f, 5f);

                Dust spark = Dust.NewDustPerfect(sparkPos, DustID.Electric, sparkVel);
                spark.noGravity = true;
                spark.scale = Main.rand.NextFloat(1f, 2f);
                spark.color = Color.Lerp(Color.Purple, Color.Yellow, Main.rand.NextFloat());
            }

            if (Main.rand.NextBool(4))
            {
                Vector2 darkPos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 1.5f, Projectile.height * 1.5f);
                Vector2 darkVel = Main.rand.NextVector2Circular(2f, 2f);

                Dust dark = Dust.NewDustPerfect(darkPos, DustID.Shadowflame, darkVel);
                dark.noGravity = true;
                dark.scale = Main.rand.NextFloat(1.2f, 2.5f);
                dark.color = Color.DarkGray;
                dark.alpha = 150;
            }

            if (Projectile.timeLeft % 45 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[ProjectileID.Fireball].Value;
            Rectangle sourceRect = texture.Frame(1, Main.projFrames[ProjectileID.Fireball], 0, Projectile.frame);
            Vector2 origin = sourceRect.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (float)i / Projectile.oldPos.Length;

                Color trailColor = Color.Lerp(Color.Purple, Color.Red, progress) * (1f - progress) * 0.8f;

                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                float trailScale = Projectile.scale * (1f - progress * 0.3f);

                Main.spriteBatch.Draw(texture, drawPos, sourceRect, trailColor, Projectile.oldRot[i],
                    origin, trailScale, SpriteEffects.None, 0f);
            }

            Vector2 mainDrawPos = Projectile.Center - Main.screenPosition;

            float pulse = (float)Math.Sin(Main.GameUpdateCount * 0.15f) * 0.3f + 0.7f;
            Color glowColor = Color.Lerp(Color.Purple, Color.Red, pulse) * 0.4f;

            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.TwoPi / 6 * i) * (4f * pulse);
                Main.spriteBatch.Draw(texture, mainDrawPos + offset, sourceRect, glowColor, Projectile.rotation,
                    origin, Projectile.scale * (1.1f + pulse * 0.2f), SpriteEffects.None, 0f);
            }

            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color bossColor = Color.Lerp(Color.Purple, Color.Red, (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.5f + 0.5f);
            return Color.Lerp(lightColor, bossColor, 0.6f);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.position);

            ModContent.GetInstance<MCameraModifiers>().Shake(Projectile.Center, 25f, 40);

            for (int i = 0; i < 50; i++)
            {
                Vector2 dustVel = Vector2.One.RotatedBy(MathHelper.TwoPi / 50 * i) * Main.rand.NextFloat(8f, 20f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, dustVel);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(2.5f, 5f);

                Color[] explosionColors = { Color.Purple, Color.Red, Color.DarkGray, Color.Magenta, Color.DarkRed };
                dust.color = explosionColors[Main.rand.Next(explosionColors.Length)];
            }

            for (int i = 0; i < 35; i++)
            {
                Vector2 fireVel = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(5f, 15f);
                Dust fire = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, fireVel);
                fire.noGravity = true;
                fire.scale = Main.rand.NextFloat(2f, 4.5f);

                Color[] fireColors = { Color.DarkRed, Color.Purple, Color.Red, Color.Magenta };
                fire.color = fireColors[Main.rand.Next(fireColors.Length)];
            }

            for (int i = 0; i < 20; i++)
            {
                Vector2 sparkVel = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(3f, 10f);
                Dust spark = Dust.NewDustPerfect(Projectile.Center, DustID.Electric, sparkVel);
                spark.noGravity = true;
                spark.scale = Main.rand.NextFloat(1.2f, 2.5f);
                spark.color = Color.Lerp(Color.Purple, Color.Yellow, Main.rand.NextFloat());
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead)
                    {
                        float distance = Vector2.Distance(player.Center, Projectile.Center);
                        if (distance < 100f)
                        {
                            int explosionDamage = (int)(Projectile.damage * 0.7f * (1f - distance / 100f));
                            Vector2 knockback = Vector2.Normalize(player.Center - Projectile.Center) * 8f;

                            player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByProjectile(-1, Projectile.whoAmI),
                                explosionDamage, (int)knockback.X, false, false, -1, false);
                        }
                    }
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D glowTexture = TextureAssets.Projectile[ProjectileID.Fireball].Value;
            Rectangle sourceRect = glowTexture.Frame(1, Main.projFrames[ProjectileID.Fireball], 0, Projectile.frame);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = sourceRect.Size() / 2f;

            float glowPulse = (float)Math.Sin(Main.GameUpdateCount * 0.2f) * 0.4f + 0.6f;
            Color glowColor = Color.Purple * (0.3f * glowPulse);

            Main.spriteBatch.Draw(glowTexture, drawPos, sourceRect, glowColor, Projectile.rotation,
                origin, Projectile.scale * (1.4f + glowPulse * 0.3f), SpriteEffects.None, 0f);
        }
    }
}
