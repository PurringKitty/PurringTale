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
        private int[] laserProjectileIds = new int[6];
        public int attackStyle;
        public Vector2 centerPoint;
        public Vector2 aDirection;
        public int entranceDelay;
        private Vector2 off;
        private bool hasStartedAttack = false;
        private Player targetPlayer;
        private int phase = 1;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(aDirection);
            writer.Write(entranceDelay);
            writer.Write(attackStyle);
            writer.WriteVector2(centerPoint);
            writer.WriteVector2(off);
            writer.Write(hasStartedAttack);
            writer.Write(phase);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            aDirection = reader.ReadVector2();
            entranceDelay = reader.ReadInt32();
            attackStyle = reader.ReadInt32();
            centerPoint = reader.ReadVector2();
            off = reader.ReadVector2();
            hasStartedAttack = reader.ReadBoolean();
            phase = reader.ReadInt32();
        }

        public override void OnSpawn(IEntitySource source)
        {
            attackStyle = (int)Projectile.ai[0];
            targetPlayer = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            centerPoint = targetPlayer.Center;
            entranceDelay = 30;

            for (int i = 0; i < laserProjectileIds.Length; i++)
            {
                laserProjectileIds[i] = -1;
            }
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

        public void SetPhase(int currentPhase)
        {
            phase = currentPhase;
        }

        private void ExecuteAttackPattern()
        {
            switch (attackStyle)
            {
                case 0:
                    if (phase == 1) BasicGunAttack();
                    else if (phase == 2) AdvancedGunAttack();
                    else RapidFireGunAttack();
                    break;
                case 1:
                    if (phase == 1) SingleLaserAttack();
                    else if (phase == 2) DoubleLaserAttack();
                    else SpinningLaserAttack();
                    break;
                case 2:
                    if (phase == 1) BasicStarAttack();
                    else if (phase == 2) HomingStarAttack();
                    else StarBarrageAttack();
                    break;
                case 3:
                    if (phase == 1) BasicRocketAttack();
                    else if (phase == 2) ClusterRocketAttack();
                    else HomingRocketAttack();
                    break;
                case 4:
                    if (phase == 1) BasicWhipAttack();
                    else if (phase == 2) DoubleWhipAttack();
                    else ChainWhipAttack();
                    break;
                case 5:
                    if (phase == 1) LaserGunCombo();
                    else if (phase == 2) StarRocketCombo();
                    else ChaosAttack();
                    break;
                default:
                    BasicGunAttack();
                    break;
            }

            Projectile.spriteDirection = Projectile.Center.DirectionTo(centerPoint).X > 0 ? 1 : -1;
        }
        private void BasicGunAttack()
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
                if (timer % 25 == 0)
                {
                    Vector2 shootDirection = Projectile.DirectionTo(centerPoint);
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vel = shootDirection.RotatedBy(MathHelper.Lerp(-0.3f, 0.3f, i / 2f)) * 10f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel,
                            ModContent.ProjectileType<BossBullet>(), 80, 3f);
                    }
                    SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                }
            }
            else if (timer >= 120)
            {
                Vector2 direction = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = direction * 12f;
                if (timer >= 180) Projectile.Kill();
            }
        }

        private void AdvancedGunAttack()
        {
            if (timer < 45)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.08f) * 220f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 10f;
            }
            else if (timer >= 45 && timer < 135)
            {
                Projectile.velocity *= 0.92f;
                if (timer % 18 == 0)
                {
                    Vector2 shootDirection = Projectile.DirectionTo(centerPoint);
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 vel = shootDirection.RotatedBy(MathHelper.Lerp(-0.5f, 0.5f, i / 4f)) * 13f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel,
                            ModContent.ProjectileType<BossBullet>(), 100, 4f);
                    }
                    SoundEngine.PlaySound(SoundID.Item40, Projectile.position);
                }
            }
            else if (timer >= 135)
            {
                Vector2 direction = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = direction * 16f;
                if (timer >= 195) Projectile.Kill();
            }
        }

        private void RapidFireGunAttack()
        {
            if (timer < 40)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.1f) * 250f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 12f;
            }
            else if (timer >= 40 && timer < 140)
            {
                Projectile.velocity *= 0.88f;
                if (timer % 8 == 0)
                {
                    Vector2 shootDirection = Projectile.DirectionTo(centerPoint);
                    Vector2 randomSpread = shootDirection.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
                    Vector2 vel = randomSpread * Main.rand.NextFloat(12f, 18f);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel,
                        ModContent.ProjectileType<BossBullet>(), 120, 5f);

                    if (timer % 16 == 0) SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                }
            }
            else if (timer >= 140)
            {
                Vector2 direction = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = direction * 20f;
                if (timer >= 200) Projectile.Kill();
            }
        }
        private void SingleLaserAttack()
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
                laserProjectileIds[0] = Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    laserDirection * 1f,
                    ModContent.ProjectileType<BossLaser>(),
                    100,
                    5f,
                    -1,
                    0f,
                    -1f,
                    25f
                );

                if (laserProjectileIds[0] >= 0 && laserProjectileIds[0] < Main.maxProjectiles)
                {
                    var laser = Main.projectile[laserProjectileIds[0]];
                    laser.timeLeft = 180;
                    laser.hostile = true;
                    laser.tileCollide = false;

                    laser.localAI[0] = Projectile.Center.X;
                    laser.localAI[1] = Projectile.Center.Y;
                }
                SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
            }
            else if (timer >= 40 && timer < 180)
            {
                Projectile.velocity = Vector2.Zero;

                int laserId = laserProjectileIds[0];
                if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                {
                    Main.projectile[laserId].localAI[0] = Projectile.Center.X;
                    Main.projectile[laserId].localAI[1] = Projectile.Center.Y;
                }
            }
            else if (timer >= 180)
            {
                CleanupLasers();
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 15f;
                if (timer >= 240) Projectile.Kill();
            }
        }

        private void DoubleLaserAttack()
        {
            if (timer < 35)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.12f) * 280f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 11f;
            }
            else if (timer >= 35 && timer < 45)
            {
                Projectile.velocity *= 0.75f;
            }
            else if (timer == 45)
            {
                Vector2 baseDirection = Projectile.DirectionTo(centerPoint);

                for (int i = 0; i < 2; i++)
                {
                    Vector2 laserDirection = baseDirection.RotatedBy(MathHelper.Lerp(-0.3f, 0.3f, i));
                    laserProjectileIds[i] = Projectile.NewProjectile(
                        Projectile.GetSource_FromAI(),
                        Projectile.Center,
                        laserDirection * 1f,
                        ModContent.ProjectileType<BossLaser>(),
                        110,
                        6f,
                        -1,
                        0f,
                        -1f,
                        25f
                    );

                    if (laserProjectileIds[i] >= 0 && laserProjectileIds[i] < Main.maxProjectiles)
                    {
                        var laser = Main.projectile[laserProjectileIds[i]];
                        laser.timeLeft = 200;
                        laser.hostile = true;
                        laser.tileCollide = false;

                        laser.localAI[0] = Projectile.Center.X;
                        laser.localAI[1] = Projectile.Center.Y;
                    }
                }
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
            else if (timer >= 45 && timer < 220)
            {
                Projectile.velocity = Vector2.Zero;

                for (int i = 0; i < 2; i++)
                {
                    int laserId = laserProjectileIds[i];
                    if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                    {
                        Main.projectile[laserId].localAI[0] = Projectile.Center.X;
                        Main.projectile[laserId].localAI[1] = Projectile.Center.Y;
                    }
                }
            }
            else if (timer >= 220)
            {
                CleanupLasers();
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 18f;
                if (timer >= 280) Projectile.Kill();
            }
        }

        private void SpinningLaserAttack()
        {
            if (timer < 30)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.15f) * 300f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 13f;
            }
            else if (timer >= 30 && timer < 40)
            {
                Projectile.velocity *= 0.7f;
            }
            else if (timer == 40)
            {
                float startAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);

                for (int i = 0; i < 4; i++)
                {
                    float angle = startAngle + (MathHelper.TwoPi / 4) * i;
                    Vector2 laserDirection = Vector2.One.RotatedBy(angle);
                    laserProjectileIds[i] = Projectile.NewProjectile(
                        Projectile.GetSource_FromAI(),
                        Projectile.Center,
                        laserDirection * 1f,
                        ModContent.ProjectileType<BossLaser>(),
                        130,
                        7f,
                        -1,
                        0f,
                        -1f,
                        25f
                    );

                    if (laserProjectileIds[i] >= 0 && laserProjectileIds[i] < Main.maxProjectiles)
                    {
                        var laser = Main.projectile[laserProjectileIds[i]];
                        laser.timeLeft = 240;
                        laser.hostile = true;
                        laser.tileCollide = false;

                        laser.localAI[0] = Projectile.Center.X;
                        laser.localAI[1] = Projectile.Center.Y;
                    }
                }
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
            else if (timer > 40 && timer < 260)
            {
                Projectile.velocity = Vector2.Zero;

                for (int i = 0; i < 4; i++)
                {
                    int laserId = laserProjectileIds[i];
                    if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                    {
                        Main.projectile[laserId].localAI[0] = Projectile.Center.X;
                        Main.projectile[laserId].localAI[1] = Projectile.Center.Y;

                        if (timer > 80 && Main.projectile[laserId].ai[0] > 30)
                        {
                            Main.projectile[laserId].velocity = Main.projectile[laserId].velocity.RotatedBy(0.03f);
                        }
                    }
                }
            }
            else if (timer >= 260)
            {
                CleanupLasers();
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 22f;
                if (timer >= 320) Projectile.Kill();
            }
        }

        private void UpdateLaserPosition(int laserIndex)
        {
            int laserId = laserProjectileIds[laserIndex];
            if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
            {
                Vector2 offset = Main.projectile[laserId].velocity.SafeNormalize(Vector2.UnitX) * 20f;
                Main.projectile[laserId].Center = Projectile.Center + offset;
            }
        }

        private void BasicStarAttack()
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
                if (timer % 20 == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = MathHelper.TwoPi / 4 * i + (timer * 0.1f);
                        Vector2 starVel = Vector2.One.RotatedBy(angle) * 7f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, starVel,
                            ModContent.ProjectileType<RegularStar>(), 70, 3f);
                    }
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            else if (timer >= 120)
            {
                Vector2 chargeDirection = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = chargeDirection * 20f;
                if (timer >= 180) Projectile.Kill();
            }
        }

        private void HomingStarAttack()
        {
            if (timer < 40)
            {
                Vector2 orbitCenter = centerPoint;
                float orbitRadius = 200f + (timer * 3f);
                float orbitAngle = timer * 0.18f;
                Vector2 targetPos = orbitCenter + Vector2.One.RotatedBy(orbitAngle) * orbitRadius;

                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 14f;
            }
            else if (timer >= 40 && timer < 130)
            {
                Projectile.velocity *= 0.96f;
                if (timer % 15 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float angle = MathHelper.TwoPi / 3 * i + (timer * 0.12f);
                        Vector2 starVel = Vector2.One.RotatedBy(angle) * 5f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, starVel,
                            ModContent.ProjectileType<HomingStar>(), 85, 4f);
                    }
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            else if (timer >= 130)
            {
                Vector2 chargeDirection = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = chargeDirection * 25f;
                if (timer >= 190) Projectile.Kill();
            }
        }

        private void StarBarrageAttack()
        {
            if (timer < 35)
            {
                Vector2 orbitCenter = centerPoint;
                float orbitRadius = 220f + (timer * 4f);
                float orbitAngle = timer * 0.2f;
                Vector2 targetPos = orbitCenter + Vector2.One.RotatedBy(orbitAngle) * orbitRadius;

                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 16f;
            }
            else if (timer >= 35 && timer < 140)
            {
                Projectile.velocity *= 0.94f;
                if (timer % 8 == 0)
                {
                    int starCount = Main.rand.Next(2, 5);
                    for (int i = 0; i < starCount; i++)
                    {
                        float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                        Vector2 starVel = Vector2.One.RotatedBy(angle) * Main.rand.NextFloat(6f, 12f);

                        int starType = Main.rand.NextBool() ? ModContent.ProjectileType<RegularStar>() : ModContent.ProjectileType<HomingStar>();
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, starVel,
                            starType, 100, 5f);
                    }
                    if (timer % 16 == 0) SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            else if (timer >= 140)
            {
                Vector2 chargeDirection = Projectile.DirectionTo(centerPoint);
                Projectile.velocity = chargeDirection * 30f;
                if (timer >= 200) Projectile.Kill();
            }
        }
        private void BasicRocketAttack()
        {
            if (timer < 50)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.08f) * 240f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 9f;
            }
            else if (timer >= 50 && timer < 80)
            {
                Projectile.velocity *= 0.9f;

                if (timer % 8 == 0)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }
            else if (timer == 80)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 direction = Projectile.DirectionTo(centerPoint).RotatedBy(MathHelper.Lerp(-0.4f, 0.4f, i / 2f));
                    Vector2 velocity = direction * 8f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity,
                        ModContent.ProjectileType<BossRocket>(), 90, 5f);
                }
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            }
            else if (timer >= 80)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 18f;
                if (timer >= 140) Projectile.Kill();
            }
        }

        private void ClusterRocketAttack()
        {
            if (timer < 45)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.1f) * 260f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 11f;
            }
            else if (timer >= 45 && timer < 90)
            {
                Projectile.velocity *= 0.85f;

                if (timer % 6 == 0)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(20, 20), DustID.Torch);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                    dust.scale = 2f;
                }
            }
            else if (timer == 90)
            {
                for (int i = 0; i < 6; i++)
                {
                    float angle = MathHelper.TwoPi / 6 * i;
                    Vector2 velocity = Vector2.One.RotatedBy(angle) * 9f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity,
                        ModContent.ProjectileType<BossRocket>(), 105, 6f);
                }
                SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
            }
            else if (timer >= 90)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 22f;
                if (timer >= 150) Projectile.Kill();
            }
        }

        private void HomingRocketAttack()
        {
            if (timer < 40)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.12f) * 280f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 13f;
            }
            else if (timer >= 40 && timer < 100)
            {
                Projectile.velocity *= 0.8f;

                if (timer % 4 == 0)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(30, 30), DustID.Torch);
                    dust.velocity = Main.rand.NextVector2Circular(2, 2);
                    dust.noGravity = true;
                    dust.scale = 2.5f;
                    dust.color = Color.Red;
                }
            }
            else if (timer >= 100 && timer <= 120)
            {
                if (timer % 5 == 0)
                {
                    Vector2 direction = Projectile.DirectionTo(centerPoint).RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
                    Vector2 velocity = direction * Main.rand.NextFloat(7f, 11f);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity,
                        ModContent.ProjectileType<BossRocket>(), 120, 7f);

                    if (timer % 10 == 0) SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                }
            }
            else if (timer >= 120)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 25f;
                if (timer >= 180) Projectile.Kill();
            }
        }
        private void BasicWhipAttack()
        {
            if (timer < 60)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.06f) * 200f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 8f;
            }
            else if (timer == 60)
            {
                Vector2 whipDirection = Projectile.DirectionTo(centerPoint);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, whipDirection,
                    ModContent.ProjectileType<WhipExtension>(), 80, 4f, -1,
                    Projectile.whoAmI + Main.maxNPCs, targetPlayer.whoAmI, 0);
                SoundEngine.PlaySound(SoundID.Item152, Projectile.position);
            }
            else if (timer >= 60 && timer < 180)
            {
                Projectile.velocity *= 0.95f;
            }
            else if (timer >= 180)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 15f;
                if (timer >= 240) Projectile.Kill();
            }
        }

        private void DoubleWhipAttack()
        {
            if (timer < 50)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.08f) * 220f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 10f;
            }
            else if (timer == 50 || timer == 80)
            {
                Vector2 whipDirection = Projectile.DirectionTo(centerPoint);
                if (timer == 80) whipDirection = whipDirection.RotatedBy(0.5f);

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, whipDirection,
                    ModContent.ProjectileType<WhipExtension>(), 95, 5f, -1,
                    Projectile.whoAmI + Main.maxNPCs, targetPlayer.whoAmI, 0);
                SoundEngine.PlaySound(SoundID.Item152, Projectile.position);
            }
            else if (timer >= 50 && timer < 200)
            {
                Projectile.velocity *= 0.92f;
            }
            else if (timer >= 200)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 18f;
                if (timer >= 260) Projectile.Kill();
            }
        }

        private void ChainWhipAttack()
        {
            if (timer < 40)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.1f) * 250f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 12f;
            }
            else if (timer >= 40 && timer <= 100)
            {
                Projectile.velocity *= 0.88f;

                if (timer % 15 == 0)
                {
                    float angle = (timer - 40) * 0.15f;
                    Vector2 whipDirection = Projectile.DirectionTo(centerPoint).RotatedBy(Math.Sin(angle) * 0.8f);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, whipDirection,
                        ModContent.ProjectileType<WhipExtension>(), 110, 6f, -1,
                        Projectile.whoAmI + Main.maxNPCs, targetPlayer.whoAmI, 0);
                    SoundEngine.PlaySound(SoundID.Item152, Projectile.position);
                }
            }
            else if (timer >= 100)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 22f;
                if (timer >= 160) Projectile.Kill();
            }
        }
        private void LaserGunCombo()
        {
            if (timer < 30)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.1f) * 230f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 11f;
            }
            else if (timer == 30)
            {
                Vector2 laserDirection = Projectile.DirectionTo(centerPoint);
                laserProjectileIds[0] = Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    laserDirection * 1f,
                    ModContent.ProjectileType<BossLaser>(),
                    100,
                    5f,
                    -1,
                    0f,
                    -1f,
                    20f
                );

                if (laserProjectileIds[0] >= 0 && laserProjectileIds[0] < Main.maxProjectiles)
                {
                    var laser = Main.projectile[laserProjectileIds[0]];
                    laser.timeLeft = 150;
                    laser.hostile = true;
                    laser.tileCollide = false;

                    laser.localAI[0] = Projectile.Center.X;
                    laser.localAI[1] = Projectile.Center.Y;
                }
                SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            }
            else if (timer >= 30 && timer < 160)
            {
                Projectile.velocity *= 0.9f;

                int laserId = laserProjectileIds[0];
                if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                {
                    Main.projectile[laserId].localAI[0] = Projectile.Center.X;
                    Main.projectile[laserId].localAI[1] = Projectile.Center.Y;
                }

                if (timer % 12 == 0 && timer >= 70)
                {
                    Vector2 bulletDirection = Projectile.DirectionTo(centerPoint).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
                    Vector2 bulletVel = bulletDirection * 14f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, bulletVel,
                        ModContent.ProjectileType<BossBullet>(), 85, 4f);

                    if (timer % 24 == 0) SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                }
            }
            else if (timer >= 160)
            {
                CleanupLasers();
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 20f;
                if (timer >= 220) Projectile.Kill();
            }
        }

        private void StarRocketCombo()
        {
            if (timer < 40)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.12f) * 260f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 12f;
            }
            else if (timer >= 40 && timer < 120)
            {
                Projectile.velocity *= 0.9f;

                if (timer % 20 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float angle = MathHelper.TwoPi / 3 * i + (timer * 0.1f);
                        Vector2 starVel = Vector2.One.RotatedBy(angle) * 8f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, starVel,
                            ModContent.ProjectileType<RegularStar>(), 90, 4f);
                    }
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
                else if (timer % 35 == 0)
                {
                    Vector2 rocketDirection = Projectile.DirectionTo(centerPoint).RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
                    Vector2 rocketVel = rocketDirection * 9f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, rocketVel,
                        ModContent.ProjectileType<BossRocket>(), 100, 5f);
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                }
            }
            else if (timer >= 120)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 24f;
                if (timer >= 180) Projectile.Kill();
            }
        }

        private void ChaosAttack()
        {
            if (timer < 35)
            {
                Vector2 targetPos = centerPoint + Vector2.One.RotatedBy(timer * 0.15f) * 280f;
                Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = direction * 14f;
            }
            else if (timer >= 35 && timer < 140)
            {
                Projectile.velocity *= 0.85f;

                if (timer % 8 == 0)
                {
                    int attackType = Main.rand.Next(4);
                    switch (attackType)
                    {
                        case 0:
                            Vector2 bulletDir = Projectile.DirectionTo(centerPoint).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
                            Vector2 bulletVel = bulletDir * Main.rand.NextFloat(10f, 16f);
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, bulletVel,
                                ModContent.ProjectileType<BossBullet>(), 100, 5f);
                            break;

                        case 1:
                            Vector2 starDir = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                            Vector2 starVel = starDir * Main.rand.NextFloat(6f, 12f);
                            int starType = Main.rand.NextBool() ? ModContent.ProjectileType<RegularStar>() : ModContent.ProjectileType<HomingStar>();
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, starVel,
                                starType, 90, 4f);
                            break;

                        case 2:
                            if (timer % 16 == 0)
                            {
                                Vector2 rocketDir = Projectile.DirectionTo(centerPoint).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
                                Vector2 rocketVel = rocketDir * Main.rand.NextFloat(8f, 12f);
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, rocketVel,
                                    ModContent.ProjectileType<BossRocket>(), 110, 6f);
                            }
                            break;

                        case 3:
                            if (timer % 20 == 0)
                            {
                                Vector2 laserDir = Projectile.DirectionTo(centerPoint).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
                                int chaosLaser = Projectile.NewProjectile(
                                    Projectile.GetSource_FromAI(),
                                    Projectile.Center,
                                    laserDir * 12f,
                                    ModContent.ProjectileType<BossLaser>(),
                                    95,
                                    4f,
                                    -1,
                                    0f,
                                    -1f,
                                    10f
                                );

                                if (chaosLaser >= 0 && chaosLaser < Main.maxProjectiles)
                                {
                                    var laser = Main.projectile[chaosLaser];
                                    laser.localAI[0] = Projectile.Center.X;
                                    laser.localAI[1] = Projectile.Center.Y;
                                    laser.timeLeft = 60;
                                }
                            }
                            break;
                    }

                    if (timer % 16 == 0)
                    {
                        SoundEngine.PlaySound(Main.rand.NextBool() ? SoundID.Item11 : SoundID.Item9, Projectile.position);
                    }
                }
            }
            else if (timer >= 140)
            {
                Vector2 retreatDirection = -Projectile.DirectionTo(centerPoint);
                Projectile.velocity = retreatDirection * 28f;
                if (timer >= 200) Projectile.Kill();
            }
        }

        private void CleanupLasers()
        {
            for (int i = 0; i < laserProjectileIds.Length; i++)
            {
                int laserId = laserProjectileIds[i];
                if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                {
                    if (Main.projectile[laserId].ai[0] < 20 || Main.projectile[laserId].timeLeft < 30)
                    {
                        Main.projectile[laserId].Kill();
                    }
                    else
                    {
                        Main.projectile[laserId].timeLeft = Math.Min(Main.projectile[laserId].timeLeft, 30);
                    }
                }
                laserProjectileIds[i] = -1;
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

            Color trailColor = attackStyle switch
            {
                0 => Color.Orange,
                1 => Color.Cyan,
                2 => Color.Yellow,
                3 => Color.Red,
                4 => Color.Purple,
                5 => Color.White,
                _ => Color.Purple
            };

            for (float i = 0; i < tl; i += (float)(tl / 3))
            {
                float percent = i / tl;
                Vector2 dpos = Projectile.oldPos[(int)i] - Main.screenPosition + t.center() * Projectile.scale - Vector2.UnitY * 12;
                Main.spriteBatch.Draw(t, dpos, source, trailColor * (1 - percent) * Projectile.Opacity, Projectile.rotation, t.center(), Projectile.scale, spriteEffects, 0);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            CleanupLasers();

            for (int i = 0; i < 15; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 15 * i) * Main.rand.NextFloat(3f, 8f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, vel);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1f, 1.5f);

                dust.color = attackStyle switch
                {
                    0 => Color.Orange,
                    1 => Color.Cyan,
                    2 => Color.Yellow,
                    3 => Color.Red,
                    4 => Color.Purple,
                    5 => Color.White,
                    _ => Color.Purple
                };
            }
            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.position);
        }
    }
}