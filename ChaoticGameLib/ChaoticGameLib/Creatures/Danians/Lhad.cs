using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Lhad : Creature
    {
        public Lhad(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0,
            true, false, false, false, false, false, Tribe.Danian, CreatureType.Mandiblor)
        {
        }

        public override string Description()
        {
            return "Lhad Creature - Danian Mandiblor Courage: 65 Power: 65 Wisdom: 70 Speed: 25 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: Fire " +
            "Unless we become one, We will become none. -- Mandiblor war cry";
        }

    }
}
