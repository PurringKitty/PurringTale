using Microsoft.Xna.Framework;
using PurringTale.Content.Items.Placeables;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons
{
    public class ImposterTongue : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 70;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 60;
            Item.knockBack = 10f;
            Item.crit = 2;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WeakValhallaBar>(1)
                .AddIngredient(ItemID.WallofFleshTrophy, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}