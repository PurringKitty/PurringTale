using PurringTale.Common.Systems;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture.Relics;
using PurringTale.Content.Items.Placeables.Furniture.Trophies;
using PurringTale.Content.Items.Weapons.Melee;
using System.IO;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using PurringTale.Content.NPCs.BossNPCs.ZeRock.Projectiles;

namespace PurringTale.Content.NPCs.BossNPCs.ZeRock
{
    public enum RockAttackType
    {
        BoulderRain,
        SpikeSlam,
        RollingRocks,
        ShardBurst,
        GroundPound
    }

    [AutoloadBossHead]
    public class RockBoss : ModNPC
    {
        private enum ActionState
        {
            Dormant,
            Awakening,
            Jumping,
            BoulderAttack,
            SpikeAttack,
            RollingAttack,
            ShardAttack,
            PoundAttack,
            Death
        }

        private uint StateData
        {
            get => BitConverter.SingleToUInt32Bits(NPC.ai[1]);
            set => NPC.ai[1] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        private ActionState AIState
        {
            get => (ActionState)StateData;
            set => StateData = (uint)value;
        }

        private uint AttackData
        {
            get => BitConverter.SingleToUInt32Bits(NPC.ai[2]);
            set => NPC.ai[2] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        private RockAttackType AttackType
        {
            get => (RockAttackType)AttackData;
            set => AttackData = (uint)value;
        }

        private ref float timer => ref NPC.ai[0];
        private ref float jumpTimer => ref NPC.ai[3];
        private ref float attackCounterRef => ref NPC.localAI[0];
        private int AttackCounter
        {
            get => (int)attackCounterRef;
            set => attackCounterRef = value;
        }

        private ref float hasLandedRef => ref NPC.localAI[1];
        private bool HasLanded
        {
            get => hasLandedRef == 1f;
            set => hasLandedRef = value ? 1f : 0f;
        }
        private bool isAwakened
        {
            get => NPC.localAI[2] == 1f;
            set => NPC.localAI[2] = value ? 1f : 0f;
        }

        private float originalGravity = 0.4f;
        private float jumpHeight = 16f;
        private Vector2 targetPosition;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(isAwakened);
            writer.WriteVector2(targetPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bool newAwakened = reader.ReadBoolean();
            targetPosition = reader.ReadVector2();

            isAwakened = newAwakened;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Inferno] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Cursed] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.8f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 26;
            NPC.height = 18;
            NPC.damage = 120;
            NPC.defense = 0;
            NPC.lifeMax = 35000;
            NPC.HitSound = SoundID.Dig;
            NPC.DeathSound = SoundID.ScaryScream;
            NPC.knockBackResist = 0f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(platinum: 1);
            NPC.boss = true;
            NPC.npcSlots = 50f;
            NPC.SpawnWithHigherTime(30);
            NPC.dontTakeDamage = false;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/SongForARock");
            }
        }

        public override void BossHeadSlot(ref int index)
        {
            if (!isAwakened)
            {
                index = -1;
            }
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            TriggerAwakening();
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            TriggerAwakening();
        }

