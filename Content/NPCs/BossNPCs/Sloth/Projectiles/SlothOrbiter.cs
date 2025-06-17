using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace PurringTale.Content.NPCs.BossNPCs.Sloth.Projectiles
{
    public class SlothOrbiter : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.NebulaBlaze2;

        private int frameCounter = 0;
        private int currentFrame = 0;
        private const int totalFrames = 4;
        private const int frameSpeed = 6;

        private ref float BossNPCIndex => ref Projectile.ai[0];
        private ref float OrbitRadius => ref Projectile.ai[1];
        private ref float OrbitAngle => ref Projectile.ai[2];

        private float baseOrbitRadius = 130f;
        private float orbitSpeed = 0.03f;
        private int shootTimer = 0;
        private int shootInterval = 240;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.alpha = 0;
            Projectile.light = 0.6f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.0f;
        }

        public override void AI()
        {
            frameCounter++;
            if (frameCounter >= frameSpeed)
            {
                frameCounter = 0;
                currentFrame++;
                if (currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }
            }

            NPC boss = null;
            if (BossNPCIndex >= 0 && BossNPCIndex < Main.maxNPCs)
            {
                boss = Main.npc[(int)BossNPCIndex];
                if (!boss.active)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                Projectile.Kill();
                return;
            }

            if (OrbitRadius == 0)
            {
                OrbitRadius = baseOrbitRadius;
                OrbitAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
            }

            OrbitAngle += orbitSpeed;
            if (OrbitAngle > MathHelper.TwoPi)
                OrbitAngle -= MathHelper.TwoPi;

            Vector2 orbitPosition = boss.Center + Vector2.UnitX.RotatedBy(OrbitAngle) * OrbitRadius;

            Vector2 moveDirection = orbitPosition - Projectile.Center;
            float moveDistance = moveDirection.Length();

            if (moveDistance > 5f)
            {
                moveDirection.Normalize();
                Projectile.velocity = moveDirection * Math.Min(moveDistance * 0.1f, 8f);
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }

            Projectile.rotation += 0.1f;

            shootTimer++;

            if (shootTimer >= shootInterval)
            {
                shootTimer = 0;
                ShootNebulaBlaze();
            }

            float radiusPulse = (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 10f;
            OrbitRadius = baseOrbitRadius + radiusPulse;

            if (Main.rand.NextBool(8))
            {
                Dust magicDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror);
                magicDust.velocity = Vector2.Zero;
                magicDust.scale = 0.8f;
                magicDust.noGravity = true;
                magicDust.color = Color.Purple;
            }
        }

        private void ShootNebulaBlaze()
        {
            Player targetPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead)
                {
                    float distance = Vector2.Distance(Projectile.Center, player.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetPlayer = player;
                    }
                }
            }

            if (targetPlayer != null && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shootDirection = Vector2.Normalize(targetPlayer.Center - Projectile.Center);

                shootDirection = shootDirection.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));

                float shootSpeed = 8f;
                Vector2 shootVelocity = shootDirection * shootSpeed;

                int nebulaBlast = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    shootVelocity,
                    ModContent.ProjectileType<SlothNebulaBlast>(),
                    45,
                    3f
                );

                SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);

                for (int i = 0; i < 6; i++)
                {
                    Dust muzzleFlash = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.RainbowMk2);
                    muzzleFlash.velocity = shootDirection.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * Main.rand.NextFloat(3f, 6f);
                    muzzleFlash.scale = Main.rand.NextFloat(1.0f, 1.5f);
                    muzzleFlash.noGravity = true;
                    muzzleFlash.color = Color.Lerp(Color.Purple, Color.Pink, Main.rand.NextFloat());
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;

            int frameHeight = texture.Height / totalFrames;
            Rectangle sourceRect = new Rectangle(0, currentFrame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = sourceRect.Size() / 2f;

            float alphaMultiplier = (255f - Projectile.alpha) / 255f;

            Color mainColor = Color.Lerp(Color.Purple, Color.Pink, (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.5f + 0.5f) * alphaMultiplier;

            Main.EntitySpriteDraw(texture, drawPosition, sourceRect, mainColor,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust contactDust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.MagicMirror);
                contactDust.velocity = Main.rand.NextVector2Circular(5f, 5f);
                contactDust.noGravity = true;
                contactDust.scale = 1.2f;
                contactDust.color = Color.Purple;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item25, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust deathDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror);
                deathDust.velocity = Main.rand.NextVector2Circular(8f, 8f);
                deathDust.noGravity = true;
                deathDust.scale = Main.rand.NextFloat(1.0f, 1.8f);
                deathDust.color = Color.Lerp(Color.Purple, Color.Cyan, Main.rand.NextFloat());
            }
        }
    }
}