using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Skithia : Creature
    {
        public Skithia(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite,  overlay, negate, energy, courage, power, wisdom, speed, 1, true, false, false, false, 0,
            false, 0, 0, false, false, false, 5, 0, 0, 0, Tribe.UnderWorld, CreatureType.Commander)
        {
        }

        public override string Description()
        {
            return "Skithia Creature - UnderWorld Commander Courage: 65 Power: 25 Wisdom: 55 Speed: 40 Energy: 35 " +
                "Mugic Ability: 1 Elemental Type: Fire Creature Ability: " +
                "Fire 5 [Fire attacks made by this Creature deal an additional 5 damage.] " +
            "Van Bloot's most trusted aide, Skithia is also Takinom's avowed enemy.";
        }

    }
}
