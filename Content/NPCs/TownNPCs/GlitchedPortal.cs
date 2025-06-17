using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Subworlds;
using SubworldLibrary;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace PurringTale.Content.NPCs.TownNPCs
{
    public class GlitchedPortal : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoTownNPCHappiness[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0) { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 122;
            NPC.height = 122;
            NPC.dontTakeDamage = true;
            NPC.immortal = false;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 999;
            NPC.damage = 0;
            NPC.rarity = 5;
            NPC.defense = 0;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.alpha = 255;
            NPC.npcSlots = 0;
        }

        public override bool UsesPartyHat() { return false; }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
                NPC.TargetClosest(true);
            switch (NPC.ai[0])
            {
                case 0:
                    NPC.scale = 0.1f;
                    NPC.ai[0] = 1;
                    break;
                case 1:
                    if (NPC.scale < 1)
                        NPC.scale += 0.02f;
                    NPC.alpha -= 3;
                    NPC.velocity.Y = -0.3f;
                    if (NPC.alpha <= 0)
                    {
                        NPC.ai[0] = 2;
                        NPC.scale = 1;
                        NPC.alpha = 0;
                    }
                    break;
                case 2:
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 18000)
                    {
                        NPC.ai[0] = 3;
                        NPC.ai[1] = 0;
                        Main.NewText("The Glitched Portal Has Disapeared", Color.DarkGreen);
                    }
                    break;
                case 3:
                    if (NPC.scale > 0)
                        NPC.scale -= 0.02f;
                    NPC.alpha += 5;
                    NPC.velocity.Y = 0.1f;
                    if (NPC.alpha >= 255 || NPC.scale <= 0)
                        NPC.active = false;
                    break;
            }
            NPC.velocity *= 0;
            NPC.wet = false;
            NPC.lavaWet = false;
            NPC.honeyWet = false;
            NPC.dontTakeDamage = true;
            NPC.rotation += .02f;
            NPC.immune[255] = 30;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.homeless = false;
                NPC.homeTileX = -1;
                NPC.homeTileY = -1;
                NPC.netUpdate = true;
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Enter Portal";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                Main.rand = new UnifiedRandom();
                SubworldSystem.Enter<GlitchDimension>();
            }
            if (SubworldSystem.IsActive<GlitchDimension>())
                SubworldSystem.Exit();
        }
    		public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }
                if (player.inventory.Any(item => item.type == ModContent.ItemType<WeaponRock>()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

