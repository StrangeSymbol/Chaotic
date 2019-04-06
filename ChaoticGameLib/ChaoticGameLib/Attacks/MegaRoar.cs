using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class MegaRoar : Attack
    {
        public MegaRoar(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 4, 10, 70, false, false, false, false) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Speed >= this.DisciplineAmount)
                enemy.Energy -= this.EnergyAmount;
            if (your.Wisdom >= this.DisciplineAmount)
                enemy.Energy -= this.EnergyAmount;
            if (your.Power >= this.DisciplineAmount)
                enemy.Energy -= this.EnergyAmount;
            if (your.Courage >= this.DisciplineAmount)
                enemy.Energy -= this.EnergyAmount;
        }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if (your.Speed >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            if (your.Wisdom >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            if (your.Power >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            if (your.Courage >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Stat Check Courage 70: Deal 10 damage. Stat Check Power 70: Deal 10 damage. " +
                "Stat Check Wisdom 70: Deal 10 damage. Stat Check Speed 70: Deal 10 damage.";
        }
    }
}
