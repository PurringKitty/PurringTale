using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Content.NPCs.BossNPCs.Sloth.Projectiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.NPCs.BossNPCs.Sloth
{
    public class SlothHead : ModNPC
    {
        private enum HeadState
        {
            Positioning,
            Attack,
            Cooldown
        }

        private enum HeadAttackType
        {
            OrbitalStrike,
            PhaseCharge,
            HuntingPattern,
            PressureWave,
            BlitzAssault
        }

        private ref float timer => ref NPC.ai[0];
        private ref float bodyWhoAmI => ref NPC.ai[1];
        private ref float stateTimer => ref NPC.ai[2];
        private ref float attackProgress => ref NPC.ai[3];

        private HeadState currentState = HeadState.Positioning;
        private HeadAttackType currentAttack = HeadAttackType.OrbitalStrike;
        private int attackCounter = 0;
        private Vector2 targetPosition;
        private Vector2 predictedPlayerPos;
        private List<Vector2> waypoints = new List<Vector2>();
        private bool attackInitialized = false;
        private float orbitalAngle = 0f;

        private bool healthLinked = false;
        private int sharedMaxHealth = 0;
        private int sharedCurrentHealth = 0;

        private float energyCharge = 0f;
        private Color currentGlow = Color.Purple;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)currentState);
            writer.Write((int)currentAttack);
            writer.Write(attackCounter);
            writer.Write(targetPosition.X);
            writer.Write(targetPosition.Y);
            writer.Write(predictedPlayerPos.X);
            writer.Write(predictedPlayerPos.Y);
            writer.Write(attackInitialized);
            writer.Write(orbitalAngle);
            writer.Write(energyCharge);

            writer.Write(healthLinked);
            writer.Write(sharedMaxHealth);
            writer.Write(sharedCurrentHealth);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            currentState = (HeadState)reader.ReadInt32();
            currentAttack = (HeadAttackType)reader.ReadInt32();
            attackCounter = reader.ReadInt32();
            targetPosition.X = reader.ReadSingle();
            targetPosition.Y = reader.ReadSingle();
            predictedPlayerPos.X = reader.ReadSingle();
            predictedPlayerPos.Y = reader.ReadSingle();
            attackInitialized = reader.ReadBoolean();
            orbitalAngle = reader.ReadSingle();
            energyCharge = reader.ReadSingle();

            healthLinked = reader.ReadBoolean();
            sharedMaxHealth = reader.ReadInt32();
            sharedCurrentHealth = reader.ReadInt32();
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.TrailCacheLength[NPC.type] = 12;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "PurringTale/Content/NPCs/BossNPCs/Sloth/SlothHead_Beastiary",
                PortraitScale = 1f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("Acedia's head will come off in as a last measure in a fight it is very fucking gross - Rukuka")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 50;
            NPC.damage = 70;
            NPC.lifeMax = 18000;
            NPC.defense = 25;
            NPC.scale = 1.3f;

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;

            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.npcSlots = 0f;
            NPC.value = 0;
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

            NPC body = ValidateBodyConnection();
            if (body == null) return;

            if (!healthLinked)
            {
                InitializeSharedHealth(body);
            }
            else
            {
                SyncSharedHealth(body);
            }

            UpdatePlayerPrediction(player);

            switch (currentState)
            {
                case HeadState.Positioning:
                    PositioningBehavior(player);
                    break;
                case HeadState.Attack:
                    AttackBehavior(player);
                    break;
                case HeadState.Cooldown:
                    CooldownBehavior(player);
                    break;
            }

            UpdateVisualEffects();

            timer++;
            stateTimer++;
        }

        private NPC ValidateBodyConnection()
        {
            if (bodyWhoAmI >= 0 && bodyWhoAmI < Main.maxNPCs)
            {
                NPC body = Main.npc[(int)bodyWhoAmI];
                if (body.active && body.type == ModContent.NPCType<SlothBoss>())
                {
                    return body;
                }
            }

            NPC.life = 0;
            NPC.HitEffect();
            NPC.checkDead();
            return null;
        }

        private void InitializeSharedHealth(NPC body)
        {
            sharedMaxHealth = body.lifeMax;
            sharedCurrentHealth = body.life;
            NPC.lifeMax = sharedMaxHealth;
            NPC.life = sharedCurrentHealth;
            healthLinked = true;
        }

        private void SyncSharedHealth(NPC body)
        {
            if (body == null || !body.active)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.checkDead();
                return;
            }

            int lowestHealth = Math.Min(NPC.life, body.life);

            if (sharedCurrentHealth != lowestHealth)
            {
                sharedCurrentHealth = lowestHealth;
                NPC.life = sharedCurrentHealth;
                body.life = sharedCurrentHealth;

                if (sharedCurrentHealth <= 0)
                {
                    NPC.life = 0;
                    body.life = 0;
                    NPC.HitEffect();
                    body.HitEffect();
                    NPC.checkDead();
                    body.checkDead();
                }

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                    NetMessage.SendData(MessageID.SyncNPC, number: (int)bodyWhoAmI);
                }
            }
        }

        private void UpdatePlayerPrediction(Player player)
        {
            Vector2 velocity = player.velocity;
            float predictionTime = 45f;
            predictedPlayerPos = player.Center + velocity * predictionTime;
        }

        private void UpdateVisualEffects()
        {
            if (currentState == HeadState.Attack)
            {
                energyCharge = Math.Min(1f, energyCharge + 0.03f);
            }
            else
            {
                energyCharge = Math.Max(0f, energyCharge - 0.02f);
            }

            switch (currentAttack)
            {
                case HeadAttackType.OrbitalStrike:
                    currentGlow = Color.Lerp(Color.Purple, Color.Cyan, (float)Math.Sin(timer * 0.1f) * 0.5f + 0.5f);
                    break;
                case HeadAttackType.PhaseCharge:
                    currentGlow = Color.Lerp(Color.Red, Color.Orange, energyCharge);
                    break;
                default:
                    currentGlow = Color.Lerp(Color.Purple, Color.Pink, energyCharge);
                    break;
            }
        }

        private void PositioningBehavior(Player player)
        {
            if (stateTimer == 1)
            {
                List<HeadAttackType> availableAttacks = new List<HeadAttackType>
                {
                    HeadAttackType.OrbitalStrike,
                    HeadAttackType.PhaseCharge,
                    HeadAttackType.HuntingPattern,
                    HeadAttackType.PressureWave,
                    HeadAttackType.BlitzAssault
                };

                currentAttack = availableAttacks[attackCounter % availableAttacks.Count];
                attackCounter++;
                attackInitialized = false;

                SetPositionTarget(player);
            }

            Vector2 direction = targetPosition - NPC.Center;
            float distance = direction.Length();

            if (distance > 20f)
            {
                direction.Normalize();
                float speed = Math.Min(distance * 0.15f, 8f);
                NPC.velocity = direction * speed;
            }
            else
            {
                NPC.velocity *= 0.9f;

                if (distance < 30f)
                {
                    currentState = HeadState.Attack;
                    stateTimer = 0;
                    attackProgress = 0f;
                }
            }

            if (stateTimer % 8 == 0)
            {
                Dust positionDust = Dust.NewDustDirect(NPC.Center, 10, 10, DustID.MagicMirror);
                positionDust.velocity = Vector2.Zero;
                positionDust.scale = 0.8f;
                positionDust.noGravity = true;
                positionDust.color = currentGlow * 0.6f;
            }

            if (stateTimer > 180)
            {
                currentState = HeadState.Attack;
                stateTimer = 0;
            }
        }

        private void SetPositionTarget(Player player)
        {
            switch (currentAttack)
            {
                case HeadAttackType.OrbitalStrike:
                    targetPosition = player.Center + Vector2.UnitY * -200f;
                    break;
                case HeadAttackType.PhaseCharge:
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    targetPosition = player.Center + Vector2.UnitX.RotatedBy(angle) * 350f;
                    break;
                case HeadAttackType.HuntingPattern:
                    targetPosition = player.Center + Vector2.UnitX.RotatedBy(timer * 0.02f) * 180f;
                    break;
                case HeadAttackType.PressureWave:
                    targetPosition = player.Center + Vector2.UnitY * -150f;
                    break;
                case HeadAttackType.BlitzAssault:
                    targetPosition = player.Center + Main.rand.NextVector2Circular(200f, 200f);
                    break;
            }
        }

        private void AttackBehavior(Player player)
        {
            if (!attackInitialized)
            {
                InitializeAttack(player);
                attackInitialized = true;
            }

            switch (currentAttack)
            {
                case HeadAttackType.OrbitalStrike:
                    OrbitalStrikeAttack(player);
                    break;
                case HeadAttackType.PhaseCharge:
                    PhaseChargeAttack(player);
                    break;
                case HeadAttackType.HuntingPattern:
                    HuntingPatternAttack(player);
                    break;
                case HeadAttackType.PressureWave:
                    PressureWaveAttack(player);
                    break;
                case HeadAttackType.BlitzAssault:
                    BlitzAssaultAttack(player);
                    break;
            }

            attackProgress += 1f;

            if (attackProgress >= GetAttackDuration())
            {
                currentState = HeadState.Cooldown;
                stateTimer = 0;
            }
        }

        private void InitializeAttack(Player player)
        {
            switch (currentAttack)
            {
                case HeadAttackType.OrbitalStrike:
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, NPC.Center);
                    orbitalAngle = 0f;
                    break;
                case HeadAttackType.PhaseCharge:
                    SoundEngine.PlaySound(SoundID.DD2_BetsyScream, NPC.Center);
                    break;
                case HeadAttackType.HuntingPattern:
                    SoundEngine.PlaySound(SoundID.Item15, NPC.Center);
                    break;
                case HeadAttackType.PressureWave:
                    SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, NPC.Center);
                    break;
                case HeadAttackType.BlitzAssault:
                    SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                    waypoints.Clear();
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 waypoint = player.Center + Main.rand.NextVector2Circular(250f, 250f);
                        waypoints.Add(waypoint);
                    }
                    break;
            }
        }

        private float GetAttackDuration()
        {
            switch (currentAttack)
            {
                case HeadAttackType.OrbitalStrike: return 180f;
                case HeadAttackType.PhaseCharge: return 150f;
                case HeadAttackType.HuntingPattern: return 200f;
                case HeadAttackType.PressureWave: return 160f;
                case HeadAttackType.BlitzAssault: return 180f;
                default: return 150f;
            }
        }

        private void OrbitalStrikeAttack(Player player)
        {
            orbitalAngle += 0.05f;
            float radius = 200f - (attackProgress * 0.5f);
            radius = Math.Max(radius, 120f);

            Vector2 orbitalPosition = player.Center + Vector2.UnitX.RotatedBy(orbitalAngle) * radius;
            Vector2 direction = orbitalPosition - NPC.Center;

            if (direction.Length() > 10f)
            {
                direction.Normalize();
                NPC.velocity = direction * 6f;
            }

            if (attackProgress % 30 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shootDirection = NPC.DirectionTo(predictedPlayerPos);
                shootDirection = shootDirection.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));

                Projectile.NewProjectile(NPC.GetSource_FromAI(),
                    NPC.Center,
                    shootDirection * 9f,
                    ModContent.ProjectileType<SlothSpear3>(),
                    65,
                    4f,
                    ai0: predictedPlayerPos.X,
                    ai1: predictedPlayerPos.Y);

                SoundEngine.PlaySound(SoundID.Item125, NPC.Center);
            }

            if (attackProgress % 6 == 0)
            {
                Vector2 trailPos = NPC.Center + Main.rand.NextVector2Circular(20f, 20f);
                Dust trail = Dust.NewDustDirect(trailPos, 4, 4, DustID.Electric);
                trail.velocity = Vector2.Zero;
                trail.scale = 1.2f;
                trail.noGravity = true;
                trail.color = currentGlow;
            }
        }

        private void PhaseChargeAttack(Player player)
        {
            int phase = (int)(attackProgress / 30f);

            if (phase < 5)
            {
                if (attackProgress % 30 == 0)
                {
                    float angle = (phase * MathHelper.TwoPi / 5f) + Main.rand.NextFloat(-0.3f, 0.3f);
                    Vector2 chargeStart = player.Center + Vector2.UnitX.RotatedBy(angle) * 400f;

                    CreateTeleportEffect(NPC.Center);
                    NPC.Center = chargeStart;
                    CreateTeleportEffect(NPC.Center);

                    NPC.velocity = Vector2.Zero;
                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                }
                else if (attackProgress % 30 >= 15)
                {
                    Vector2 chargeDirection = NPC.DirectionTo(predictedPlayerPos);
                    NPC.velocity = chargeDirection * 18f;
                }
            }
            else
            {
                Vector2 returnDirection = NPC.DirectionTo(player.Center + Vector2.UnitY * -150f);
                NPC.velocity = returnDirection * 8f;
            }
        }

        private void HuntingPatternAttack(Player player)
        {
            Vector2 huntTarget = Vector2.Lerp(player.Center, predictedPlayerPos, 0.7f);
            Vector2 direction = NPC.DirectionTo(huntTarget);
            float distance = NPC.Distance(huntTarget);

            float speed = MathHelper.Lerp(6f, 14f, Math.Min(distance / 300f, 1f));

            Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
            Vector2 weave = perpendicular * (float)Math.Sin(attackProgress * 0.15f) * 4f;

            NPC.velocity = direction * speed + weave;

            if (distance < 100f && attackProgress % 60 == 30)
            {
                NPC.velocity = direction * 22f;
                SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
            }
        }

        private void PressureWaveAttack(Player player)
        {
            Vector2 centerPos = player.Center + Vector2.UnitY * -150f;
            Vector2 direction = centerPos - NPC.Center;
            NPC.velocity = direction * 0.05f;

            if (attackProgress % 40 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int waveCount = 8;
                for (int i = 0; i < waveCount; i++)
                {
                    float angle = (MathHelper.TwoPi / waveCount) * i;
                    Vector2 waveDirection = Vector2.UnitX.RotatedBy(angle);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(),
                        NPC.Center,
                        waveDirection * 6f,
                        ModContent.ProjectileType<SlothSpear>(),
                        55,
                        3f);
                }

                SoundEngine.PlaySound(SoundID.Item84, NPC.Center);
            }
        }

        private void BlitzAssaultAttack(Player player)
        {
            int waypointIndex = (int)(attackProgress / 30f);

            if (waypointIndex < waypoints.Count)
            {
                Vector2 currentWaypoint = waypoints[waypointIndex];
                Vector2 direction = currentWaypoint - NPC.Center;
                float distance = direction.Length();

                if (distance > 20f)
                {
                    direction.Normalize();
                    NPC.velocity = direction * 16f;
                }

                if (attackProgress % 30 == 15 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 attackDirection = NPC.DirectionTo(player.Center);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(),
                        NPC.Center,
                        attackDirection * 10f,
                        ModContent.ProjectileType<SlothSpear3>(),
                        60,
                        4f,
                        ai0: player.Center.X,
                        ai1: player.Center.Y);

                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                }
            }
            else
            {
                Vector2 finalDirection = NPC.DirectionTo(player.Center);
                NPC.velocity = finalDirection * 20f;
            }
        }

        private void CooldownBehavior(Player player)
        {
            Vector2 safePosition = player.Center + Vector2.UnitY * -180f;
            Vector2 direction = safePosition - NPC.Center;

            if (direction.Length() > 30f)
            {
                direction.Normalize();
                NPC.velocity = direction * 4f;
            }
            else
            {
                NPC.velocity *= 0.9f;
            }

            if (stateTimer % 12 == 0)
            {
                Dust cooldown = Dust.NewDustDirect(NPC.Center, 15, 15, DustID.Smoke);
                cooldown.velocity = Main.rand.NextVector2Circular(2f, 2f);
                cooldown.scale = 1.0f;
                cooldown.alpha = 150;
                cooldown.noGravity = true;
            }

            if (stateTimer >= 90)
            {
                currentState = HeadState.Positioning;
                stateTimer = 0;
            }
        }

        private void CreateTeleportEffect(Vector2 position)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust teleportDust = Dust.NewDustDirect(position, 20, 20, DustID.Shadowflame);
                teleportDust.velocity = Main.rand.NextVector2Circular(8f, 8f);
                teleportDust.scale = Main.rand.NextFloat(1.0f, 2.0f);
                teleportDust.noGravity = true;
                teleportDust.color = currentGlow;
            }
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            SyncDamageToBody();
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            SyncDamageToBody();
        }

        private void SyncDamageToBody()
        {
            if (bodyWhoAmI >= 0 && bodyWhoAmI < Main.maxNPCs)
            {
                var body = Main.npc[(int)bodyWhoAmI];
                if (body.active)
                {
                    body.life = NPC.life;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: (int)bodyWhoAmI);
                    }
                }
            }
        }

        public override void OnKill()
        {
            if (bodyWhoAmI >= 0 && bodyWhoAmI < Main.maxNPCs)
            {
                var body = Main.npc[(int)bodyWhoAmI];
                if (body.active)
                {
                    body.life = 0;
                    body.HitEffect();
                    body.checkDead();
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = (int)MathHelper.Lerp(8f, 3f, energyCharge);
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
            Main.instance.LoadNPC(Type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[Type].Value;

            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                float factor = (float)(NPC.oldPos.Length - i) / NPC.oldPos.Length;
                Vector2 drawPos = NPC.oldPos[i] - screenPos + NPC.Size / 2f;

                float intensity = energyCharge * 0.8f;
                Color trailColor = currentGlow * factor * intensity;

                float scale = NPC.scale * factor * (0.8f + energyCharge * 0.2f);
                spriteBatch.Draw(texture, drawPos, NPC.frame, trailColor, NPC.rotation, NPC.frame.Size() / 2f, scale, SpriteEffects.None, 0f);
            }

            if (energyCharge > 0.1f)
            {
                Color glowColor = currentGlow * energyCharge * 0.6f;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.PiOver2 * i) * (2f + energyCharge * 3f);
                    Vector2 glowPos = NPC.Center - screenPos + offset;
                    spriteBatch.Draw(texture, glowPos, NPC.frame, glowColor, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);
                }
            }

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (currentState == HeadState.Attack)
            {
                switch (currentAttack)
                {
                    case HeadAttackType.PhaseCharge:
                        DrawChargeTelegraph(spriteBatch, screenPos);
                        break;
                }
            }
        }

        private void DrawChargeTelegraph(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            if (attackProgress % 30 < 15)
            {
                float telegraphIntensity = (attackProgress % 30) / 15f;
                Color warningColor = Color.Red * telegraphIntensity * 0.7f;

                Vector2 warningPos = NPC.Center - screenPos;

                for (int i = 0; i < 16; i++)
                {
                    Vector2 circlePos = warningPos + Vector2.UnitX.RotatedBy((MathHelper.TwoPi / 16) * i) * 40f;
                    Dust.NewDustPerfect(circlePos + screenPos, DustID.RedTorch, Vector2.Zero, 100, warningColor, 1.2f);
                }
            }
        }
    }
}