using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace PurringTale.Common.Systems
{
    public class ScreenShakePlayer : ModPlayer
    {
        private int shakeTimer = 0;
        private float shakeIntensity = 0f;

        public void AddScreenShake(int duration, float intensity)
        {
            if (intensity > shakeIntensity)
            {
                shakeTimer = duration;
                shakeIntensity = intensity;
            }
        }

        public override void ModifyScreenPosition()
        {
            if (shakeTimer > 0)
            {
                float shake = shakeIntensity * (shakeTimer / 60f);
                Main.screenPosition += Main.rand.NextVector2Circular(shake, shake);
                shakeTimer--;

                if (shakeTimer <= 0)
                    shakeIntensity = 0f;
            }
        }
    }
}
