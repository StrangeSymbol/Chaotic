using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Heptadd : Creature
    {
        public Heptadd(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, true, true, true, true, 0,
            false, 0, 0, false, true, true, Tribe.OverWorld, CreatureType.GuardianMuge)
        {
        }

        public override string Description()
        {
            return "Heptadd Creature - Overworld GuardianMuge Courage: 55 Power: 60 Wisdom: 50 Speed: 40 Energy: 55 Mugic Ability: 1" +
                " Elemental Type: Fire Air Earth Water Creature Ability: " +
                "Heptadd may cast Mugic from any tribe. Heptadd may not enter mixed armies. Unique " +
            "Some in Perim believe that Heptadd knows where the Cothica is.";
        }
    }
}
