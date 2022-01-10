using Terraria;
using Terraria.ModLoader;

namespace BankPet.Buffs
{
	public class PiggyPetBuff : ModBuff
	{
		public override void SetDefaults() {
			//DisplayName and Description are automatically set from the .lang files, but below is how it is done normally.
			DisplayName.SetDefault("Flying Piggybank");
			Description.SetDefault("\'When Pigs Fly\'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<ModdedPlayer>().pigPetActive = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.PigPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.PigPet>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}