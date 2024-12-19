using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Projectiles;
using Terraria.Audio;

namespace PurringTale.Content.Items.Weapons;

public class SpearOfSloth : ModItem
{
    // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

    public override void SetDefaults()
    {
        Item.damage = 210;
        Item.DamageType = DamageClass.Magic;
        Item.width = 35;
        Item.height = 37;
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.useTurn = true;
        Item.knockBack = 6.5f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.LightPurple;
        Item.UseSound = SoundID.Item71;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
        Item.shoot = ModContent.ProjectileType<SlothSpearProj>();
        Item.mana = 10;
        Item.shootSpeed = 20f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<SlothyBar>(15);
        recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
        recipe.Register();


    }
    public override void SetStaticDefaults()
    {
        ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
        ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
    }
    public override bool CanUseItem(Player player)
    {
        // Ensures no more than one spear can be thrown out, use this when using autoReuse
        return player.ownedProjectileCounts[Item.shoot] < 1;
    }

    public override bool? UseItem(Player player)
    {
        // Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
        if (!Main.dedServ && Item.UseSound.HasValue)
        {
            SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
        }

        return null;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(100, 2);
        return offset;

    }
}
