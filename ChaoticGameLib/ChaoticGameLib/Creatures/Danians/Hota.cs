using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Danians
{
    public class Hota : Creature
    {
        public Hota(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2,
            true, false, false, false, false, false, Tribe.Danian, CreatureType.MandiblorMuge)
        {
        }

        public override string Description()
        {
            return "Hota Creature - Danian Mandiblor Muge Courage: 40 Power: 55 Wisdom: 35 Speed: 30 Energy: 30 " +
                "Mugic Ability: 2 Elemental Type: Fire Creature Ability: " +
            "\"Hota is the least 'Danian' Danian I know\" -- Ibiaan";
        }

    }
}
