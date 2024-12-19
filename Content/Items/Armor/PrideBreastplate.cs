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
    public class PrideBreastplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Purple; 
            Item.defense = 17;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            player.statLifeMax2 += 50;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PridefulBar>(25)
                .AddIngredient<CoreOfPride>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}