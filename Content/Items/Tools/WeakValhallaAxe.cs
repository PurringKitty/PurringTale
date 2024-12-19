using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Tools
{
    public class WeakValhallaAxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 13;
            Item.useTurn = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //some dirt effect
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WeakValhallaBar>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
