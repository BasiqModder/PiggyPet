using Terraria.ModLoader;

namespace BankPet

{
	public class ModdedPlayer : ModPlayer
	{
		public bool pigPetActive;
		public override void ResetEffects()
		{
			pigPetActive = false;
		}

	}
}
