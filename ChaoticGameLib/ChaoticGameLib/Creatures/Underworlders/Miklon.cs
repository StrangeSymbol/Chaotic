using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Miklon : Creature
    {
        public Miklon(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override string Description()
        {
            return "Miklon Creature - UnderWorld Taskmaster Courage: 20 Power: 75 Wisdom: 30 Speed: 60 Energy: 50 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
            "After the \"accident,\" Khybon crafted the gyropod that keeps this prison guard on patrol.";
        }
    }
}
