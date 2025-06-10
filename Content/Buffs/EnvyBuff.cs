using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Buffs
{
    public class EnvyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}