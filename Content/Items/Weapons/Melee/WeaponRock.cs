using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace PurringTale.Content.Items.Weapons.Melee;

public class WeaponRock : ModItem
{
        public override void SetDefaults()
        {
            Item.damage = 1000;
            Item.DamageType = DamageClass.Melee;
            Item.width = 26;
            Item.height = 18;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 10;
            Item.value = Item.sellPrice(gold: 100);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item1;
            Item.master = true;
            Item.masterOnly = true;
            Item.autoReuse = true;
        }
}
