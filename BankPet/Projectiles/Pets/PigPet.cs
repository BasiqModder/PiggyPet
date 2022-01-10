using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BankPet.Projectiles.Pets
{
	public class PigPet : ModProjectile
	{
		public bool moving = false;
		public bool fast = false;
		public int timer = 0;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Piggy pet");
			Main.projFrames[projectile.type] = 5;
			Main.projPet[projectile.type] = true;
			
		}

		public override void SetDefaults() {

			projectile.tileCollide = false;
			projectile.width = 30;
			projectile.height = 28;
			drawOffsetX = -20;
		}

        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			ModdedPlayer modPlayer = player.GetModPlayer<ModdedPlayer>();

			float distanceFromPlayer = Vector2.Distance(player.Center, projectile.Center);

			if (distanceFromPlayer > 4000)
            {
				projectile.position = player.position;
				
            }
			if (distanceFromPlayer > 100)
			{
				moving = true;
            } 
			if (distanceFromPlayer < 50)
            {
				if (moving)
                {
					moving = false;
					fast = false;
					timer = 0;
					projectile.velocity = Vector2.Zero;
				}
				
			}

			//make it moving up and down
			timer++;
			if (moving == false)
			{
				if (timer < 20)
				{
					projectile.velocity.Y = 0.3f;
				}
				else if (timer > 20 && timer < 40)
				{
					projectile.velocity.Y = 0.25f;
				}
				else if (timer > 40 && timer < 50)
				{
					projectile.velocity.Y = 0;
				}
				else if (timer > 50 && timer < 70)
				{
					projectile.velocity.Y = -0.3f;
				}
				else if (timer > 70 && timer < 90)
				{
					projectile.velocity.Y = -0.25f;
				}
				else if (timer > 90 && timer < 100)
				{
					projectile.velocity.Y = 0;
				}
				
			}
			//Spawn dusts
			if (timer % 16 == 0)
			{
				Vector2 dustPos;
				dustPos.X = projectile.Center.X + Main.rand.Next(-projectile.width/2, projectile.width/2);
				dustPos.Y = projectile.Center.Y;
				Dust.NewDust(dustPos, 3, 3, DustID.Gold, 0.1f, 0.1f);
			}

			if (timer > 100)
			{
				timer = 0;
			}

			if (moving)
            {
				if (distanceFromPlayer > 100 && fast == false)
				{
					projectile.velocity = Vector2.Normalize(player.Center - projectile.Center) * 4;
				}
				if (distanceFromPlayer > 300 && fast == false)
				{
					projectile.velocity = Vector2.Normalize(player.Center - projectile.Center) * 6;
				}
				if (distanceFromPlayer > 700 || fast == true)
                {
					projectile.velocity = Vector2.Normalize(player.Center - projectile.Center) * 12;
					fast = true;
				}
			}

			//Animation when going fast
			if (fast == true)
			{
				int frameSpeed = 20;
				projectile.frameCounter++;
				if (projectile.frameCounter >= frameSpeed)
				{
					projectile.frameCounter = 1;
					projectile.frame++;
					if (projectile.frame >= Main.projFrames[projectile.type])
					{
						projectile.frame = 1;
					}
				}
			}
			else
			{
				projectile.frame = 0;
				projectile.frameCounter = 0;
			}

			//If player is dead, no pet
			if (player.dead)
			{
				modPlayer.pigPetActive = false;
			}

			//If pet not active, let it despawn
			if (modPlayer.pigPetActive)
			{
				projectile.timeLeft = 2;
			}

			//Make it turn towards the player
			if (Main.player[projectile.owner].Center.X < projectile.Center.X)
			{
				projectile.direction = -1;
				drawOffsetX = 0; 
				drawOriginOffsetX = -31;
			}
			else
			{
				projectile.direction = 1;
				drawOffsetX = -20; // These values match the values in SetDefaults
				drawOriginOffsetX = 31;
			}
			projectile.spriteDirection = projectile.direction;

			//Clicking on it will do something
			Vector2 mouse;
			mouse = Main.MouseWorld;
			if (projectile.position.X < mouse.X && projectile.position.Y < mouse.Y && projectile.position.X + 100 > mouse.X && 100 + projectile.position.Y > mouse.Y && player.controlUseTile && Main.mouseRightRelease == true)
			{
				Main.PlaySound(SoundID.Item59);
				if (player.chest == -2 && player.flyingPigChest == projectile.whoAmI)
				{
					player.chest = -1;
					player.flyingPigChest = -1;
					Main.mouseRightRelease = true;
				}
				else
				{
					Main.playerInventory = true;
					Main.mouseRightRelease = false;
					player.flyingPigChest = projectile.whoAmI;
					player.chest = -2;
				}
				Recipe.FindRecipes();
			}
		}
	}
}