using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class TelekineticBolt : Attack
    {
        public TelekineticBolt(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 10, 0, 0, 0, 0, 3, 10, 15, false, false, false, false) { }

        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if ((your.Courage - enemy.Courage) >= this.DisciplineAmount && (your.Wisdom - enemy.Wisdom) >= this.DisciplineAmount)
            {
                enemy.Energy -= this.EnergyAmount;
                your.Heal(this.EnergyAmount);
            }
        }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if ((your.Courage - enemy.Courage) >= this.DisciplineAmount && (your.Wisdom - enemy.Wisdom) >= this.DisciplineAmount)
            {
                energy2 -= this.EnergyAmount;
                energy1 += this.EnergyAmount;
            }
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Courage Challenge 15, Wisdom Challenge 15: Deal 10 damage and heal 10 damage.";
        }
    }
}
