using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Biondu : Creature
    {
        public Biondu(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, true, false, false, false, 0,
            false, 0, 15, false, false, false, Tribe.Mipedian, CreatureType.Elite)
        {
        }

        public override string Description()
        {
            return "Biondu Creature - Mipedian Elite Courage: 45 Power: 40 Wisdom: 30 Speed: 35 Energy: 40 Mugic Ability: 1" +
                " Elemental Type: Fire Creature Ability: " +
                "Invisibility: Strike 15 [Add 15 damage to the first attack this Creature makes each combat.] " +
            "The Mipedians desert may be home, but Biondu covets the land beyond the sand.";
        }
    }
}
