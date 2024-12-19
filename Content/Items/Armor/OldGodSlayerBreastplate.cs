using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Body)]
    public class OldGodSlayerBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18; // Width of the item
            Item.height = 18; // Height of the item
            Item.value = Item.sellPrice(gold: 50); // How many coins the item is worth
            Item.rare = ItemRarityID.Orange; // The rarity of the item
            Item.defense = 9; // The amount of defense the item will give when equipped
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 25; // Increase how many mana points the player can have by 20
            player.maxMinions += 2; // Increase how many minions the player can have by one
            player.statLifeMax2 += 25;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<WeakValhallaBar>(20);
            recipe.AddIngredient<CoreOfValhalla>(10);
            recipe.AddIngredient<DirtBreastplate>(1);
            recipe.AddIngredient<WeakValhallaBreastplate>(1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
