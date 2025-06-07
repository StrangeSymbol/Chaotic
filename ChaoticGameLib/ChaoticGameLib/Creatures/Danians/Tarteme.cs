using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Tarteme : Creature
    {
        public Tarteme(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, false, true, false, false, 0,
            false, 0, 0, false, false, false, 0, 5, 0, 0, Tribe.Danian, CreatureType.Mandiblor)
        {
        }

        public override string Description()
        {
            return "Tarteme Creature - Danian Mandiblor Courage: 50 Power: 65 Wisdom: 35 Speed: 40 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: Air Creature Ability: " +
                "Air 5 [Air attacks made by this Creature deal an additional 5 damage.]";
        }
    }
}
