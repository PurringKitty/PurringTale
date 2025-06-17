using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace PurringTale.Content.NPCs.BossNPCs.Sloth.Projectiles
{
    public class SlothSpear3 : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RainbowCrystalExplosion;

        private Vector2 targetCenter = Vector2.Zero;
        private bool hasTarget = false;
        private float originalDistance = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.alpha = 0;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            if (!hasTarget && Projectile.velocity.Length() > 0)
            {
                Vector2 normalizedVelocity = Vector2.Normalize(Projectile.velocity);

                if (Projectile.ai[0] != 0 || Projectile.ai[1] != 0)
                {
                    targetCenter = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                }
                else
                {
                    targetCenter = Projectile.Center + normalizedVelocity * 800f;
                }

                originalDistance = Vector2.Distance(Projectile.Center, targetCenter);
                hasTarget = true;
            }

            if (hasTarget)
            {
                float distanceToTarget = Vector2.Distance(Projectile.Center, targetCenter);

                if (distanceToTarget <= 200f)
                {
                    CreateCenterImpactEffect();

                    Projectile.Kill();
                    return;
                }
            }

            Projectile.scale = 1.2f + (float)Math.Sin(Projectile.timeLeft * 0.1f) * 0.3f;
            Projectile.rotation += 0.15f;

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha = (int)((60 - Projectile.timeLeft) / 60f * 255f);
            }

            bool shouldSparkle = Projectile.timeLeft > 60 ? Main.rand.NextBool(3) : Main.rand.NextBool(6);
            if (shouldSparkle)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.RainbowMk2, 0f, 0f, 100, default, 0.8f);
                dust.velocity = Projectile.velocity * 0.1f;
                dust.noGravity = true;
                dust.fadeIn = 0.8f;
            }

            bool shouldPrismatic = Projectile.timeLeft > 60 ? Main.rand.NextBool(5) : Main.rand.NextBool(10);
            if (shouldPrismatic)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 2, 2, DustID.PurpleTorch, 0f, 0f, 150);
                dust.velocity = Main.rand.NextVector2Circular(2f, 2f);
                dust.noGravity = true;
                dust.scale = 0.6f;
            }
        }

        private void CreateCenterImpactEffect()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 25; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.RainbowMk2);
                dust.velocity = Main.rand.NextVector2Circular(12f, 12f);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1.5f, 2.5f);
            }

            for (int i = 0; i < 15; i++)
            {
                Dust electric = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric);
                electric.velocity = Main.rand.NextVector2Circular(8f, 8f);
                electric.noGravity = true;
                electric.scale = Main.rand.NextFloat(1.2f, 2.0f);
            }

            for (int i = 0; i < 20; i++)
            {
                Dust energy = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.PurpleTorch);
                energy.velocity = Main.rand.NextVector2Circular(10f, 10f);
                energy.noGravity = true;
                energy.scale = Main.rand.NextFloat(1.0f, 1.8f);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.RainbowMk2);
                dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Rectangle sourceRect = texture.Frame();
            Vector2 origin = sourceRect.Size() / 2f;

            float alphaMultiplier = (255f - Projectile.alpha) / 255f;

            Color glowColor = Color.Purple * 0.4f * alphaMultiplier;
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.PiOver2 * i) * 3f;
                Main.EntitySpriteDraw(texture, drawPosition + offset, sourceRect, glowColor,
                    Projectile.rotation, origin, Projectile.scale * 1.1f, SpriteEffects.None, 0);
            }

            Color mainColor = Color.White * alphaMultiplier;

            float colorCycle = (Main.GameUpdateCount * 0.05f) % (MathHelper.TwoPi);
            mainColor = Color.Lerp(Color.Purple, Color.Cyan, (float)Math.Sin(colorCycle) * 0.5f + 0.5f) * alphaMultiplier;

            Main.EntitySpriteDraw(texture, drawPosition, sourceRect, mainColor,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        DustID.RainbowMk2, 0f, 0f, 100, default, 1.2f);
                    dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
                    dust.noGravity = true;
                }
            }
        }
    }
}