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
    public class SlothDagger : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.PurpleLaser}";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1.2f;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.type != ModContent.NPCType<SlothBoss>() && target.type != ModContent.NPCType<SlothHead>();
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.scale = 0.8f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Purple * 0.9f;
        }
    }
}
