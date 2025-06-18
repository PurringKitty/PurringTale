using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles
{
    public class FallingBoulder : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.4f;
            Projectile.rotation += Projectile.velocity.X * 0.02f;

            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 0.6f);
            }

            if (Projectile.velocity.Y > 10f && Main.rand.NextBool(15))
            {
                int checkTileX = (int)(Projectile.Center.X / 16f);
                int checkTileY = (int)((Projectile.Center.Y + 100f) / 16f);

                if (checkTileX >= 0 && checkTileX < Main.maxTilesX && checkTileY >= 0 && checkTileY < Main.maxTilesY)
                {
                    if (Main.tile[checkTileX, checkTileY].HasTile)
                    {
                        Vector2 dustPos = new Vector2(checkTileX * 16f + Main.rand.Next(16), checkTileY * 16f);
                        Dust.NewDust(dustPos, 16, 16, DustID.Stone, 0, -0.5f, 100, Color.Gray, 0.4f);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ModContent.GetInstance<MCameraModifiers>().Shake(Projectile.Center, 10f, 20);

            SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);

            for (int i = 0; i < 12; i++)
            {
                Vector2 dustVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 6f);
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Stone, dustVel.X, dustVel.Y, 100, default, 1.2f);
            }

            for (int i = 0; i < 2; i++)
            {
                Vector2 fragmentVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1f, 4f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, fragmentVel,
                    ModContent.ProjectileType<BoulderFragment>(), Projectile.damage / 3, 2f);
            }

            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            for (int i = 0; i < Math.Min(Projectile.oldPos.Length, 3); i++)
            {
                float alpha = (float)(3 - i) / 3f * 0.3f;
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                Color trailColor = Color.Brown * alpha;

                Main.spriteBatch.Draw(texture, drawPos, null, trailColor, Projectile.rotation,
                    texture.Size() / 2f, Projectile.scale * (1f - i * 0.15f), SpriteEffects.None, 0f);
            }

            return true;
        }
    }
}
