using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class EnvyHelmet : ModItem
    {
        
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EnvyBreastplate>() && legs.type == ModContent.ItemType<EnvyLeggings>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EnvyousBar>(20)
                .AddIngredient<CoreOfEnvy>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}