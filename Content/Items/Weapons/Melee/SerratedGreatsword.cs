using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;

namespace PurringTale.Content.Items.Weapons.Melee;

public class SerratedGreatsword: ModItem
{
        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.DamageType = DamageClass.Melee;
            Item.width = 78;
            Item.height = 92;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 10;
            Item.crit = 10;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ItemRarityID.Quest;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }


    public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<BrokenSerratedGreatsword>(1);
            recipe.AddIngredient<TopiumBar>(20);
            recipe.AddIngredient<CoreOfValhalla>(20);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
