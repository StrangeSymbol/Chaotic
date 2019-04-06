using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Staluk : Creature
    {
        public Staluk(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, true, false, 1, false, 
            0, 0, false, false, false, 0, 0, 5, 0, Tribe.OverWorld, CreatureType.Warrior)
        {
        }

        public override string Description()
        {
            return "Staluk Creature - Overworld Warrior Courage: 35 Power: 45 Wisdom: 25 Speed: 40 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: Earth Creature Ability: " +
                "Earth 5 [Earth attacks made by this Creature deal an additional 5 damage.] " +
                "Swift 1 [This Creature may move one additional space.]" +
            "This Mommark creation is slow to start...and difficult to stop.";
        }
    }
}
