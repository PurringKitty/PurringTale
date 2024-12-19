using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.GameContent.UI.BigProgressBar;
using PurringTale.Content.NPCs.BossNPCs;

namespace PurringTale.Content.NPCs.BossNPCs
{
    // Showcases a custom boss bar with basic logic for displaying the icon, life, and shields properly.
    // Has no custom texture, meaning it will use the default vanilla boss bar texture
    public class BossBar : ModBossBar
    {
        private int bossHeadIndex = -1;

        public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
        {
            // Display the previously assigned head index
            if (bossHeadIndex != -1)
            {
                return TextureAssets.NpcHeadBoss[bossHeadIndex];
            }
            return null;
        }




            }

            
        }
    
