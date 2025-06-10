using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Legs)]
    public class LustLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 11;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LusterBar>(15)
                .AddIngredient<CoreOfLust>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
