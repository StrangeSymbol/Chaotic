using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ghatup : Creature
    {
        public Ghatup(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, false, true, false, false, 0,
            false, 0, 0, false, false, false, Tribe.Mipedian, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Ghatup Creature - Mipedian Scout Courage: 60 Power: 20 Wisdom: 65 Speed: 60 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: Air Creature Ability: None " +
                "Traveling far beyond the desert, Ghatup has seen " +
                "many sights, including those he wishes he never had.";
        }
    }
}
