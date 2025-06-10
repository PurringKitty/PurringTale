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
    public class SlothLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 19;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SlothyBar>(15)
                .AddIngredient<CoreOfSloth>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
