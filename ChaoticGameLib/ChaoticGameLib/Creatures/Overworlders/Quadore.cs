using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Quadore : Creature
    {
        public Quadore(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, false, true, false, false, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Muge)
        {
        }

        public override string Description()
        {
            return "Quadore Creature - Overworld Muge Courage: 60 Power: 20 Wisdom: 65 Speed: 60 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: Air Creature Ability: None";
        }
    }
}
