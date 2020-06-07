using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Crawsectus : Creature
    {
        public Crawsectus(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, 
            byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0,
            false, false, true, true, 0, false, 0, 0, false, false, false, 0, 0, 5, 0, Tribe.OverWorld, CreatureType.Elementalist)
        {
        }
        public override string Description()
        {
            return "Crawsectus Creature - Overworld Elementalist Courage: 60 Power: 45 Wisdom: 40 Speed: 25 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: Earth Water Creature Ability: " +
                "Earth 5 [Earth attacks made by this Creature deal an additional 5 damage.] " +
            "His hard exterior hides an even harder interior.";
        }
    }
}
