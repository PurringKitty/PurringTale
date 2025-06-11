using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture;
using Terraria;
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
        Whip
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

        private float ShaderTimer = 0;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ShaderTimer);
            writer.Write(atkCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ShaderTimer = reader.Read();
            atkCounter = reader.Read();
        }
        public override void SetStaticDefaults()
        {

            Main.npcFrameCount[Type] = 26;

            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "PurringTale/CatBoss/TopHatCatBoss_Beastiaray",
                PortraitScale = 0.6f, /* Portrait refers to the full picture when clicking on the icon in the bestiary*/
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }
        private int[] laserIndex = new int[4];
        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Top Hat God";
            potionType = ItemID.SuperHealingPotion;
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 36;
            NPC.scale = 1.5f;

            NPC.damage = 15;

            NPC.lifeMax = 3000000;
            NPC.defense = 10;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.knockBackResist = 0f;

            NPC.value = Item.buyPrice(platinum: 5);
            NPC.SpawnWithHigherTime(30);

            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.HitSound = SoundID.NPCHit8;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
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
                new Terraria.GameContent.Bestiary.MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new Terraria.GameContent.Bestiary.FlavorTextBestiaryInfoElement("The God Of Top Hats! Can Be A Jerk Sometimes...")
            });

        }
        public override void OnSpawn(IEntitySource source)
        {
            AIState = ActionState.Spawn;
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

            if (AIState != oldState)
            {
                timer = 0;
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
                default:
                    break;
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
            if (timer <= 1)
            {
                Vector2 pos = target.Center + ModdingusUtils.randomVector();
                NPC.velocity = NPC.DirectionTo(pos) * (9 + (target.Distance(NPC.Center) / 80));
            }
            if (timer == Main.rand.Next(30, 90))
            {
                NPC.velocity = Vector2.Zero;
            }
            if (timer > 89)
            {
                NPC.velocity = Vector2.Zero;
            }

            if (timer > 120)
            {
                timer = 0;
                switch (atkCounter)
                {
                    case 0:
                        AtkType = AttackType.Staff;
                        break;
                    case 1:
                        AtkType = AttackType.Sword;
                        break;
                    case 2:
                        AtkType = AttackType.Gun;
                        break;
                    default:
                        break;
                }
                AIState = ActionState.Attack;
                atkCounter += 1;
                if (atkCounter > 2)
                {
                    atkCounter = 0;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<THGBossBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 0, 100));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VanityVoucher>(), 5, 0, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THGBossTrophy>(), 4, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<THGBossRelic>()));
        }
        public void Attack()
        {
            Player target = Main.player[NPC.target];

            if (timer > 0)
            {
                switch (AtkType)
                {
                    case AttackType.Sword:
                        if (timer % 60 == 0 && timer < 220)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / 12 * i + (timer / 60) / 3) * 30;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, NPC.Center.DirectionTo(pos) * 15, ModContent.ProjectileType<gss>(), 220, 5);
                            }
                        }
                        if (timer % 80 == 0 && timer < 220)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / 12 * i + (timer / 60) / 3 + 1) * 30;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, NPC.Center.DirectionTo(pos) * 20, ModContent.ProjectileType<gss>(), 220, 5);
                            }
                        }
                        if (timer == 220)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY, ModContent.ProjectileType<Slash>(), 220, 15, -1, NPC.whoAmI);

                        }
                        if (timer == 380)
                        {
                            timer = 0;
                            AIState = ActionState.Choose;
                        }
                        break;
                    case AttackType.Gun:
                        //chase player and shoot them
                        if (timer == 45)
                        {
                            NPC.velocity = NPC.DirectionTo(target.Center) * NPC.Distance(target.Center) / 30f;
                        }
                        if (timer % 60 == 0 && timer <= 240)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                var velocity = NPC.DirectionTo(target.Center).RotatedBy(MathHelper.Lerp(-0.3490658504f, 0.3490658504f, i / 5f)) * 15;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<Bolt>(), NPC.damage, 4f);
                            }

                        }
                        if (timer == 150)
                        {
                            for (int i = 0; i < 10; ++i)
                            {
                                var position = new Vector2(NPC.Center.X - 1000 + i * 200, NPC.Center.Y - 750);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.1745329252f, 0.1745329252f)) * 30, ModContent.ProjectileType<BossBullet>(), NPC.damage, 4f, -1, 0, 120 + i * 10f);
                            }
                        }
                        if (timer == 300)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi / 6 * i);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(pos) * 10, ModContent.ProjectileType<BossRocket>(), NPC.damage, 4f);
                            }
                        }
                        if (timer >= 370)
                        {
                            AIState = ActionState.Choose;
                            timer = 0;
                        }
                        break;
                    case AttackType.Staff:
                        if (timer == 1)
                        {
                            Vector2 nextPos = ModdingusUtils.randomCorner() * 550;
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 clonePos(int i)
                                {
                                    return i switch
                                    {
                                        0 => new Vector2(-1, 1),
                                        1 => new Vector2(-1, -1),
                                        _ => new Vector2(1, -1),
                                    };
                                }
                                var a = createClone(target.Center + nextPos * clonePos(i), 3, target.Center);
                                a.entranceDelay = (i + 1) * 25;
                            }
                            Vector2 pos = NPC.Center + Vector2.UnitX * 20 * NPC.direction;
                            NPC.Center = target.Center + nextPos;
                            NPC.velocity = NPC.DirectionTo(target.Center) * 5;
                        }
                        if (timer > 5 && timer < 10)
                        {
                            NPC.velocity *= 0.25f;
                        }
                        if (timer == 9)
                        {
                            int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ModContent.ProjectileType<BossLaser>(), 100, 5, -1, 0, NPC.whoAmI, 20);
                            Main.projectile[a].timeLeft = 80;
                        }
                        if (timer == 89)
                        {
                            NPC.Center = (target.Center + Vector2.UnitX * 300 * ModdingusUtils.PoN1() - Vector2.UnitY * 50);
                        }
                        if (timer == 133)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        if (timer == 134)
                        {
                            NPC.velocity = Vector2.Zero;
                            NPC.velocity = -Vector2.UnitY * 2.5f;
                            var off = Main.rand.NextFloat(0, MathHelper.TwoPi);
                            for (int a = 0; a < 4; a++)
                            {
                                laserIndex[a] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(off + MathHelper.PiOver2 * a), ModContent.ProjectileType<BossLaser>(), 100, 5, -1, 0, NPC.whoAmI, 20);
                            }
                        }
                        if (timer > 134 && timer < 270)
                        {
                            foreach (int c in laserIndex)
                                Main.projectile[c].velocity = Main.projectile[c].velocity.RotatedBy(0.01f);
                        }
                        if (timer == 270)
                        {
                            foreach (int c in laserIndex)
                                Main.projectile[c].Kill();
                        }
                        if (timer == 80 + 230)
                        {
                            Vector2 nextPos = ModdingusUtils.randomSide() * 550 * 1.41421356f;

                            for (int i = 0; i < 3; i++)
                            {
                                var a = createClone(target.Center + nextPos.RotatedBy(MathHelper.PiOver2 * (i + 1)), 3, target.Center);
                                a.entranceDelay = (i + 1) * 25;
                            }

                            Vector2 pos = NPC.Center + Vector2.UnitX * 20 * NPC.direction;
                            NPC.Center = target.Center + nextPos;
                            NPC.velocity = NPC.DirectionTo(target.Center) * 5;
                        }
                        if (timer > 315 && timer < 320)
                        {
                            NPC.velocity *= 0.25f;
                        }
                        if (timer == 319)
                        {
                            int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ModContent.ProjectileType<BossLaser>(), 100, 5, -1, 0, NPC.whoAmI, 20);
                            Main.projectile[a].timeLeft = 80;
                        }
                        if (timer == 399)
                        {
                            NPC.Center = (target.Center + Vector2.UnitX * 300 * ModdingusUtils.PoN1() - Vector2.UnitY * 50);
                        }
                        if (timer == 400)
                        {
                            timer = 0;
                            AIState = ActionState.Choose;
                        }
                        break;
                }
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
            for (float i = 0; i < tl; i += (float)(tl / 3))
            {
                float percent = i / tl;
                Vector2 dpos = NPC.oldPos[(int)i] - screenPos + new Vector2(t.Width * scale / 4, NPC.height * scale / 2);
                spriteBatch.Draw(t, dpos, source, Color.Purple * (1 - percent), NPC.rotation, NPC.origin(), scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return true;
        }
        private BossClone createClone(Vector2 pos, int attackStyle, Vector2 center = new Vector2())
        {
            int a = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<BossClone>(), 0, 0, -1, 0, (float)(AtkType));
            var b = (Main.projectile[a].ModProjectile as BossClone);
            b.attackStyle = attackStyle;
            b.centerPoint = center;

            return b;
        }
        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedTopHat, -1);
        }
    }
}