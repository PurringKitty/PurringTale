using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;

namespace PurringTale
{
    public class PurringTail : Mod
    {
        public override void Load()
        {
            if (!Main.dedServ)
            {
                Ref<Effect> Shockwave = new Ref<Effect>(ModContent.Request<Effect>("PurringTale/CatBoss/Assets/Shockwave", AssetRequestMode.ImmediateLoad).Value);

                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(Shockwave, "Shockwave"), EffectPriority.VeryHigh);
            }
        }
    }
}

