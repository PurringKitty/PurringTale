using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Boots
{
    [AutoloadEquip(EquipType.Shoes)]
    public class TreadBoots : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults() 
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
            Item.UseSound = SoundID.Item24;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.rocketBoots = 0;
			player.moveSpeed += 1.40f;
			player.accRunSpeed += 0.55f;
			player.waterWalk2 = true;
			player.fireWalk = true;
        }
	}
}