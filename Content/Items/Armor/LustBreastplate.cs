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
    public class LustBreastplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Pink; 
            Item.defense = 13;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 50;
            player.statLifeMax2 += 40;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LusterBar>(25)
                .AddIngredient<CoreOfLust>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}