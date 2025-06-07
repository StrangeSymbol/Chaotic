using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Raimusa : Creature
    {
        public Raimusa(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, true, true, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Warrior)
        {
        }

        public override string Description()
        {
            return "Raimusa Creature - Overworld Warrior Courage: 60 Power: 65 Wisdom: 40 Speed: 45 Energy: 50 Mugic Ability: 1" +
                " Elemental Type: Earth Water Creature Ability: None" +
            "The vicious winds of the Crystal Range " +
            "slash the flesh but sharpen the soul.";
        }
    }
}
