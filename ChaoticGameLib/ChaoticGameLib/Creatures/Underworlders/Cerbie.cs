using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Cerbie : Creature
    {
        public Cerbie(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            false, 0, 0, false, false, false,
            0, 0, 0, 0, 0, 10, 0, 0, Tribe.UnderWorld, CreatureType.Taskmaster)
        { }

        public override string Description()
        {
            return "Cerbie Creature - UnderWorld Taskmaster Courage: 50 Power: 95 Wisdom: 25 Speed: 45 Energy: 60 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "As the UnderWorld guardian of The Passage, Cerbie is forever vigilant against " +
            "any potential invasion from the OverWorld.";
        }
    }
}
