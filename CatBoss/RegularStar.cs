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
    public class RegularStar : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FallingStar}";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 300;
            Projectile.light = 0.7f;
            Projectile.scale = 0.9f;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        private ref float timer => ref Projectile.ai[0];

        public override void AI()
        {
            timer++;

            if (Projectile.alpha > 0)
                Projectile.alpha = Math.Max(0, Projectile.alpha - 6);

            Projectile.rotation += 0.15f;

            if (timer > 60)
            {
                Projectile.velocity *= 1.002f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Wraith);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
                dust.scale = Main.rand.NextFloat(0.6f, 1.0f);
                dust.color = Color.DarkGray;
            }

            if (Main.rand.NextBool(12))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 0.5f;
                dust.color = Color.Black;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!target.HasBuff(ModContent.BuffType<Consumed>()))
                target.AddBuff(ModContent.BuffType<Consumed>(), 180);
            else
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Consumed>())] = 180;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float alpha = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length * 0.4f;
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;

                Color trailColor = Color.DarkGray * alpha * (1f - Projectile.alpha / 255f);

                Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, drawPos, null, trailColor,
                    Projectile.oldRot[i], TextureAssets.Projectile[Type].Value.Size() / 2f,
                    Projectile.scale * (1f - i * 0.05f), SpriteEffects.None, 0f);
            }

            Vector2 mainDrawPos = Projectile.Center - Main.screenPosition;
            Color mainColor = Color.Lerp(lightColor, Color.Black, 0.6f) * (1f - Projectile.alpha / 255f);

            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, mainDrawPos, null, mainColor,
                Projectile.rotation, TextureAssets.Projectile[Type].Value.Size() / 2f,
                Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 8 * i) * 3f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Wraith, vel);
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust.color = Color.DarkGray;
            }

            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 4 * i) * 1.5f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, vel);
                dust.noGravity = true;
                dust.scale = 0.8f;
                dust.color = Color.Black;
            }
        }
    }
}
