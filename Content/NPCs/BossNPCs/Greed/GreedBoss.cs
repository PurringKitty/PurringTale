using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture.Relics;
using PurringTale.Content.Items.Placeables.Furniture.Trophies;
using PurringTale.Content.NPCs.BossNPCs.Greed.Projectiles;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.NPCs.BossNPCs.Greed
{
    public enum GreedAttackType
    {
        CoinBarrage,
        TreasureShower,
        GoldenDash,
        GreedSpiral,
        TreasureOrbit,
        GoldStorm,
        RichesRain,
        MoneyWhirlwind,
        Beam,
        CoinBomb
    }

    [AutoloadBossHead]
    public class GreedBoss : ModNPC
    {
        private enum ActionState
        {
            Spawn,
            Hovering,
            Attack,
            Circling,
            Retreating
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

        private GreedAttackType CurrentAttack
        {
            get => (GreedAttackType)AttackData;
            set => AttackData = (uint)value;
        }

        private ref float timer => ref NPC.ai[0];
        private ref float attackCounter => ref NPC.ai[3];
        private ref float chatTimer => ref NPC.localAI[0];
        private ref float movementTimer => ref NPC.localAI[1];
        private ref float phase => ref NPC.localAI[2];

        private Vector2 hoverTarget;
        private Vector2 circleCenter;
        private float circleAngle = 0f;
        private bool isCirclingClockwise = true;

        private Vector2 targetVelocity;
        private Vector2 smoothPosition;

        private List<string> greedQuotes = new List<string> // Add more shit talking here if u want
        {
            "You can't afford to fight me!",
            "Special offer on dirt - only 99 gold per block!",
            "My wealth knows no bounds!",
            "Gold, gold, GOLD! I need more!",
            "My shop has the finest wares!",
            "Money makes the world go round!",
            "I'll make you pay... literally!"
        };

        private int lastQuoteIndex = -1;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(lastQuoteIndex);
            writer.WriteVector2(hoverTarget);
            writer.WriteVector2(circleCenter);
            writer.Write(circleAngle);
            writer.Write(isCirclingClockwise);
            writer.WriteVector2(targetVelocity);
            writer.WriteVector2(smoothPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            lastQuoteIndex = reader.ReadInt32();
            hoverTarget = reader.ReadVector2();
            circleCenter = reader.ReadVector2();
            circleAngle = reader.ReadSingle();
            isCirclingClockwise = reader.ReadBoolean();
            targetVelocity = reader.ReadVector2();
            smoothPosition = reader.ReadVector2();
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.TrailCacheLength[NPC.type] = 12;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.7f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 80;
            NPC.height = 90;
            NPC.damage = 55;
            NPC.defense = 20;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.CoinPickup;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(platinum: 2);
            NPC.boss = true;
            NPC.npcSlots = 30f;
            NPC.SpawnWithHigherTime(30);

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Greed");
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A greedy merchant whose love of gold has corrupted him beyond redemption. He'll sell you anything... for the right price. - Rukuka"),
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            AIState = ActionState.Spawn;
            timer = 0;
            attackCounter = 0;
            chatTimer = 0;
            movementTimer = 0;
            lastQuoteIndex = -1;

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];
            hoverTarget = player.Center + new Vector2(0, -200);
            circleCenter = player.Center;
            targetVelocity = Vector2.Zero;
            smoothPosition = NPC.Center;

            SayGreedQuote();
        }

        private void SayGreedQuote()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            List<int> availableQuotes = new List<int>();
            for (int i = 0; i < greedQuotes.Count; i++)
            {
                if (i != lastQuoteIndex)
                    availableQuotes.Add(i);
            }

            if (availableQuotes.Count > 0)
            {
                int selectedIndex = availableQuotes[Main.rand.Next(availableQuotes.Count)];
                lastQuoteIndex = selectedIndex;

                Main.NewText($"{greedQuotes[selectedIndex]}", Color.Gold);

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                }
            }
        }
        private void SmoothMoveTowards(Vector2 targetPosition, float speed, float acceleration = 0.08f)
        {
            Vector2 direction = (targetPosition - NPC.Center).SafeNormalize(Vector2.Zero);
            targetVelocity = direction * speed;

            NPC.velocity = Vector2.Lerp(NPC.velocity, targetVelocity, acceleration);

            if (NPC.velocity.Length() > speed * 1.2f)
            {
                NPC.velocity = Vector2.Normalize(NPC.velocity) * speed * 1.2f;
            }
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

            chatTimer++;
            if (chatTimer >= 480)
            {
                SayGreedQuote();
                chatTimer = 0;
            }

            NPC.spriteDirection = NPC.direction = (player.Center.X > NPC.Center.X) ? 1 : -1;

            if (NPC.life <= NPC.lifeMax * 0.6f && phase == 0)
            {
                phase = 1;
                Main.NewText("You're hurting my PROFITS!", Color.Red);
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            }
            else if (NPC.life <= NPC.lifeMax * 0.25f && phase == 1)
            {
                phase = 2;
                Main.NewText("MY TREASURE! I'LL BURY YOU IN GOLD!", Color.DarkRed);
                SoundEngine.PlaySound(SoundID.DD2_BetsyScream, NPC.Center);
            }

            ChooseAction(player);
            timer++;
            movementTimer++;
        }

        private void ChooseAction(Player player)
        {
            switch (AIState)
            {
                case ActionState.Spawn:
                    SpawnBehavior(player);
                    break;
                case ActionState.Hovering:
                    HoverBehavior(player);
                    break;
                case ActionState.Attack:
                    ExecuteAttack(player);
                    break;
                case ActionState.Circling:
                    CircleBehavior(player);
                    break;
                case ActionState.Retreating:
                    RetreatBehavior(player);
                    break;
            }
        }

        private void SpawnBehavior(Player player)
        {
            Vector2 targetPos = player.Center + new Vector2(0, -250);
            SmoothMoveTowards(targetPos, 6f, 0.05f);

            if (timer < 120)
            {
                NPC.dontTakeDamage = true;

                if (timer % 10 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 dustPos = NPC.Center + Main.rand.NextVector2Circular(80, 80);
                        Dust dust = Dust.NewDustPerfect(dustPos, DustID.GoldCoin);
                        dust.velocity = Vector2.Zero;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                    }
                }
            }
            else
            {
                NPC.dontTakeDamage = false;
                AIState = ActionState.Hovering;
                timer = 0;
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-200, -100));
            }
        }

        private void HoverBehavior(Player player)
        {
            float distance = Vector2.Distance(NPC.Center, hoverTarget);

            if (distance > 60f)
            {
                SmoothMoveTowards(hoverTarget, 7f, 0.06f);
            }
            else
            {
                NPC.velocity *= 0.96f;
                Vector2 floatOffset = new Vector2(
                    (float)Math.Sin(movementTimer * 0.03f) * 2f,
                    (float)Math.Cos(movementTimer * 0.025f) * 1.5f
                );
                NPC.velocity += floatOffset * 0.1f;

                if (timer % 120 == 0)
                {
                    hoverTarget = player.Center + new Vector2(
                        Main.rand.Next(-400, 400),
                        Main.rand.Next(-250, -80)
                    );
                }
            }

            if (timer >= 60 + Main.rand.Next(60, 120))
            {
                ChooseNewAttack();
                AIState = ActionState.Attack;
                timer = 0;
            }
        }

        private void CircleBehavior(Player player)
        {
            circleCenter = Vector2.Lerp(circleCenter, player.Center, 0.01f);

            float radius = 250f + (float)Math.Sin(movementTimer * 0.02f) * 50f;
            float speed = (isCirclingClockwise ? 0.035f : -0.035f) * (1f + phase * 0.3f);

            circleAngle += speed;
            Vector2 targetPos = circleCenter + Vector2.UnitX.RotatedBy(circleAngle) * radius;

            SmoothMoveTowards(targetPos, 10f, 0.12f);

            if (timer >= 180 + Main.rand.Next(60, 120))
            {
                AIState = ActionState.Hovering;
                timer = 0;
                isCirclingClockwise = !isCirclingClockwise;
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-200, -100));
            }
        }

        private void RetreatBehavior(Player player)
        {
            Vector2 retreatDirection = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero);
            Vector2 retreatTarget = player.Center + retreatDirection * 400f;

            SmoothMoveTowards(retreatTarget, 12f, 0.15f);

            if (timer >= 90)
            {
                AIState = ActionState.Hovering;
                timer = 0;
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-200, -100));
            }
        }

        private void ChooseNewAttack()
        {
            GreedAttackType[] phase1Attacks = {
                GreedAttackType.CoinBarrage,
                GreedAttackType.TreasureShower,
                GreedAttackType.GoldenDash,
                GreedAttackType.TreasureOrbit,
                GreedAttackType.CoinBomb
            };

            GreedAttackType[] phase2Attacks = {
                GreedAttackType.CoinBarrage,
                GreedAttackType.TreasureShower,
                GreedAttackType.GoldenDash,
                GreedAttackType.GreedSpiral,
                GreedAttackType.GoldStorm,
                GreedAttackType.RichesRain,
                GreedAttackType.CoinBomb
            };

            GreedAttackType[] phase3Attacks = {
                GreedAttackType.GoldenDash,
                GreedAttackType.GreedSpiral,
                GreedAttackType.GoldStorm,
                GreedAttackType.RichesRain,
                GreedAttackType.MoneyWhirlwind,
                GreedAttackType.Beam,
                GreedAttackType.CoinBomb
            };

            GreedAttackType[] availableAttacks = phase switch
            {
                0 => phase1Attacks,
                1 => phase2Attacks,
                _ => phase3Attacks
            };

            CurrentAttack = availableAttacks[Main.rand.Next(availableAttacks.Length)];
            attackCounter++;
        }

        private void ExecuteAttack(Player player)
        {
            switch (CurrentAttack)
            {
                case GreedAttackType.CoinBarrage:
                    CoinBarrageAttack(player);
                    break;
                case GreedAttackType.TreasureShower:
                    TreasureShowerAttack(player);
                    break;
                case GreedAttackType.GoldenDash:
                    GoldenDashAttack(player);
                    break;
                case GreedAttackType.GreedSpiral:
                    GreedSpiralAttack(player);
                    break;
                case GreedAttackType.TreasureOrbit:
                    TreasureOrbitAttack(player);
                    break;
                case GreedAttackType.GoldStorm:
                    GoldStormAttack(player);
                    break;
                case GreedAttackType.RichesRain:
                    RichesRainAttack(player);
                    break;
                case GreedAttackType.MoneyWhirlwind:
                    MoneyWhirlwindAttack(player);
                    break;
                case GreedAttackType.Beam:
                    BeamAttack(player);
                    break;
                case GreedAttackType.CoinBomb:
                    CoinBombAttack(player);
                    break;
            }
        }

        private void CoinBarrageAttack(Player player)
        {
            Vector2 targetPos = player.Center + new Vector2(Main.rand.Next(-200, 200), -150);
            SmoothMoveTowards(targetPos, 5f, 0.04f);

            if (timer >= 20 && timer <= 100 && timer % (phase == 2 ? 4 : 6) == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 shootDirection = NPC.DirectionTo(player.Center);

                    int coinCount = phase == 2 ? 5 : 3;
                    for (int i = 0; i < coinCount; i++)
                    {
                        Vector2 velocity = shootDirection.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(10f, 15f);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<GreedCoin>(), 45, 3f);
                    }
                }

                SoundEngine.PlaySound(SoundID.CoinPickup, NPC.Center);

                for (int i = 0; i < 10; i++)
                {
                    Vector2 dustVel = NPC.DirectionTo(player.Center).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * 4f;
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.GoldCoin, dustVel);
                    dust.noGravity = true;
                    dust.scale = 1.3f;
                }
            }

            if (timer >= 120)
            {
                AIState = ActionState.Circling;
                timer = 0;
                circleCenter = player.Center;
            }
        }

        private void TreasureShowerAttack(Player player)
        {
            Vector2 targetPos = player.Center + new Vector2(0, -300);
            SmoothMoveTowards(targetPos, 6f, 0.06f);

            if (timer == 1)
            {
                Main.NewText(phase == 2 ? "I'll bury you in riches!" : "Taste my precious treasures!", Color.Gold);
                SoundEngine.PlaySound(SoundID.Item37, NPC.Center);
            }

            if (timer >= 60 && timer <= 180 && timer % (phase == 2 ? 8 : 12) == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int treasureCount = phase == 2 ? 6 : 4;
                    for (int i = 0; i < treasureCount; i++)
                    {
                        Vector2 spawnPos = player.Center + new Vector2(Main.rand.NextFloat(-500f, 500f), -700f);
                        Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(10f, 15f);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                            ModContent.ProjectileType<GreedTreasure>(), 50, 4f);
                    }
                }

                SoundEngine.PlaySound(SoundID.Item37, NPC.Center);
            }

            if (timer >= 220)
            {
                AIState = ActionState.Hovering;
                timer = 0;
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-200, -100));
            }
        }

        private void GoldenDashAttack(Player player)
        {
            if (timer <= 45)
            {
                Vector2 chargePos = player.Center + (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 400f;
                SmoothMoveTowards(chargePos, 5f, 0.08f);

                if (timer % 8 == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 dustPos = NPC.Center + Main.rand.NextVector2Circular(100, 100);
                        Dust dust = Dust.NewDustPerfect(dustPos, DustID.GoldCoin);
                        dust.velocity = (NPC.Center - dustPos) * 0.15f;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                    }
                }
            }
            else if (timer == 46)
            {
                Vector2 dashDirection = NPC.DirectionTo(player.Center);
                NPC.velocity = dashDirection * (phase == 2 ? 28f : 22f);
                SoundEngine.PlaySound(SoundID.Item74, NPC.Center);
                Main.NewText("GOLDEN RUSH!", Color.Yellow);
            }
            else if (timer > 46 && timer <= 80)
            {
                if (timer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<GreedCoin>(), 40, 2f);
                }

                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GoldCoin);
                    dust.velocity = -NPC.velocity * 0.4f;
                    dust.noGravity = true;
                    dust.scale = 2f;
                }
            }
            else if (timer > 80)
            {
                NPC.velocity *= 0.92f;

                if (timer >= 120)
                {
                    AIState = ActionState.Retreating;
                    timer = 0;
                }
            }
        }

        private void GreedSpiralAttack(Player player)
        {
            Vector2 orbitCenter = player.Center;
            float orbitRadius = 200f;
            float orbitSpeed = 0.05f;
            float orbitAngle = timer * orbitSpeed;

            Vector2 targetPos = orbitCenter + Vector2.UnitX.RotatedBy(orbitAngle) * orbitRadius;
            SmoothMoveTowards(targetPos, 8f, 0.12f);

            if (timer >= 30 && timer <= 180 && timer % 5 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float spiralAngle = (timer - 30) * 0.4f;
                    Vector2 spiralDirection = Vector2.UnitX.RotatedBy(spiralAngle);
                    Vector2 velocity = spiralDirection * (6f + (timer - 30) * 0.08f);

                    int projectileType = (timer / 5) % 2 == 0
                        ? ModContent.ProjectileType<GreedCoin>()
                        : ModContent.ProjectileType<GreedTreasure>();

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        projectileType, 45, 3f);
                }
            }

            if (timer >= 220)
            {
                AIState = ActionState.Hovering;
                timer = 0;
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-200, -100));
            }
        }

        private void TreasureOrbitAttack(Player player)
        {
            Vector2 targetPos = player.Center + new Vector2(0, -180);
            SmoothMoveTowards(targetPos, 5f, 0.06f);

            if (timer >= 60 && timer <= 180 && timer % 15 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        float angle = MathHelper.TwoPi / 6 * i + timer * 0.1f;
                        Vector2 orbitVel = Vector2.UnitX.RotatedBy(angle) * 8f;

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, orbitVel,
                            ModContent.ProjectileType<GreedTreasure>(), 40, 3f);
                    }
                }
                SoundEngine.PlaySound(SoundID.CoinPickup, NPC.Center);
                if (Main.rand.NextBool(2))
                {
                    SoundEngine.PlaySound(SoundID.Coins, NPC.Center);
                }
            }

            if (timer >= 200)
            {
                AIState = ActionState.Circling;
                timer = 0;
                circleCenter = player.Center;
            }
        }

        private void GoldStormAttack(Player player)
        {
            if (timer == 1)
            {
                Main.NewText("A STORM OF RICHES!", Color.Orange);
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
            }

            if (timer % 40 == 0)
            {
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-350, 350), Main.rand.Next(-250, -100));
            }

            SmoothMoveTowards(hoverTarget, 10f, 0.15f);

            if (timer >= 30 && timer <= 150 && timer % 3 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 randomVel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(8f, 15f);

                    int projectileType = Main.rand.Next(3) switch
                    {
                        0 => ModContent.ProjectileType<GreedCoin>(),
                        1 => ModContent.ProjectileType<GreedTreasure>(),
                        _ => ModContent.ProjectileType<GreedCoin>()
                    };

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, randomVel,
                        projectileType, 35, 2f);
                }

                if (timer % 6 == 0)
                {
                    SoundEngine.PlaySound(SoundID.CoinPickup, NPC.Center);
                }
                else if (timer % 9 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Coins, NPC.Center);
                }
            }

            if (timer >= 180)
            {
                AIState = ActionState.Retreating;
                timer = 0;
            }
        }

        private void RichesRainAttack(Player player)
        {
            float figure8Time = timer * 0.04f;
            Vector2 figure8Offset = new Vector2(
                (float)Math.Sin(figure8Time) * 300f,
                (float)Math.Sin(figure8Time * 2f) * 100f - 250f
            );
            Vector2 targetPos = player.Center + figure8Offset;

            SmoothMoveTowards(targetPos, 8f, 0.08f);

            if (timer == 1)
            {
                SoundEngine.PlaySound(SoundID.Item4, NPC.Center);
                Main.NewText("Let it rain GOLD!", Color.Gold);
            }

            if (timer >= 60 && timer <= 240 && timer % 6 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 spawnPos = NPC.Center + new Vector2(Main.rand.NextFloat(-100f, 100f), 50f);
                    Vector2 velocity = Vector2.UnitY * Main.rand.NextFloat(8f, 12f);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                        Main.rand.NextBool() ? ModContent.ProjectileType<GreedCoin>() : ModContent.ProjectileType<GreedTreasure>(),
                        40, 3f);
                }

                if (timer % 12 == 0)
                {
                    SoundEngine.PlaySound(SoundID.CoinPickup, NPC.Center);
                }
            }

            if (timer >= 280)
            {
                AIState = ActionState.Hovering;
                timer = 0;
                hoverTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-200, -100));
            }
        }

        private void MoneyWhirlwindAttack(Player player)
        {
            Vector2 spinCenter = player.Center;
            float spinRadius = 180f + (float)Math.Sin(timer * 0.08f) * 50f;
            float spinSpeed = 0.12f;

            Vector2 targetPos = spinCenter + Vector2.UnitX.RotatedBy(timer * spinSpeed) * spinRadius;
            SmoothMoveTowards(targetPos, 12f, 0.25f);

            if (timer >= 30 && timer <= 150 && timer % 4 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 whirlVel = NPC.velocity.RotatedBy(MathHelper.PiOver2) * 0.8f;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, whirlVel,
                        ModContent.ProjectileType<GreedCoin>(), 45, 4f);
                }
            }

            if (timer >= 180)
            {
                AIState = ActionState.Retreating;
                timer = 0;
            }
        }

        private void BeamAttack(Player player)
        {
            if (timer <= 90)
            {
                Vector2 idealPos = player.Center + (player.velocity * 30f) +
                                  (NPC.Center - player.Center).SafeNormalize(Vector2.Zero) * 400f;

                idealPos.Y -= 50f;

                SmoothMoveTowards(idealPos, 6f * (1f + timer / 180f), 0.08f);

                if (timer >= 30)
                {
                    float chargeIntensity = (timer - 30f) / 60f;

                    if (timer % (int)MathHelper.Max(1, 8 - chargeIntensity * 6) == 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 dustPos = NPC.Center + Main.rand.NextVector2Circular(80f * chargeIntensity, 80f * chargeIntensity);
                            Vector2 dustVel = (NPC.Center - dustPos) * 0.1f;

                            Dust dust = Dust.NewDustPerfect(dustPos, DustID.GoldFlame, dustVel);
                            dust.noGravity = true;
                            dust.scale = 1.5f * chargeIntensity;
                            dust.color = Color.Lerp(Color.Gold, Color.Orange, chargeIntensity);
                        }
                    }

                    if (timer % 10 == 0 && chargeIntensity > 0.5f)
                    {
                        foreach (Player p in Main.player)
                        {
                            if (p.active && !p.dead)
                            {
                                p.GetModPlayer<ScreenShakePlayer>().AddScreenShake((int)(15 * chargeIntensity), 3f * chargeIntensity);
                            }
                        }
                    }
                }

                if (timer == 45)
                {
                    Main.NewText("FEEL THE WEIGHT OF MY WEALTH!", Color.Orange);
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.Center);
                }

                if (timer == 75)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack, NPC.Center);
                }
            }
            else if (timer <= 170)
            {
                NPC.velocity *= 0.95f;

                int beamTimer = (int)(timer - 90);
                float beamProgress = beamTimer / 80f;

                if (beamTimer == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, NPC.Center);

                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 20f, 25);

                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 vel = NPC.DirectionTo(player.Center).RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(8f, 15f);
                        Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.GoldFlame, vel);
                        dust.noGravity = true;
                        dust.scale = 2f;
                        dust.color = Color.Gold;
                    }
                }

                if (beamTimer >= 5 && beamTimer <= 75)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 baseDirection = NPC.DirectionTo(player.Center + player.velocity * 20f);

                        int projectileCount = phase >= 2 ? 4 : 3;
                        float spreadAngle = 0.3f * (1f + phase * 0.3f);

                        if (beamTimer % (phase >= 2 ? 4 : 6) == 0)
                        {
                            for (int i = 0; i < projectileCount; i++)
                            {
                                float angle = -spreadAngle / 2f + (spreadAngle / (projectileCount - 1)) * i;
                                Vector2 velocity = baseDirection.RotatedBy(angle) * (14f + i * 1.5f);

                                int projectileType = (beamTimer / 6) % 3 == 0
                                    ? ModContent.ProjectileType<GreedTreasure>()
                                    : ModContent.ProjectileType<GreedCoin>();

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                                    projectileType, 55 + (int)(phase * 10), 5f);
                            }

                            if (beamTimer % 12 == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                            }
                            else if (beamTimer % 18 == 0)
                            {
                                SoundEngine.PlaySound(SoundID.CoinPickup, NPC.Center);
                            }
                        }

                        if (beamTimer == 25 || beamTimer == 50)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                float burstAngle = MathHelper.TwoPi / 8 * i;
                                Vector2 burstVel = baseDirection.RotatedBy(burstAngle * 0.6f) * Main.rand.NextFloat(10f, 16f);

                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, burstVel,
                                    ModContent.ProjectileType<GreedTreasure>(), 60, 6f);
                            }

                            SoundEngine.PlaySound(SoundID.Item62, NPC.Center);

                            ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 12f, 15);
                        }
                    }

                    if (beamTimer % 2 == 0)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 muzzlePos = NPC.Center + NPC.DirectionTo(player.Center) * 40f;
                            Vector2 dustVel = NPC.DirectionTo(player.Center).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * Main.rand.NextFloat(3f, 8f);

                            Dust dust = Dust.NewDustPerfect(muzzlePos, DustID.GoldFlame, dustVel);
                            dust.noGravity = true;
                            dust.scale = 1.8f;
                            dust.color = Color.Lerp(Color.Gold, Color.OrangeRed, beamProgress);
                        }

                        if (beamTimer % 6 == 0)
                        {
                            foreach (Player p in Main.player)
                            {
                                if (p.active && !p.dead)
                                {
                                    p.GetModPlayer<ScreenShakePlayer>().AddScreenShake(10, 1.5f);
                                }
                            }
                        }
                    }
                }

                if (beamTimer == 76)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);

                    ModContent.GetInstance<MCameraModifiers>().Shake(NPC.Center, 15f, 20);

                    for (int i = 0; i < 25; i++)
                    {
                        Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(5f, 12f);
                        Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.GoldCoin, vel);
                        dust.noGravity = true;
                        dust.scale = 2.5f;
                    }
                }
            }
            else
            {
                NPC.velocity *= 0.96f;
                Vector2 recoveryFloat = new Vector2(
                    (float)Math.Sin(timer * 0.02f) * 1.5f,
                    (float)Math.Cos(timer * 0.025f) * 1f
                );
                NPC.velocity += recoveryFloat * 0.1f;

                if (timer % 15 == 0 && timer < 200)
                {
                    Dust dust = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Circular(50, 50),
                        DustID.GoldCoin, Vector2.Zero);
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                }

                if (timer >= 190)
                {
                    AIState = ActionState.Circling;
                    timer = 0;
                    circleCenter = player.Center;

                    if (Main.rand.NextBool(3))
                    {
                        Main.NewText("That's the price of greed!", Color.Gold);
                    }
                }
            }
        }

        private void CoinBombAttack(Player player)
        {
            Vector2 targetPos = player.Center + new Vector2(0, -120);

            if (timer <= 60)
            {
                SmoothMoveTowards(targetPos, 6f, 0.12f);
            }
            else if (timer == 61)
            {
                NPC.velocity = Vector2.Zero;
                Main.NewText("EXPLOSIVE SAVINGS!", Color.Red);
                SoundEngine.PlaySound(SoundID.Item62, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        float angle = MathHelper.TwoPi / 20 * i;
                        Vector2 velocity = Vector2.UnitX.RotatedBy(angle) * Main.rand.NextFloat(8f, 16f);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<GreedCoin>(), 50, 6f);
                    }
                }

                for (int i = 0; i < 30; i++)
                {
                    Vector2 dustVel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(5f, 15f);
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.GoldCoin, dustVel);
                    dust.noGravity = true;
                    dust.scale = 2f;
                }
            }
            else if (timer > 61)
            {
                NPC.velocity *= 0.95f;

                if (timer >= 120)
                {
                    AIState = ActionState.Retreating;
                    timer = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = NPC.velocity.Length() > 5f ? 8 : 12;
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
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Vector2 drawPosition = NPC.Center - screenPos;
            Rectangle sourceRectangle = NPC.frame;
            Vector2 origin = sourceRectangle.Size() / 2f;
            SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            if (NPC.velocity.Length() > 6f)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    if (NPC.oldPos[i] == Vector2.Zero) continue;

                    float alpha = (float)(NPC.oldPos.Length - i) / NPC.oldPos.Length * 0.5f;

                    Vector2 trailDrawPos = NPC.oldPos[i] + NPC.Size / 2f - screenPos;
                    Color trailColor = Color.Gold * alpha;

                    spriteBatch.Draw(texture, trailDrawPos, sourceRectangle, trailColor,
                        NPC.oldRot[i], origin, NPC.scale * (0.8f + alpha * 0.2f), effects, 0f);
                }
            }

            Color glowColor = Color.Gold * (0.2f + phase * 0.1f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.PiOver2 * i) * (2f + phase);
                spriteBatch.Draw(texture, drawPosition + offset, sourceRectangle, glowColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            }

            Color mainColor = phase switch
            {
                0 => drawColor,
                1 => Color.Lerp(drawColor, Color.Orange, 0.3f),
                _ => Color.Lerp(drawColor, Color.Red, 0.4f)
            };

            spriteBatch.Draw(texture, drawPosition, sourceRectangle, mainColor, NPC.rotation, origin, NPC.scale, effects, 0f);

            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AvaritiaBossTrophy>(), 6, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<AvaritiaBossRelic>()));
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<GreedBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 2, 10, 50));
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedGreed, -1);

            Main.NewText("My... my precious... TREASURES!", Color.DarkGoldenrod);

            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.GoldCoin, velocity);
                dust.noGravity = true;
                dust.scale = 2f;
            }
        }
    }
}