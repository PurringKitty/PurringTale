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
    public class WrathHelmet : ModItem
    {
        public static LocalizedText SetBonusText { get; private set; }
        public static readonly int AdditiveBonus = 100;
        public static readonly int MeleeDMG = 10;

        public override void SetStaticDefaults()
        {
            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveBonus);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Red;
            Item.defense = 23;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WrathBreastplate>() && legs.type == ModContent.ItemType<WrathLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.AddBuff(BuffID.Rage, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WrathiorBar>(20)
                .AddIngredient<CoreOfWrath>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}