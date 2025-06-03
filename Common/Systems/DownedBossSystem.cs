using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PurringTale.Common.Systems
{
    // Acts as a container for "downed boss" flags.
    // Set a flag like this in your bosses OnKill hook:
    ///NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);

    // Saving and loading these flags requires TagCompounds, a guide exists on the wiki: https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound
    public class DownedBossSystem : ModSystem
    {
        public static bool downedTopHat = false;
        public static bool downedEnvy = false;
        public static bool downedGluttony = false;
        public static bool downedGreed = false;
        public static bool downedLust = false;
        public static bool downedPride = false;
        public static bool downedSloth = false;
        public static bool downedWrath = false;
        public static bool downedRock = false;

        public override void ClearWorld()
        {
            downedTopHat = false;
            downedEnvy = false;
            downedGluttony = false;
            downedGreed = false;
            downedLust = false;
            downedPride = false;
            downedSloth = false;
            downedWrath = false;
            downedRock = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedTopHat)
            {
                tag["downedTopHat"] = true;
            }
            if (downedEnvy)
            {
                tag["downedEnvy"] = true;
            }
            if (downedGluttony)
            {
                tag["downedGluttony"] = true;
            }
            if (downedGreed)
            {
                tag["downedGreed"] = true;
            }
            if (downedLust)
            {
                tag["downedLust"] = true;
            }
            if (downedPride)
            {
                tag["downedPride"] = true;
            }
            if (downedSloth)
            {
                tag["downedSloth"] = true;
            }
            if (downedWrath)
            {
                tag["downedWrath"] = true;
            }
            if (downedRock)
            {
                tag["downedRock"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedTopHat = tag.ContainsKey("downedTopHat");
            downedEnvy = tag.ContainsKey("downedEnvy");
            downedGluttony = tag.ContainsKey("downedGluttony");
            downedGreed = tag.ContainsKey("downedGreed");
            downedLust = tag.ContainsKey("downedLust");
            downedPride = tag.ContainsKey("downedPride");
            downedSloth = tag.ContainsKey("downedSloth");
            downedWrath = tag.ContainsKey("downedWrath");
            downedRock = tag.ContainsKey("downedRock");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[1] = downedTopHat;
            flags[2] = downedEnvy;
            flags[3] = downedGluttony;
            flags[4] = downedGreed;
            flags[5] = downedLust;
            flags[6] = downedPride;
            flags[7] = downedSloth;
            flags[8] = downedWrath;
            flags[9] = downedRock;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedTopHat = flags[1];
            downedEnvy = flags[2];
            downedGluttony = flags[3];
            downedGreed = flags[4];
            downedLust = flags[5];
            downedPride = flags[6];
            downedSloth = flags[7];
            downedWrath = flags[8];
            downedRock = flags[9];
        }
    }
}