using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Shimmark : Creature
    {
        public Shimmark(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0, 
            false, 0, 10, false, false, false, Tribe.Mipedian, CreatureType.Stalker)
        {
        }

        public override string Description()
        {
            return "Shimmark Creature - Mipedian Stalker Courage: 60 Power: 50 Wisdom: 30 Speed: 70 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Invisibility: Strike 10 [Add 10 damage to the first attack this Creature makes each combat.] " +
                "Shimmark is like a footprint in the sand: quickly gone with the wind.";
        }

    }
}
