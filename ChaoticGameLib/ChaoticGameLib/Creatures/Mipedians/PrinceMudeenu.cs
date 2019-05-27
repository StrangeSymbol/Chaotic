using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class PrinceMudeenu : Creature
    {
        public PrinceMudeenu(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0, 
            false, 0, 20, false, false, false, Tribe.Mipedian, CreatureType.Royal)
        {
        }

        public override string Description()
        {
            return "Prince Mudeenu Creature - Mipedian Royal Courage: 55 Power: 35 Wisdom: 70 Speed: 30 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: Air Creature Ability: " +
                "Invisibility: Strike 20 [Add 20 damage to the first attack this Creature makes each combat.] Unique " +
            "Obsessed with controlling the Cothica, he will risk the freedom of the Mipedians to find it.";
        }

    }
}
