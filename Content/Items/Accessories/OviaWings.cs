using PurringTale.Content.Items.Placeables;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class OviaWings : ModItem
    {



        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(100, 5f, 2f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1.10f; // Falling glide speed
            ascentWhenRising = 0.90f; // Rising speed
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 5f;
            constantAscend = 0.050f;
        }
    }
}