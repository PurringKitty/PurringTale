using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Vanity;
using PurringTale.Content.Items.MobLoot;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace PurringTale.Content.NPCs.HostileNPCs
{
    public class TopHatSlime : ModNPC
    {
        private enum ActionState
        {
            Asleep,
            Notice,
            Jump,
            Hover,
            Fall
        }
        private enum Frame
        {
            Asleep,
            Notice,
            Falling,
            Flutter1,
            Flutter2,
            Flutter3
        }
        public ref float AI_State => ref NPC.ai[0];
        public ref float AI_Timer => ref NPC.ai[1];
        public ref float AI_FlutterTime => ref NPC.ai[2];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 36;
            NPC.aiStyle = -1;
            NPC.damage = 5;
            NPC.defense = 50;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 10;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.01f;
        }
        public override void AI()
        {
            switch (AI_State)
            {
                case (float)ActionState.Asleep:
                    FallAsleep();
                    break;
                case (float)ActionState.Notice:
                    Notice();
                    break;
                case (float)ActionState.Jump:
                    Jump();
                    break;
                case (float)ActionState.Hover:
                    Hover();
                    break;
                case (float)ActionState.Fall:
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                        AI_State = (float)ActionState.Asleep;
                        AI_Timer = 0;
                    }

                    break;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
            switch (AI_State)
            {
                case (float)ActionState.Asleep:
                    NPC.frame.Y = (int)Frame.Asleep * frameHeight;
                    break;
                case (float)ActionState.Notice:
                    if (AI_Timer < 10)
                    {
                        NPC.frame.Y = (int)Frame.Notice * frameHeight;
                    }
                    else
                    {
                        NPC.frame.Y = (int)Frame.Asleep * frameHeight;
                    }

                    break;
                case (float)ActionState.Jump:
                    NPC.frame.Y = (int)Frame.Falling * frameHeight;
                    break;
                case (float)ActionState.Hover:
                    NPC.frameCounter++;

                    if (NPC.frameCounter < 10)
                    {
                        NPC.frame.Y = (int)Frame.Flutter1 * frameHeight;
                    }
                    else if (NPC.frameCounter < 20)
                    {
                        NPC.frame.Y = (int)Frame.Flutter2 * frameHeight;
                    }
                    else if (NPC.frameCounter < 30)
                    {
                        NPC.frame.Y = (int)Frame.Flutter3 * frameHeight;
                    }
                    else
                    {
                        NPC.frameCounter = 0;
                    }

                    break;
                case (float)ActionState.Fall:
                    NPC.frame.Y = (int)Frame.Falling * frameHeight;
                    break;
            }
        }
        public override bool? CanFallThroughPlatforms()
        {
            if (AI_State == (float)ActionState.Fall && NPC.HasValidTarget && Main.player[NPC.target].Top.Y > NPC.Bottom.Y)
            {
                return true;
            }

            return false;
        }

        private void FallAsleep()
        {
            NPC.TargetClosest(true);
            if (NPC.HasValidTarget && Main.player[NPC.target].Distance(NPC.Center) < 500f)
            {
                AI_State = (float)ActionState.Notice;
                AI_Timer = 0;
            }
        }

        private void Notice()
        {
            if (Main.player[NPC.target].Distance(NPC.Center) < 250f)
            {
                AI_Timer++;

                if (AI_Timer >= 20)
                {
                    AI_State = (float)ActionState.Jump;
                    AI_Timer = 0;
                }
            }
            else
            {
                NPC.TargetClosest(true);

                if (!NPC.HasValidTarget || Main.player[NPC.target].Distance(NPC.Center) > 500f)
                {
                    AI_State = (float)ActionState.Asleep;
                    AI_Timer = 0;
                }
            }
        }

        private void Jump()
        {
            AI_Timer++;

            if (AI_Timer == 1)
            {
                NPC.velocity = new Vector2(NPC.direction * 2, -10f);
            }
            else if (AI_Timer > 40)
            {
                AI_State = (float)ActionState.Hover;
                AI_Timer = 0;
            }
        }

        private void Hover()
        {
            AI_Timer++;
            if (AI_Timer == 1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AI_FlutterTime = Main.rand.NextBool() ? 100 : 50;
                NPC.netUpdate = true;
            }
            NPC.velocity += new Vector2(0, -.35f);
            if (Math.Abs(NPC.velocity.X) < 2)
            {
                NPC.velocity += new Vector2(NPC.direction * .05f, 0);
            }
            if (AI_Timer > AI_FlutterTime)
            {
                AI_State = (float)ActionState.Fall;
                AI_Timer = 0;
            }
        }

        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            if (AI_State == (float)ActionState.Fall)
            {
                Rectangle extraDamageHitbox = new Rectangle(npcHitbox.X + 12, npcHitbox.Y + 18, npcHitbox.Width - 24, npcHitbox.Height - 18);
                if (victimHitbox.Intersects(extraDamageHitbox))
                {
                    damageMultiplier *= 2f;
                    Main.NewText("Squished By A Rar Monster!");
                }
            }
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A Cute Little Slime That Wears A Top Hat! I'm Pretty Sure Its One Of The Top Hat Gods Minions..."),
            });
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Gel, Main.rand.Next(0, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<THCTopHat>(), 1, 0, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoreOfValhalla>(), 1, 0, 10));
        }

    }
}
