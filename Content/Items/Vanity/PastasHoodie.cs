using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class PastasHoodie : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<VanityVoucher>(1);
            recipe.AddTile<ValhallaWorkbench>();
            recipe.Register();
        }
    }
}
