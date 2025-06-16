using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Accessories.Masks;
using PurringTale.Content.Items.Accessories.Necklaces;
using PurringTale.Content.Items.Armor;
using PurringTale.Content.Items.Consumables.Bags;
using PurringTale.Content.Items.Consumables.Summons;
using PurringTale.Content.Items.Lore;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Furniture;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Items.Tools;
using PurringTale.Content.Items.Weapons.Magic;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Weapons.Ranged;
using PurringTale.Content.Items.Weapons.Summoner;
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
                ModContent.ItemType<THGBossRelic>(),
                ModContent.ItemType<THGBossTrophy>(),
                ModContent.ItemType<OldBook1>(),
                ModContent.ItemType<OldBook2>(),
                ModContent.ItemType<OldBook3>(),
                ModContent.ItemType<OldBook4>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<TopiumOre>(),
                ModContent.ItemType<MoonlightGreatSword>(),
                ModContent.ItemType<EldritchBlaster>(),
                ModContent.ItemType<TopHatDemonEye>(),
                ModContent.ItemType<TopHatDemonPendent>()
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
                ModContent.ItemType<EnvyHelmet>(),
                ModContent.ItemType<EnvyBreastplate>(),
                ModContent.ItemType<EnvyLeggings>(),
                ModContent.ItemType<TheEnvyousEye>(),
                ModContent.ItemType<BladeOfEnvy>(),
                ModContent.ItemType<EnvyousBow>(),
                ModContent.ItemType<WhipOfEnvy>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
                ModContent.ItemType<GluttonyHelmet>(),
                ModContent.ItemType<GluttonyBreastplate>(),
                ModContent.ItemType<GluttonyLeggings>(),
                ModContent.ItemType<TheGluttonsGun>(),
                ModContent.ItemType<GluttonsGreatsword>(),
                ModContent.ItemType<WhipOfGluttony>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
                ModContent.ItemType<GreedHelmet>(),
                ModContent.ItemType<GreedBreastplate>(),
                ModContent.ItemType<GreedLeggings>(),
                ModContent.ItemType<GoldOnAStick>(),
                ModContent.ItemType<WhipOfGreed>(),
                ModContent.ItemType<GemIncrustedRevolver>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
                ModContent.ItemType<LustHelmet>(),
                ModContent.ItemType<LustBreastplate>(),
                ModContent.ItemType<LustLeggings>(),
                ModContent.ItemType<WhipOfLust>(),
                ModContent.ItemType<LanceOfLust>(),
                ModContent.ItemType<LustBazooka>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
                ModContent.ItemType<PrideHelmet>(),
                ModContent.ItemType<PrideBreastplate>(),
                ModContent.ItemType<PrideLeggings>(),
                ModContent.ItemType<AxeOfPride>(),
                ModContent.ItemType<WhipOfPride>(),
                ModContent.ItemType<PridePistol>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
                ModContent.ItemType<SlothHelmet>(),
                ModContent.ItemType<SlothBreastplate>(),
                ModContent.ItemType<SlothLeggings>(),
                ModContent.ItemType<BookOfSloth>(),
                ModContent.ItemType<SlothfulLance>(),
                ModContent.ItemType<WhipOfSloth>(),
                ModContent.ItemType<SlothfulShotgun>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
                ModContent.ItemType<WrathHelmet>(),
                ModContent.ItemType<WrathBreastplate>(),
                ModContent.ItemType<WrathLeggings>(),
                ModContent.ItemType<SinsBane>(),
                ModContent.ItemType<WhipOfWrath>(),
                ModContent.ItemType<WrathfulMachineGun>(),
                ModContent.ItemType<SinsBossRelic>(),
                ModContent.ItemType<SinsBossTrophy>(),
                ModContent.ItemType<VanityVoucher>(),
                ModContent.ItemType<CoreOfValhalla>(),
                ModContent.ItemType<ValhallaOre>()
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
            bossType = ModContent.NPCType<Content.NPCs.BossNPCs.ZeRock.RockBoss>();
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