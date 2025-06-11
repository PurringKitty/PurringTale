using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Tiles.Blocks
{
    public class GlitchTreeLeaf : ModGore
    {
        public override string Texture => "PurringTale/Content/Tiles/Blocks/GlitchTree_Leaf";

        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
            GoreID.Sets.SpecialAI[Type] = 3; 
            GoreID.Sets.PaintedFallingLeaf[Type] = true;
        }
    }
}