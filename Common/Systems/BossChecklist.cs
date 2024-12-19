using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Common.Systems;
using PurringTale.Content.Items.BossDrops;
using PurringTale.Content.Items.Consumables;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace PurringTale.Common.Systems
{
    // Showcases using Mod.Call of other mods to facilitate mod integration/compatibility/support
    // Mod.Call is explained here https://github.com/tModLoader/tModLoader/wiki/Expert-Cross-Mod-Content#call-aka-modcall-intermediate
    // This only showcases one way to implement such integrations, you are free to explore your own options and other mods examples

    // You need to look for resources the mod developers provide regarding how they want you to add mod compatibility
    // This can be their homepage, workshop page, wiki, GitHub, Discord, other contacts etc.
    // If the mod is open source, you can visit its code distribution platform (usually GitHub) and look for "Call" in its Mod class

    // In addition to the examples shown here, ExampleMod also integrates with the Census Mod (https://steamcommunity.com/sharedfiles/filedetails/?id=2687866031)
    // That integration is done solely through localization files, look for "Census.SpawnCondition" in the .hjson files. 
    public class BossChecklist : ModSystem
    {
        public override void PostSetupContent()
        {
            // Most often, mods require you to use the PostSetupContent hook to call their methods. This guarantees various data is initialized and set up properly

            // Boss Checklist shows comprehensive information about bosses in its own UI. We can customize it:
            // https://forums.terraria.org/index.php?threads/.50668/
            DoBossChecklistIntegration();

            // We can integrate with other mods here by following the same pattern. Some modders may prefer a ModSystem for each mod they integrate with, or some other design.
        }

        private void DoBossChecklistIntegration()
        {
            // The mods homepage links to its own wiki where the calls are explained: https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
            // If we navigate the wiki, we can find the "LogBoss" method, which we want in this case
            // A feature of the call is that it will create an entry in the localization file of the specified NPC type for its spawn info, so make sure to visit the localization file after your mod runs once to edit it

            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }

            // For some messages, mods might not have them at release, so we need to verify when the last iteration of the method variation was first added to the mod, in this case 1.6
            // Usually mods either provide that information themselves in some way, or it's found on the GitHub through commit history/blame
            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            // The "LogBoss" method requires many parameters, defined separately below:

            // Your entry key can be used by other developers to submit mod-collaborative data to your entry. It should not be changed once defined
            string internalName = "TopHat";

            // Value inferred from boss progression, see the wiki for details
            float weight = 23f;

            // Used for tracking checklist progress
            Func<bool> downed = () => DownedBossSystem.downedTopHat;

            // The NPC type of the boss
            int bossType = ModContent.NPCType<CatBoss.TopHatCatBoss>();

            // The item used to summon the boss with (if available)
            int spawnItem = ModContent.ItemType<Content.Items.Consumables.StackOfTopiumBars>();

            // "collectibles" like relic, trophy, mask, pet
            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<Content.Items.Placeables.Furniture.THGBossRelic>(),
                ModContent.ItemType<Content.Items.Placeables.Furniture.THGBossTrophy>(),
                ModContent.ItemType<Content.Items.Vanity.THCTopHat>()
            };

            // By default, it draws the first frame of the boss, omit if you don't need custom drawing
            // But we want to draw the bestiary texture instead, so we create the code for that to draw centered on the intended location
            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("PurringTale/CatBoss/TopHatCat_Head_Boss").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = spawnItem,
                    ["collectibles"] = collectibles,
                }
            );
            internalName = "Envy";
            weight = 0.9f;
            downed = () => DownedBossSystem.downedEnvy;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Envy.EyeOfEnvyBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Envy>(),
                ModContent.ItemType<EnvyousOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfEnvy>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfEnvy>(),
                }
            );
            internalName = "Gluttony";
            weight = 1.6f;
            downed = () => DownedBossSystem.downedGluttony;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Gluttony.EyeOfGluttonyBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Gluttony>(),
                ModContent.ItemType<GluttonusOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfGluttony>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfGluttony>(),
                }
            );
            internalName = "Greed";
            weight = 4.9f;
            downed = () => DownedBossSystem.downedGreed;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Greed.EyeOfGreedBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Greed>(),
                ModContent.ItemType<GreedyOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfGreed>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfGreed>(),
                }
            );
            internalName = "Lust";
            weight = 6.9f;
            downed = () => DownedBossSystem.downedLust;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Lust.EyeOfLustBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Lust>(),
                ModContent.ItemType<LusterOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfLust>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfLust>(),
                }
            );
            internalName = "Pride";
            weight = 11.1f;
            downed = () => DownedBossSystem.downedPride;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Pride.EyeOfPrideBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Pride>(),
                ModContent.ItemType<PridefulOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfPride>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfPride>(),
                }
            );
            internalName = "Sloth";
            weight = 13.65f;
            downed = () => DownedBossSystem.downedSloth;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Sloth.EyeOfSlothBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Sloth>(),
                ModContent.ItemType<SlothyOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfSloth>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfSloth>(),
                }
            );
            internalName = "Wrath";
            weight = 16.99f;
            downed = () => DownedBossSystem.downedWrath;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.Wrath.EyeOfWrathBody>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<Wrath>(),
                ModContent.ItemType<WrathiorOre>(),
                ModContent.ItemType<WeakValhallaOre>(),
                ModContent.ItemType<CoreOfWrath>()
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<BowlOfWrath>(),
                }
            );
        }
    }
}