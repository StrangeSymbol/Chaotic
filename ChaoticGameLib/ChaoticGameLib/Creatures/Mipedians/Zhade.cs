using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Zhade : Creature
    {
        public Zhade(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, true, false, false, 0,
            false, 0, 0, true, false, false, Tribe.Mipedian, CreatureType.Stalker)
        {
        }

        public override string Description()
        {
            return "Zhade Creature - Mipedian Stalker Courage: 65 Power: 60 Wisdom: 40 Speed: 60 Energy: 30 Mugic Ability: 1" +
                " Elemental Type: Air Creature Ability: " +
                "Invisibility: Surprise [This Creature wins initiative checks against Creatures without invisibility.] " +
                "Some lead. Some follow. Zhade is content to just hang around.";
        }
    }
}
