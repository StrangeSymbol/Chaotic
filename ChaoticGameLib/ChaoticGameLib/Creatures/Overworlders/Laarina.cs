using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Laarina : Creature
    {
        public Laarina(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, false, false, true, true, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Laarina Creature - Overworld Scout Courage: 35 Power: 20 Wisdom: 30 Speed: 30 Energy: 30 Mugic Ability: 2" +
                " Elemental Type: Earth Water Creature Ability: None" +
                "\" I don't steal; I liberate lost valuables. And the only problem " +
            "I have with what I do is the possibility of getting caught.\" -- Laarina ";
        }
    }
}
