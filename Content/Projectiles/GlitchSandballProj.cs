using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PurringTale.Content.Projectiles
{
    public abstract class GlitchSandballProj : ModProjectile
    {
        public override string Texture => "PurringTale/Content/Projectiles/GlitchSandballProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
            ProjectileID.Sets.ForcePlateDetection[Type] = true;
        }
    }
    
    public class ExampleSandBallFallingProjectile : GlitchSandballProj
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<Tiles.GlitchSand>(), ModContent.ItemType<Items.Placeables.GlitchSand>());
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EbonsandBallFalling);
        }
    }

    public class GlitchSandballProje : GlitchSandballProj
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<Tiles.GlitchSand>());
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EbonsandBallGun);
            AIType = ProjectileID.EbonsandBallGun; 
        }
    }
}