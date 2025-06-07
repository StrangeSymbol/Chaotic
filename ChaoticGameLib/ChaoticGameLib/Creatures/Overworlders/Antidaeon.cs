using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Antidaeon : Creature
    {
        public Antidaeon(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, false, false, false, true, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Antidaeon Creature - Overworld Scout Courage: 35 Power: 60 Wisdom: 40 Speed: 70 Energy: 40 Mugic Ability: 1" +
                " Elemental Type: Water Creature Ability: None" +
            "With thousands of hidden waterways linking the Overworld " + 
            "and the Underworld, Antidaeon remains on constant alert " +
            "against enemy invasion.";
        }
    }
}
