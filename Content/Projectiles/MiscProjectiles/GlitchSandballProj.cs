using PurringTale.Content.Tiles.Blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PurringTale.Content.Projectiles.MiscProjectiles
{
    public abstract class GlitchSandballProj : ModProjectile
    {
        public override string Texture => "PurringTale/Content/Projectiles/MiscProjectiles/GlitchSandballProj";

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
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<GlitchSand>(), ModContent.ItemType<Items.Placeables.Blocks.GlitchSand>());
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
            ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<GlitchSand>());
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EbonsandBallGun);
            AIType = ProjectileID.EbonsandBallGun; 
        }
    }
}