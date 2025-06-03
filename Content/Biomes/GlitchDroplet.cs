using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Biomes
{
    public class GlitchDroplet : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
            GoreID.Sets.LiquidDroplet[Type] = true;


            UpdateType = GoreID.WaterDrip;
        }
    }
}