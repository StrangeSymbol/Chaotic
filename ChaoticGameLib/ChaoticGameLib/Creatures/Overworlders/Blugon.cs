using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Blugon : Creature
    {
        public Blugon(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, 
            byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, false, false, false, true, 0, false, 
            0, 0, false, false, false, 0, 0, 0, 5, Tribe.OverWorld, CreatureType.Elementalist)
        {
        }
        public override string Description()
        {
            return "Blugon Creature - Overworld Elementalist Courage: 35 Power: 65 Wisdom: 70 Speed: 45 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: Water Creature Ability: " +
                "Water 5 [Water attacks made by this Creature deal an additional 5 damage.] " +
            "The Frozen - mysterious creatures who inhabit Glacier Plains - count Blugon as their friend.";
        }
    }
}
