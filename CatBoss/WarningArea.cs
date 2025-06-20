using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace PurringTale.CatBoss
{
    public class WarningArea : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 0 && Projectile.timeLeft == 120)
            {
                Projectile.timeLeft = (int)Projectile.ai[0];
            }

            if (Projectile.ai[1] > 0)
            {
                float radius = Projectile.ai[1];
                Projectile.width = (int)radius;
                Projectile.height = (int)radius;
            }

            Projectile.velocity = Vector2.Zero;

            float pulseSpeed = 0.1f;
            float minAlpha = 0.3f;
            float maxAlpha = 0.8f;

            float pulse = (float)System.Math.Sin(Main.GameUpdateCount * pulseSpeed) * 0.5f + 0.5f;
            Projectile.alpha = (int)(255 * (1f - (minAlpha + pulse * (maxAlpha - minAlpha))));

            float basScale = Projectile.ai[1] > 0 ? Projectile.ai[1] / 100f : 1f;
            Projectile.scale = basScale + (pulse * 0.1f);

            Projectile.rotation += 0.02f;

            if (Main.rand.NextBool(8))
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                float distance = (Projectile.width / 2f) * 0.8f;
                Vector2 dustPos = Projectile.Center + Vector2.One.RotatedBy(angle) * distance;

                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Torch);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust.color = Color.Lerp(Color.Orange, Color.Red, pulse);
                dust.alpha = 100;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("PurringTale/CatBoss/WarningArea").Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;

            Color warningColor = Color.Red * (1f - Projectile.alpha / 255f);

            Main.spriteBatch.Draw(texture, drawPos, null, warningColor, Projectile.rotation,
                origin, Projectile.scale, SpriteEffects.None, 0f);

            Color secondColor = Color.Orange * (1f - Projectile.alpha / 255f) * 0.5f;
            Main.spriteBatch.Draw(texture, drawPos, null, secondColor, -Projectile.rotation * 0.5f,
                origin, Projectile.scale * 1.1f, SpriteEffects.None, 0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - Projectile.alpha / 255f);
        }
    }
}