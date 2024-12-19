using Microsoft.Xna.Framework;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons
{
    public class GoldOnAStick : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Generic;
            Item.damage = 35;
            Item.knockBack = 4.5f;
            Item.crit = 5;
            Item.value = Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GreedyBar>(15)
                .AddIngredient<CoreOfGreed>(15)
                .AddTile<Tiles.Furniture.ValhallaWorkbench>()
                .Register();
        }
    }
}