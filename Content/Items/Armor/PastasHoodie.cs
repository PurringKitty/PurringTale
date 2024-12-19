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
	public class PastasHoodie : ModItem
	{

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 20;
			Item.defense = 10;
            Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }
	}
}
