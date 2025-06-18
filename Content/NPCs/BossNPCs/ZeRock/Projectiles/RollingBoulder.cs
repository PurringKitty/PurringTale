using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles
{
    public class RollingBoulder : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;

            Projectile.rotation += Projectile.velocity.X * 0.02f;

            if (Math.Abs(Projectile.velocity.X) > 1f && Main.rand.NextBool(6))
            {
                Vector2 dustPos = Projectile.Bottom + new Vector2(Main.rand.NextFloat(-16f, 16f), 0);
                Dust.NewDust(dustPos, 0, 0, DustID.Stone, -Projectile.velocity.X * 0.3f, -1f, 100, default, 0.8f);
            }

            if (Math.Abs(Projectile.velocity.X) > 5f && Main.rand.NextBool(20))
            {
                ModContent.GetInstance<MCameraModifiers>().Shake(Projectile.Center, 3f, 10);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

                for (int i = 0; i < 8; i++)
                {
                    Vector2 dustVel = new Vector2(Projectile.velocity.X > 0 ? -2f : 2f, Main.rand.NextFloat(-2f, 0f));
                    Dust.NewDust(Projectile.Center, 0, 0, DustID.Stone, dustVel.X, dustVel.Y, 100, default, 1.0f);
                }
            }

            if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 0f)
            {
                Projectile.velocity.Y = 0f;
            }

            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == ModContent.NPCType<RockBoss>())
                return false;
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length * 0.3f;
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                Color trailColor = Color.SaddleBrown * alpha;

                Main.spriteBatch.Draw(texture, drawPos, null, trailColor, Projectile.oldRot[i],
                    texture.Size() / 2f, Projectile.scale * (1f - i * 0.1f), SpriteEffects.None, 0f);
            }

            return true;
        }
    }
}