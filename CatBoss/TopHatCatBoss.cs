using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture.Relics;
using PurringTale.Content.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.CatBoss
{
    public enum AttackType
    {
        Gun,
        Sword,
        Book,
        Staff,
        Whip,
        HomingStars,
        RegularStars,
        Clones,
        BulletRain,
        Rockets,
        Lasers
    }

    public enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    }

    [AutoloadBossHead]
    public class TopHatCatBoss : ModNPC
    {
        private enum ActionState
        {
            Spawn,
            Choose,
            Attack,
            Move,
            PhaseTransition,
            Death,
            DeathMode
        }

        private uint Bussy
        {
            get => BitConverter.SingleToUInt32Bits(NPC.ai[1]);
            set => NPC.ai[1] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }
        private ActionState AIState
        {
            get => (ActionState)Bussy;
            set => Bussy = (uint)value;
        }
        private uint Bussy2
        {
            get => BitConverter.SingleToUInt32Bits(NPC.ai[2]);
            set => NPC.ai[2] = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }
        private AttackType AtkType
        {
            get => (AttackType)Bussy2;
            set => Bussy2 = (uint)value;
        }

        private ref float timer => ref NPC.ai[0];
        private int atkCounter = 0;
        private BossPhase currentPhase = BossPhase.Phase1;
        private bool hasTransitioned = false;
        private float ShaderTimer = 0;
        private bool holdingWeapon = false;
        private string currentWeapon = "";
        private List<int> activeClones = new List<int>();
        private int phase2HealthThreshold => NPC.lifeMax * 2 / 3;
        private int phase3HealthThreshold => NPC.lifeMax / 3;

        private bool inDeathMode = false;
        private bool deathModeTriggered = false;
        private int deathModeAttackCounter = 0;
        private float deathModeTimer = 0;
        private List<int> deathModeProjectiles = new List<int>();

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ShaderTimer);
            writer.Write(atkCounter);
            writer.Write(holdingWeapon);
            writer.Write(currentWeapon);
            writer.Write((int)currentPhase);
            writer.Write(hasTransitioned);
            writer.Write(activeClones.Count);
            foreach (int clone in activeClones)
            {
                writer.Write(clone);
            }
            writer.Write(inDeathMode);
            writer.Write(deathModeTriggered);
            writer.Write(deathModeAttackCounter);
            writer.Write(deathModeTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ShaderTimer = reader.ReadSingle();
            atkCounter = reader.ReadInt32();
            holdingWeapon = reader.ReadBoolean();
            currentWeapon = reader.ReadString();
            currentPhase = (BossPhase)reader.ReadInt32();
            hasTransitioned = reader.ReadBoolean();
            int cloneCount = reader.ReadInt32();
            activeClones.Clear();
            for (int i = 0; i < cloneCount; i++)
            {
                activeClones.Add(reader.ReadInt32());
            }
            inDeathMode = reader.ReadBoolean();
            deathModeTriggered = reader.ReadBoolean();
            deathModeAttackCounter = reader.ReadInt32();
            deathModeTimer = reader.ReadSingle();
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "PurringTale/CatBoss/TopHatCatBoss_Beastiaray",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Top Hat God";
            potionType = ItemID.SuperHealingPotion;
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 50;
            NPC.damage = 10;
            NPC.lifeMax = 1000000;
            NPC.defense = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.AbigailCry;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(platinum: 5);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.HitSound = SoundID.NPCHit8;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            NPC.ScaleStats_UseStrengthMultiplier(0.6f);

            inDeathMode = false;
            deathModeTriggered = false;
            deathModeAttackCounter = 0;
            deathModeTimer = 0;
            deathModeProjectiles = new List<int>();

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Prometheus");
            }
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<Terraria.GameContent.Bestiary.IBestiaryInfoElement> {
                new Terraria.GameContent.Bestiary.MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("Oh Look It Is Myself! - Rukuka")
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            AIState = ActionState.Spawn;
            currentPhase = BossPhase.Phase1;
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            NPC.FaceTarget();
        }

        private ActionState oldState = ActionState.Spawn;

        public override void AI()
        {
            NPC.FaceTarget();
            NPC.spriteDirection = NPC.direction;

            activeClones.RemoveAll(cloneId => cloneId < 0 || cloneId >= Main.maxProjectiles || !Main.projectile[cloneId].active || Main.projectile[cloneId].type != ModContent.ProjectileType<BossClone>());
            deathModeProjectiles.RemoveAll(projId => projId < 0 || projId >= Main.maxProjectiles || !Main.projectile[projId].active);

            if (NPC.life <= 1 && !deathModeTriggered)
            {
                TriggerDeathMode();
            }

            if (!inDeathMode)
            {
                CheckPhaseTransition();
            }

            if (AIState != oldState)
            {
                timer = 0;
                if (oldState == ActionState.Attack)
                {
                    holdingWeapon = false;
                    currentWeapon = "";
                }
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (player.dead && !inDeathMode)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            ChooseAction();

            ShaderTimer++;
            timer++;
            if (inDeathMode) deathModeTimer++;
            oldState = AIState;
        }

        private void TriggerDeathMode()
        {
            if (deathModeTriggered) return;

            deathModeTriggered = true;
            inDeathMode = true;
            AIState = ActionState.DeathMode;
            timer = 0;
            deathModeTimer = 0;
            deathModeAttackCounter = 0;

            NPC.dontTakeDamage = true;
            NPC.life = 1;

            SoundEngine.PlaySound(SoundID.Roar, NPC.position);
            SoundEngine.PlaySound(SoundID.DD2_BetsyScream, NPC.position);

            if (!Main.dedServ)
            {
                Main.NewText("THE TOP HAT GOD ENTERS A FINAL RAGE!", Color.Red);
                Main.NewText("SURVIVE THE ONSLAUGHT!", Color.Orange);
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 50f, 120);
            }

            for (int i = 0; i < 100; i++)
            {
                Vector2 pos = NPC.Center + Main.rand.NextVector2Circular(200, 200);
                Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 100 * i) * Main.rand.NextFloat(5f, 20f);
                Dust dust = Dust.NewDustPerfect(pos, DustID.Shadowflame, vel);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(2f, 4f);
                dust.color = Color.Red;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].hostile)
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }
        }

        private void CheckPhaseTransition()
        {
            BossPhase newPhase = currentPhase;

            if (NPC.life <= phase3HealthThreshold && currentPhase != BossPhase.Phase3)
            {
                newPhase = BossPhase.Phase3;
            }
            else if (NPC.life <= phase2HealthThreshold && currentPhase == BossPhase.Phase1)
            {
                newPhase = BossPhase.Phase2;
            }

            if (newPhase != currentPhase && !hasTransitioned)
            {
                currentPhase = newPhase;
                AIState = ActionState.PhaseTransition;
                hasTransitioned = true;
                timer = 0;
                atkCounter = 0;

                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 30f, 60);

                for (int i = 0; i < 50; i++)
                {
                    Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 50 * i) * Main.rand.NextFloat(8f, 15f);
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Shadowflame, vel);
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1.5f, 2.5f);
                    dust.color = GetPhaseColor();
                }
            }
        }

        private Color GetPhaseColor()
        {
            return currentPhase switch
            {
                BossPhase.Phase1 => Color.Purple,
                BossPhase.Phase2 => Color.Red,
                BossPhase.Phase3 => Color.Gray,
                _ => Color.Purple
            };
        }

        public void ChooseAction()
        {
            switch (AIState)
            {
                case ActionState.Spawn:
                    Spawn();
                    break;
                case ActionState.Choose:
                    ChooseAttack();
                    break;
                case ActionState.Attack:
                    Attack();
                    break;
                case ActionState.PhaseTransition:
                    PhaseTransition();
                    break;
                case ActionState.DeathMode:
                    ExecuteDeathMode();
                    break;
                default:
                    break;
            }
        }

        private void PhaseTransition()
        {
            NPC.velocity *= 0.9f;

            if (timer < 120)
            {
                NPC.dontTakeDamage = true;

                if (timer % 10 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 8 * i) * 5f;
                        Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Shadowflame, vel);
                        dust.noGravity = true;
                        dust.scale = 2f;
                        dust.color = GetPhaseColor();
                    }
                }

                if (timer % 20 == 0)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 15f - (timer / 8f), 20);
                }
            }

            if (timer >= 120)
            {
                NPC.dontTakeDamage = false;
                hasTransitioned = false;
                AIState = ActionState.Choose;
                timer = 0;

                string phaseText = currentPhase switch
                {
                    BossPhase.Phase2 => "The Top Hat God grows more aggressive!",
                    BossPhase.Phase3 => "The Top Hat God enters a desperate rage!",
                    _ => ""
                };

                if (!string.IsNullOrEmpty(phaseText))
                {
                    Main.NewText(phaseText, GetPhaseColor());
                }
            }
        }

        public void Spawn()
        {
            if (timer < 120)
            {
                NPC.dontTakeDamage = true;
                NPC.velocity.Y = -.6f;
            }
            if (timer >= 120)
            {
                NPC.velocity = Vector2.Zero;
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 20f, 1);
            }
            if (timer >= 150)
            {
                timer = 0;
                NPC.dontTakeDamage = false;
                AIState = ActionState.Choose;
            }
        }

        public void ChooseAttack()
        {
            Player target = Main.player[NPC.target];

            float moveSpeed = currentPhase switch
            {
                BossPhase.Phase1 => 9f,
                BossPhase.Phase2 => 12f,
                BossPhase.Phase3 => 15f,
                _ => 9f
            };

            if (timer <= 1)
            {
                Vector2 pos = target.Center + ModdingusUtils.randomVector();
                NPC.velocity = NPC.DirectionTo(pos) * (moveSpeed + (target.Distance(NPC.Center) / 80));
            }

            int waitTime = currentPhase switch
            {
                BossPhase.Phase1 => Main.rand.Next(30, 90),
                BossPhase.Phase2 => Main.rand.Next(20, 70),
                BossPhase.Phase3 => Main.rand.Next(15, 50),
                _ => Main.rand.Next(30, 90)
            };

            if (timer == waitTime)
            {
                NPC.velocity = Vector2.Zero;
            }
            if (timer > waitTime - 1)
            {
                NPC.velocity = Vector2.Zero;
            }

            int transitionTime = currentPhase switch
            {
                BossPhase.Phase1 => 120,
                BossPhase.Phase2 => 90,
                BossPhase.Phase3 => 60,
                _ => 120
            };

            if (timer > transitionTime)
            {
                timer = 0;
                ChoosePhaseAttack();
                AIState = ActionState.Attack;
                atkCounter += 1;

                int maxAttacks = currentPhase switch
                {
                    BossPhase.Phase1 => 6,
                    BossPhase.Phase2 => 8,
                    BossPhase.Phase3 => 10,
                    _ => 6
                };

                if (atkCounter > maxAttacks)
                {
                    atkCounter = 0;
                }
            }
        }

        private void ChoosePhaseAttack()
        {
            switch (currentPhase)
            {
                case BossPhase.Phase1:
                    AtkType = atkCounter switch
                    {
                        0 => AttackType.HomingStars,
                        1 => AttackType.RegularStars,
                        2 => AttackType.Gun,
                        3 => AttackType.Lasers,
                        4 => AttackType.BulletRain,
                        5 => AttackType.Clones,
                        6 => AttackType.Sword,
                        7 => AttackType.Rockets,
                        _ => AttackType.HomingStars
                    };
                    break;

                case BossPhase.Phase2:
                    AtkType = atkCounter switch
                    {
                        0 => AttackType.Gun,
                        1 => AttackType.HomingStars,
                        2 => AttackType.Lasers,
                        3 => AttackType.BulletRain,
                        4 => AttackType.Clones,
                        5 => AttackType.Rockets,
                        6 => AttackType.Whip,
                        7 => AttackType.Sword,
                        8 => AttackType.RegularStars,
                        _ => AttackType.Gun
                    };
                    break;

                case BossPhase.Phase3:
                    AtkType = atkCounter switch
                    {
                        0 => AttackType.Lasers,
                        1 => AttackType.Clones,
                        2 => AttackType.BulletRain,
                        3 => AttackType.Whip,
                        4 => AttackType.Rockets,
                        5 => AttackType.Lasers,
                        6 => AttackType.HomingStars,
                        7 => AttackType.BulletRain,
                        8 => AttackType.Rockets,
                        9 => AttackType.Sword,
                        10 => AttackType.RegularStars,
                        _ => AttackType.Lasers
                    };
                    break;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<THGBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 10, 100));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THGBossTrophy>(), 4, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<THGBossRelic>()));
        }

        public void Attack()
        {
            Player target = Main.player[NPC.target];

            float phaseMultiplier = currentPhase switch
            {
                BossPhase.Phase1 => 1.0f,
                BossPhase.Phase2 => 1.3f,
                BossPhase.Phase3 => 1.6f,
                _ => 1.0f
            };

            int damageBonus = currentPhase switch
            {
                BossPhase.Phase1 => 0,
                BossPhase.Phase2 => 10,
                BossPhase.Phase3 => 25,
                _ => 0
            };

            if (timer > 0)
            {
                switch (AtkType)
                {
                    case AttackType.Sword:
                        ExecuteSwordAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.Gun:
                        ExecuteGunAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.HomingStars:
                        ExecuteHomingStarsAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.RegularStars:
                        ExecuteRegularStarsAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.Whip:
                        ExecuteWhipAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.Clones:
                        ExecuteCloneAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.BulletRain:
                        ExecuteBulletRainAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.Rockets:
                        ExecuteRocketAttack(target, phaseMultiplier, damageBonus);
                        break;
                    case AttackType.Lasers:
                        ExecuteLaserAttack(target, phaseMultiplier, damageBonus);
                        break;
                }
            }
        }

        private void ExecuteCloneAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                Vector2 teleportPos = target.Center + ModdingusUtils.randomCorner() * 400;
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            }

            int cloneCount = currentPhase switch
            {
                BossPhase.Phase1 => 2,
                BossPhase.Phase2 => 3,
                BossPhase.Phase3 => 4,
                _ => 2
            };

            int spawnInterval = 60;
            for (int c = 0; c < cloneCount; c++)
            {
                int spawnTime = 60 + (c * spawnInterval);
                if (timer == spawnTime)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float angleStep = MathHelper.TwoPi / cloneCount;
                        float cloneAngle = angleStep * c + (timer * 0.01f);
                        Vector2 clonePos = target.Center + Vector2.One.RotatedBy(cloneAngle) * 300;

                        int attackStyle = currentPhase switch
                        {
                            BossPhase.Phase1 => c % 3,
                            BossPhase.Phase2 => c % 4,
                            BossPhase.Phase3 => c % 6,
                            _ => c % 3
                        };

                        int cloneProjectileId = Projectile.NewProjectile(
                            NPC.GetSource_FromAI(),
                            clonePos,
                            Vector2.Zero,
                            ModContent.ProjectileType<BossClone>(),
                            NPC.damage + damageBonus,
                            0f,
                            -1,
                            attackStyle,
                            0f,
                            0f
                        );

                        if (cloneProjectileId >= 0 && cloneProjectileId < Main.maxProjectiles)
                        {
                            var cloneProjectile = Main.projectile[cloneProjectileId].ModProjectile as BossClone;
                            if (cloneProjectile != null)
                            {
                                cloneProjectile.SetPhase((int)currentPhase + 1);
                            }

                            activeClones.Add(cloneProjectileId);

                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncProjectile, number: cloneProjectileId);
                                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                            }
                        }

                        SoundEngine.PlaySound(SoundID.Item8, clonePos);

                        Color effectColor = attackStyle switch
                        {
                            0 => Color.Orange,
                            1 => Color.Cyan,
                            2 => Color.Yellow,
                            3 => Color.Red,
                            4 => Color.Purple,
                            5 => Color.White,
                            _ => GetPhaseColor()
                        };

                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 20 * i) * Main.rand.NextFloat(3f, 8f);
                            Dust dust = Dust.NewDustPerfect(clonePos, DustID.Shadowflame, vel);
                            dust.noGravity = true;
                            dust.scale = Main.rand.NextFloat(1f, 1.8f);
                            dust.color = effectColor;
                        }
                    }
                }
            }

            if (timer > 120 && timer < 300)
            {
                Vector2 orbitCenter = target.Center;
                float orbitRadius = 350f;
                float orbitSpeed = 0.02f * phaseMultiplier;
                float orbitAngle = timer * orbitSpeed;

                Vector2 orbitPos = orbitCenter + Vector2.One.RotatedBy(orbitAngle) * orbitRadius;
                Vector2 moveDirection = (orbitPos - NPC.Center).SafeNormalize(Vector2.Zero);
                NPC.velocity = Vector2.Lerp(NPC.velocity, moveDirection * 4f, 0.1f);

                if (timer % (int)(40 / phaseMultiplier) == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 shotDirection = NPC.DirectionTo(target.Center);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shotDirection * 8f,
                            ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 3f, -1);
                    }
                    SoundEngine.PlaySound(SoundID.Item11, NPC.position);
                }
            }

            if (timer >= 400 || (timer > 240 && activeClones.Count == 0))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    foreach (int cloneId in activeClones)
                    {
                        if (cloneId >= 0 && cloneId < Main.maxProjectiles && Main.projectile[cloneId].active)
                        {
                            Main.projectile[cloneId].Kill();
                        }
                    }
                }
                activeClones.Clear();

                timer = 0;
                AIState = ActionState.Choose;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }
        }

        private void ExecuteSwordAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = false;
                currentWeapon = "";
            }

            int projectileCount = (int)(12 * phaseMultiplier);
            int attackDuration = (int)(220 / phaseMultiplier);

            float swordRange = currentPhase switch
            {
                BossPhase.Phase1 => 30f,
                BossPhase.Phase2 => 45f,
                BossPhase.Phase3 => 60f,
                _ => 30f
            };

            float swordSpeed = currentPhase switch
            {
                BossPhase.Phase1 => 15f,
                BossPhase.Phase2 => 20f,
                BossPhase.Phase3 => 25f,
                _ => 15f
            };

            int waveCount = currentPhase switch
            {
                BossPhase.Phase1 => 1,
                BossPhase.Phase2 => 2,
                BossPhase.Phase3 => 3,
                _ => 1
            };

            for (int wave = 0; wave < waveCount; wave++)
            {
                int waveTimer = (int)(60 / phaseMultiplier) + (wave * (int)(40 / phaseMultiplier));
                if (timer % waveTimer == 0 && timer < attackDuration)
                {
                    for (int i = 0; i < projectileCount; i++)
                    {
                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / projectileCount * i + (timer / 60) / 3 + (wave * 0.5f)) * swordRange;
                        Vector2 direction = NPC.Center.DirectionTo(pos);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, direction * (swordSpeed * phaseMultiplier),
                            ModContent.ProjectileType<Gss>(), 220 + damageBonus, 5);
                    }
                }
            }

            if (timer == attackDuration)
            {
                SoundStyle slashSound = SoundID.Item1;
                slashSound = slashSound with { Volume = 0.5f + (phaseMultiplier * 0.3f), Pitch = -0.2f + (phaseMultiplier * 0.1f) };
                SoundEngine.PlaySound(slashSound, NPC.position);

                Vector2 slashDirection = -Vector2.UnitY;

                int slashDamage = currentPhase switch
                {
                    BossPhase.Phase1 => 120 + damageBonus,
                    BossPhase.Phase2 => 140 + damageBonus, 
                    BossPhase.Phase3 => 160 + damageBonus,
                    _ => 120 + damageBonus
                };

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, slashDirection,
                    ModContent.ProjectileType<Slash>(), slashDamage, 15, -1,
                    NPC.whoAmI,
                    0f);
            }

            holdingWeapon = false;
            currentWeapon = "";

            if (timer == (int)(380 / phaseMultiplier))
            {
                holdingWeapon = false;
                currentWeapon = "";
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private void ExecuteGunAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = true;
                currentWeapon = "Gun";
            }

            if (timer == 30)
            {
                Vector2 aimDirection = NPC.DirectionTo(target.Center);
                NPC.velocity = aimDirection * (6f * phaseMultiplier);
                SoundEngine.PlaySound(SoundID.Item11, NPC.position);
            }

            if (timer >= 60 && timer <= 65)
            {
                NPC.velocity *= 0.8f;
            }

            if (timer > 65)
            {
                NPC.velocity = Vector2.Zero;
            }

            int burstInterval = currentPhase switch
            {
                BossPhase.Phase1 => 25,
                BossPhase.Phase2 => 20,
                BossPhase.Phase3 => 15,
                _ => 25
            };

            if (timer % burstInterval == 0 && timer >= 80 && timer <= 200)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int bulletCount = currentPhase switch
                    {
                        BossPhase.Phase1 => 3,
                        BossPhase.Phase2 => 4,
                        BossPhase.Phase3 => 5,
                        _ => 3
                    };

                    for (int i = 0; i < bulletCount; i++)
                    {
                        float spreadAngle = MathHelper.Lerp(-0.3f, 0.3f, i / (bulletCount - 1f));
                        Vector2 aimDirection = NPC.DirectionTo(target.Center).RotatedBy(spreadAngle);
                        float bulletSpeed = 12f + (currentPhase switch
                        {
                            BossPhase.Phase1 => 0f,
                            BossPhase.Phase2 => 2f,
                            BossPhase.Phase3 => 4f,
                            _ => 0f
                        });

                        Vector2 velocity = aimDirection * bulletSpeed;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 4f, -1);
                    }
                }
                SoundEngine.PlaySound(SoundID.Item40, NPC.position);
            }

            if (timer == 250)
            {
                int spreadCount = (int)(8 * phaseMultiplier);
                for (int i = 0; i < spreadCount; i++)
                {
                    float angle = MathHelper.TwoPi / spreadCount * i;
                    Vector2 spreadVel = Vector2.One.RotatedBy(angle) * (10f + phaseMultiplier * 2f);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, spreadVel,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 4f);
                }
                SoundEngine.PlaySound(SoundID.Item62, NPC.position);
            }

            if (timer >= (int)(320 / Math.Max(phaseMultiplier, 1.2f)))
            {
                holdingWeapon = false;
                currentWeapon = "";
                AIState = ActionState.Choose;
                timer = 0;
            }
        }

        private void ExecuteWhipAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = false;
                currentWeapon = "";

                Vector2 teleportPos = target.Center + ModdingusUtils.randomVector() * 300;
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
            }

            int hookCount = currentPhase switch
            {
                BossPhase.Phase1 => 1,
                BossPhase.Phase2 => 1,
                BossPhase.Phase3 => 1,
                _ => 1
            };

            for (int h = 0; h < hookCount; h++)
            {
                int hookTime = 30 + h * 180;
                if (timer == hookTime && timer <= 400)
                {
                    Vector2 hookDirection = NPC.DirectionTo(target.Center);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, hookDirection,
                        ModContent.ProjectileType<WhipExtension>(), NPC.damage + 15 + damageBonus, 8f, -1,
                        NPC.whoAmI, target.whoAmI, 0);

                    SoundEngine.PlaySound(SoundID.Item152, NPC.position);
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 8f, 15);
                }
            }

            int orbInterval = (int)(120 / phaseMultiplier);
            if (timer >= 100 && timer <= 300 && timer % orbInterval == 0)
            {
                int orbCount = (int)(3 * phaseMultiplier);
                for (int i = 0; i < orbCount; i++)
                {
                    Vector2 orbPos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / orbCount * i) * 120;
                    Vector2 orbVel = Vector2.One.RotatedBy(MathHelper.TwoPi / orbCount * i) * (4f * phaseMultiplier);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), orbPos, orbVel,
                        ModContent.ProjectileType<Bolt>(), NPC.damage + damageBonus, 3f);
                }
            }

            if (timer == (int)(420 / phaseMultiplier))
            {
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private void ExecuteHomingStarsAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = true;
                currentWeapon = "Staff";
                Vector2 teleportPos = target.Center + ModdingusUtils.randomCorner() * 450;
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            }

            int deployDuration = (int)(70 / phaseMultiplier);
            int deployInterval = Math.Max(1, (int)(10 / phaseMultiplier));

            if (timer >= 30 && timer <= 30 + deployDuration && timer % deployInterval == 0)
            {
                float deployAngle = (timer - 30) * (0.4f * phaseMultiplier);
                Vector2 starPos = NPC.Center + Vector2.One.RotatedBy(deployAngle) * 60;
                Vector2 starVel = Vector2.One.RotatedBy(deployAngle) * (2f * phaseMultiplier);

                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), starPos, starVel,
                    ModContent.ProjectileType<HomingStar>(), NPC.damage + damageBonus, 3f);

                SoundEngine.PlaySound(SoundID.Item9, NPC.position);
            }

            int retreatTime = (int)(100 / phaseMultiplier);
            if (timer == retreatTime)
            {
                Vector2 retreatPos = target.Center + ModdingusUtils.randomSide() * 500;
                NPC.Center = retreatPos;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            }

            int secondWaveStart = (int)(140 / phaseMultiplier);
            int secondWaveEnd = (int)(160 / phaseMultiplier);
            if (timer >= secondWaveStart && timer <= secondWaveEnd && timer % deployInterval == 0)
            {
                float waveAngle = (timer - secondWaveStart) * (0.5f * phaseMultiplier) + MathHelper.Pi;
                Vector2 starPos = NPC.Center + Vector2.One.RotatedBy(waveAngle) * 80;
                Vector2 starVel = Vector2.One.RotatedBy(waveAngle) * (1.5f * phaseMultiplier);

                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), starPos, starVel,
                    ModContent.ProjectileType<HomingStar>(), NPC.damage + 5 + damageBonus, 3f);

                SoundEngine.PlaySound(SoundID.Item9, NPC.position);
            }

            int chargeStart = (int)(200 / phaseMultiplier);
            int chargeEnd = (int)(230 / phaseMultiplier);
            if (timer >= chargeStart && timer <= chargeEnd)
            {
                if (timer % (int)(8 / phaseMultiplier) == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 dustPos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / 4 * i) * 30;
                        Dust dust = Dust.NewDustPerfect(dustPos, DustID.Shadowflame);
                        dust.velocity = Vector2.Zero;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                        dust.color = GetPhaseColor();
                    }
                }

                if (timer % (int)(20 / phaseMultiplier) == 0)
                {
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 8f, 10);
                }
            }

            int burstTime = (int)(250 / phaseMultiplier);
            if (timer == burstTime)
            {
                SoundEngine.PlaySound(SoundID.Item62, NPC.position);

                int burstCount = (int)(8 * phaseMultiplier);
                for (int i = 0; i < burstCount; i++)
                {
                    float burstAngle = MathHelper.TwoPi / burstCount * i;
                    Vector2 starPos = NPC.Center + Vector2.One.RotatedBy(burstAngle) * 40;
                    Vector2 starVel = Vector2.One.RotatedBy(burstAngle) * (5f * phaseMultiplier);

                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), starPos, starVel,
                        ModContent.ProjectileType<HomingStar>(), NPC.damage + 10 + damageBonus, 4f);

                    Main.projectile[proj].ai[1] = 1;
                    Main.projectile[proj].ai[0] = 15;
                }
            }

            if (timer == (int)(320 / phaseMultiplier))
            {
                holdingWeapon = false;
                currentWeapon = "";
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private void ExecuteRegularStarsAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = true;
                currentWeapon = "Staff";
                Vector2 teleportPos = target.Center + ModdingusUtils.randomCorner() * 350;
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            }

            int ringInterval = (int)(25 / phaseMultiplier);
            if (timer >= 40 && timer <= 180 && timer % ringInterval == 0)
            {
                float ringSize = 10 + (timer - 40) / ringInterval * (2 * phaseMultiplier);
                float baseSpeed = (7f + (timer - 40) / ringInterval * 1.5f) * phaseMultiplier;

                for (int i = 0; i < (int)ringSize; i++)
                {
                    float angle = MathHelper.TwoPi / ringSize * i + (timer * 0.05f * phaseMultiplier);
                    Vector2 starPos = NPC.Center + Vector2.One.RotatedBy(angle) * 50;
                    Vector2 starVel = Vector2.One.RotatedBy(angle) * baseSpeed;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), starPos, starVel,
                        ModContent.ProjectileType<RegularStar>(), NPC.damage + damageBonus, 3f);
                }
                SoundEngine.PlaySound(SoundID.Item9, NPC.position);
            }

            if (timer == 220)
            {
                Vector2 newPos = target.Center + ModdingusUtils.randomSide() * 300;
                NPC.Center = newPos;

                for (int dir = 0; dir < 4; dir++)
                {
                    float baseAngle = MathHelper.PiOver2 * dir;
                    int streamCount = (int)(8 * phaseMultiplier);
                    for (int j = 0; j < streamCount; j++)
                    {
                        float spreadAngle = baseAngle + MathHelper.Lerp(-0.3f, 0.3f, j / (streamCount - 1f));
                        Vector2 starVel = Vector2.One.RotatedBy(spreadAngle) * ((8f + j * 0.5f) * phaseMultiplier);

                        Vector2 starPos = NPC.Center;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), starPos, starVel,
                            ModContent.ProjectileType<RegularStar>(), NPC.damage + 5 + damageBonus, 3f);
                    }
                }
                SoundEngine.PlaySound(SoundID.Item10, NPC.position);
            }

            int showerInterval = (int)(4 / phaseMultiplier);
            if (timer >= 280 && timer <= 320 && timer % showerInterval == 0)
            {
                Vector2 randomDir = ModdingusUtils.randomVector();
                Vector2 starPos = NPC.Center + randomDir * Main.rand.NextFloat(60f, 100f);
                Vector2 starVel = randomDir * Main.rand.NextFloat(9f * phaseMultiplier, 15f * phaseMultiplier);

                Projectile.NewProjectile(NPC.GetSource_FromAI(), starPos, starVel,
                    ModContent.ProjectileType<RegularStar>(), NPC.damage + damageBonus, 3f);
            }

            if (timer == (int)(370 / phaseMultiplier))
            {
                holdingWeapon = false;
                currentWeapon = "";
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private void ExecuteBulletRainAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = true;
                currentWeapon = "Gun";

                Vector2 teleportPos = target.Center + new Vector2(Main.rand.Next(-400, 400), -300);
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item11, NPC.position);
            }

            if (timer >= 30 && timer <= 120)
            {
                float moveSpeed = 3f + phaseMultiplier;
                Vector2 moveDirection = NPC.DirectionTo(target.Center) * moveSpeed;
                NPC.velocity = Vector2.Lerp(NPC.velocity, moveDirection, 0.1f);

                int shootInterval = currentPhase switch
                {
                    BossPhase.Phase1 => 30,
                    BossPhase.Phase2 => 20,
                    BossPhase.Phase3 => 15,
                    _ => 30
                };

                if (timer % shootInterval == 0)
                {
                    int bulletCount = currentPhase switch
                    {
                        BossPhase.Phase1 => 3,
                        BossPhase.Phase2 => 4,
                        BossPhase.Phase3 => 6,
                        _ => 3
                    };

                    for (int i = 0; i < bulletCount; i++)
                    {
                        float spreadAngle = MathHelper.Lerp(-0.4f, 0.4f, i / (bulletCount - 1f));
                        Vector2 aimDirection = NPC.DirectionTo(target.Center).RotatedBy(spreadAngle);
                        float bulletSpeed = 12f + phaseMultiplier * 3f;

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, aimDirection * bulletSpeed,
                            ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 4f);
                    }
                    SoundEngine.PlaySound(SoundID.Item40, NPC.position);
                }
            }

            if (timer == 150)
            {
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item62, NPC.position);

                string warningText = currentPhase switch
                {
                    BossPhase.Phase1 => "Bullets rain from above!",
                    BossPhase.Phase2 => "Heavy bullet barrage incoming!",
                    BossPhase.Phase3 => "Devastating bullet storm!",
                    _ => "Bullets rain from above!"
                };

                Main.NewText(warningText, GetPhaseColor());
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 15f + phaseMultiplier * 5f, 30);
            }

            if (timer == 180)
            {
                int bulletColumns = currentPhase switch
                {
                    BossPhase.Phase1 => 8,
                    BossPhase.Phase2 => 12,
                    BossPhase.Phase3 => 16,
                    _ => 8
                };

                for (int i = 0; i < bulletColumns; i++)
                {
                    Vector2 position = new Vector2(
                        target.Center.X - 1200 + i * (2400f / bulletColumns),
                        target.Center.Y - 800
                    );

                    float randomAngle = currentPhase switch
                    {
                        BossPhase.Phase1 => Main.rand.NextFloat(-0.1f, 0.1f),
                        BossPhase.Phase2 => Main.rand.NextFloat(-0.2f, 0.2f),
                        BossPhase.Phase3 => Main.rand.NextFloat(-0.3f, 0.3f),
                        _ => Main.rand.NextFloat(-0.1f, 0.1f)
                    };

                    Vector2 velocity = Vector2.UnitY.RotatedBy(randomAngle) * (20f + phaseMultiplier * 8f);
                    float delay = i * (8f / phaseMultiplier);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 4f, -1, 0, 120 + delay);
                }
            }

            if (currentPhase == BossPhase.Phase2 && timer == 220)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 position = new Vector2(
                        target.Center.X - 1000 + i * 200f,
                        target.Center.Y - 900
                    );

                    Vector2 velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f)) * 28f;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus + 5, 4f, -1, 0, 90 + i * 6f);
                }
            }

            if (currentPhase == BossPhase.Phase3 && timer == 220)
            {
                for (int i = 0; i < 14; i++)
                {
                    Vector2 position = new Vector2(
                        target.Center.X - 1400 + i * 200f,
                        target.Center.Y - 900
                    );

                    Vector2 velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * 32f;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus + 10, 4f, -1, 0, 70 + i * 4f);
                }
            }

            if (currentPhase == BossPhase.Phase3 && timer == 260)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 position = new Vector2(
                        target.Center.X - 1200 + i * 200f,
                        target.Center.Y - 1000
                    );

                    Vector2 velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * 35f;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus + 15, 5f, -1, 0, 60 + i * 3f);
                }
            }

            int endTime = currentPhase switch
            {
                BossPhase.Phase1 => 280,
                BossPhase.Phase2 => 320,
                BossPhase.Phase3 => 360,
                _ => 280
            };

            if (timer >= endTime)
            {
                holdingWeapon = false;
                currentWeapon = "";
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private void ExecuteRocketAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = true;
                currentWeapon = "Gun";

                Vector2 teleportPos = target.Center + ModdingusUtils.randomVector() * 200;
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item14, NPC.position);
            }

            int chargeTime = currentPhase switch
            {
                BossPhase.Phase1 => 60,
                BossPhase.Phase2 => 80,
                BossPhase.Phase3 => 100,
                _ => 60
            };

            if (timer >= 30 && timer <= 30 + chargeTime)
            {
                NPC.velocity *= 0.95f;

                int particleFreq = currentPhase switch
                {
                    BossPhase.Phase1 => 8,
                    BossPhase.Phase2 => 5,
                    BossPhase.Phase3 => 3,
                    _ => 8
                };

                if (timer % particleFreq == 0)
                {
                    int particleCount = currentPhase switch
                    {
                        BossPhase.Phase1 => 4,
                        BossPhase.Phase2 => 6,
                        BossPhase.Phase3 => 8,
                        _ => 4
                    };

                    for (int i = 0; i < particleCount; i++)
                    {
                        Vector2 dustPos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / particleCount * i) * (30 + timer);
                        Dust dust = Dust.NewDustPerfect(dustPos, DustID.Torch);
                        dust.velocity = Vector2.Zero;
                        dust.noGravity = true;
                        dust.scale = 1f + phaseMultiplier * 0.5f;
                        dust.color = Color.Lerp(Color.Orange, Color.Red, phaseMultiplier / 2f);
                    }
                }

                if (timer % 10 == 0)
                {
                    float intensity = ((timer - 30f) / chargeTime) * (10f + phaseMultiplier * 5f);
                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, intensity, 10);
                }
            }

            if (timer == 30 + chargeTime + 10)
            {
                SoundEngine.PlaySound(SoundID.Item62, NPC.position);

                string warningText = currentPhase switch
                {
                    BossPhase.Phase1 => "Incoming rockets!",
                    BossPhase.Phase2 => "Massive rocket barrage!",
                    BossPhase.Phase3 => "APOCALYPTIC ROCKET STORM!",
                    _ => "Incoming rockets!"
                };

                Main.NewText(warningText, GetPhaseColor());

                int rocketCount = currentPhase switch
                {
                    BossPhase.Phase1 => 6,
                    BossPhase.Phase2 => 10,
                    BossPhase.Phase3 => 16,
                    _ => 6
                };

                for (int i = 0; i < rocketCount; i++)
                {
                    float angle = MathHelper.TwoPi / rocketCount * i;
                    Vector2 direction = Vector2.One.RotatedBy(angle);
                    float speed = currentPhase switch
                    {
                        BossPhase.Phase1 => 6f,
                        BossPhase.Phase2 => 8f,
                        BossPhase.Phase3 => 10f,
                        _ => 6f
                    };

                    Vector2 velocity = direction * speed;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<BossRocket>(), NPC.damage + damageBonus, 6f);
                }

                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 20f + phaseMultiplier * 10f, 40);
            }

            if (currentPhase == BossPhase.Phase2 && timer == 30 + chargeTime + 50)
            {
                for (int i = 0; i < 6; i++)
                {
                    float spreadAngle = MathHelper.Lerp(-0.6f, 0.6f, i / 5f);
                    Vector2 aimDirection = NPC.DirectionTo(target.Center).RotatedBy(spreadAngle);
                    Vector2 velocity = aimDirection * 9f;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<BossRocket>(), NPC.damage + damageBonus + 5, 8f);
                }
                SoundEngine.PlaySound(SoundID.Item14, NPC.position);
            }

            if (currentPhase == BossPhase.Phase3)
            {
                if (timer == 30 + chargeTime + 40)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float angle = MathHelper.TwoPi / 12 * i + MathHelper.PiOver4;
                        Vector2 direction = Vector2.One.RotatedBy(angle);
                        Vector2 velocity = direction * 9f;

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<BossRocket>(), NPC.damage + damageBonus + 8, 8f);
                    }
                }

                if (timer == 30 + chargeTime + 70)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        float spreadAngle = MathHelper.Lerp(-0.8f, 0.8f, i / 7f);
                        Vector2 aimDirection = NPC.DirectionTo(target.Center).RotatedBy(spreadAngle);
                        Vector2 velocity = aimDirection * 12f;

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<BossRocket>(), NPC.damage + damageBonus + 12, 10f);
                    }
                    SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                }
            }

            int endTime = currentPhase switch
            {
                BossPhase.Phase1 => 180,
                BossPhase.Phase2 => 220,
                BossPhase.Phase3 => 280,
                _ => 180
            };

            if (timer >= endTime)
            {
                holdingWeapon = false;
                currentWeapon = "";
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private int[] laserProjectileIds = new int[12];
        private int initialLaserId = -1;

        private void ExecuteLaserAttack(Player target, float phaseMultiplier, int damageBonus)
        {
            if (timer == 1)
            {
                holdingWeapon = true;
                currentWeapon = "Staff";

                Vector2 teleportPos = target.Center + new Vector2(0, -200);
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item72, NPC.position);

                for (int i = 0; i < laserProjectileIds.Length; i++)
                {
                    laserProjectileIds[i] = -1;
                }
                initialLaserId = -1;
            }

            if (timer >= 20 && timer <= 25)
            {
                Vector2 dashDirection = NPC.DirectionTo(target.Center);
                NPC.velocity = dashDirection * 8f;
            }

            if (timer > 25 && timer <= 35)
            {
                NPC.velocity *= 0.8f;
            }

            if (timer == 40)
            {
                Vector2 laserDirection = NPC.velocity.SafeNormalize(Vector2.UnitX);
                initialLaserId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, laserDirection,
                    ModContent.ProjectileType<BossLaser>(), NPC.damage + damageBonus, 5f, -1, 0, NPC.whoAmI, 20);

                if (initialLaserId >= 0 && initialLaserId < Main.maxProjectiles)
                {
                    Main.projectile[initialLaserId].timeLeft = 80;
                }
            }

            if (timer == 95)
            {
                if (initialLaserId >= 0 && initialLaserId < Main.maxProjectiles && Main.projectile[initialLaserId].active)
                {
                    Main.projectile[initialLaserId].Kill();
                }
                initialLaserId = -1;
            }

            if (timer == 100)
            {
                NPC.Center = target.Center + new Vector2(0, -100);
                NPC.velocity = -Vector2.UnitY * 2f;

                Main.NewText("Laser barrage incoming!", Color.Cyan);

                int laserCount = currentPhase switch
                {
                    BossPhase.Phase1 => 4,
                    BossPhase.Phase2 => 6,
                    BossPhase.Phase3 => 10,
                    _ => 4
                };

                for (int i = 0; i < laserProjectileIds.Length; i++)
                {
                    if (laserProjectileIds[i] >= 0 && laserProjectileIds[i] < Main.maxProjectiles && Main.projectile[laserProjectileIds[i]].active)
                    {
                        Main.projectile[laserProjectileIds[i]].Kill();
                    }
                    laserProjectileIds[i] = -1;
                }

                for (int i = 0; i < Math.Min(laserCount, laserProjectileIds.Length); i++)
                {
                    float angle = (MathHelper.TwoPi / laserCount) * i;
                    Vector2 laserDirection = Vector2.One.RotatedBy(angle);

                    int laserId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, laserDirection,
                        ModContent.ProjectileType<BossLaser>(), NPC.damage + damageBonus, 5f, -1, 0, NPC.whoAmI, 20);

                    if (laserId >= 0 && laserId < Main.maxProjectiles)
                    {
                        laserProjectileIds[i] = laserId;
                        Main.projectile[laserId].timeLeft = 200;
                    }
                }

                SoundEngine.PlaySound(SoundID.Item72, NPC.position);
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 20f, 60);
            }

            if (timer > 100 && timer <= 250)
            {
                float rotationSpeed = 0.02f * phaseMultiplier;

                for (int i = 0; i < laserProjectileIds.Length; i++)
                {
                    int laserId = laserProjectileIds[i];
                    if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                    {
                        Main.projectile[laserId].velocity = Main.projectile[laserId].velocity.RotatedBy(rotationSpeed);
                    }
                }

                if (timer % 8 == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 dustPos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / 4 * i) * 30;
                        Dust dust = Dust.NewDustPerfect(dustPos, DustID.Electric);
                        dust.velocity = Vector2.Zero;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                        dust.color = Color.Cyan;
                    }
                }
            }

            if (timer == 250)
            {
                for (int i = 0; i < laserProjectileIds.Length; i++)
                {
                    int laserId = laserProjectileIds[i];
                    if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                    {
                        Main.projectile[laserId].Kill();
                    }
                    laserProjectileIds[i] = -1;
                }
            }

            if (currentPhase != BossPhase.Phase1 && timer == 280)
            {
                Vector2 dashDirection = NPC.DirectionTo(target.Center);
                NPC.velocity = dashDirection * 10f;

                int finalLaserId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dashDirection,
                    ModContent.ProjectileType<BossLaser>(), NPC.damage + damageBonus + 15, 8f, -1, 0, NPC.whoAmI, 20);

                if (finalLaserId >= 0 && finalLaserId < Main.maxProjectiles)
                {
                    Main.projectile[finalLaserId].timeLeft = 100;
                }
            }

            if (timer >= (int)(350 / Math.Max(phaseMultiplier, 1.2f)))
            {
                if (initialLaserId >= 0 && initialLaserId < Main.maxProjectiles && Main.projectile[initialLaserId].active)
                {
                    Main.projectile[initialLaserId].Kill();
                }

                for (int i = 0; i < laserProjectileIds.Length; i++)
                {
                    int laserId = laserProjectileIds[i];
                    if (laserId >= 0 && laserId < Main.maxProjectiles && Main.projectile[laserId].active)
                    {
                        Main.projectile[laserId].Kill();
                    }
                    laserProjectileIds[i] = -1;
                }

                holdingWeapon = false;
                currentWeapon = "";
                timer = 0;
                AIState = ActionState.Choose;
            }
        }

        private void ExecuteDeathMode()
        {
            Player target = Main.player[NPC.target];

            if (deathModeTimer >= 2700)
            {
                foreach (int projId in deathModeProjectiles)
                {
                    if (projId >= 0 && projId < Main.maxProjectiles && Main.projectile[projId].active)
                    {
                        Main.projectile[projId].Kill();
                    }
                }
                deathModeProjectiles.Clear();

                foreach (int cloneId in activeClones)
                {
                    if (cloneId >= 0 && cloneId < Main.maxProjectiles && Main.projectile[cloneId].active)
                    {
                        Main.projectile[cloneId].Kill();
                    }
                }
                activeClones.Clear();

                Main.NewText("The Top Hat God's rage subsides...", Color.Gray);

                inDeathMode = false;
                NPC.dontTakeDamage = false;
                NPC.life = 0;
                NPC.checkDead();
                return;
            }

            NPC.life = 1;

            if (timer % 5 == 0)
            {
                Vector2 dustPos = NPC.Center + Main.rand.NextVector2Circular(60, 60);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Shadowflame);
                dust.velocity = Main.rand.NextVector2Circular(3, 3);
                dust.noGravity = true;
                dust.scale = 2f;
                dust.color = Color.Lerp(Color.Red, Color.Black, Main.rand.NextFloat());
            }

            if (timer % 60 == 0)
            {
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 20f, 30);
            }

            ExecuteDeathModeAttacks(target);
        }

        private void ExecuteDeathModeAttacks(Player target)
        {
            if (deathModeTimer < 900)
            {
                BulletHellPhase(target);
            }
            else if (deathModeTimer < 1800)
            {
                LaserChaosPhase(target);
            }
            else
            {
                FinalDesperationPhase(target);
            }
        }

        private void BulletHellPhase(Player target)
        {
            if (timer % 120 == 0)
            {
                Vector2 teleportPos = target.Center + Main.rand.NextVector2Circular(400, 400);
                NPC.Center = teleportPos;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);

                for (int i = 0; i < 20; i++)
                {
                    Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 20 * i) * 8f;
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Shadowflame, vel);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                    dust.color = Color.Red;
                }
            }

            if (timer % 5 == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    float angle = MathHelper.TwoPi / 8 * i + (timer * 0.1f);
                    Vector2 vel = Vector2.One.RotatedBy(angle) * 15f;

                    int projId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + 30, 6f);
                    deathModeProjectiles.Add(projId);
                }

                if (timer % 20 == 0) SoundEngine.PlaySound(SoundID.Item11, NPC.position);
            }

            if (timer % 30 == 0)
            {
                Vector2 aimDirection = NPC.DirectionTo(target.Center);
                for (int i = 0; i < 5; i++)
                {
                    Vector2 vel = aimDirection.RotatedBy(MathHelper.Lerp(-0.5f, 0.5f, i / 4f)) * 20f;
                    int projId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + 25, 5f);
                    deathModeProjectiles.Add(projId);
                }
            }

            if (timer % 60 == 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    Vector2 position = new Vector2(
                        target.Center.X - 800 + i * 100f,
                        target.Center.Y - 600
                    );
                    Vector2 velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 25f;

                    int projId = Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + 20, 4f, -1, 0, 60 + i * 3f);
                    deathModeProjectiles.Add(projId);
                }
            }
        }

        private void LaserChaosPhase(Player target)
        {
            if (timer % 180 == 0)
            {
                Vector2 teleportPos = target.Center + new Vector2(0, -250);
                NPC.Center = teleportPos;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item72, NPC.position);

                for (int i = 0; i < 50; i++)
                {
                    Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 50 * i) * Main.rand.NextFloat(5f, 15f);
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Electric, vel);
                    dust.noGravity = true;
                    dust.scale = 2f;
                    dust.color = Color.Cyan;
                }
            }

            if (timer % 90 == 0)
            {
                float startAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                for (int i = 0; i < 6; i++)
                {
                    float angle = startAngle + (MathHelper.TwoPi / 6) * i;
                    Vector2 laserDirection = Vector2.One.RotatedBy(angle);

                    int laserId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, laserDirection,
                        ModContent.ProjectileType<BossLaser>(), NPC.damage + 40, 8f, -1, 0, NPC.whoAmI, 25);

                    if (laserId >= 0)
                    {
                        Main.projectile[laserId].timeLeft = 180;
                        deathModeProjectiles.Add(laserId);
                    }
                }
            }

            if (timer > 90)
            {
                foreach (int projId in deathModeProjectiles.ToList())
                {
                    if (projId >= 0 && projId < Main.maxProjectiles && Main.projectile[projId].active && Main.projectile[projId].type == ModContent.ProjectileType<BossLaser>())
                    {
                        Main.projectile[projId].velocity = Main.projectile[projId].velocity.RotatedBy(0.04f);
                    }
                }
            }

            if (timer % 120 == 60)
            {
                Vector2 dashDirection = NPC.DirectionTo(target.Center);
                NPC.velocity = dashDirection * 25f;

                int laserId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dashDirection,
                    ModContent.ProjectileType<BossLaser>(), NPC.damage + 35, 10f, -1, 0, NPC.whoAmI, 15);

                if (laserId >= 0)
                {
                    Main.projectile[laserId].timeLeft = 60;
                    deathModeProjectiles.Add(laserId);
                }
            }

            if (timer % 120 > 60 && timer % 120 < 80)
            {
                NPC.velocity *= 0.9f;
            }
        }

        private void FinalDesperationPhase(Player target)
        {
            if (timer % 60 == 0)
            {
                Vector2 teleportPos = target.Center + Main.rand.NextVector2Circular(300, 300);
                NPC.Center = teleportPos;
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            }

            if (timer % 3 == 0)
            {
                Vector2 randomDirection = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                Vector2 vel = randomDirection * Main.rand.NextFloat(12f, 25f);

                int projId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                    ModContent.ProjectileType<BossBullet>(), NPC.damage + 35, 7f);
                deathModeProjectiles.Add(projId);
            }

            if (timer % 20 == 0)
            {
                Vector2 laserDirection = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));

                int laserId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, laserDirection,
                    ModContent.ProjectileType<BossLaser>(), NPC.damage + 45, 12f, -1, 0, NPC.whoAmI, 10);

                if (laserId >= 0)
                {
                    Main.projectile[laserId].timeLeft = 80;
                    deathModeProjectiles.Add(laserId);
                }
            }

            if (timer % 40 == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 rocketDirection = Vector2.One.RotatedBy(MathHelper.TwoPi / 4 * i + Main.rand.NextFloat(-0.2f, 0.2f));
                    Vector2 vel = rocketDirection * 12f;

                    int projId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                        ModContent.ProjectileType<BossRocket>(), NPC.damage + 40, 8f);
                    deathModeProjectiles.Add(projId);
                }
                SoundEngine.PlaySound(SoundID.Item14, NPC.position);
            }

            if (timer % 15 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 starDirection = Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    Vector2 vel = starDirection * Main.rand.NextFloat(8f, 16f);

                    int starType = Main.rand.NextBool() ? ModContent.ProjectileType<RegularStar>() : ModContent.ProjectileType<HomingStar>();
                    int projId = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                        starType, NPC.damage + 30, 6f);
                    deathModeProjectiles.Add(projId);
                }
            }

            if (timer % 180 == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    float angle = MathHelper.TwoPi / 6 * i;
                    Vector2 clonePos = target.Center + Vector2.One.RotatedBy(angle) * 400;

                    int cloneId = Projectile.NewProjectile(NPC.GetSource_FromAI(), clonePos, Vector2.Zero,
                        ModContent.ProjectileType<BossClone>(), NPC.damage + 50, 0f, Main.myPlayer, 5, 0f, 0f);

                    if (cloneId >= 0)
                    {
                        var clone = Main.projectile[cloneId].ModProjectile as BossClone;
                        if (clone != null)
                        {
                            clone.SetPhase(4);
                        }
                        deathModeProjectiles.Add(cloneId);
                    }
                }
            }

            int timeLeft = 2700 - (int)deathModeTimer;
            if (timeLeft <= 300 && timeLeft % 60 == 0)
            {
                int secondsLeft = timeLeft / 60;
                Main.NewText($"The rage fades in {secondsLeft}...", Color.Yellow);
                ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 30f, 20);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (AIState == ActionState.Spawn)
            {
                NPC.frame.Y = 0;
            }

            int frameSpeed = 5;
            NPC.frameCounter += 0.5f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float tl = (float)NPC.oldPos.Length;
            float scale = NPC.scale;
            Main.instance.LoadNPC(Type);
            Texture2D t = TextureAssets.Npc[Type].Value;
            Rectangle source = new Rectangle(0, NPC.frame.Y, t.Width, NPC.frame.Height);
            Vector2 weaponOffset = Vector2.Zero;

            Color trailColor = inDeathMode ? Color.Red : GetPhaseColor();
            Color mainColor = inDeathMode ? Color.Lerp(drawColor, Color.Red, 0.8f) : Color.Lerp(drawColor, GetPhaseColor(), 0.3f);

            int trailSegments = inDeathMode ? 6 : 3;
            for (float i = 0; i < tl; i += (float)(tl / trailSegments))
            {
                float percent = i / tl;
                Vector2 dpos = NPC.oldPos[(int)i] - screenPos + new Vector2(t.Width * scale / 4, NPC.height * scale / 2);
                Color trailAlpha = trailColor * (1 - percent) * (inDeathMode ? 1.5f : 1f);
                spriteBatch.Draw(t, dpos, source, trailAlpha, NPC.rotation, NPC.origin(), scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            Vector2 mainDrawPos = NPC.Center - screenPos;

            if (inDeathMode)
            {
                float pulse = 1f + 0.3f * (float)Math.Sin(deathModeTimer * 0.2f);
                scale *= pulse;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 offset = Vector2.One.RotatedBy(deathModeTimer * 0.1f + i * MathHelper.TwoPi / 3) * (2f + i);
                    Color glowColor = Color.Red * (0.5f - i * 0.1f);
                    spriteBatch.Draw(t, mainDrawPos + offset, source, glowColor, NPC.rotation, NPC.origin(), scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }

            spriteBatch.Draw(t, mainDrawPos, source, mainColor, NPC.rotation, NPC.origin(), scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            if (holdingWeapon && !string.IsNullOrEmpty(currentWeapon))
            {
                try
                {
                    Texture2D weaponTexture = ModContent.Request<Texture2D>($"PurringTale/CatBoss/Assets/{currentWeapon}").Value;

                    //Vector2 weaponOffset;
                    float weaponRotation = 0f;
                    SpriteEffects weaponEffects = SpriteEffects.None;
                    Vector2 weaponOrigin = weaponTexture.Size() / 2f;

                    if (currentWeapon == "Gun")
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
                            shotgunTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/Gun_Left").Value;
                            drawRotation = directionToPlayer.ToRotation() + MathHelper.Pi;
                            shotgunOrigin = new Vector2(shotgunTexture.Width * 0.8f, shotgunTexture.Height * 0.5f);
                            positionOffset = Vector2.UnitX.RotatedBy(drawRotation) * -20f;
                        }
                        else
                        {
                            shotgunTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/Gun").Value;
                            drawRotation = directionToPlayer.ToRotation();
                            shotgunOrigin = new Vector2(shotgunTexture.Width * 0.2f, shotgunTexture.Height * 0.5f);
                            positionOffset = Vector2.UnitX.RotatedBy(drawRotation) * 20f;
                        }

                        Vector2 shotgunPosition = NPC.Center - screenPos + positionOffset;

                        spriteBatch.Draw(shotgunTexture, shotgunPosition, null, drawColor, drawRotation,
                            shotgunOrigin, NPC.scale * 0.8f, SpriteEffects.None, 0f);
                    }
                    else if (currentWeapon == "Staff")
                    {
                        Texture2D staffTexture;
                        Vector2 staffOrigin;

                        if (NPC.direction == 1)
                        {
                            staffTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/Staff").Value;
                            staffOrigin = new Vector2(staffTexture.Width * 0.2f, staffTexture.Height * 0.8f);
                            weaponOffset = new Vector2(8, 5);
                            weaponEffects = SpriteEffects.None;
                            weaponRotation = MathHelper.PiOver4 * 0.3f;
                        }
                        else
                        {
                            staffTexture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/Staff").Value;
                            staffOrigin = new Vector2(staffTexture.Width * 0.8f, staffTexture.Height * 0.8f);
                            weaponOffset = new Vector2(-8, 5);
                            weaponEffects = SpriteEffects.FlipHorizontally;
                            weaponRotation = -MathHelper.PiOver4 * 0.3f;
                        }

                        Vector2 staffPosition = NPC.Center - screenPos + weaponOffset;

                        spriteBatch.Draw(staffTexture, staffPosition, null, drawColor, weaponRotation,
                            staffOrigin, NPC.scale * 0.8f, weaponEffects, 0f);
                    }
                    else if (currentWeapon == "Sword")
                    {
                        if (NPC.direction == 1)
                        {
                            weaponOffset = new Vector2(22, -8);
                            weaponEffects = SpriteEffects.None;
                            weaponRotation = MathHelper.PiOver4 * 0.7f;
                        }
                        else
                        {
                            weaponOffset = new Vector2(-22, -8);
                            weaponEffects = SpriteEffects.FlipHorizontally;
                            weaponRotation = -MathHelper.PiOver4 * 0.7f;
                        }
                    }
                    else if (currentWeapon == "Whip")
                    {
                        Player target = Main.player[NPC.target];
                        if (target.active)
                        {
                            Vector2 directionToTarget = Vector2.Normalize(target.Center - NPC.Center);
                            weaponRotation = directionToTarget.ToRotation();

                            if (NPC.direction == 1)
                            {
                                weaponOffset = new Vector2(25, -8);
                                weaponEffects = SpriteEffects.None;
                            }
                            else
                            {
                                weaponOffset = new Vector2(-25, -8);
                                weaponEffects = SpriteEffects.FlipVertically;
                                weaponRotation += MathHelper.Pi;
                            }
                        }
                        else
                        {
                            if (NPC.direction == 1)
                            {
                                weaponOffset = new Vector2(25, -8);
                                weaponEffects = SpriteEffects.None;
                                weaponRotation = 0f;
                            }
                            else
                            {
                                weaponOffset = new Vector2(-25, -8);
                                weaponEffects = SpriteEffects.FlipVertically;
                                weaponRotation = MathHelper.Pi;
                            }
                        }
                    }
                    else
                    {
                        if (NPC.direction == 1)
                        {
                            weaponOffset = new Vector2(18, -8);
                            weaponEffects = SpriteEffects.None;
                        }
                        else
                        {
                            weaponOffset = new Vector2(-18, -8);
                            weaponEffects = SpriteEffects.FlipHorizontally;
                        }
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (NPC.life - modifiers.FinalDamage.Base <= 0 && !deathModeTriggered)
            {
                int damageToTake = NPC.life - 1;
                if (damageToTake > 0)
                {
                    modifiers.FinalDamage.Base = damageToTake;
                }
                else
                {
                    modifiers.FinalDamage.Base = 0;
                }
                modifiers.DisableKnockback();

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }

            if (inDeathMode)
            {
                modifiers.FinalDamage.Base = 0;
                modifiers.DisableKnockback();
            }
        }

        public override bool CheckDead()
        {
            if (NPC.life <= 1 && !deathModeTriggered)
            {
                NPC.life = 1;
                return false;
            }

            if (inDeathMode && deathModeTimer < 2700)
            {
                NPC.life = 1;
                return false;
            }

            return true;
        }

        public override void PostAI()
        {
            if (NPC.life <= 1 && !deathModeTriggered)
            {
                TriggerDeathMode();
            }
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedTopHat, -1);
        }
    }
}