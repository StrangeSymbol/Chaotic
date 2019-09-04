using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Pyrithion : Creature
    {
        public Pyrithion(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 10, 0, 10, 0, Tribe.UnderWorld, CreatureType.Commander)
        {
        }

        public override string Description()
        {
            return "Pyrithion Creature - UnderWorld Commander Courage: 45 Power: 80 Wisdom: 40 Speed: 65 Energy: 50 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "Intimidate Courage 10 [Opposing Creature loses 10 Courage until the end of combat.] " +
            "Intimidate Wisdom 10 [Opposing Creature loses 10 Wisdom until the end of combat.] " +
            "\"Telling the truth only goes so far. But lying? That gets you where you're going\" -- Pyrithion";
        }
    }
}
