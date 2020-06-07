using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class ParalEyes : Attack
    {
        public ParalEyes(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 10, 0, 0, 0, 0, 2, 10, 15, false, false, false, false) { }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy, Location location)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy, location);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if ((your.Speed - enemy.Speed) >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Challenge Speed 15: Deal 10 damage.";
        }
    }
}
