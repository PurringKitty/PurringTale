﻿using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Tools
{
    public class TopiumHamaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 100f;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 55;
            Item.hammer = 300;
            Item.useTurn = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //some dirt effect
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<TopiumBar>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
