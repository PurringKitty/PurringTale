using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent;
using Terraria.Audio;

namespace PurringTale.CatBoss
{
    public class HomingStar : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FallingStar}";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 240;
            Projectile.light = 0.9f;
            Projectile.scale = 1.1f;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        private ref float timer => ref Projectile.ai[0];
        private ref float phase => ref Projectile.ai[1];
        private ref float orbitRadius => ref Projectile.ai[2];

        private Vector2 orbitCenter;
        private float orbitAngle;
        private bool hasFoundTarget = false;

        public override void OnSpawn(IEntitySource source)
        {
            NPC boss = null;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<TopHatCatBoss>())
                {
                    boss = Main.npc[i];
                    break;
                }
            }

            if (boss != null)
            {
                orbitCenter = boss.Center;
                orbitRadius = Main.rand.NextFloat(80f, 140f);
                orbitAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
            }
            else
            {
                orbitCenter = Projectile.Center;
                orbitRadius = 120f;
            }
        }

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.WriteVector2(orbitCenter);
            writer.Write(orbitAngle);
            writer.Write(hasFoundTarget);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            orbitCenter = reader.ReadVector2();
            orbitAngle = reader.ReadSingle();
            hasFoundTarget = reader.ReadBoolean();
        }

        public override void AI()
        {
            timer++;

            if (Projectile.alpha > 0)
                Projectile.alpha = Math.Max(0, Projectile.alpha - 8);

            switch (phase)
            {
                case 0:
                    if (timer < 80)
                    {
                        orbitAngle += 0.04f;
                        Vector2 orbitPosition = orbitCenter + Vector2.One.RotatedBy(orbitAngle) * orbitRadius;

                        Vector2 direction = orbitPosition - Projectile.Center;
                        Projectile.velocity = direction * 0.08f;
                    }
                    else
                    {
                        phase = 1;
                        timer = 0;
                    }
                    break;

                case 1:
                    Player target = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                    if (target != null && target.active && !target.dead && timer < 20)
                    {
                        Vector2 targetDirection = Projectile.DirectionTo(target.Center);
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetDirection * 1.0f, 0.06f);

                        Projectile.scale = 1.1f + (float)Math.Sin(timer * 0.6f) * 0.15f;
                    }
                    else
                    {
                        phase = 2;
                        timer = 0;
                        Projectile.scale = 1.1f;
                        SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                    }
                    break;

                case 2:
                    Player homingTarget = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                    if (homingTarget != null && homingTarget.active && !homingTarget.dead)
                    {
                        float homingStrength = 0.025f + (timer * 0.0003f);
                        float maxSpeed = 6f + (timer * 0.015f);

                        Vector2 direction = Projectile.DirectionTo(homingTarget.Center);
                        Vector2 desiredVelocity = direction * maxSpeed;

                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, homingStrength);

                        if (Projectile.velocity.Length() > maxSpeed)
                            Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
                    }
                    break;
            }

            Projectile.rotation += 0.25f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.scale = Main.rand.NextFloat(0.8f, 1.4f);
                dust.color = Color.Purple;
            }

            if (Main.rand.NextBool(8))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 0.6f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!target.HasBuff(ModContent.BuffType<Consumed>()))
                target.AddBuff(ModContent.BuffType<Consumed>(), 240);
            else
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Consumed>())] = 240;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float alpha = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length * 0.6f;
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;

                Color trailColor = Color.Purple * alpha * (1f - Projectile.alpha / 255f);

                Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, drawPos, null, trailColor,
                    Projectile.oldRot[i], TextureAssets.Projectile[Type].Value.Size() / 2f,
                    Projectile.scale * (1f - i * 0.03f), SpriteEffects.None, 0f);
            }

            Vector2 mainDrawPos = Projectile.Center - Main.screenPosition;
            Color mainColor = Color.Lerp(lightColor, Color.Purple, 0.7f) * (1f - Projectile.alpha / 255f);

            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, mainDrawPos, null, mainColor,
                Projectile.rotation, TextureAssets.Projectile[Type].Value.Size() / 2f,
                Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 12; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 12 * i) * 4f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, vel);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.color = Color.Purple;
            }

            for (int i = 0; i < 6; i++)
            {
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 6 * i) * 2f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleCrystalShard, vel);
                dust.noGravity = true;
                dust.scale = 1.0f;
            }
        }
    }
}