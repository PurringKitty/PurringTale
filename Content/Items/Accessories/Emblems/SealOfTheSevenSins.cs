using PurringTale.Common.Players;
using PurringTale.Content.Buffs;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Emblems;


public class SealOfTheSevenSins : ModItem
{
    public static readonly int SinsBoost = 100;
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 26;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        // THE STATS
        player.GetDamage(DamageClass.Generic) += SinsBoost / 100f;

        player.AddBuff(ModContent.BuffType<SinsBuff>(), 0);

        player.maxMinions += 4;

        player.statLifeMax2 += 100;

        player.statManaMax2 += 100;

        // THE BUFFS
        player.AddBuff(BuffID.Featherfall, 0);

        player.AddBuff(BuffID.Warmth, 0);

        player.AddBuff(BuffID.Wrath, 0);

        player.AddBuff(BuffID.Lovestruck, 0);

        player.AddBuff(BuffID.WeaponImbueGold, 0);

        player.AddBuff(BuffID.WellFed3, 0);

        player.AddBuff(BuffID.WaterWalking, 0);
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<Envy>(1);
        recipe.AddIngredient<Gluttony>(1);
        recipe.AddIngredient<Greed>(1);
        recipe.AddIngredient<Lust>(1);
        recipe.AddIngredient<Pride>(1);
        recipe.AddIngredient<Sloth>(1);
        recipe.AddIngredient<Wrath>(1);
        recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
        recipe.Register();
    }
}