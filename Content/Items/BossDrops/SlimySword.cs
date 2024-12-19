using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using PurringTale.Content.Items.MobLoot;
using Terraria.GameContent.ItemDropRules;

namespace PurringTale.Content.Items.BossDrops;

public class SlimySword : ModItem
{
    // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

    public override void SetDefaults()
    {
        Item.damage = 12;
        Item.DamageType = DamageClass.Melee;
        Item.width = 58;
        Item.height = 64;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.knockBack = 2;
        Item.value = Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }
}