        private void TriggerAwakening()
        {
            if (!isAwakened && AIState == ActionState.Dormant)
            {
                AIState = ActionState.Awakening;
                isAwakened = true;
                timer = 0;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }

                if (!Main.dedServ)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 15f, 60);
                }
                SoundEngine.PlaySound(SoundID.DD2_BetsyScream, NPC.Center);

                for (int i = 0; i < 30; i++)
                {
                    Vector2 dustVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 8f);
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, dustVel.X, dustVel.Y, 100, default, 1.5f);
                }
            }
        }

        public override void AI()
        {
            if (isAwakened)
            {
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest();
                }

                Player player = Main.player[NPC.target];

                if (player.dead)
                {
                    NPC.velocity.Y -= 0.04f;
                    NPC.EncourageDespawn(10);
                    return;
                }

                NPC.spriteDirection = NPC.direction = (player.Center.X > NPC.Center.X) ? 1 : -1;
            }

            switch (AIState)
            {
                case ActionState.Dormant:
                    DormantState();
                    break;
                case ActionState.Awakening:
                    AwakeningState();
                    break;
                case ActionState.Jumping:
                    if (isAwakened) JumpingState(Main.player[NPC.target]);
                    break;
                case ActionState.BoulderAttack:
                    if (isAwakened) BoulderAttackState(Main.player[NPC.target]);
                    break;
                case ActionState.SpikeAttack:
                    if (isAwakened) SpikeAttackState(Main.player[NPC.target]);
                    break;
                case ActionState.RollingAttack:
                    if (isAwakened) RollingAttackState(Main.player[NPC.target]);
                    break;
                case ActionState.ShardAttack:
                    if (isAwakened) ShardAttackState(Main.player[NPC.target]);
                    break;
                case ActionState.PoundAttack:
                    if (isAwakened) PoundAttackState(Main.player[NPC.target]);
                    break;
            }

            timer++;
        }

        private void DormantState()
        {
            NPC.velocity = Vector2.Zero;

            if (!Collision.SolidCollision(NPC.position + new Vector2(0, 1), NPC.width, NPC.height))
            {
                NPC.velocity.Y = 0.4f;
            }
        }

        private void AwakeningState()
        {
            if (timer < 60)
            {
                Vector2 originalPos = NPC.position;
                NPC.position.X += Main.rand.NextFloat(-2f, 2f);
                NPC.position.Y += Main.rand.NextFloat(-1f, 1f);

                if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    NPC.position = originalPos;
                }

                if (timer % 10 == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 dustPos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                        Dust.NewDust(dustPos, 0, 0, DustID.Stone, 0, -2f, 100, default, 1.2f);
                    }
                }
            }
            else if (timer >= 60 && timer < 120)
            {
                NPC.velocity.Y = -2f;
                NPC.velocity.X = 0;
            }
            else if (timer >= 120)
            {
                AIState = ActionState.Jumping;
                timer = 0;
                jumpTimer = 0;
                AttackCounter = 0;
                HasLanded = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }
        }

        private void JumpingState(Player player)
        {
            bool playerBelow = player.Center.Y > NPC.Center.Y + 50f;

            if (playerBelow && NPC.velocity.Y > 0f)
            {
                NPC.noTileCollide = true;
            }
            else
            {
                NPC.noTileCollide = false;
            }

            if (NPC.velocity.Y > 0f)
            {
                HasLanded = false;
            }

            if (NPC.velocity.Y == 0f && NPC.oldVelocity.Y > 0f && !HasLanded)
            {
                HasLanded = true;
                NPC.noTileCollide = false;

                if (!Main.dedServ)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 8f, 20);
                }
                SoundEngine.PlaySound(SoundID.Item70, NPC.Center);

                for (int i = 0; i < 10; i++)
                {
                    Vector2 dustVel = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-1f, 1f));
                    Dust.NewDust(NPC.Bottom, NPC.width, 4, DustID.Stone, dustVel.X, dustVel.Y);
                }

                AttackCounter++;

                if (AttackCounter >= 3)
                {
                    jumpTimer = 0;
                }
            }

            if (NPC.velocity.Y == 0f)
            {
                jumpTimer++;

                if (AttackCounter >= 3 && jumpTimer > 30)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int attackChoice = Main.rand.Next(5);
                        switch (attackChoice)
                        {
                            case 0:
                                AttackType = RockAttackType.BoulderRain;
                                AIState = ActionState.BoulderAttack;
                                break;
                            case 1:
                                AttackType = RockAttackType.SpikeSlam;
                                AIState = ActionState.SpikeAttack;
                                break;
                            case 2:
                                AttackType = RockAttackType.RollingRocks;
                                AIState = ActionState.RollingAttack;
                                break;
                            case 3:
                                AttackType = RockAttackType.ShardBurst;
                                AIState = ActionState.ShardAttack;
                                break;
                            case 4:
                                AttackType = RockAttackType.GroundPound;
                                AIState = ActionState.PoundAttack;
                                break;
                        }
                        timer = 0;
                        AttackCounter = 0;
                        HasLanded = false;

                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                        }
                        return;
                    }
                }

                if (AttackCounter < 3 && jumpTimer > 60)
                {
                    float distanceX = player.Center.X - NPC.Center.X;
                    float distanceY = player.Center.Y - NPC.Center.Y;
                    float distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                    float jumpMultiplier = 1f;
                    bool playerAbove = player.Center.Y < NPC.Center.Y - 100f;

                    if (playerAbove)
                    {
                        float verticalDistance = NPC.Center.Y - player.Center.Y;

                        if (verticalDistance <= 200f)
                        {
                            jumpMultiplier = 1.3f;
                        }
                        else if (verticalDistance <= 300f)
                        {
                            jumpMultiplier = 1.6f;
                        }
                        else if (verticalDistance <= 400f)
                        {
                            jumpMultiplier = 1.9f;
                        }
                        else
                        {
                            jumpMultiplier = 2.2f;
                        }
                    }

                    if (distance > 100f)
                    {
                        NPC.velocity.X = distanceX / distance * 8f;
                        NPC.velocity.Y = -jumpHeight * jumpMultiplier;
                    }
                    else
                    {
                        NPC.velocity.X = -distanceX / distance * 6f;
                        NPC.velocity.Y = -jumpHeight * 0.7f * jumpMultiplier;
                    }

                    jumpTimer = 0;
                    SoundEngine.PlaySound(SoundID.Item1, NPC.Center);

                    if (jumpMultiplier > 1.5f)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with
                        {
                            Pitch = -0.6f,
                            Volume = 0.8f
                        }, NPC.Center);

                        if (!Main.dedServ)
                        {
                            ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 8f, 20);
                        }

                        for (int i = 0; i < 15; i++)
                        {
                            Vector2 dustVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, dustVel.X, dustVel.Y, 100, Color.Orange, 1.5f);
                        }
                    }
                }
            }
            else
            {
                NPC.velocity.Y += originalGravity;
            }

            NPC.velocity.X *= 0.98f;
        }

        private void BoulderAttackState(Player player)
        {
            if (timer == 1)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -25f;

                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with
                {
                    Pitch = -0.5f,
                    Volume = 1.2f
                }, NPC.Center);

                if (!Main.dedServ)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 8f, 60);
                }

                for (int i = 0; i < 20; i++)
                {
                    Vector2 dustVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, dustVel.X, dustVel.Y, 100, Color.Orange, 1.5f);
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (timer > 30 && timer < 60 && timer % 10 == 0)
                {
                    Vector2 spawnPos = player.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);
                    Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(8f, 12f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                        ModContent.ProjectileType<FallingBoulder>(), 35, 6f, -1);
                }

                if (timer > 65 && timer < 95 && timer % 15 == 0)
                {
                    Vector2 predictedPos = player.Center + player.velocity * 30f;

                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 spawnPos = predictedPos + new Vector2(i * 120f, -650f);
                        Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(9f, 13f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                            ModContent.ProjectileType<FallingBoulder>(), 35, 6f, -1);
                    }
                }

                if (timer > 100 && timer < 120 && timer % 8 == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 spawnPos = player.Center + new Vector2(Main.rand.NextFloat(-500f, 500f), -700f);
                        Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(10f, 14f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                            ModContent.ProjectileType<FallingBoulder>(), 40, 7f, -1);
                    }
                }

                if (timer > 30 && timer < 120 && timer % 30 == 0)
                {
                    Vector2 randomSpawn = player.Center + new Vector2(Main.rand.NextFloat(-600f, 600f), -800f);
                    Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(7f, 11f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position: randomSpawn, velocity,
                        ModContent.ProjectileType<FallingBoulder>(), 30, 5f, -1);
                }
            }

            if (timer > 30 && timer < 120)
            {
                if (timer % 10 == 0 && !Main.dedServ)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 5f, 15);
                }

                if (timer % 15 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item14 with
                    {
                        Pitch = Main.rand.NextFloat(-0.8f, -0.4f),
                        Volume = 0.6f
                    }, player.Center);
                }

                if (timer % 5 == 0)
                {
                    Vector2 skyPos = player.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -500f);
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(skyPos, 20, 20, DustID.Stone, 0, 2f, 100, Color.DarkGray, 0.8f);
                    }
                }
            }

            if (timer > 140)
            {
                AIState = ActionState.Jumping;
                timer = 0;
                jumpTimer = 0;
                HasLanded = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }

            NPC.velocity.Y += originalGravity;
        }

        private void SpikeAttackState(Player player)
        {
            if (timer == 1)
            {
                targetPosition = player.Center;
                Vector2 teleportPos = targetPosition + new Vector2(0, -250f);
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;

                HasLanded = false;

                for (int i = 0; i < 20; i++)
                {
                    Vector2 dustPos = targetPosition + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(50f, 100f);
                    Dust.NewDust(dustPos, 0, 0, DustID.Stone, 0, 0, 100, Color.Red, 1.5f);
                }
            }

            if (timer > 30 && timer < 60)
            {
                NPC.velocity.Y = 25f;
                NPC.velocity.X = 0;
            }

            if (timer > 30 && !HasLanded)
            {
                float distanceToTarget = Math.Abs(NPC.Center.Y - targetPosition.Y);

                if (distanceToTarget < 50f)
                {
                    HasLanded = true;

                    if (!Main.dedServ)
                    {
                        ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 25f, 40);
                    }
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = -4; i <= 4; i++)
                        {
                            Vector2 spikePos = targetPosition + new Vector2(i * 35f, 0);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spikePos, Vector2.Zero,
                                ModContent.ProjectileType<GroundSpike>(), 35, 8f, -1, 0, Math.Abs(i) * 8);
                        }
                    }
                }
            }

            if (timer > 90)
            {
                AIState = ActionState.Jumping;
                timer = 0;
                jumpTimer = 0;
                HasLanded = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }

            if (timer > 60)
                NPC.velocity.Y += originalGravity;
        }

        private void RollingAttackState(Player player)
        {
            if (timer == 1)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -15f;
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with
                {
                    Pitch = -0.3f,
                    Volume = 1.0f
                }, NPC.Center);
            }

            if (timer > 30 && timer < 90 && timer % 20 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 leftSpawn = new Vector2(player.Center.X - 600f, player.Center.Y - 100f);
                    Vector2 leftVel = new Vector2(Main.rand.NextFloat(6f, 9f), 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), leftSpawn, leftVel,
                        ModContent.ProjectileType<RollingBoulder>(), 25, 3f, -1);

                    Vector2 rightSpawn = new Vector2(player.Center.X + 600f, player.Center.Y - 100f);
                    Vector2 rightVel = new Vector2(Main.rand.NextFloat(-9f, -6f), 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), rightSpawn, rightVel,
                        ModContent.ProjectileType<RollingBoulder>(), 25, 3f, -1);
                }
            }

            if (timer > 120)
            {
                AIState = ActionState.Jumping;
                timer = 0;
                jumpTimer = 0;
                HasLanded = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }

            NPC.velocity.Y += originalGravity;
        }

        private void ShardAttackState(Player player)
        {
            if (timer == 1)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -10f;
                SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
            }

            if (timer == 45)
            {
                if (!Main.dedServ)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 15f, 30);
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float angle = (float)(i * Math.PI * 2 / 12);
                        Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(3f, 7f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<StoneShards>(), 20, 2f, -1);
                    }
                }

                for (int i = 0; i < 30; i++)
                {
                    Vector2 dustVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 10f);
                    Dust.NewDust(NPC.Center, 0, 0, DustID.Stone, dustVel.X, dustVel.Y, 100, default, 1.2f);
                }
            }

            if (timer > 80)
            {
                AIState = ActionState.Jumping;
                timer = 0;
                jumpTimer = 0;
                HasLanded = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }

            NPC.velocity.Y += originalGravity;
        }

        private void PoundAttackState(Player player)
        {
            if (timer == 1)
            {
                targetPosition = player.Center;
                Vector2 teleportPos = targetPosition + new Vector2(0, -400f);
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with
                {
                    Pitch = -0.4f,
                    Volume = 1.1f
                }, NPC.Center);
            }

            if (timer > 30 && timer < 45)
            {
                NPC.velocity.Y = 35f;
                NPC.velocity.X = 0;
            }

            if (timer == 45)
            {
                if (!Main.dedServ)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 35f, 60);
                }
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = -6; i <= 6; i++)
                    {
                        Vector2 spikePos = NPC.Center + new Vector2(i * 70f, 0);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spikePos, Vector2.Zero,
                            ModContent.ProjectileType<ShockwaveSpike>(), 45, 12f, -1, 0, Math.Abs(i) * 6);
                    }
                }

                for (int i = 0; i < 50; i++)
                {
                    Vector2 dustVel = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(5f, 15f);
                    Dust.NewDust(NPC.Center, 0, 0, DustID.Stone, dustVel.X, dustVel.Y, 100, default, 2.0f);
                }
            }

            if (timer > 100)
            {
                AIState = ActionState.Jumping;
                timer = 0;
                jumpTimer = 0;
                HasLanded = false;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }

            if (timer > 45)
                NPC.velocity.Y += originalGravity;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color finalColor = drawColor;
            float finalScale = NPC.scale;

            switch (AIState)
            {
                case ActionState.Dormant:
                    finalColor = drawColor;
                    break;

                case ActionState.Awakening:
                    float pulse = (float)Math.Sin(timer * 0.3f) * 0.3f + 0.7f;
                    finalColor = Color.Lerp(drawColor, Color.Red, 0.3f) * pulse;
                    finalScale *= 1f + (float)Math.Sin(timer * 0.2f) * 0.1f;
                    break;

                case ActionState.BoulderAttack:
                    finalColor = Color.Lerp(drawColor, Color.Orange, 0.2f);
                    break;

                case ActionState.SpikeAttack:
                    finalColor = Color.Lerp(drawColor, Color.Purple, 0.2f);
                    break;

                case ActionState.RollingAttack:
                    finalColor = Color.Lerp(drawColor, Color.Brown, 0.2f);
                    break;

                case ActionState.ShardAttack:
                    finalColor = Color.Lerp(drawColor, Color.Gray, 0.2f);
                    break;

                case ActionState.PoundAttack:
                    finalColor = Color.Lerp(drawColor, Color.DarkRed, 0.3f);
                    break;

                default:
                    if (isAwakened)
                    {
                        finalColor = Color.Lerp(drawColor, Color.White, 0.1f);
                    }
                    break;
            }

            if (isAwakened && NPC.velocity.Length() > 2f)
            {
                Main.instance.LoadNPC(Type);
                Texture2D texture = TextureAssets.Npc[Type].Value;

                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    float alpha = (float)(NPC.oldPos.Length - i) / NPC.oldPos.Length * 0.3f;
                    Vector2 drawPos = NPC.oldPos[i] - screenPos + new Vector2(NPC.width / 2, NPC.height / 2);
                    Color trailColor = Color.Brown * alpha;

                    spriteBatch.Draw(texture, drawPos, NPC.frame, trailColor, NPC.rotation,
                        new Vector2(texture.Width / 2, texture.Height / 2), finalScale * 0.9f,
                        NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                }
            }

            Main.instance.LoadNPC(Type);
            Texture2D mainTexture = TextureAssets.Npc[Type].Value;
            Vector2 mainDrawPos = NPC.Center - screenPos;

            spriteBatch.Draw(mainTexture, mainDrawPos, NPC.frame, finalColor, NPC.rotation,
                new Vector2(mainTexture.Width / 2, mainTexture.Height / 2), finalScale,
                NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("...Why The Hell Are You Fighting A Rock? - Rukuka"),
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            AIState = ActionState.Dormant;
            isAwakened = false;
            NPC.gfxOffY = 0;
            AttackCounter = 0;
            HasLanded = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDay.Chance * 0.0001f;
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedRock, -1);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RockBossTrophy>(), 6, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<RockBossRelic>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WeaponRock>(), 15, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VanityVoucher>(), 4, 1, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 10, 100));
        }
    }
}