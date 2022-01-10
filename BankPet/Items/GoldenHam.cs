
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BankPet.Buffs;

namespace BankPet.Items
{
	public class GoldenHam : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Golden Ham");
			Tooltip.SetDefault("Summons a Flying Piggybank to follow you");
		}

		public override void SetDefaults() {
			item.CloneDefaults(ItemID.ZephyrFish);
			item.useTime = 32;
			item.shoot = ModContent.ProjectileType<Projectiles.Pets.PigPet>();
			item.buffType = ModContent.BuffType<PiggyPetBuff>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bacon, 3);
			recipe.AddIngredient(ItemID.GoldBar, 12);
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void UseStyle(Player player) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}