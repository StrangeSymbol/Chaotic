using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Neffa : Creature, ISupport
    {
        public Neffa(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2,
            false, false, false, false, 0, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Strategist)
        {
        }

        public void Ability(byte numAdjacent)
        {
            this.Wisdom -= (byte)(5 * PreNumAdja);
            this.Wisdom += (byte)(5 * numAdjacent);
            PreNumAdja = numAdjacent;
        }

        public override string Description()
        {
            return "Neffa Creature - Overworld Strategist Courage: 50 Power: 25 Wisdom: 80 Speed: 35 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Support Wisdom 5 [This Creature gains 5 Wisdom for each adjacent OverWorld Creature you control.] " +
            "No matter how many times he recalculated the ancient equations, the solution always added to seven.";
        }
    }
}
