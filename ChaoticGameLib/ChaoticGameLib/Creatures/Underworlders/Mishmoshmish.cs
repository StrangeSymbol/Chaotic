using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Mishmoshmish : Creature
    {
        public Mishmoshmish(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1,
            false, false, false, false, 1, false, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Scout)
        {
        }
        public override string Description()
        {
            return "Mishmoshmish Creature - Overworld Scout Courage: 35 Power: 65 Wisdom: 45 Speed: 65 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Swift 1 [This Creature may move one additional space.]";
        }
    }
}
