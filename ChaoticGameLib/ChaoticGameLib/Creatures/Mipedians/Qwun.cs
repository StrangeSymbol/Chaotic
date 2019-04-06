using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Qwun : Creature
    {
        public Qwun(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, true, false, false, 0,
            false, 0, 0, true, false, false, Tribe.Mipedian, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Qwun Creature - Mipedian Scout Courage: 55 Power: 40 Wisdom: 55 Speed: 90 Energy: 35 Mugic Ability: 0" +
                " Elemental Type: Air Creature Ability: " +
                "Invisibility: Surprise [This Creature wins initiative checks against Creatures without invisibility.] " +
                "With Qwun's blinding speed, using his invisibility is rarely necessary.";
        }

    }
}
