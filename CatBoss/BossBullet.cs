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
using Terraria.Graphics;
using Microsoft.CodeAnalysis;

namespace PurringTale.CatBoss
{
    public class BossBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 13;
            Projectile.height = 39;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                dust.velocity *= 0.3f;
                dust.scale = 0.7f;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float alpha = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length * 0.5f;
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                Color trailColor = Color.Yellow * alpha;

                Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, drawPos, null, trailColor,
                    Projectile.oldRot[i], TextureAssets.Projectile[Type].Value.Size() / 2f,
                    Projectile.scale * (1f - i * 0.1f), SpriteEffects.None, 0f);
            }

            Vector2 mainDrawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, mainDrawPos, null, lightColor,
                Projectile.rotation, TextureAssets.Projectile[Type].Value.Size() / 2f,
                Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!target.HasBuff(ModContent.BuffType<Consumed>()))
                target.AddBuff(ModContent.BuffType<Consumed>(), 180);
            else
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Consumed>())] = 180;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 5 * i) * 2f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, vel);
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }
    }
}