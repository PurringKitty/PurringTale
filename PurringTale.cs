using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using System.Security.Permissions;
using PurringTale.CatBoss;

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

