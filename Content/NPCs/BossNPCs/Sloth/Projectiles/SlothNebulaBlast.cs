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
    public class SlothNebulaBlast : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.NebulaBlaze1;

        private int frameCounter = 0;
        private int currentFrame = 0;
        private const int totalFrames = 4;
        private const int frameSpeed = 6;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.light = 0.3f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.0f;
        }

        public override void AI()
        {
            frameCounter++;
            if (frameCounter >= frameSpeed)
            {
                frameCounter = 0;
                currentFrame++;
                if (currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(6))
            {
                Dust nebulaDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.RainbowMk2, 0f, 0f, 100, default, 0.6f);
                nebulaDust.velocity = Projectile.velocity * 0.05f;
                nebulaDust.noGravity = true;
                nebulaDust.fadeIn = 0.6f;
                nebulaDust.color = Color.Lerp(Color.Purple, Color.Pink, Main.rand.NextFloat());
            }

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha = (int)((60 - Projectile.timeLeft) / 60f * 255f);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.RainbowMk2);
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.8f, 1.4f);
                dust.color = Color.Lerp(Color.Purple, Color.Pink, Main.rand.NextFloat());
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.RainbowMk2, 0f, 0f, 100, default, 1.0f);
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                dust.noGravity = true;
                dust.color = Color.Lerp(Color.Purple, Color.Pink, Main.rand.NextFloat());
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;

            int frameHeight = texture.Height / totalFrames;
            Rectangle sourceRect = new Rectangle(0, currentFrame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = sourceRect.Size() / 2f;

            float alphaMultiplier = (255f - Projectile.alpha) / 255f;

            Color mainColor = Color.Lerp(Color.Purple, Color.Pink, (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.5f + 0.5f) * alphaMultiplier;

            Main.EntitySpriteDraw(texture, drawPosition, sourceRect, mainColor,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}