using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using BankPet.Projectiles.Pets;

namespace BankPet
{
	public class PiggyPetMod : Mod
	{
        public override void Load()
        {
            base.Load();
            IL.Terraria.Player.Update += Player_Update;
        }

        private void Player_Update(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdfld<Player>(nameof(Player.flyingPigChest))))
            {
                // could not find opcode
                return;
            }

            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
            {
                return;
            }
            c.Index++;
            int insertionPos = c.Index;

            ILLabel validFlyingPigChestLabel = null;
            if (!c.TryGotoNext(i => i.MatchBeq(out validFlyingPigChestLabel))) // label when type == 525 (piggy bank projectile type)
            {
                return;
            }

            c.Goto(insertionPos); // go back to after the brfalse
            c.Emit(OpCodes.Ldarg_0); // push this (the player instance)
            c.Emit(OpCodes.Ldfld, typeof(Player).GetField(nameof(Player.flyingPigChest))); // player.flyingPigChest

            // if this method returns true, it will skip the normal checks
            // if it returns false, it will let the original checks happen
            c.EmitDelegate<Func<int, bool>>(delegate (int flyingPigChest)
            {
                int type = ModContent.ProjectileType<PigPet>(); // change this for the type of your projectile
                Projectile proj = Main.projectile[flyingPigChest];
                return proj.active && proj.type == type;
            });

            c.Emit(OpCodes.Brtrue, validFlyingPigChestLabel);
        }
    }

}