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
        public override string Texture => "PurringTale/CatBoss/Assets/Sword";

        private ref float timer => ref Projectile.ai[1];
        private float baseScale = 3f;
        private float phaseMultiplier = 1f;
        private Vector2 fixedCenter;

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
            NPC owner = null;
            int bossIndex = (int)Projectile.ai[0];

            if (bossIndex >= 0 && bossIndex < Main.maxNPCs && Main.npc[bossIndex].active)
            {
                owner = Main.npc[bossIndex];
            }

            if (owner != null && owner.type == ModContent.NPCType<TopHatCatBoss>())
            {
                float healthPercent = (float)owner.life / owner.lifeMax;

                if (healthPercent > 0.66f)
                {
                    phaseMultiplier = 1.0f;
                }
                else if (healthPercent > 0.33f)
                {
                    phaseMultiplier = 1.5f;
                }
                else
                {
                    phaseMultiplier = 2.2f;
                }

                Projectile.Center = owner.Center;
                fixedCenter = owner.Center;
            }
            else if (timer == 0)
            {
                fixedCenter = Projectile.Center;
            }
            else
            {
                Projectile.Center = fixedCenter;
            }

            Projectile.scale = baseScale * phaseMultiplier;

            if (timer < 40)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(bruh(timer / 2) / 2f);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                if (timer == 20)
                {
                    float shakeIntensity = 45f * phaseMultiplier;
                    Vector2 shakeCenter = owner?.Center ?? fixedCenter;
                    ModContent.GetInstance<MCameraModifiers>().Shake(shakeCenter, shakeIntensity, 30);
                }

                if (timer > 20)
                {
                    Vector2 effectCenter = owner?.Center ?? fixedCenter;

                    if (!Filters.Scene["Shockwave"].IsActive())
                    {
                        float shockwaveRed = 3f * phaseMultiplier;
                        float shockwaveGreen = 10f * phaseMultiplier;
                        float shockwaveBlue = 30f * phaseMultiplier;

                        Filters.Scene["Shockwave"].GetShader()
                            .UseColor(shockwaveRed, shockwaveGreen, shockwaveBlue)
                            .UseTargetPosition(effectCenter);

                        Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader()
                            .UseColor(shockwaveRed, shockwaveGreen, shockwaveBlue)
                            .UseTargetPosition(Projectile.Center);
                    }
                    else
                    {
                        float progress = (timer - 20) / 20f;
                        float opacity = 100 * (1 - progress / 3f) * phaseMultiplier;

                        Filters.Scene["Shockwave"].GetShader()
                            .UseProgress(progress)
                            .UseOpacity(opacity)
                            .UseTargetPosition(Projectile.Center);
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
            float scaledReach = Projectile.width * 1.4142135624f * Projectile.scale;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                Projectile.Center, Projectile.Center + Vector2.Normalize(Projectile.velocity) * scaledReach);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition + Vector2.Normalize(Projectile.velocity) * (20 * phaseMultiplier);
            Texture2D tex = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/Sword").Value;
            float scale2 = Projectile.scale;

            if (timer > 35)
            {
                float fadeScale = Math.Max(scale2 - (timer - 35) / 6, 0);
                Projectile.scale = fadeScale;
            }

            Main.spriteBatch.Draw(tex, drawPos, tex.source(), lightColor,
                Projectile.velocity.ToRotation() + MathHelper.PiOver4,
                new Vector2(2, 72), Projectile.scale, SpriteEffects.None, 0);

            if (timer > 20)
            {
                Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
                float scale = (float)Math.Exp((timer - 20) / 5) * phaseMultiplier;
                Texture2D tex2 = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/GlowRing").Value;
            }

            return false;
        }

        private float bruh(float i)
        {
            return (float)(1 / (1 + Math.Pow((i - 10) / 2, 2))) + 0.0395f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int debuffDuration = (int)(240 * phaseMultiplier);

            if (!target.HasBuff(ModContent.BuffType<Consumed>()))
                target.AddBuff(ModContent.BuffType<Consumed>(), debuffDuration);
            else
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Consumed>())] = debuffDuration;
        }
    }
}