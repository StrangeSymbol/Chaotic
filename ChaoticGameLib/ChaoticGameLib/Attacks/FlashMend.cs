using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class FlashMend : Attack
    {
        public FlashMend(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 0, 0, 0, 0, 0, 0, 5, 50, false, false, false, false) { }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy, Location location)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy, location);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if (your.Wisdom >= this.DisciplineAmount)
                energy1 += predictedHealAmount(your);
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Stat Check Wisdom 50: Heal 5 damage.";
        }
    }
}
