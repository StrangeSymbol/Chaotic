using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class ShriekShock : Attack
    {
        public ShriekShock(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 0, 5, 50, false, false, false, false) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Speed >= this.DisciplineAmount)
                enemy.Energy -= this.EnergyAmount;
        }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if (your.Speed >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Stat Check Speed 50: Deal 5 damage.";
        }
    }
}
