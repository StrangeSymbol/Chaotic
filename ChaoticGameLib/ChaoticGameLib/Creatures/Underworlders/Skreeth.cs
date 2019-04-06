using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Skreeth : Creature
    {
        public Skreeth(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, false, false, 1,
            true, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Warrior)
        {
        }

        public override string Description()
        {
            return "Skreeth Creature - UnderWorld Warrior Courage: 80 Power: 65 Wisdom: 60 Speed: 20 Energy: 55 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Range [This Creature may move though occupied spaces.] Swift 1 [This Creature may move one additional space.] " +
            "Skreeth's boldness may be his undoing some day. Spineless he's not!";
        }

    }
}
