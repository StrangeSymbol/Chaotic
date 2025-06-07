using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Mezzmarr : Creature
    {
        public Mezzmarr(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0,
            false, false, false, true, 1, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Scout)
        {
        }
        public override string Description()
        {
            return "Mezzmarr Creature - Overworld Scout Courage: 30 Power: 55 Wisdom: 55 Speed: 55 Energy: 45 Mugic Ability: 0" +
                " Elemental Type: Water Creature Ability: " +
                "Swift 1 [This Creature may move one additional space.]";
        }
    }
}
