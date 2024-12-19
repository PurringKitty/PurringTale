using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PurringTale.Content.Projectiles
{
	public class CrystalSlimeProj : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			//DisplayName.SetDefault("Gel Bullet"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
		}

		public override void SetDefaults() 
		{
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 16;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
		}



        public override void AI()
        {
			Projectile.aiStyle = 0;
			Lighting.AddLight(Projectile.position, 0.2f, 0.2f, 0.6f);
			Lighting.Brightness(5, 5);
		}

		[System.Obsolete]
        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Dig.WithVolumeScale(0.5f).WithPitchOffset(0.8f), Projectile.position);
			for (var i = 0; i < 6; i++)
            {
#pragma warning disable IDE0034 // Simplify 'default' expression
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CrimsonSpray, 0f, 0f, 0, default(Color), 1f);
#pragma warning restore IDE0034 // Simplify 'default' expression
            }
        }
	}
}
