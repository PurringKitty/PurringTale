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
    public class DirtHelmet : ModItem
    {
        public static LocalizedText SetBonusText { get; private set; }
        public static readonly int AdditiveBonus = 10;

        public override void SetStaticDefaults()
        {
            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveBonus);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Gray;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DirtBreastplate>() && legs.type == ModContent.ItemType<DirtLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonusText.Value;
            player.statLifeMax2 += AdditiveBonus;
            player.statManaMax2 += AdditiveBonus;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 15)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}