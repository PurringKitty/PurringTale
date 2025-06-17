using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles
{
    public class BoulderFragment : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.scale = Main.rand.NextFloat(0.5f, 0.8f);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity.X *= 0.98f;

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, 0, 0, 100, default, 0.4f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 2f)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.3f;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.3f;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Stone, 0, 0, 100, default, 0.6f);
            }

            return false;
        }
    }
}