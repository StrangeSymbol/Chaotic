using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Rellim : Creature, ISupport
    {
        public Rellim(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, true, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Caretaker)
        {
        }
        // TODO: Get Stats.
        public override string Description()
        {
            return "Rellim Creature - Overworld Caretaker Courage: 50 Power: 65 Wisdom: 60 Speed: 40 Energy: 50 Mugic Ability: 1" +
                " Elemental Type: Water Creature Ability: " +
                "Support Courage 5 [This Creature gains 5 Courage for each adjacent OverWorld Creature you control.] " +
            "\"The Mill is not my only interest, I also enjoy beating on UnderWorlders.\" -- Rellim.";
        }

        public void Ability(byte numAdjacent)
        {
            this.Courage -= (byte)(5 * PreNumAdja);
            this.Courage += (byte)(5 * numAdjacent);
            PreNumAdja = numAdjacent;
        }
    }
}
