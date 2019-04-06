using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Nauthilax : Creature
    {
        public Nauthilax(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, true, 0,
            false, 5, 0, false, false, false, Tribe.UnderWorld, CreatureType.Elementalist)
        {
        }

        public override string Description()
        {
            return "Nauthilax Creature - UnderWorld Elementalist Courage: 65 Power: 60 Wisdom: 45 Speed: 55 Energy: 60 " +
                "Mugic Ability: 1 Elemental Type: Water Creature Ability: " +
                "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.] " +
                "Few UnderWorlders can swim, which makes Nauthilax crucial to Chaor's maritime intelligence gathering.";
        }

    }
}
