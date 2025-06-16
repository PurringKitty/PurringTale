using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;

namespace PurringTale.CatBoss
{
	public class Gss: ModProjectile
	{
        public override string Texture => "PurringTale/CatBoss/Assets/2";

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 70;
            Projectile.height = 74;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 180;
            Projectile.light = 1;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        private ref float timer => ref Projectile.ai[0];
        public override void AI()
        {
            timer++;
            Projectile.alpha = (int)Math.Clamp(Projectile.alpha - timer * 3, 0, 255);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            base.AI();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 velNormal = Vector2.Normalize(Projectile.velocity);
            Vector2 endPos = Projectile.Center - velNormal * 50;
            Vector2 startPos = Projectile.Center + velNormal * 50;

            return Collision.CheckAABBvLineCollision(new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height), startPos, endPos);
        }
    }
}

