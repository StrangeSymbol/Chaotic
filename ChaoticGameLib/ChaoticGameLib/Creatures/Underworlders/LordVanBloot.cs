using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class LordVanBloot : Creature
    {
        public LordVanBloot(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, false, false, true, false, 0,
            false, 0, 0, false, true, true, Tribe.UnderWorld, CreatureType.ConquerorMuge)
        {
        }

        public override string Description()
        {
            return "Lord Van Bloot Creature - UnderWorld Conqueror Muge Courage: 75 Power: 115 Wisdom: 50 Speed: 95 Energy: 65 " +
                "Mugic Ability: 2 Elemental Type: Earth Creature Ability: " +
                "If the opposing Creature's Courage is less than 65, that Creature loses 10 Energy. " +
                "Lord Van Bloot may not enter mixed armies. Unique " +
            "\"I may serve Chaor, but in time, Chaor will serve me!\" -- Lord Van Bloot";
        }
    }
}
