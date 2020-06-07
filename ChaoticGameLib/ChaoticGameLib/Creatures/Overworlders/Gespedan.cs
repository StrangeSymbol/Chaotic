using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Gespedan : Creature, ISupport
    {
        public Gespedan(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, false, 1,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Gespedan Creature - Overworld Scout Courage: 45 Power: 50 Wisdom: 35 Speed: 100 Energy: 35 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Support Speed 5 [This Creature gains 5 Speed for each adjacent OverWorld Creature you control.] " +
            "Swift 1 [This Creature may move one additional space.] " +
            "No matter how swiftly Gespedan sprints, he can't outrun his one, tragic mistake.";
        }

        public void Ability(byte numAdjacent)
        {
            this.Speed -= (byte)(5 * PreNumAdja);
            this.Speed += (byte)(5 * numAdjacent);
            PreNumAdja = numAdjacent;
        }
    }
}
