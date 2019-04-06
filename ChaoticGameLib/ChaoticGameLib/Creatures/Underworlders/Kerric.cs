using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Kerric : Creature
    {
        public Kerric(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, true, false, false, 1,
            true, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Kerric Creature - UnderWorld Scout Courage: 50 Power: 30 Wisdom: 45 Speed: 65 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: Air Creature Ability: " +
                "Range [This Creature may move though occupied spaces.] Swift 1 [This Creature may move one additional space.] " +
            "Provides crucial aerial surveillance of the Cordac Plungepool.";
        }
    }
}
