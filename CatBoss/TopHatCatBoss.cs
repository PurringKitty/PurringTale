using System;
using System.Collections.Generic;
using System.IO;
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
        Clones
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
            NPC.lifeMax = 500000;
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

            CheckPhaseTransition();

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

            if (player.dead)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            ChooseAction();

            ShaderTimer++;
            timer++;
            oldState = AIState;
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
                BossPhase.Phase3 => Color.Black,
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
                    BossPhase.Phase1 => 5,
                    BossPhase.Phase2 => 6,
                    BossPhase.Phase3 => 7,
                    _ => 5
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
                        3 => AttackType.Clones,
                        4 => AttackType.Sword,
                        _ => AttackType.HomingStars
                    };
                    break;

                case BossPhase.Phase2:
                    AtkType = atkCounter switch
                    {
                        0 => AttackType.Gun,
                        1 => AttackType.HomingStars,
                        2 => AttackType.Clones,
                        3 => AttackType.Whip,
                        4 => AttackType.Sword,
                        5 => AttackType.RegularStars,
                        _ => AttackType.Gun
                    };
                    break;

                case BossPhase.Phase3:
                    AtkType = atkCounter switch
                    {
                        0 => AttackType.Gun,
                        1 => AttackType.Clones,
                        2 => AttackType.Whip,
                        3 => AttackType.HomingStars,
                        4 => AttackType.Sword,
                        5 => AttackType.Gun,
                        6 => AttackType.RegularStars,
                        _ => AttackType.Gun
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
                    float angleStep = MathHelper.TwoPi / cloneCount;
                    float cloneAngle = angleStep * c + (timer * 0.01f);
                    Vector2 clonePos = target.Center + Vector2.One.RotatedBy(cloneAngle) * 300;

                    int cloneProjectileId = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        clonePos,
                        Vector2.Zero,
                        ModContent.ProjectileType<BossClone>(),
                        NPC.damage + damageBonus,
                        0f,
                        Main.myPlayer,
                        c % 3,
                        0f,
                        0f
                    );

                    if (cloneProjectileId >= 0 && cloneProjectileId < Main.maxProjectiles)
                    {
                        activeClones.Add(cloneProjectileId);
                        SoundEngine.PlaySound(SoundID.Item8, clonePos);

                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 vel = Vector2.One.RotatedBy(MathHelper.TwoPi / 20 * i) * Main.rand.NextFloat(3f, 8f);
                            Dust dust = Dust.NewDustPerfect(clonePos, DustID.Shadowflame, vel);
                            dust.noGravity = true;
                            dust.scale = Main.rand.NextFloat(1f, 1.8f);
                            dust.color = GetPhaseColor();
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
                    Vector2 shotDirection = NPC.DirectionTo(target.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shotDirection * 8f,
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 3f);
                    SoundEngine.PlaySound(SoundID.Item11, NPC.position);
                }
            }

            if (timer >= 400 || (timer > 240 && activeClones.Count == 0))
            {
                foreach (int cloneId in activeClones)
                {
                    if (cloneId >= 0 && cloneId < Main.maxProjectiles && Main.projectile[cloneId].active)
                    {
                        Main.projectile[cloneId].Kill();
                    }
                }
                activeClones.Clear();

                timer = 0;
                AIState = ActionState.Choose;
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
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, slashDirection,
                    ModContent.ProjectileType<Slash>(), 220 + damageBonus + (currentPhase == BossPhase.Phase3 ? 50 : 0), 15, -1,
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
                        ModContent.ProjectileType<BossBullet>(), NPC.damage + damageBonus, 4f);
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

            Color trailColor = GetPhaseColor();

            for (float i = 0; i < tl; i += (float)(tl / 3))
            {
                float percent = i / tl;
                Vector2 dpos = NPC.oldPos[(int)i] - screenPos + new Vector2(t.Width * scale / 4, NPC.height * scale / 2);
                spriteBatch.Draw(t, dpos, source, trailColor * (1 - percent), NPC.rotation, NPC.origin(), scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }

            Vector2 mainDrawPos = NPC.Center - screenPos;
            Color phaseDrawColor = Color.Lerp(drawColor, GetPhaseColor(), 0.3f);
            spriteBatch.Draw(t, mainDrawPos, source, phaseDrawColor, NPC.rotation, NPC.origin(), scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

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

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedTopHat, -1);
        }
    }
}