using Microsoft.Xna.Framework;
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
using Terraria.GameContent.Golf;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;

namespace PurringTale.CatBoss
{
    //big rocket
    /*
    To restore the original use, delete the other comments
    */
    public class BossRocket : ModProjectile
    {
        /// ai[0] timer
        /// ai[1] owner

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NebulaArcanum}";

        private Vector2[] orbitPositions = new Vector2[3];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            //Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
        }
        private ref float timer => ref Projectile.ai[0];
        private ref float deathTimer => ref Projectile.ai[2];

        public override bool PreAI()
        {
            if (deathTimer <= 0)
            {
                return true;
            } else
            {
                DeathAnimation();
                deathTimer++;
                return false;
            }
        }
        public override void AI()
        {
            float speed = 15;
            float rotation = (Projectile.velocity.X >= 0 ? -1 : 1) / 4f;
            Projectile.rotation += rotation;

            if (timer <= 100 && timer > 20)
                Projectile.TrackClosestPlayer(speed, 20, 650);

            for (int i = 0; i < 3; ++i)
            {
                float distance = 40;
                orbitPositions[i] = Projectile.Center + Vector2.One.RotatedBy(timer * 0.05f + (i * MathHelper.Pi / 3)) * 20;
            }
            if (timer >= 480)
            {
                deathTimer++;
            }




            timer++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                var oldP = Projectile.oldPos.Reverse().ToArray()[i];
                var oldR = Projectile.oldRot.Reverse().ToArray()[i];
                float alpha = i / (float)Projectile.oldPos.Length;
                Vector2 position1 = oldP + Projectile.Size()/2 + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Texture2D texture2D1 = TextureAssets.Projectile[Projectile.type].Value;
                Color color1 = Projectile.GetAlpha(lightColor) * alpha;
                Vector2 origin1 = new Vector2((float)texture2D1.Width, (float)texture2D1.Height) / 2f;
                float rotation1 = oldR;
                Rectangle? sourceRectangle = new Rectangle?();
                SpriteEffects spriteEffects = SpriteEffects.None;
                Texture2D texture2D2 = TextureAssets.Extra[50].Value;
                

                Color color2 = color1 * 0.8f * alpha;
                color2.A /= (byte)2;
                Color color4 = Color.Lerp(color1, Color.Black, 0.5f) * alpha;
                color4.A = color1.A;
                float num2 = (float)(0.949999988079071 + (double)(Projectile.rotation * 0.75f).ToRotationVector2().Y * 0.100000001490116);
                Color color5 = color4 * num2 * alpha;
                float scale2 = (float)(0.600000023841858 + (double)Projectile.scale * 0.600000023841858 * (double)num2) * alpha;
                Vector2 origin2 = texture2D2.Size() / 2f;

                Main.EntitySpriteDraw(texture2D2, position1, new Microsoft.Xna.Framework.Rectangle?(), color5, (float)(-(double)rotation1 + 0.349999994039536), origin2, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(texture2D2, position1, new Microsoft.Xna.Framework.Rectangle?(), color1, -rotation1, origin2, Projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(texture2D1, position1, new Microsoft.Xna.Framework.Rectangle?(), color2, (float)(-(double)rotation1 * 0.699999988079071), origin1, Projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(texture2D2, position1, new Microsoft.Xna.Framework.Rectangle?(), color1 * 0.8f, rotation1 * 0.5f, origin2, Projectile.scale * 0.9f, spriteEffects, 0);
                color1.A = (byte)0;

                Main.EntitySpriteDraw(TextureAssets.Extra[50].Value, position1, new Rectangle?(), color5, (float)(-(double)Projectile.rotation + 0.349999994039536), origin1, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(TextureAssets.Extra[50].Value, position1, new Rectangle?(), color1, -Projectile.rotation, origin1, Projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(texture2D1, position1, new Rectangle?(), color2, (float)(-(double)Projectile.rotation * 0.699999988079071), origin1, Projectile.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(TextureAssets.Extra[50].Value, position1, new Rectangle?(), color1 * 0.8f, Projectile.rotation * 0.5f, origin1, Projectile.scale * 0.9f, spriteEffects, 0);
                color1.A = (byte)0;
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            deathTimer++;
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            deathTimer++;
        }
        public void DeathAnimation()
        {
            Projectile.scale *= 0.95f;
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation += timer / 10f;

            for (int i = 0; i < 2; ++i)
            {
                Vector2 away = Vector2.One.RotatedBy(timer * 0.2f + i * MathHelper.Pi);
                Dust a = Dust.NewDustPerfect(Projectile.Center, DustID.Vortex, away * (5 * (1 - deathTimer/30f)) , 0, Color.Black);
                a.noGravity = true;
            }
            if (deathTimer > 30)
            {
                Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 50; i++)
                {
                    var vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 50 * i);
                    Dust a = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleCrystalShard, vel * j, 0, default, 2);
                    a.noGravity = true;
                }
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (target.WithinRange(Projectile.Center, 75))
                {
                    //target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, Projectile.whoAmI), Projectile.damage, Projectile.Center.X > target.Center.X ? -1 : 1, false, false, -1, true, 0, 0, 8);
                }
            }
        }
    }
    public static class MyClass{public static Vector2 Size(this Projectile projectile){return new Vector2(projectile.width, projectile.height);}}
}

