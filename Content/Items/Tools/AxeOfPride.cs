using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Tools
{
    public class AxeOfPride : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee;
            Item.width = 35;
            Item.height = 35;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 30;
            Item.useTurn = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //some dirt effect
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PridefulBar>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
