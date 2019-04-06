using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Zaur : Creature
    {
        public Zaur(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Warrior)
        {
        }

        public override string Description()
        {
            return "Zaur Creature - UnderWorld Warrior Courage: 65 Power: 75 Wisdom: 35 Speed: 25 Energy: 50 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "\"Failure is such a harsh word. Let's just say my experiment had unintended consequences.\" -- Mommark";
        }
    }
}
