using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Necklaces;

[AutoloadEquip(EquipType.Neck)]

public class PastaNecklace : ModItem
{
    public static readonly int PastaBoost = 5;
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 14;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Green;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetAttackSpeed(DamageClass.Melee) += PastaBoost / 100f;
        player.GetDamage(DamageClass.Melee) += PastaBoost / 100f;
        player.sunflower = true;
    }
}