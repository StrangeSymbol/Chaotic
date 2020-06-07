using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Maglax : Creature
    {
        public Maglax(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, false, false, true, false, 0, 
            false, 0, 0, false, false, false, 0, 0, 5, 0, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Maglax Creature - Overworld Guardian Courage: 70 Power: 60 Wisdom: 25 Speed: 30 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: Earth Creature Ability: " +
                "Earth 5 [Earth attacks made by this Creature deal an additional 5 damage.] " +
            "Mommark's monstrous creation has one thing going for him: a magnetic personality!";
        }
    }
}
