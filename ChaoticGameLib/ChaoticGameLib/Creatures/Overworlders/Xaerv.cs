using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Xaerv : Creature
    {
        public Xaerv(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, true, false, false, 0,
            false, 0, 0, false, false, false, 0, 5, 0, 0, Tribe.OverWorld, CreatureType.Elementalist)
        {
        }

        public override string Description()
        {
            return "Xaerv Creature - Overworld Elementalist Courage: 40 Power: 25 Wisdom: 45 Speed: 60 Energy: 30 Mugic Ability: 2" +
                " Elemental Type: Air Creature Ability: " +
                "Air 5 [Air attacks made by this Creature deal an additional 5 damage.] " +
            "\"You got Xaerved.\" -- Xaerv";
        }
    }
}
