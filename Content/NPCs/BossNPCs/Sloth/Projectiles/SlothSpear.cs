using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.NPCs.BossNPCs.Sloth.Projectiles
{
    public class SlothSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 76;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.alpha = 50;
            Projectile.light = 0.3f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        private ref float initialRotation => ref Projectile.ai[0];
        private ref float homingTimer => ref Projectile.ai[1];
        private bool hasSetRotation = false;

        public override bool? CanHitNPC(NPC target)
        {
            return target.type != ModContent.NPCType<SlothBoss>() && target.type != ModContent.NPCType<SlothHead>();
        }

        public override void AI()
        {
            if (!hasSetRotation)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                initialRotation = Projectile.rotation;
                hasSetRotation = true;
            }

            homingTimer++;

            if (homingTimer > 30 && Projectile.timeLeft > 60)
            {
                Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                if (target != null && !target.dead)
                {
                    Vector2 direction = Projectile.DirectionTo(target.Center);

                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * Projectile.velocity.Length(), 0.01f);

                    float targetRotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                    float rotationDiff = MathHelper.WrapAngle(targetRotation - Projectile.rotation);
                    Projectile.rotation += rotationDiff * 0.05f;
                }
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.1f;
                dust.scale = 0.6f;
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 8;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2f;
                float trailAlpha = (float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length;
                Color trailColor = Color.Purple * trailAlpha * 0.4f * (1f - Projectile.alpha / 255f);

                Main.EntitySpriteDraw(texture, drawPos, null, trailColor, Projectile.rotation,
                    texture.Size() / 2f, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame);
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                dust.noGravity = true;
            }
        }
    }
}