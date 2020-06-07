using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Magmon : Creature
    {
        public Magmon(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, true, false, false, false, 0,
            false, 5, 0, false, false, false, 5, 0, 0, 0, Tribe.UnderWorld, CreatureType.Elementalist)
        {
        }

        public override string Description()
        {
            return "Magmon Creature - UnderWorld Elementalist Courage: 75 Power: 60 Wisdom: 20 Speed: 35 Energy: 55 " +
                "Mugic Ability: 1 Elemental Type: Fire Creature Ability: " +
                "Fire 5 [Fire attacks made by this Creature deal an additional 5 damage.] " +
                "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.] " +
            "Magmon's demeanor will melt your heart... after burning through the rest of you.";
        }
    }
}
