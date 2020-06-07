using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ghuul : Creature
    {
        public Ghuul(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, true, false, false, false, 1,
            true, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override string Description()
        {
            return "Ghuul Creature - UnderWorld Taskmaster Courage: 50 Power: 85 Wisdom: 15 Speed: 35 Energy: 35 Mugic Ability: 0" +
                " Elemental Type: Fire Creature Ability: " +
                "Range [This Creature may move though occupied spaces.] Swift 1 [This Creature may move one additional space.] " +
            "\"How many more until you're satisfied, Mommark? Every mistake grows their numbers!\" -- Maxxor";
        }
    }
}
