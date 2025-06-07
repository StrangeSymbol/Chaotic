using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Banshor : Creature
    {
        public Banshor(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 5, 0, false, false, false, 0, 0, 0, 0, 0, 0, 0, 0, Tribe.UnderWorld, CreatureType.Ethereal)
        {
        }

        public override string Description()
        {
            return "Banshor Creature - UnderWorld Ethereal Courage: 75 Power: 85 Wisdom: 65 Speed: 50 Energy: 60 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
            "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.]";
        }
    }
}
