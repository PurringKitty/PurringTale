using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shoes)]
    public class TreadBoots : ModItem
	{
		public override void SetStaticDefaults() 
		{
		}

		public override void SetDefaults() 
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Master;
			Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.moveSpeed += 2.50f;
			player.accRunSpeed += 0.50f;
        }
	}
}