﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class FrighteningMuck : Attack
    {
        public FrighteningMuck(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 5, 0, 0, 5, 5, 2, 10, 15, false, false, true, true) { }

        public override Tuple<short, short> PotentialDamage(Creature your, Creature enemy, Location location)
        {
            Tuple<short, short> damage = base.PotentialDamage(your, enemy, location);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if ((your.Courage - enemy.Courage) >= this.DisciplineAmount)
                energy2 -= this.EnergyAmount;
            return new Tuple<short, short>(energy1, energy2);
        }

        public override string Description()
        {
            return base.Description() + "Challenge Courage 15: Deal 10 damage.";
        }
    }
}
