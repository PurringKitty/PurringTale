using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons
{
    public class EyeDrop : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 25;
            Item.knockBack = 5f;
            Item.crit = 3;
            Item.value = Item.sellPrice(copper: 66);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item1;
        }
    }
}