using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture.Relics;
using PurringTale.Content.Items.Placeables.Furniture.Trophies;
using PurringTale.Content.Items.Vanity;
using PurringTale.Content.NPCs.BossNPCs.Sloth.Projectiles;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PurringTale.Content.NPCs.BossNPCs.Sloth
{
    public enum SlothAttackType
    {
        LaserBarrage,
        ProjectileSpiral,
        AreaDenial,
        SummonOrbs,
        ShotgunAttack,
        CrossLasers,
        SweepingBeams,
        LaserGrid
    }

    [AutoloadBossHead]
    public class SlothBoss : ModNPC
    {
        private enum ActionState
        {
            Spawn,
            Phase1Choose,
            Phase1Attack,
            Transition,
            Phase2
        }

        private uint StateStorage
        {
            get => BitConverter.SingleToUInt32Bits(NPC.ai[1]);
            set => NPC.ai[1] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        private ActionState AIState
        {
            get => (ActionState)StateStorage;
            set => StateStorage = (uint)value;
        }

        private uint AttackStorage
        {
            get => BitConverter.SingleToUInt32Bits(NPC.ai[2]);
            set => NPC.ai[2] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        private SlothAttackType CurrentAttack
        {
            get => (SlothAttackType)AttackStorage;
            set => AttackStorage = (uint)value;
        }

        private ref float timer => ref NPC.ai[0];
        private ref float headSpawned => ref NPC.ai[3];
        private int attackCounter = 0;
        private bool phase2 = false;
        private int detachedHeadWhoAmI = -1;
        private bool headKilled = false;
        private bool headLinked = false;
        private int sharedMaxHealth = 0;
        private int sharedCurrentHealth = 0;

        private bool holdingShotgun = false;
        private float shotgunRotation = 0f;
        private int shotgunShotCount = 0;

        private Vector2 storedTargetPosition = Vector2.Zero;
        private bool hasStoredPosition = false;

        private SlothAttackType lastPhase1Attack = SlothAttackType.LaserBarrage;
        private SlothAttackType lastPhase2Attack = SlothAttackType.CrossLasers;

        private const float PHASE2_HEALTH_RATIO = 0.5f;

        public static int headlessSlot = -1;

        private void AddScreenShakeToNearbyPlayers(float intensity, int duration = 30)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && player.Distance(NPC.Center) < 2000f)
                {
                    player.GetModPlayer<ScreenShakePlayer>().AddScreenShake(duration, intensity);
                }
            }
        }

        public override void Load()
        {
            string headlessTexture = Texture + "_Headless_Head";
            headlessSlot = Mod.AddBossHeadTexture(headlessTexture, -1);
        }

        public override void BossHeadSlot(ref int index)
        {
            if (phase2 && headlessSlot != -1)
            {
                index = headlessSlot;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
            writer.Write(phase2);
            writer.Write(detachedHeadWhoAmI);
            writer.Write(headKilled);
            writer.Write(holdingShotgun);
            writer.Write(shotgunRotation);
            writer.Write(shotgunShotCount);
            writer.Write(hasStoredPosition);
            writer.Write(storedTargetPosition.X);
            writer.Write(storedTargetPosition.Y);
            writer.Write(headLinked);
            writer.Write(sharedMaxHealth);
            writer.Write(sharedCurrentHealth);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
            phase2 = reader.ReadBoolean();
            detachedHeadWhoAmI = reader.ReadInt32();
            headKilled = reader.ReadBoolean();
            holdingShotgun = reader.ReadBoolean();
            shotgunRotation = reader.ReadSingle();
            shotgunShotCount = reader.ReadInt32();
            hasStoredPosition = reader.ReadBoolean();
            storedTargetPosition.X = reader.ReadSingle();
            storedTargetPosition.Y = reader.ReadSingle();
            headLinked = reader.ReadBoolean();
            sharedMaxHealth = reader.ReadInt32();
            sharedCurrentHealth = reader.ReadInt32();
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "PurringTale/Content/NPCs/BossNPCs/Sloth/SlothBoss_Beastiary",
                PortraitScale = 1f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 180;
            NPC.damage = 55;
            NPC.lifeMax = 18000;
            NPC.defense = 30;
            NPC.scale = 1.3f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 60);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 20f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Sloth");
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("Acedia a colossal entity of sloth, wielding devastating magical powers while remaining eternally stationary. Its mere presence warps reality around it. - Rukuka")
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            AIState = ActionState.Spawn;
            phase2 = false;
            headSpawned = 0;
            headKilled = false;
            holdingShotgun = false;
            hasStoredPosition = false;
            storedTargetPosition = Vector2.Zero;

            lastPhase1Attack = SlothAttackType.LaserBarrage;
            lastPhase2Attack = SlothAttackType.CrossLasers;

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (phase2 && detachedHeadWhoAmI >= 0 && Main.npc[detachedHeadWhoAmI].active && !headKilled)
                return false;
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (phase2 && detachedHeadWhoAmI >= 0 && Main.npc[detachedHeadWhoAmI].active && !headKilled)
                return false;
            return true;
        }

        public override void AI()
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

            if (phase2 && headLinked)
            {
                SyncSharedHealth();
            }

            if (!phase2 && NPC.life <= NPC.lifeMax * PHASE2_HEALTH_RATIO)
            {
                StartPhase2Transition();
            }

            ChooseAction();
            timer++;
        }


        private void ChooseAction()
        {
            switch (AIState)
            {
                case ActionState.Spawn:
                    SpawnBehavior();
                    break;
                case ActionState.Phase1Choose:
                    Phase1ChooseAttack();
                    break;
                case ActionState.Phase1Attack:
                    Phase1Attack();
                    break;
                case ActionState.Transition:
                    Phase2Transition();
                    break;
                case ActionState.Phase2:
                    Phase2Behavior();
                    break;
            }
        }

        private void SpawnBehavior()
        {
            NPC.velocity = Vector2.Zero;

            if (timer < 120)
            {
                NPC.dontTakeDamage = true;
                if (timer % 20 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                    AddScreenShakeToNearbyPlayers(8f, 20);
                }

                if (timer > 60)
                {
                    AddScreenShakeToNearbyPlayers(2f, 5);
                }
            }
            else if (timer >= 120)
            {
                NPC.dontTakeDamage = false;
                AddScreenShakeToNearbyPlayers(15f, 40);
                AIState = ActionState.Phase1Choose;
                timer = 0;
            }
        }

        private void Phase1ChooseAttack()
        {
            NPC.velocity = Vector2.Zero;

            if (timer >= 60)
            {
                List<SlothAttackType> phase1Attacks = new List<SlothAttackType>
                {
                    SlothAttackType.ShotgunAttack,
                    SlothAttackType.LaserBarrage,
                    SlothAttackType.ProjectileSpiral,
                    SlothAttackType.AreaDenial,
                    SlothAttackType.SummonOrbs
                };

                phase1Attacks.Remove(lastPhase1Attack);

                CurrentAttack = phase1Attacks[Main.rand.Next(phase1Attacks.Count)];
                lastPhase1Attack = CurrentAttack;

                AIState = ActionState.Phase1Attack;
                timer = 0;
            }
        }

        private void Phase1Attack()
        {
            NPC.velocity = Vector2.Zero;
            Player target = Main.player[NPC.target];

            switch (CurrentAttack)
            {
                case SlothAttackType.LaserBarrage:
                    LaserBarrageAttack(target);
                    break;
                case SlothAttackType.ProjectileSpiral:
                    ProjectileSpiralAttack(target);
                    break;
                case SlothAttackType.AreaDenial:
                    AreaDenialAttack(target);
                    break;
                case SlothAttackType.SummonOrbs:
                    SummonOrbsAttack(target);
                    break;
                case SlothAttackType.ShotgunAttack:
                    ShotgunAttack(target);
                    break;
            }
        }

        private void ShotgunAttack(Player target)
        {
            if (timer == 1)
            {
                holdingShotgun = true;
                shotgunShotCount = 0;
                SoundEngine.PlaySound(SoundID.Item149, NPC.Center);

                for (int i = 0; i < 40; i++)
                {
                    Dust materialDust = Dust.NewDustDirect(NPC.Center, 50, 50, DustID.MagicMirror);
                    materialDust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(3f, 8f);
                    materialDust.scale = 1.5f;
                    materialDust.noGravity = true;
                    materialDust.color = Color.DarkRed;
                }
                AddScreenShakeToNearbyPlayers(8f, 25);
            }

            if (holdingShotgun && timer < 300)
            {
                Vector2 directionToPlayer = target.Center - NPC.Center;
                float desiredRotation = directionToPlayer.ToRotation();

                float rotationDifference = MathHelper.WrapAngle(desiredRotation - shotgunRotation);
                shotgunRotation += rotationDifference * 0.15f;

                if (timer < 60)
                {
                    if (timer % 8 == 0)
                    {
                        Vector2 barrelPos = NPC.Center + Vector2.UnitX.RotatedBy(shotgunRotation) * 70f;
                        Dust chargeDust = Dust.NewDustDirect(barrelPos, 8, 8, DustID.Torch);
                        chargeDust.velocity = Vector2.UnitX.RotatedBy(shotgunRotation) * 2f;
                        chargeDust.scale = 1.2f;
                        chargeDust.noGravity = true;
                    }
                }

                if (timer >= 60 && timer < 270)
                {
                    int[] shotTimings = { 60, 90, 115, 140, 160, 180, 195, 210 };

                    if (shotTimings.Contains((int)timer) && shotgunShotCount < 8)
                    {
                        SoundEngine.PlaySound(SoundID.Item38, NPC.Center);
                        AddScreenShakeToNearbyPlayers(7f, 20);

                        int pelletCount = 8;
                        float spreadAngle = 0.4f;
                        float patternOffset = shotgunShotCount * 0.1f;

                        for (int i = 0; i < pelletCount; i++)
                        {
                            float pelletAngle = shotgunRotation + MathHelper.Lerp(-spreadAngle, spreadAngle, i / (float)(pelletCount - 1));
                            pelletAngle += patternOffset;
                            Vector2 pelletDirection = Vector2.UnitX.RotatedBy(pelletAngle);

                            pelletDirection = pelletDirection.RotatedBy(Main.rand.NextFloat(-0.08f, 0.08f));
                            float speed = Main.rand.NextFloat(14f, 18f);

                            Projectile.NewProjectile(NPC.GetSource_FromAI(),
                                NPC.Center + pelletDirection * 75f,
                                pelletDirection * speed,
                                ModContent.ProjectileType<SlothDagger>(),
                                48,
                                4f);
                        }

                        shotgunShotCount++;
                        CreateShotgunMuzzleFlash();
                    }
                }
            }

            if (timer >= 270 && timer < 300)
            {
                if (timer == 270)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Dust dissolveDust = Dust.NewDustDirect(NPC.Center, 40, 40, DustID.Smoke);
                        dissolveDust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                        dissolveDust.scale = 1.8f;
                        dissolveDust.alpha = 100;
                    }
                }
            }

            if (timer >= 300)
            {
                holdingShotgun = false;
                shotgunRotation = 0f;
                shotgunShotCount = 0;
                AIState = ActionState.Phase1Choose;
                timer = 0;
            }
        }

        private void CreateShotgunMuzzleFlash()
        {
            Vector2 muzzlePosition = NPC.Center + Vector2.UnitX.RotatedBy(shotgunRotation) * 75f;

            for (int i = 0; i < 30; i++)
            {
                Dust flash = Dust.NewDustDirect(muzzlePosition, 20, 20, DustID.Torch);
                Vector2 flashVelocity = Vector2.UnitX.RotatedBy(shotgunRotation + Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(8f, 16f);
                flash.velocity = flashVelocity;
                flash.scale = Main.rand.NextFloat(1.5f, 2.5f);
                flash.noGravity = true;
                flash.color = Color.Lerp(Color.Orange, Color.Yellow, Main.rand.NextFloat());
            }

            for (int i = 0; i < 15; i++)
            {
                Dust smoke = Dust.NewDustDirect(muzzlePosition, 12, 12, DustID.Smoke);
                smoke.velocity = Vector2.UnitX.RotatedBy(shotgunRotation) * Main.rand.NextFloat(3f, 8f);
                smoke.scale = Main.rand.NextFloat(1.8f, 2.5f);
                smoke.alpha = 150;
            }
        }

        private void LaserBarrageAttack(Player target)
        {
            if (timer < 60)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    AddScreenShakeToNearbyPlayers(4f, 20);
                }

                if (timer % 10 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        float angle = (MathHelper.TwoPi / 8) * i;
                        Vector2 dustPos = NPC.Center + Vector2.One.RotatedBy(angle) * (120 - timer * 1.5f);

                        Dust energyDust = Dust.NewDustDirect(dustPos, 6, 6, DustID.MagicMirror);
                        energyDust.velocity = (NPC.Center - dustPos) * 0.05f;
                        energyDust.scale = 1.2f;
                        energyDust.noGravity = true;
                        energyDust.color = Color.Lerp(Color.Purple, Color.Cyan, timer / 60f);
                    }
                }

                if (timer % 20 == 0)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = timer / 60f * 0.4f }, NPC.Center);
                    AddScreenShakeToNearbyPlayers(2f, 10);
                }

                if (timer >= 40 && timer % 6 == 0)
                {
                    Vector2 telegraphPos = target.Center + Main.rand.NextVector2Circular(20f, 20f);
                    Dust warning = Dust.NewDustDirect(telegraphPos, 8, 8, DustID.RedTorch);
                    warning.velocity = Vector2.Zero;
                    warning.scale = 1.5f;
                    warning.noGravity = true;
                    warning.alpha = 120;
                }
            }
            else if (timer >= 60 && timer < 180)
            {
                int adjustedTimer = (int)(timer - 60);

                if (adjustedTimer % 30 == 0)
                {
                    int burstNumber = adjustedTimer / 30;

                    switch (burstNumber)
                    {
                        case 0:
                            FireOrganizedBurst(target, 3, 0.15f, 12f);
                            break;
                        case 1:
                            FireOrganizedBurst(target, 5, 0.3f, 11f);
                            break;
                        case 2:
                            FireOrganizedBurst(target, 5, 0.4f, 10f);
                            break;
                        case 3:
                            Vector2 predictedPos = target.Center + target.velocity * 15f;
                            FireOrganizedBurst(predictedPos, 4, 0.2f, 13f);
                            break;
                    }

                    SoundEngine.PlaySound(SoundID.Item125, NPC.Center);
                    AddScreenShakeToNearbyPlayers(4f, 15);
                    CreateSimpleMuzzleFlash();
                }

                if (timer % 8 == 0)
                {
                    Dust energyParticle = Dust.NewDustDirect(NPC.Center, 16, 16, DustID.Electric);
                    energyParticle.velocity = Main.rand.NextVector2Circular(2f, 2f);
                    energyParticle.scale = 1.0f;
                    energyParticle.noGravity = true;
                    energyParticle.color = Color.Cyan;
                }
            }
            else if (timer >= 180 && timer < 210)
            {
                if (timer % 15 == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Dust coolDust = Dust.NewDustDirect(NPC.Center, 20, 20, DustID.Smoke);
                        coolDust.velocity = Main.rand.NextVector2Circular(1.5f, 1.5f);
                        coolDust.scale = 1.2f;
                        coolDust.alpha = 120;
                    }
                }
            }

            if (timer >= 210)
            {
                holdingShotgun = false;
                AIState = ActionState.Phase1Choose;
                timer = 0;
            }
        }

        private void FireOrganizedBurst(Player target, int projectileCount, float spread, float speed)
        {
            Vector2 baseDirection = NPC.DirectionTo(target.Center);
            FireOrganizedBurst(target.Center, projectileCount, spread, speed, baseDirection);
        }

        private void FireOrganizedBurst(Vector2 targetPosition, int projectileCount, float spread, float speed, Vector2? overrideDirection = null)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 baseDirection = overrideDirection ?? NPC.DirectionTo(targetPosition);

            for (int i = 0; i < projectileCount; i++)
            {
                float spreadAngle;
                if (projectileCount == 1)
                {
                    spreadAngle = 0f;
                }
                else
                {
                    spreadAngle = MathHelper.Lerp(-spread, spread, i / (float)(projectileCount - 1));
                }

                Vector2 shootDirection = baseDirection.RotatedBy(spreadAngle);
                float projectileSpeed = speed + Main.rand.NextFloat(-0.5f, 0.5f);

                Projectile.NewProjectile(NPC.GetSource_FromAI(),
                    NPC.Center + shootDirection * 40f,
                    shootDirection * projectileSpeed,
                    ModContent.ProjectileType<SlothDagger>(),
                    42,
                    3f,
                    -1);
            }
        }

        private void CreateSimpleMuzzleFlash()
        {
            Player target = Main.player[NPC.target];
            Vector2 muzzleDirection = NPC.DirectionTo(target.Center);
            Vector2 muzzlePosition = NPC.Center + muzzleDirection * 50f;

            for (int i = 0; i < 8; i++)
            {
                Dust flash = Dust.NewDustDirect(muzzlePosition, 10, 10, DustID.Electric);
                flash.velocity = muzzleDirection.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f)) * Main.rand.NextFloat(2f, 5f);
                flash.scale = Main.rand.NextFloat(1.0f, 1.5f);
                flash.noGravity = true;
                flash.color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat());
            }

            for (int i = 0; i < 4; i++)
            {
                Dust spark = Dust.NewDustDirect(muzzlePosition, 4, 4, DustID.MagicMirror);
                spark.velocity = Main.rand.NextVector2Circular(3f, 3f);
                spark.scale = 0.8f;
                spark.noGravity = true;
                spark.color = Color.Purple;
            }
        }

        private void ProjectileSpiralAttack(Player target)
        {
            if (timer < 45)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    AddScreenShakeToNearbyPlayers(4f, 25);
                }

                if (timer % 10 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        float angle = (MathHelper.TwoPi / 8) * i;
                        Vector2 energyPos = NPC.Center + Vector2.UnitX.RotatedBy(angle) * 100f;

                        Dust energy = Dust.NewDustDirect(energyPos, 6, 6, DustID.MagicMirror);
                        energy.velocity = (NPC.Center - energyPos) * 0.05f;
                        energy.scale = 1.3f;
                        energy.noGravity = true;
                        energy.color = Color.Purple;
                    }
                }
            }
            else if (timer >= 45 && timer < 225)
            {
                int adjustedTimer = (int)(timer - 45);
                float rotationProgress = adjustedTimer / 180f;
                float currentAngle = rotationProgress * MathHelper.TwoPi * 3f;

                if (adjustedTimer % 6 == 0)
                {
                    Vector2 direction = Vector2.UnitX.RotatedBy(currentAngle);
                    float radiusMultiplier = 0.7f + (rotationProgress * 0.6f);
                    Vector2 spawnPos = NPC.Center + direction * (80f * radiusMultiplier);
                    Vector2 shootDirection = direction.RotatedBy(0.3f);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(),
                        spawnPos,
                        shootDirection * 9f,
                        ModContent.ProjectileType<SlothSpear>(),
                        38,
                        2.5f);

                    if (adjustedTimer % 30 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item20 with
                        {
                            Pitch = 0.2f + (rotationProgress * 0.4f)
                        }, NPC.Center);
                    }
                }

                if (adjustedTimer % 4 == 0)
                {
                    Vector2 energyDirection = Vector2.UnitX.RotatedBy(currentAngle);
                    Vector2 energyPos = NPC.Center + energyDirection * (50f * (0.8f + rotationProgress * 0.4f));

                    Dust spiralDust = Dust.NewDustDirect(energyPos, 8, 8, DustID.Electric);
                    spiralDust.velocity = energyDirection * 1.5f;
                    spiralDust.scale = 1.2f + (rotationProgress * 0.5f);
                    spiralDust.noGravity = true;
                    spiralDust.color = Color.Lerp(Color.Purple, Color.Cyan, rotationProgress);
                }
            }
            else if (timer >= 225 && timer < 240)
            {
                if (timer % 8 == 0)
                {
                    Dust cooldown = Dust.NewDustDirect(NPC.Center, 20, 20, DustID.Smoke);
                    cooldown.velocity = Main.rand.NextVector2Circular(1f, 1f);
                    cooldown.scale = 1.0f;
                    cooldown.alpha = 150;
                }
            }

            if (timer >= 240)
            {
                holdingShotgun = false;
                AIState = ActionState.Phase1Choose;
                timer = 0;
            }
        }

        private void AreaDenialAttack(Player target)
        {
            if (timer < 60)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana, NPC.Center);
                    AddScreenShakeToNearbyPlayers(4f, 25);

                    NPC.ai[0] = target.Center.X;
                    NPC.ai[3] = target.Center.Y;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                    }
                }

                Vector2 lockedTargetPos = new Vector2(NPC.ai[0], NPC.ai[3]);

                if (timer % 8 == 0)
                {
                    float scanAngle = MathHelper.TwoPi * (timer / 60f);
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 scanPos = lockedTargetPos + Vector2.UnitX.RotatedBy(scanAngle + i * MathHelper.TwoPi / 3) * (200 + i * 50);

                        Dust scanDust = Dust.NewDustDirect(scanPos, 15, 15, DustID.RedTorch);
                        scanDust.velocity = Vector2.Zero;
                        scanDust.scale = 2f;
                        scanDust.noGravity = true;
                        scanDust.alpha = 100;
                    }
                }

                if (timer >= 40)
                {
                    if (timer % 4 == 0)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Vector2 targetZone = lockedTargetPos + new Vector2((i - 4.5f) * 80, 0);

                            for (int j = 0; j < 3; j++)
                            {
                                Dust targetDust = Dust.NewDustDirect(targetZone + Vector2.UnitY * j * 20, 10, 10, DustID.RedTorch);
                                targetDust.velocity = Vector2.UnitY * -2f;
                                targetDust.scale = 1.5f;
                                targetDust.noGravity = true;
                                targetDust.alpha = 150;
                            }
                        }
                    }
                }
            }
            else if (timer >= 60 && timer < 240)
            {
                int adjustedTimer = (int)(timer - 60);
                Vector2 lockedTargetPos = new Vector2(NPC.ai[0], NPC.ai[3]);

                if (adjustedTimer % 25 == 0)
                {
                    int waveNumber = adjustedTimer / 25;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        switch (waveNumber % 3)
                        {
                            case 0:
                                ExecuteLinearBombardment(lockedTargetPos);
                                break;
                            case 1:
                                ExecuteCircularBombardment(lockedTargetPos);
                                break;
                            case 2:
                                ExecuteScatteredBombardment(lockedTargetPos);
                                break;
                        }
                    }

                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);
                    AddScreenShakeToNearbyPlayers(6f, 15);
                }

                if (adjustedTimer % 10 == 0)
                {
                    Vector2 orbitalPos = NPC.Center + Vector2.UnitY * -400 + Main.rand.NextVector2Circular(100f, 20f);
                    Dust orbital = Dust.NewDustDirect(orbitalPos, 8, 8, DustID.Electric);
                    orbital.velocity = Vector2.UnitY * 2f;
                    orbital.scale = 1.4f;
                    orbital.noGravity = true;
                    orbital.color = Color.Cyan;
                }
            }

            if (timer >= 300)
            {
                holdingShotgun = false;
                AIState = ActionState.Phase1Choose;
                timer = 0;
            }
        }

        private void ExecuteLinearBombardment(Vector2 targetPos)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < 8; i++)
            {
                Vector2 position = targetPos + new Vector2((i - 3.5f) * 75, -650 - Main.rand.NextFloat(0f, 50f));
                Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(15f, 18f);

                Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity,
                    ModContent.ProjectileType<SlothSpear>(), 48, 5f, -1);
            }
        }

        private void ExecuteCircularBombardment(Vector2 targetPos)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < 6; i++)
            {
                float angle = (MathHelper.TwoPi / 6) * i;
                Vector2 position = targetPos + Vector2.UnitX.RotatedBy(angle) * 200f + Vector2.UnitY * -650;
                Vector2 direction = (targetPos - position).SafeNormalize(Vector2.UnitY) * 16f;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), position, direction,
                    ModContent.ProjectileType<SlothSpear>(), 48, 5f, -1);
            }
        }

        private void ExecuteScatteredBombardment(Vector2 targetPos)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < 5; i++)
            {
                Vector2 position = targetPos + Main.rand.NextVector2Circular(120f, 60f) + Vector2.UnitY * -650;
                Vector2 direction = (targetPos - position).SafeNormalize(Vector2.UnitY) * 17f;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), position, direction,
                    ModContent.ProjectileType<SlothSpear>(), 48, 5f, -1);
            }
        }

        private void SummonOrbsAttack(Player target)
        {
            if (timer < 45)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, NPC.Center);
                    AddScreenShakeToNearbyPlayers(6f, 30);
                }

                if (timer % 5 == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        float angle = (MathHelper.TwoPi / 6) * i;
                        Vector2 ritualPos = NPC.Center + Vector2.UnitX.RotatedBy(angle) * (140 - timer * 2f);

                        for (int j = 0; j < 8; j++)
                        {
                            Dust ritual = Dust.NewDustDirect(ritualPos, 12, 12, DustID.MagicMirror);
                            ritual.velocity = Vector2.UnitX.RotatedBy(angle + j * MathHelper.PiOver4) * 0.5f;
                            ritual.scale = 1.8f;
                            ritual.noGravity = true;
                            ritual.color = Color.Lerp(Color.Purple, Color.Gold, timer / 45f);
                        }
                    }
                }
            }
            else if (timer >= 45 && timer <= 75)
            {
                if (timer % 5 == 0)
                {
                    int orbIndex = (int)((timer - 45) / 5);
                    if (orbIndex < 6)
                    {
                        Vector2 orbPosition = NPC.Center + Vector2.UnitX.RotatedBy((MathHelper.TwoPi / 6) * orbIndex) * 130;

                        for (int i = 0; i < 35; i++)
                        {
                            Dust manifest = Dust.NewDustDirect(orbPosition, 15, 15, DustID.MagicMirror);
                            manifest.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(5f, 12f);
                            manifest.scale = Main.rand.NextFloat(1.5f, 2.2f);
                            manifest.noGravity = true;
                            manifest.color = Color.Lerp(Color.Purple, Color.Cyan, Main.rand.NextFloat());
                        }

                        int orbProjectile = Projectile.NewProjectile(NPC.GetSource_FromAI(), orbPosition, Vector2.Zero,
                            ModContent.ProjectileType<SlothOrbiter>(), 45, 3f, ai0: NPC.whoAmI);

                        SoundEngine.PlaySound(SoundID.Item25 with { Pitch = orbIndex * 0.1f }, orbPosition);
                        AddScreenShakeToNearbyPlayers(4f, 12);
                    }
                }
            }
            else if (timer >= 75 && timer < 120)
            {
                if (timer % 8 == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 syncPos = NPC.Center + Vector2.UnitX.RotatedBy((MathHelper.TwoPi / 6) * i) * 130;

                        Dust beam = Dust.NewDustDirect(syncPos, 6, 6, DustID.Electric);
                        beam.velocity = Vector2.Zero;
                        beam.scale = 1.5f;
                        beam.noGravity = true;
                        beam.color = Color.Gold;
                    }

                    Dust center = Dust.NewDustDirect(NPC.Center, 20, 20, DustID.MagicMirror);
                    center.velocity = Vector2.Zero;
                    center.scale = 2.5f;
                    center.noGravity = true;
                    center.color = Color.White;
                }

                if (timer == 100)
                {
                    SoundEngine.PlaySound(SoundID.Item117, NPC.Center);
                    AddScreenShakeToNearbyPlayers(8f, 25);
                }
            }

            if (timer >= 420)
            {
                holdingShotgun = false;
                AIState = ActionState.Phase1Choose;
                timer = 0;
            }
        }

        private void StartPhase2Transition()
        {
            phase2 = true;
            holdingShotgun = false;
            AIState = ActionState.Transition;
            timer = 0;

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }
        }

        private void Phase2Transition()
        {
            NPC.velocity = Vector2.Zero;
            NPC.dontTakeDamage = true;

            if (timer == 30)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.position);
                AddScreenShakeToNearbyPlayers(20f, 60);
            }

            if (timer == 60 && headSpawned == 0)
            {
                AddScreenShakeToNearbyPlayers(15f, 40);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int headIndex = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)(NPC.Center.Y - 80),
                        ModContent.NPCType<SlothHead>(), ai1: NPC.whoAmI);

                    if (headIndex < Main.maxNPCs)
                    {
                        detachedHeadWhoAmI = headIndex;
                        headSpawned = 1;

                        sharedMaxHealth = NPC.lifeMax;
                        sharedCurrentHealth = NPC.life;
                        headLinked = true;

                        var head = Main.npc[headIndex];
                        head.boss = true;
                        head.lifeMax = sharedMaxHealth;
                        head.life = sharedCurrentHealth;
                        head.netUpdate = true;

                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                            NetMessage.SendData(MessageID.SyncNPC, number: headIndex);
                        }
                    }
                }
            }

            if (timer >= 120)
            {
                NPC.dontTakeDamage = false;
                AIState = ActionState.Phase2;
                timer = 0;
            }
        }

        private void SyncSharedHealth()
        {
            if (!headLinked || detachedHeadWhoAmI < 0 || detachedHeadWhoAmI >= Main.maxNPCs)
                return;

            var head = Main.npc[detachedHeadWhoAmI];
            if (!head.active)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.checkDead();
                return;
            }

            int lowestHealth = Math.Min(NPC.life, head.life);

            if (sharedCurrentHealth != lowestHealth)
            {
                sharedCurrentHealth = lowestHealth;
                NPC.life = sharedCurrentHealth;
                head.life = sharedCurrentHealth;

                if (sharedCurrentHealth <= 0)
                {
                    NPC.life = 0;
                    head.life = 0;
                    NPC.HitEffect();
                    head.HitEffect();
                    NPC.checkDead();
                    head.checkDead();
                }

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                    NetMessage.SendData(MessageID.SyncNPC, number: detachedHeadWhoAmI);
                }
            }
        }

        private void Phase2Behavior()
        {
            NPC.velocity = Vector2.Zero;

            if (timer >= 120)
            {
                List<SlothAttackType> phase2Attacks = new List<SlothAttackType>
                {
                    SlothAttackType.CrossLasers,
                    SlothAttackType.SweepingBeams,
                    SlothAttackType.LaserGrid
                };

                phase2Attacks.Remove(lastPhase2Attack);

                CurrentAttack = phase2Attacks[Main.rand.Next(phase2Attacks.Count)];
                lastPhase2Attack = CurrentAttack;

                timer = 0;
            }

            Player target = Main.player[NPC.target];

            switch (CurrentAttack)
            {
                case SlothAttackType.CrossLasers:
                    Phase2RayAttack(target);
                    break;
                case SlothAttackType.SweepingBeams:
                    Phase2CrossAttack(target);
                    break;
                case SlothAttackType.LaserGrid:
                    Phase2CircleAttack(target);
                    break;
            }
        }

        private void Phase2RayAttack(Player target)
        {
            if (timer < 60)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    AddScreenShakeToNearbyPlayers(6f, 30);
                    hasStoredPosition = false;
                }

                if (timer == 30)
                {
                    if (target != null && target.active && !target.dead)
                    {
                        storedTargetPosition = target.Center;
                        hasStoredPosition = true;

                        SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.3f }, target.Center);
                        AddScreenShakeToNearbyPlayers(8f, 25);
                    }
                }

                if (timer >= 30 && timer % 4 == 0 && hasStoredPosition)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float angle = (MathHelper.TwoPi / 12) * i + timer * 0.1f;
                        Vector2 energyPos = storedTargetPosition + Vector2.UnitX.RotatedBy(angle) * (300 - (timer - 30) * 8f);

                        Dust energy = Dust.NewDustDirect(energyPos, 8, 8, DustID.Electric);
                        energy.velocity = (storedTargetPosition - energyPos) * 0.1f;
                        energy.scale = 1.5f + (timer - 30) / 30f * 0.5f;
                        energy.noGravity = true;
                        energy.color = Color.Lerp(Color.Cyan, Color.White, (timer - 30) / 30f);
                    }
                }
            }
            else if (timer >= 60 && timer < 80)
            {
                if (timer == 60)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyScream, NPC.Center);
                    AddScreenShakeToNearbyPlayers(10f, 30);
                }

                if (hasStoredPosition)
                {
                    int rayCount = 8;
                    float warningIntensity = (timer - 60) / 20f;

                    for (int i = 0; i < rayCount; i++)
                    {
                        float rayAngle = (MathHelper.TwoPi / rayCount) * i;

                        for (int j = 0; j < 15; j++)
                        {
                            Vector2 linePos = storedTargetPosition + Vector2.UnitX.RotatedBy(rayAngle) * (j * 40f);

                            if (timer % Math.Max(2, 6 - (int)(warningIntensity * 4)) == 0)
                            {
                                Dust warning = Dust.NewDustDirect(linePos, 12, 12, DustID.RedTorch);
                                warning.velocity = Vector2.Zero;
                                warning.scale = 2f * warningIntensity;
                                warning.noGravity = true;
                                warning.alpha = 100;
                            }
                        }
                    }
                }
            }
            else if (timer == 80)
            {
                SoundEngine.PlaySound(SoundID.Item122, NPC.Center);
                AddScreenShakeToNearbyPlayers(15f, 30);

                if (hasStoredPosition)
                {
                    Vector2 finalTargetPos = storedTargetPosition;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int rayCount = 8;

                        for (int i = 0; i < rayCount; i++)
                        {
                            float angle = (MathHelper.TwoPi / rayCount) * i;
                            Vector2 rayDirection = Vector2.UnitX.RotatedBy(angle);

                            Vector2 orbSpawnPos = finalTargetPos + rayDirection * 400f;
                            Vector2 direction = Vector2.Normalize(finalTargetPos - orbSpawnPos);

                            Projectile.NewProjectile(
                                NPC.GetSource_FromAI(),
                                orbSpawnPos,
                                direction * 8f,
                                ModContent.ProjectileType<SlothSpear3>(),
                                70,
                                5f,
                                ai0: finalTargetPos.X,
                                ai1: finalTargetPos.Y
                            );
                        }
                    }

                    SoundEngine.PlaySound(SoundID.Item125, finalTargetPos);
                }
                else
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 currentPos = target.Center;
                        for (int i = 0; i < 8; i++)
                        {
                            float angle = (MathHelper.TwoPi / 8) * i;
                            Vector2 spawnPos = currentPos + Vector2.UnitX.RotatedBy(angle) * 400f;
                            Vector2 velocity = Vector2.Normalize(currentPos - spawnPos) * 8f;

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                                ModContent.ProjectileType<SlothSpear3>(), 70, 5f,
                                ai0: currentPos.X, ai1: currentPos.Y);
                        }
                    }
                }
            }
            else if (timer > 80 && timer < 100)
            {
                if (hasStoredPosition && timer % 3 == 0)
                {
                    Dust impact = Dust.NewDustDirect(storedTargetPosition, 40, 40, DustID.Electric);
                    impact.velocity = Main.rand.NextVector2Circular(8f, 8f);
                    impact.scale = 2.5f;
                    impact.noGravity = true;
                    impact.color = Color.White;
                }
            }

            if (timer >= 120)
            {
                hasStoredPosition = false;
                storedTargetPosition = Vector2.Zero;
                AIState = ActionState.Phase2;
                timer = 0;
            }
        }

        private void Phase2CrossAttack(Player target)
        {
            if (timer < 75)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalIdleLoop, NPC.Center);
                    AddScreenShakeToNearbyPlayers(5f, 25);
                    hasStoredPosition = false;
                }

                if (timer == 45)
                {
                    if (target != null && target.active && !target.dead)
                    {
                        storedTargetPosition = target.Center;
                        hasStoredPosition = true;

                        SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.5f }, target.Center);
                        AddScreenShakeToNearbyPlayers(7f, 20);
                    }
                }

                if (timer >= 45 && timer % 3 == 0 && hasStoredPosition)
                {
                    float[] angles = { 0f, MathHelper.PiOver2, MathHelper.Pi, 3f * MathHelper.PiOver2 };
                    float buildupIntensity = (timer - 45) / 30f;

                    foreach (float angle in angles)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            Vector2 crossPos = storedTargetPosition + Vector2.UnitX.RotatedBy(angle) * (i * 50f);

                            Dust crossDust = Dust.NewDustDirect(crossPos, 10, 10, DustID.MagicMirror);
                            crossDust.velocity = Vector2.Zero;
                            crossDust.scale = 1.5f * buildupIntensity;
                            crossDust.noGravity = true;
                            crossDust.color = Color.Lerp(Color.Purple, Color.Gold, buildupIntensity);
                            crossDust.alpha = (int)(150 * (1f - buildupIntensity));
                        }
                    }
                }
            }
            else if (timer >= 75 && timer < 105)
            {
                if (timer == 75)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, NPC.Center);
                    AddScreenShakeToNearbyPlayers(8f, 30);
                }

                if (hasStoredPosition)
                {
                    float[] angles = { 0f, MathHelper.PiOver2, MathHelper.Pi, 3f * MathHelper.PiOver2 };
                    float warningIntensity = (timer - 75) / 30f;

                    foreach (float angle in angles)
                    {
                        for (int i = 0; i < 25; i++)
                        {
                            Vector2 linePos = storedTargetPosition + Vector2.UnitX.RotatedBy(angle) * (i * 40f);

                            if (timer % Math.Max(1, 8 - (int)(warningIntensity * 6)) == 0)
                            {
                                Dust warning = Dust.NewDustDirect(linePos, 15, 15, DustID.RedTorch);
                                warning.velocity = Vector2.Zero;
                                warning.scale = 2.5f * warningIntensity;
                                warning.noGravity = true;
                                warning.alpha = 50;
                            }
                        }
                    }
                }
            }
            else if (timer >= 105 && timer < 135)
            {
                if (timer == 105)
                {
                    SoundEngine.PlaySound(SoundID.Item162, NPC.Center);
                    AddScreenShakeToNearbyPlayers(12f, 25);
                }

                if (hasStoredPosition)
                {
                    float[] angles = { 0f, MathHelper.PiOver2, MathHelper.Pi, 3f * MathHelper.PiOver2 };

                    int executionFrame = (int)(timer - 105);
                    if (executionFrame % 8 == 0 && executionFrame < 24)
                    {
                        int waveNumber = executionFrame / 8;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            foreach (float angle in angles)
                            {
                                Vector2 rayDirection = Vector2.UnitX.RotatedBy(angle);

                                for (int distance = 1; distance <= 3; distance++)
                                {
                                    Vector2 orbSpawnPos = storedTargetPosition + rayDirection * (400f + distance * 120f);
                                    Vector2 direction = (storedTargetPosition - orbSpawnPos).SafeNormalize(Vector2.Zero);

                                    float speed = 9f + waveNumber * 1.5f;
                                    int damage = 75 + waveNumber * 5;

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), orbSpawnPos, direction * speed,
                                        ModContent.ProjectileType<SlothSpear3>(), damage, 6f,
                                        ai0: storedTargetPosition.X, ai1: storedTargetPosition.Y);
                                }
                            }
                        }

                        SoundEngine.PlaySound(SoundID.Item125, storedTargetPosition);
                    }

                    if (timer % 3 == 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust impact = Dust.NewDustDirect(storedTargetPosition, 30, 30, DustID.Electric);
                            impact.velocity = Main.rand.NextVector2Circular(10f, 10f);
                            impact.scale = 2.8f;
                            impact.noGravity = true;
                            impact.color = Color.Lerp(Color.Gold, Color.White, Main.rand.NextFloat());
                        }
                    }
                }
                else
                {
                    if (timer == 105 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 currentPos = target.Center;
                        float[] angles = { 0f, MathHelper.PiOver2, MathHelper.Pi, 3f * MathHelper.PiOver2 };

                        foreach (float angle in angles)
                        {
                            Vector2 rayDirection = Vector2.UnitX.RotatedBy(angle);
                            Vector2 orbSpawnPos = currentPos + rayDirection * 400f;
                            Vector2 direction = (currentPos - orbSpawnPos).SafeNormalize(Vector2.Zero);

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), orbSpawnPos, direction * 9f,
                                ModContent.ProjectileType<SlothSpear3>(), 75, 6f,
                                ai0: currentPos.X, ai1: currentPos.Y);
                        }
                    }
                }
            }

            if (timer >= 160)
            {
                hasStoredPosition = false;
                storedTargetPosition = Vector2.Zero;
                AIState = ActionState.Phase2;
                timer = 0;
            }
        }

        private void Phase2CircleAttack(Player target)
        {
            if (timer < 50)
            {
                if (timer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    AddScreenShakeToNearbyPlayers(7f, 35);
                    hasStoredPosition = false;
                }

                if (timer == 25)
                {
                    if (target != null && target.active && !target.dead)
                    {
                        storedTargetPosition = target.Center;
                        hasStoredPosition = true;

                        SoundEngine.PlaySound(SoundID.DD2_EtherianPortalIdleLoop, target.Center);
                        AddScreenShakeToNearbyPlayers(9f, 25);
                    }
                }

                if (timer >= 25 && timer % 4 == 0 && hasStoredPosition)
                {
                    int ringCount = 16;
                    float expansionProgress = (timer - 25) / 25f;

                    for (int ring = 0; ring < 3; ring++)
                    {
                        for (int i = 0; i < ringCount; i++)
                        {
                            float angle = (MathHelper.TwoPi / ringCount) * i + timer * 0.05f * (ring + 1);
                            float radius = (200 + ring * 150) * expansionProgress;
                            Vector2 ringPos = storedTargetPosition + Vector2.UnitX.RotatedBy(angle) * radius;

                            Dust orbital = Dust.NewDustDirect(ringPos, 8, 8, DustID.MagicMirror);
                            orbital.velocity = Vector2.Zero;
                            orbital.scale = 1.8f - ring * 0.3f;
                            orbital.noGravity = true;
                            orbital.color = Color.Lerp(Color.Purple, Color.Cyan, ring / 3f);
                            orbital.alpha = (int)(100 + expansionProgress * 100);
                        }
                    }
                }
            }
            else if (timer >= 50 && timer < 80)
            {
                if (timer == 50)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyScream, NPC.Center);
                    AddScreenShakeToNearbyPlayers(10f, 40);
                }

                if (hasStoredPosition)
                {
                    int rayCount = 16;
                    float warningIntensity = (timer - 50) / 30f;

                    for (int i = 0; i < rayCount; i++)
                    {
                        float angle = (MathHelper.TwoPi / rayCount) * i;

                        for (int j = 0; j < 30; j++)
                        {
                            Vector2 linePos = storedTargetPosition + Vector2.UnitX.RotatedBy(angle) * (j * 35f);

                            if (timer % Math.Max(1, 6 - (int)(warningIntensity * 4)) == 0)
                            {
                                Dust warning = Dust.NewDustDirect(linePos, 12, 12, DustID.RedTorch);
                                warning.velocity = Vector2.Zero;
                                warning.scale = 2.2f * warningIntensity;
                                warning.noGravity = true;
                                warning.alpha = (int)(80 + warningIntensity * 80);
                            }
                        }
                    }

                    if (timer % 5 == 0)
                    {
                        Dust center = Dust.NewDustDirect(storedTargetPosition, 25, 25, DustID.Electric);
                        center.velocity = Vector2.Zero;
                        center.scale = 3f * warningIntensity;
                        center.noGravity = true;
                        center.color = Color.White;
                    }
                }
            }
            else if (timer >= 80 && timer < 110)
            {
                if (timer == 80)
                {
                    SoundEngine.PlaySound(SoundID.Item163, NPC.Center);
                    AddScreenShakeToNearbyPlayers(15f, 35);
                }

                if (hasStoredPosition)
                {
                    int rayCount = 16;
                    int executionFrame = (int)(timer - 80);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (executionFrame == 0)
                        {
                            for (int i = 0; i < rayCount; i += 2)
                            {
                                float angle = (MathHelper.TwoPi / rayCount) * i;
                                Vector2 rayDirection = Vector2.UnitX.RotatedBy(angle);

                                Vector2 orbSpawnPos = storedTargetPosition + rayDirection * 600f;
                                Vector2 direction = (storedTargetPosition - orbSpawnPos).SafeNormalize(Vector2.Zero);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), orbSpawnPos, direction * 7f,
                                    ModContent.ProjectileType<SlothSpear3>(), 68, 5f,
                                    ai0: storedTargetPosition.X, ai1: storedTargetPosition.Y);
                            }

                            SoundEngine.PlaySound(SoundID.Item125, storedTargetPosition);
                        }

                        if (executionFrame == 10)
                        {
                            for (int i = 1; i < rayCount; i += 2)
                            {
                                float angle = (MathHelper.TwoPi / rayCount) * i;
                                Vector2 rayDirection = Vector2.UnitX.RotatedBy(angle);

                                Vector2 orbSpawnPos = storedTargetPosition + rayDirection * 600f;
                                Vector2 direction = (storedTargetPosition - orbSpawnPos).SafeNormalize(Vector2.Zero);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), orbSpawnPos, direction * 8f,
                                    ModContent.ProjectileType<SlothSpear3>(), 70, 5f,
                                    ai0: storedTargetPosition.X, ai1: storedTargetPosition.Y);
                            }

                            SoundEngine.PlaySound(SoundID.Item125, storedTargetPosition);
                        }

                        if (executionFrame == 20)
                        {
                            for (int i = 0; i < rayCount; i += 4)
                            {
                                float angle = (MathHelper.TwoPi / rayCount) * i;
                                Vector2 rayDirection = Vector2.UnitX.RotatedBy(angle);

                                for (int j = 0; j < 3; j++)
                                {
                                    Vector2 orbSpawnPos = storedTargetPosition + rayDirection * (550f + j * 100f);
                                    Vector2 direction = (storedTargetPosition - orbSpawnPos).SafeNormalize(Vector2.Zero);

                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), orbSpawnPos, direction * (9f + j),
                                        ModContent.ProjectileType<SlothSpear3>(), 75, 6f,
                                        ai0: storedTargetPosition.X, ai1: storedTargetPosition.Y);
                                }
                            }

                            SoundEngine.PlaySound(SoundID.Item122, storedTargetPosition);
                            AddScreenShakeToNearbyPlayers(18f, 30);
                        }
                    }
                }
                else
                {
                    if (timer == 80 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 currentPos = target.Center;

                        for (int i = 0; i < 12; i++)
                        {
                            float angle = (MathHelper.TwoPi / 12) * i;
                            Vector2 spawnPos = currentPos + Vector2.UnitX.RotatedBy(angle) * 600f;
                            Vector2 velocity = Vector2.Normalize(currentPos - spawnPos) * 7f;

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                                ModContent.ProjectileType<SlothSpear3>(), 68, 5f,
                                ai0: currentPos.X, ai1: currentPos.Y);
                        }
                    }
                }
            }

            if (timer >= 120)
            {
                hasStoredPosition = false;
                storedTargetPosition = Vector2.Zero;
                AIState = ActionState.Phase2;
                timer = 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = 15;
            NPC.frameCounter += 1f;

            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture;
            if (phase2)
            {
                try
                {
                    texture = ModContent.Request<Texture2D>("PurringTale/Content/NPCs/BossNPCs/Sloth/SlothBoss_Headless").Value;
                }
                catch
                {
                    texture = ModContent.Request<Texture2D>("PurringTale/Content/NPCs/BossNPCs/Sloth/SlothBoss").Value;
                }
            }
            else
            {
                texture = ModContent.Request<Texture2D>("PurringTale/Content/NPCs/BossNPCs/Sloth/SlothBoss").Value;
            }

            Vector2 drawPosition = NPC.Center - screenPos;
            Rectangle sourceRectangle = NPC.frame;

            if (sourceRectangle.Y + sourceRectangle.Height > texture.Height)
            {
                sourceRectangle.Y = 0;
            }

            Vector2 origin = sourceRectangle.Size() / 2f;
            SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Color glowColor = Color.Purple * 0.3f;
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.PiOver2 * i) * 4f;
                spriteBatch.Draw(texture, drawPosition + offset, sourceRectangle, glowColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            }

            spriteBatch.Draw(texture, drawPosition, sourceRectangle, drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);

            if (holdingShotgun)
            {
                try
                {
                    Player target = Main.player[NPC.target];
                    Vector2 directionToPlayer = target.Center - NPC.Center;

                    bool aimingLeft = directionToPlayer.X < 0;

                    Texture2D shotgunTexture;
                    float drawRotation;
                    Vector2 shotgunOrigin;
                    Vector2 positionOffset = Vector2.Zero;

                    if (aimingLeft)
                    {
                        shotgunTexture = ModContent.Request<Texture2D>("PurringTale/Content/NPCs/BossNPCs/Sloth/SlothShotgun_Left").Value;
                        drawRotation = directionToPlayer.ToRotation() + MathHelper.Pi;
                        shotgunOrigin = new Vector2(shotgunTexture.Width * 0.8f, shotgunTexture.Height * 0.5f);
                        positionOffset = Vector2.UnitX.RotatedBy(drawRotation) * -20f;
                    }
                    else
                    {
                        shotgunTexture = ModContent.Request<Texture2D>("PurringTale/Content/NPCs/BossNPCs/Sloth/SlothShotgun").Value;
                        drawRotation = directionToPlayer.ToRotation();
                        shotgunOrigin = new Vector2(shotgunTexture.Width * 0.2f, shotgunTexture.Height * 0.5f);
                        positionOffset = Vector2.UnitX.RotatedBy(drawRotation) * 20f;
                    }

                    Vector2 shotgunPosition = NPC.Center - screenPos + positionOffset;

                    spriteBatch.Draw(shotgunTexture, shotgunPosition, null, drawColor, drawRotation,
                        shotgunOrigin, NPC.scale * 0.8f, SpriteEffects.None, 0f);
                }
                catch
                {
                }
            }

            return false;
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (phase2 && headLinked && detachedHeadWhoAmI >= 0 && Main.npc[detachedHeadWhoAmI].active)
            {
                var head = Main.npc[detachedHeadWhoAmI];
                head.life = NPC.life;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: detachedHeadWhoAmI);
                }
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (phase2 && headLinked && detachedHeadWhoAmI >= 0 && Main.npc[detachedHeadWhoAmI].active)
            {
                var head = Main.npc[detachedHeadWhoAmI];
                head.life = NPC.life;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: detachedHeadWhoAmI);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AcediaBossTrophy>(), 6, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<AcediaBossRelic>()));
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<SlothBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
        }

        public override void OnKill()
        {
            AddScreenShakeToNearbyPlayers(25f, 80);

            if (detachedHeadWhoAmI >= 0 && Main.npc[detachedHeadWhoAmI].active)
            {
                Main.npc[detachedHeadWhoAmI].life = 0;
                Main.npc[detachedHeadWhoAmI].HitEffect();
                Main.npc[detachedHeadWhoAmI].active = false;
            }

            NPC.SetEventFlagCleared(ref DownedBossSystem.downedSloth, -1);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Sloth";
            potionType = ItemID.GreaterHealingPotion;
        }
    }
}