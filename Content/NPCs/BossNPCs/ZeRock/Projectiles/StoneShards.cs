using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles
{
    public class StoneShards : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.scale = Main.rand.NextFloat(0.7f, 1.0f);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;

            Projectile.velocity.X *= 0.99f;

            Projectile.rotation += Projectile.velocity.X * 0.1f;

            if (Main.rand.NextBool(15))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, 0, 0, 100, default, 0.4f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 1f)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.4f;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Stone, 0, 0, 100, default, 0.6f);
            }

            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }

            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == ModContent.NPCType<RockBoss>())
                return false;
            return null;
        }
    }
}
