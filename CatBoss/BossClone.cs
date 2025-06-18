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
using static Humanizer.In;

namespace PurringTale.CatBoss
{
    public class BossClone : ModProjectile
    {
        public override string Texture => "PurringTale/CatBoss/TopHatShadow";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 54;
            Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0;
            Projectile.damage = 0;
            Projectile.timeLeft = 600;
        }

        private ref float timer => ref Projectile.ai[0];
        private int[] laserIndex = new int[4];
        public int attackStyle;
        public Vector2 centerPoint;
        public Vector2 aDirection;
        public int entranceDelay;
        private Vector2 off;
        private bool hasStartedAttack = false;
        private Player targetPlayer;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(aDirection);
            writer.Write(entranceDelay);
            writer.Write(attackStyle);
            writer.WriteVector2(centerPoint);
            writer.WriteVector2(off);
            writer.Write(hasStartedAttack);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            aDirection = reader.ReadVector2();
            entranceDelay = reader.ReadInt32();
            attackStyle = reader.ReadInt32();
            centerPoint = reader.ReadVector2();
            off = reader.ReadVector2();
            hasStartedAttack = reader.ReadBoolean();
        }

        public override void OnSpawn(IEntitySource source)
        {
            attackStyle = (int)Projectile.ai[0];
            targetPlayer = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            centerPoint = targetPlayer.Center;
            entranceDelay = 30;
        }

        public override bool PreAI()
        {
            if (--entranceDelay <= 0)
            {
                return true;
            }
            else
            {
                Projectile.Opacity = Math.Min(1f, (30 - entranceDelay) / 15f);
            }
            return false;
        }

        public override void AI()
        {
            Projectile.Opacity = Math.Min(1f, Projectile.Opacity + 0.05f);
            timer++;

            targetPlayer = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            if (targetPlayer == null || !targetPlayer.active || targetPlayer.dead)
            {
                Projectile.Kill();
                return;
            }

            centerPoint = targetPlayer.Center;
            Projectile.alpha = (int)Math.Clamp(Projectile.alpha - timer * 3, 0, 255);
            Projectile.frame = 0;

            ExecuteAttackPattern();
        }

        private void ExecuteAttackPattern()
        {
            switch (attackStyle)
            {
                case 0:
                    GunAttackPattern();
                    break;
                case 1:
                    LaserAttackPattern();
                    break;
                case 2:
                    StarAttackPattern();
                    break;
                default:
                    GunAttackPattern();
                    break;
            }

            Projectile.spriteDirection = Projectile.Center.DirectionTo(centerPoint).X > 0 ? 1 : -1;
        }

        private void GunAttackPattern()
        {
            if (timer < 60)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.05f) * 200f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 8f;
            }
            else if (timer >= 60 && timer < 120)
            {
                Projectile.velocity *= 0.95f;

                if (timer % 20 == 0)
                {
                    Vector2 shootDirection = Projectile.DirectionTo(centerPoint);
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vel = shootDirection.RotatedBy(MathHelper.Lerp(-0.3f, 0.3f, i / 2f)) * 12f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel,
                            ModContent.ProjectileType<BossBullet>(), 100, 3f);
                    }
                    SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                }
            }
            else if (timer >= 120 && timer < 180)
            {
                Vector2 direction = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = direction * 15f;
            }
            else if (timer >= 180)
            {
                Projectile.velocity *= 1.02f;
                if (timer >= 240)
                {
                    Projectile.Kill();
                }
            }
        }

        private void LaserAttackPattern()
        {
            if (timer < 30)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.1f) * 250f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 10f;
            }
            else if (timer >= 30 && timer < 40)
            {
                Projectile.velocity *= 0.8f;
            }
            else if (timer == 40)
            {
                Vector2 laserDirection = Projectile.DirectionTo(centerPoint);
                laserIndex[0] = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center,
                    laserDirection, ModContent.ProjectileType<BossLaser>(), 120, 5, -1, 0,
                    Projectile.whoAmI + Main.maxNPCs, 20);
                Main.projectile[laserIndex[0]].timeLeft = 120;
                SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
            }
            else if (timer >= 40 && timer < 160)
            {
                Projectile.velocity = Vector2.Zero;
            }
            else if (timer == 160)
            {
                if (laserIndex[0] >= 0 && laserIndex[0] < Main.maxProjectiles && Main.projectile[laserIndex[0]].active)
                {
                    Main.projectile[laserIndex[0]].Kill();
                }
            }
            else if (timer > 160)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 20f;
                if (timer >= 220)
                {
                    Projectile.Kill();
                }
            }
        }

        private void StarAttackPattern()
        {
            if (timer < 45)
            {
                Vector2 orbitCenter = centerPoint;
                float orbitRadius = 180f + (timer * 2f);
                float orbitAngle = timer * 0.15f;
                Vector2 targetPos = orbitCenter + Vector2.One.RotatedBy(orbitAngle) * orbitRadius;

                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 12f;
            }
            else if (timer >= 45 && timer < 120)
            {
                Projectile.velocity *= 0.98f;

                if (timer % 15 == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = MathHelper.TwoPi / 4 * i + (timer * 0.1f);
                        Vector2 starVel = Vector2.One.RotatedBy(angle) * 8f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, starVel,
                            ModContent.ProjectileType<RegularStar>(), 90, 3f);
                    }
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            else if (timer >= 120 && timer < 140)
            {
                Vector2 chargeDirection = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = chargeDirection * 25f;
            }
            else if (timer >= 140)
            {
                Projectile.velocity *= 1.01f;
                if (timer >= 200)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float tl = (float)Projectile.oldPos.Length;
            Main.instance.LoadProjectile(Type);
            Texture2D t = TextureAssets.Projectile[Type].Value;
            Rectangle source = new Rectangle(0, 0, t.Width, (int)(36));
            SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (float i = 0; i < tl; i += (float)(tl / 3))
            {
                float percent = i / tl;
                Vector2 dpos = Projectile.oldPos[(int)i] - Main.screenPosition + t.center() * Projectile.scale - Vector2.UnitY * 12;
                Main.spriteBatch.Draw(t, dpos, source, Color.Purple * (1 - percent) * Projectile.Opacity, Projectile.rotation, t.center(), Projectile.scale, spriteEffects, 0);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 15 * i) * Main.rand.NextFloat(3f, 8f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, vel);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1f, 1.5f);
                dust.color = Color.Purple;
            }
            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.position);
        }
    }
}