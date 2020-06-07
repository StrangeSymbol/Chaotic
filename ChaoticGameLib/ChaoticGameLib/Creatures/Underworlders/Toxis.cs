using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Toxis : Creature
    {
        public Toxis(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, true, false, false, false, 0,
            false, 5, 0, false, false, false, Tribe.UnderWorld, CreatureType.Elementalist)
        {
        }

        public override string Description()
        {
            return "Toxis Creature - UnderWorld Elementalist Courage: 45 Power: 70 Wisdom: 40 Speed: 50 Energy: 50 " +
                "Mugic Ability: 2 Elemental Type: Fire Creature Ability: " +
                "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.] " +
                "A resident of the Pits outside the UnderWorld prison. Escapees beg to return to their cells after encountering Toxis.";
        }

    }
}
