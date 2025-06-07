using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Prantix : Creature
    {
        public Prantix(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, true, false, true, false, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Caretaker)
        {
        }

        public override string Description()
        {
            return "Prantix Creature - Overworld Caretaker Courage: 40 Power: 50 Wisdom: 30 Speed: 40 Energy: 30 Mugic Ability: 1" +
                " Elemental Type: Fire Earth Creature Ability: None";
        }
    }
}
