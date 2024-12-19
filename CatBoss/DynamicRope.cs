using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;

namespace PurringTale.CatBoss
{
	public class DynamicRope : ModProjectile
	{
		struct RopePoint
		{
			public Vector2 position, lastPosition;
			public float rotation;
			public float index;
			public bool _fixed;
		}

		private RopePoint[] points;
		private ref float ChainTexture => ref Projectile.ai[3];

        public override string Texture => "Terraria/Images/Projectile_0";
        public void UpdatePositions()
		{
			for (int i = 0; i < points.Length-1; i++)
			{
				var p = points[i];

				if (!p._fixed)
				{
					Vector2 lastPos = p.position;
                    p.position += p.position - p.lastPosition;
					p.position.Y += 1f;
					p.lastPosition = lastPos;
				}
				if (i < points.Length -1)
					p.rotation = p.position.DirectionTo(points[i + 1].position).ToRotation();
				
			}
		}
        public override void PostDraw(Color lightColor)
        {
			Texture2D chainTexture = TextureAssets.Chain.Value;
            //Main.spriteBatch.Draw(chainTexture, )
        }
    }
}

