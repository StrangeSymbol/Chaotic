using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Siado : Creature
    {
        public Siado(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, true, false, false, 0,
            false, 0, 0, true, false, false, Tribe.Mipedian, CreatureType.Stalker)
        {
        }

        public override string Description()
        {
            return "Siado Creature - Mipedian Stalker Courage: 60 Power: 45 Wisdom: 60 Speed: 55 Energy: 30 Mugic Ability: 1" +
                " Elemental Type: Air Creature Ability: " +
                "Invisibility: Surprise [This Creature wins initiative checks against Creatures without invisibility.] " +
                "Scout, sentry, spy: Siado is a triple threat to every Mipedian enemy.";
        }

    }
}
