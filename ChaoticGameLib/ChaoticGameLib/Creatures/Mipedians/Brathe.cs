using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Brathe : Creature
    {
        public Brathe(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 15, false, false, false, Tribe.Mipedian, CreatureType.Elite)
        {
        }

        public override string Description()
        {
            return "Brathe Creature - Mipedian Elite Courage: 55 Power: 45 Wisdom: 65 Speed: 55 Energy: 55 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Invisibility: Strike 15 [Add 15 damage to the first attack this Creature makes each combat.] " +
            "Because invisible Mipedians cast no reflection, Brathe's vanity is a dangerous thing.";
        }

    }
}
