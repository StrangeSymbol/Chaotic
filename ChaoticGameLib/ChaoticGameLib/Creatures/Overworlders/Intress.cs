using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Intress : Creature
    {
        public Intress(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, true, 0,
            false, 0, 0, false, false, false, 
            0, 0, 0, 5, Tribe.OverWorld, CreatureType.Hero)
        {
        }

        public override string Description()
        {
            return "Intress Creature - Overworld Hero Courage: 40 Power: 35 Wisdom: 40 Speed: 55 Energy: 40 Mugic Ability: 1" +
                " Elemental Type: Water Creature Ability: " +
                "Water 5 [Water attacks made by this Creature deal an additional 5 damage.] " +
            "The secret behind Maxxor's bond with Intress is also the wedge that keeps them apart.";
        }
    }
}
