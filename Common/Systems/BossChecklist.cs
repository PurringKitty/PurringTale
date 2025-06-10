using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Consumables.Summons;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Items.Weapons.Melee;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace PurringTale.Common.Systems
{
    public class BossChecklist : ModSystem
    {
        public override void PostSetupContent()
        {
            DoBossChecklistIntegration();
        }

        private void DoBossChecklistIntegration()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }
            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }
            
            string internalName = "TopHat";

            float weight = 23f;

            Func<bool> downed = () => DownedBossSystem.downedTopHat;

            int bossType = ModContent.NPCType<CatBoss.TopHatCatBoss>();

            int spawnItem = ModContent.ItemType<StackOfTopiumBars>();

            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<Content.Items.Placeables.Furniture.THGBossRelic>(),
                ModContent.ItemType<Content.Items.Placeables.Furniture.THGBossTrophy>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<Content.Items.Vanity.THCTopHat>()
            };
                
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
                ModContent.ItemType<VanityVoucher>(),
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
                ModContent.ItemType<VanityVoucher>(),
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
                ModContent.ItemType<VanityVoucher>(),
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
                ModContent.ItemType<VanityVoucher>(),
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
                ModContent.ItemType<VanityVoucher>(),
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
                ModContent.ItemType<VanityVoucher>(),
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
                ModContent.ItemType<VanityVoucher>(),
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
            internalName = "Rock";
            weight = 50f;
            downed = () => DownedBossSystem.downedRock;
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.ZeRock.ZeRock>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<WeaponRock>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>()
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
                    ["spawnItems"] = ModContent.ItemType<TheRock>(),
                }
            );
        }
    }
}