using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ettala : Creature
    {
        public Ettala(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, false, false, true, false, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Ettala Creature - Overworld Guardian Courage: 50 Power: 50 Wisdom: 20 Speed: 30 Energy: 35 Mugic Ability: 0" +
                " Elemental Type: Earth Creature Ability: None";
        }
    }
}
