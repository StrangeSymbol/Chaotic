using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ChaoticGameLib.Creatures
{
    public class Slurhk : Creature
    {
        public Slurhk(Texture2D sprite, Texture2D overlay,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, true, true, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Slurhk Creature - Overworld Guardian Courage: 45 Power: 35 Wisdom: 35 Speed: 40 Energy: 50 Mugic Ability: 1" +
                " Elemental Type: Earth Water Creature Ability: None" +
                "As poison spreads throughout the Mipedian's body, Slurhk sneers. He will enjoy reclaiming " +
            "his desert homeland - one lizard at a time.";
        }
    }
}
