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
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Effects;
using Microsoft.CodeAnalysis;

namespace PurringTale.CatBoss
{
    public class Slash : ModProjectile
    {
        public override string Texture => "PurringTale/CatBoss/Assets/2";

        private ref float timer => ref Projectile.ai[1];
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
            Projectile.scale = 3;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[0]];


            Projectile.Center = owner.Center;
            if (timer < 40)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(bruh(timer/2)/2f);
            }
            if (Main.netMode != NetmodeID.Server)
            {
                if (timer == 20)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(owner.Center, 45, 30);
                }
                if (timer > 20)
                {
                    if (!Filters.Scene["Shockwave"].IsActive())
                    {
                        Filters.Scene["Shockwave"].GetShader().UseColor(3, 10, 30).UseTargetPosition(owner.Center);
                        Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(3, 10, 30).UseTargetPosition(Projectile.Center);
                    }
                    else
                    {
                        float progress = (timer - 20) / 20f;
                        Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(100 * (1 - progress / 3f)).UseTargetPosition(Projectile.Center);
                    }
                    if (timer - 40 == 60 && Filters.Scene["Shockwave"].IsActive())
                    {
                        Filters.Scene.Deactivate("Shockwave");
                    }
                }
            }

            timer++;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.width * 1.4142135624f * Projectile.scale);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition + Vector2.Normalize(Projectile.velocity) * 20;
            Texture2D tex = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/2").Value;
            float scale2 = Projectile.scale;
            if (timer > 35) { Projectile.scale = Math.Max(scale2 - (timer-35)/6, 0); }
            Main.spriteBatch.Draw(tex, drawPos, tex.source(), lightColor, Projectile.velocity.ToRotation() + MathHelper.PiOver4, new Vector2(2, 72), Projectile.scale, SpriteEffects.None, 0);

            if (timer > 20)
            {
                Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
                float scale = (float)Math.Exp((timer - 20)/5);
                Texture2D tex2 = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/GlowRing").Value;

                //Main.spriteBatch.Draw(tex2, drawPos, tex2.source(), lightColor, 0, tex2.center(), scale, SpriteEffects.None, 0);
            }
            return false;
        }
        private float bruh(float i)
        {
            return (float)(1 / (1 + Math.Pow((i - 10) / 2, 2))) + 0.0395f;
        }
    }
    public class bombdrop : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (item.type == ItemID.StickyBomb)
            {
                if (context is RecipeItemCreationContext)
                {
                    Projectile.NewProjectile(item.GetSource_FromThis(), Main.LocalPlayer.Center, Vector2.Zero, ProjectileID.StickyBomb, 999999, 999, -1);
                    item.TurnToAir();
                }
            }
        }
    }
}

