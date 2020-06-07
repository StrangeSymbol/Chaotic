using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Dractyl : Creature
    {
        public Dractyl(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed)
            : base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0,
            false, false, false, false, 1, true, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Dractyl Creature - Overworld Scout Courage: 25 Power: 70 Wisdom: 50 Speed: 70 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Range [This Creature may move though occupied spaces.] Swift 1 [This Creature may move one additional space.] " +
            "The high-flying OverWorld messenger and spy.";
        }
    }
}
