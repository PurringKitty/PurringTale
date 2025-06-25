using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.NPCs.BossNPCs.Greed.Projectiles
{
    public class GreedCoin : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.GoldCoin}";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;

            Projectile.velocity.Y += 0.15f;
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin);
                dust.velocity = Projectile.velocity * 0.2f;
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Item[ItemID.GoldCoin].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;

            Color glowColor = Color.Gold * 0.5f;
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.PiOver2 * i) * 2f;
                Main.spriteBatch.Draw(texture, drawPos + offset, null, glowColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.position);

            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Vector2.One.RotatedBy(MathHelper.TwoPi / 8 * i) * 3f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GoldCoin, velocity);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
        }
    }
}