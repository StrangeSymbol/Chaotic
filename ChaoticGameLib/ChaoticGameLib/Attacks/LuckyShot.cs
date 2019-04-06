using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class LuckyShot : Attack
    {
        public LuckyShot(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 4, 40, 0, false, false, false, false) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Power < enemy.Power && your.Courage < enemy.Courage
                && your.Wisdom < enemy.Wisdom && your.Speed < enemy.Speed && your.Energy < enemy.Energy)
                enemy.Energy -= this.EnergyAmount;
        }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if (your.Power < enemy.Power && your.Courage < enemy.Courage
                && your.Wisdom < enemy.Wisdom && your.Speed < enemy.Speed && your.Energy < enemy.Energy)
                energy2 -= this.EnergyAmount;
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "If your engaged Creature's Power, Courage, Wisdom, Speed, and Energy are all lower than " +
                "opposing Creature's deal 40 damage. Unique";
        }
    }
}
