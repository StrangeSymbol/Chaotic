using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class FrostBlight : Attack
    {
        public FrostBlight(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 5, 0, 0, 0, 5, 2, 10, 75, false, false, false, true) { }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy, Location location)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy, location);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if (your.Speed >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Stat Check Speed 75: Deal 10 damage.";
        }
    }
}
