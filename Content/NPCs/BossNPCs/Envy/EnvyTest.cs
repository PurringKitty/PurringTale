// WORK IN PROGRESS IS HELL//




using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace PurringTale.Content.NPCs.BossNPCs.Envy
{
    [AutoloadBossHead]
    public class EnvyTest : ModNPC
    {
        private int state
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private int substate
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float statetimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private float statetimer2
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        private bool secondPhase => state == 1;



        public override void SetStaticDefaults()
        {

        }



        public override void AI()
        {
            // Le Boss Handle Target
            if (NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
            }

            // Boss Targeting
            Player player = Main.player[NPC.target];

            // Despawning Behaviour
            if (player.dead || !player.active)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            // State Handling
            switch (state)
            {
                case 0:
                    HandleFirstState(player);
                    break;
                case 1:
                    HandleSecondState(player);
                    break;
            }
        }

        private void HandleFirstState(Player player) 
        {
            // Move To Le Player
            if (substate == 0)
            {
                // Start Speed
                float baseMoveSpeed = 5f;
                float accelerationSpeed = 0.04f;

                // Expert Speed

                if (Main.expertMode)
                {
                    baseMoveSpeed = 7f;
                    accelerationSpeed = 0.15f;
                }

                // Master Speed

                if (Main.masterMode)
                {
                    baseMoveSpeed = 10f;
                    accelerationSpeed = 0.30f;
                }

                // Move To Target
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                // Increase State Timer
                statetimer += 1f;

                //Check If Change Substate
                float threshold = 600f;
                if(Main.expertMode)
                {
                    threshold += 0.3f;
                }
                if (Main.masterMode)
                {
                    threshold += 0.6f;
                }

                // Change Substate If Conditions Met
                if(statetimer >= threshold)
                {
                    substate = 1;
                    statetimer = 0f;
                    statetimer2 = 0f;
                    NPC.netUpdate = true;
                    return;
                }
            }
            else
            //Charge At Player
            if (substate == 1)
            {
                float baseSpeed = 6f;
                if (Main.expertMode || Main.masterMode)
                {
                    baseSpeed = 8f;
                }

                // Charge Velocity
                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                // Get Distance
                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                // Caculate Velocity
                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                // Apply Velocity
                NPC.velocity = velocity;

                // Move To Post Charge
                substate = 2;

                // Update Network
                NPC.netUpdate = true;
                if (NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }
            else
            // Post Charge State
            if (substate == 2)
            {
                // Increase Timer
                statetimer += 1f;

                // Slow Down The Charge
                if (statetimer >= 48f)
                {
                    // Slow Velocity
                    NPC.velocity *= 0.98f;

                    // Adjust Based ON Difficulty
                    if (Main.expertMode || Main.masterMode)
                    {
                        NPC.velocity *= 0.985f;
                    }

                    // If Velocity Close To 0
                    if (Math.Abs(NPC.velocity.X) < 0.05) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.05) NPC.velocity.Y = 0f;
                }

                // Handle State Changing
                int threshold = 150;
                if (Main.expertMode || Main.masterMode)
                {
                    threshold = 100;
                }

                //Handle State Change
                if (statetimer >= threshold)
                {
                    // INcrement SECOND Timer
                    statetimer2 += 1f;

                    // Reset Main Timer
                    statetimer = 0f;

                    // Reset Target
                    NPC.target = 255;

                    // Change Sub State
                    if (statetimer2 >= 2f)
                    {
                        substate = 0;
                        statetimer2 = 0f;
                    }
                    else
                    {
                        substate = 1;
                    }
                }
            }
        }

        private void HandleSecondState(Player player)
        {

        }

        private void MoveToTarget(Player player, float moveSpeed, float accelerationRate, out float distanceToPlayer)
        {
            //set Distance To Person
            distanceToPlayer = Vector2.Distance(NPC.Center, player.Center);

            // Move Speeds
            float movementSpeed = moveSpeed / distanceToPlayer;

            float targetVelocityX = (player.Center.X - NPC.Center.X) * movementSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * movementSpeed;

            //Apply Acceleration For X
            if (NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X += accelerationRate;
                if(NPC.velocity.X < 0f && targetVelocityX > 0f)
                {
                    NPC.velocity.X += accelerationRate;
                }
            }
            if (NPC.velocity.X > targetVelocityX)
            {
                NPC.velocity.X -= accelerationRate;
                if (NPC.velocity.X > 0f && targetVelocityX < 0f)
                {
                    NPC.velocity.X -= accelerationRate;
                }
            }

            //Apply Acceleration For Y
            if (NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y += accelerationRate;
                if (NPC.velocity.Y < 0f && targetVelocityY > 0f)
                {
                    NPC.velocity.Y += accelerationRate;
                }
            }
            if (NPC.velocity.Y > targetVelocityY)
            {
                NPC.velocity.Y -= accelerationRate;
                if (NPC.velocity.Y > 0f && targetVelocityY < 0f)
                {
                    NPC.velocity.Y -= accelerationRate;
                }
            }
        }
    }
}
