using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Danians
{
    public class Lhad : Creature
    {
        public Lhad(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            true, false, false, false, false, false, Tribe.Danian, CreatureType.Mandiblor)
        {
        }

        public override string Description()
        {
            return "Lhad Creature - Danian Mandiblor Courage: 65 Power: 65 Wisdom: 70 Speed: 25 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: Fire Creature Ability: " +
            "Unless we become one, We will become none. -- Mandiblor war cry";
        }

    }
}
