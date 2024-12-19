using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using PurringTale.Content.Pets.Boots;

namespace PurringTale.Content.Pets.Boots
{
	public class BootsPetBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) { // This method gets called every frame your buff is active on your player.
			bool unused = false;
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<BootsPetProjectile>());
		}
	}
}
