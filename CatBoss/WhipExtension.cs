using System;
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
    public class WhipExtension : ModProjectile
    {
        public override string Texture => "PurringTale/CatBoss/Assets/WhipBody";

        private ref float bossWhoAmI => ref Projectile.ai[0];
        private ref float targetPlayer => ref Projectile.ai[1];
        private ref float timer => ref Projectile.ai[2];

        private Vector2 bossHandPosition;
        private Vector2 targetPosition;
        private List<Vector2> whipCurve = new List<Vector2>();
        private float whipLength = 0f;
        private float maxLength = 400f;
        private bool hasLatched = false;
        private bool hasMissed = false;
        private Vector2 latchPosition;
        private Player latchedPlayer;

        private Texture2D whipHandleTexture;
        private Texture2D whipBodyTexture;
        private Texture2D whipTipTexture;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.3f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;

            whipHandleTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/WhipHandle").Value;
            whipBodyTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/WhipBody").Value;
            whipTipTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/WhipTip").Value;
        }

        public override void AI()
        {
            timer++;

            NPC boss = null;
            Player target = null;

            if (bossWhoAmI >= 0 && bossWhoAmI < Main.maxNPCs && Main.npc[(int)bossWhoAmI].active)
            {
                boss = Main.npc[(int)bossWhoAmI];
                Vector2 handOffset = new Vector2(boss.direction * 25, -8);
                bossHandPosition = boss.Center + handOffset;
            }

            if (targetPlayer >= 0 && targetPlayer < Main.maxPlayers && Main.player[(int)targetPlayer].active)
            {
                target = Main.player[(int)targetPlayer];
                latchedPlayer = target;
            }

            if (boss == null || target == null)
            {
                Projectile.Kill();
                return;
            }

            if (hasLatched)
            {
                targetPosition = target.Center;
            }
            else
            {
                targetPosition = target.Center;
            }

            GenerateLatchingWhipCurve();

            if (whipCurve.Count > 0)
            {
                Projectile.Center = whipCurve[whipCurve.Count - 1];
            }

            if (!hasLatched && !hasMissed && timer >= 20 && timer < 80)
            {
                CheckWhipLatching(target);
            }

            if (!hasLatched && !hasMissed && timer >= 60)
            {
                hasMissed = true;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            }

            if (hasLatched)
            {
                ApplyLatchEffects(target);
            }

            int releaseTime = hasLatched ? 240 : 120;
            if (timer >= releaseTime)
            {
                if (hasLatched)
                {
                    ReleaseLatch(target);
                }
                Projectile.Kill();
            }
        }

        private void GenerateLatchingWhipCurve()
        {
            whipCurve.Clear();

            if (!hasLatched && !hasMissed)
            {
                if (timer <= 15)
                {
                    float progress = timer / 15f;
                    whipLength = 80f + progress * 120f;
                }
                else if (timer <= 40)
                {
                    float progress = (timer - 15f) / 25f;
                    whipLength = 200f + progress * 200f;
                }
                else
                {
                    whipLength = maxLength;
                }
            }
            else if (hasMissed && !hasLatched)
            {
                float retractProgress = (timer - 60f) / 60f;
                whipLength = maxLength * (1f - retractProgress);
                whipLength = Math.Max(whipLength, 50f);
            }
            else
            {
                whipLength = Vector2.Distance(bossHandPosition, targetPosition);
            }

            Vector2 direction = Vector2.Normalize(targetPosition - bossHandPosition);
            Vector2 endPoint = bossHandPosition + direction * whipLength;

            int curveResolution = 100;
            for (int i = 0; i <= curveResolution; i++)
            {
                float t = (float)i / curveResolution;

                Vector2 straightLine = Vector2.Lerp(bossHandPosition, hasLatched ? targetPosition : endPoint, t);

                float sagAmount = hasLatched ? 10f : 30f;
                float sag = (float)Math.Sin(t * MathHelper.Pi) * sagAmount;

                Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
                Vector2 curvedPoint = straightLine + perpendicular * sag;

                whipCurve.Add(curvedPoint);
            }
        }

        private void CheckWhipLatching(Player target)
        {
            if (whipCurve.Count == 0) return;

            Vector2 tipPosition = whipCurve[whipCurve.Count - 1];
            if (Vector2.Distance(tipPosition, target.Center) < 60f)
            {
                hasLatched = true;
                latchPosition = target.Center;

                SoundEngine.PlaySound(SoundID.NPCHit1, target.position);
                SoundEngine.PlaySound(SoundID.Zombie53, target.position);

                for (int j = 0; j < 15; j++)
                {
                    Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 15 * j) * 6f;
                    Dust dust = Dust.NewDustPerfect(tipPosition, DustID.Blood, vel);
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                }

                target.AddBuff(ModContent.BuffType<Consumed>(), 300);
            }
        }

        private void ApplyLatchEffects(Player target)
        {
            Vector2 pullDirection = Vector2.Normalize(bossHandPosition - target.Center);
            float pullStrength = 2f;

            if (Vector2.Distance(target.Center, bossHandPosition) > 150f)
            {
                target.velocity += pullDirection * pullStrength;
            }

            target.velocity *= 0.85f;

            target.gravControl = false;
            target.gravControl2 = false;

            if (timer % 60 == 0)
            {
                target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByProjectile(-1, Projectile.whoAmI),
                    25, 0);
            }

            if (timer % 10 == 0)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
            }

            if (timer % 30 == 0)
            {
                ModContent.GetInstance<MCameraModifiers>().Shake(target.Center, 5f, 10);
            }
        }

        private void ReleaseLatch(Player target)
        {
            hasLatched = false;

            SoundEngine.PlaySound(SoundID.NPCDeath1, target.position);

            for (int j = 0; j < 20; j++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 20 * j) * 8f;
                Dust dust = Dust.NewDustPerfect(target.Center, DustID.Smoke, vel);
                dust.noGravity = true;
            }

            target.gravControl = true;
            target.gravControl2 = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (whipCurve.Count < 2) return false;

            float totalLength = 0f;
            for (int i = 0; i < whipCurve.Count - 1; i++)
            {
                totalLength += Vector2.Distance(whipCurve[i], whipCurve[i + 1]);
            }

            Color baseColor = hasLatched ? Color.Red : (hasMissed ? Color.Gray : lightColor);

            DrawWhipHandle(baseColor);

            DrawWhipBody(totalLength, baseColor);

            DrawWhipTip(baseColor);

            if (hasLatched)
            {
                DrawLatchEffect();
            }

            return false;
        }

        private void DrawWhipHandle(Color lightColor)
        {
            if (whipCurve.Count < 2) return;

            Vector2 handleDirection = Vector2.Normalize(whipCurve[1] - whipCurve[0]);
            float handleRotation = handleDirection.ToRotation() + MathHelper.PiOver2;

            Vector2 handleDrawPos = bossHandPosition - Main.screenPosition;

            Main.spriteBatch.Draw(whipHandleTexture, handleDrawPos, null, lightColor, handleRotation,
                new Vector2(whipHandleTexture.Width / 2f, whipHandleTexture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }

        private void DrawWhipBody(float totalLength, Color lightColor)
        {
            float segmentSpacing = whipBodyTexture.Height * 0.4f;
            int totalBodySegments = (int)(totalLength / segmentSpacing);

            float startProgress = 0.05f;
            float endProgress = 0.95f;

            int bodySegmentCount = (int)(totalBodySegments * (endProgress - startProgress));

            for (int i = 0; i < bodySegmentCount; i++)
            {
                float progress = startProgress + ((float)i / bodySegmentCount) * (endProgress - startProgress);

                Vector2 segmentPos = GetSmoothPositionAlongCurve(progress);
                Vector2 nextPos = GetSmoothPositionAlongCurve(Math.Min(progress + 0.01f, endProgress));

                Vector2 direction = Vector2.Normalize(nextPos - segmentPos);
                float rotation = direction.ToRotation() + MathHelper.PiOver2;

                Vector2 drawPos = segmentPos - Main.screenPosition;

                float scale = 1f - (progress - startProgress) * 0.15f;
                Color color = lightColor;

                if (hasLatched)
                {
                    float pulse = 1f + 0.3f * (float)Math.Sin(timer * 0.2f);
                    color *= pulse;
                }
                else if (hasMissed)
                {
                    color *= 0.6f;
                }

                Main.spriteBatch.Draw(whipBodyTexture, drawPos, null, color, rotation,
                    new Vector2(whipBodyTexture.Width / 2f, whipBodyTexture.Height / 2f), scale, SpriteEffects.None, 0f);
            }
        }

        private void DrawWhipTip(Color lightColor)
        {
            if (whipCurve.Count < 2) return;

            Vector2 tipPos = whipCurve[whipCurve.Count - 1];
            Vector2 tipDirection = Vector2.Normalize(whipCurve[whipCurve.Count - 1] - whipCurve[whipCurve.Count - 2]);
            float tipRotation = tipDirection.ToRotation() + MathHelper.PiOver2;

            Vector2 connectionOffset = -tipDirection * (whipTipTexture.Height * 0.3f);
            Vector2 tipDrawPos = tipPos + connectionOffset - Main.screenPosition;

            Color tipColor = lightColor;
            float tipScale = hasLatched ? 1.5f : 1.2f;

            if (hasLatched)
            {
                tipColor = Color.Lerp(lightColor, Color.White, 0.5f);

                for (int i = 0; i < 3; i++)
                {
                    float glowScale = tipScale + i * 0.2f;
                    Color glowColor = tipColor * (0.4f - i * 0.1f);
                    Main.spriteBatch.Draw(whipTipTexture, tipDrawPos, null, glowColor, tipRotation,
                        new Vector2(whipTipTexture.Width / 2f, whipTipTexture.Height / 2f), glowScale, SpriteEffects.None, 0f);
                }
            }
            else if (hasMissed)
            {
                tipColor *= 0.6f;
                tipScale *= 0.9f;
            }

            Main.spriteBatch.Draw(whipTipTexture, tipDrawPos, null, tipColor, tipRotation,
                new Vector2(whipTipTexture.Width / 2f, whipTipTexture.Height / 2f), tipScale, SpriteEffects.None, 0f);
        }

        private void DrawLatchEffect()
        {
            if (!hasLatched || latchedPlayer == null) return;

            Vector2 tipPos = whipCurve[whipCurve.Count - 1];
            Vector2 playerPos = latchedPlayer.Center;

            if (timer % 5 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 sparkPos = Vector2.Lerp(tipPos, playerPos, Main.rand.NextFloat());
                    Dust spark = Dust.NewDustPerfect(sparkPos, DustID.Electric, Vector2.Zero);
                    spark.noGravity = true;
                    spark.scale = 0.8f;
                }
            }
        }

        private Vector2 GetSmoothPositionAlongCurve(float progress)
        {
            if (whipCurve.Count < 2) return bossHandPosition;

            float exactIndex = progress * (whipCurve.Count - 1);
            int lowerIndex = (int)exactIndex;
            int upperIndex = Math.Min(lowerIndex + 1, whipCurve.Count - 1);

            if (lowerIndex == upperIndex)
                return whipCurve[lowerIndex];

            float localProgress = exactIndex - lowerIndex;
            return Vector2.Lerp(whipCurve[lowerIndex], whipCurve[upperIndex], localProgress);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (hasLatched && latchedPlayer != null)
            {
                ReleaseLatch(latchedPlayer);
            }

            SoundEngine.PlaySound(SoundID.Item153, Projectile.position);
        }
    }
}