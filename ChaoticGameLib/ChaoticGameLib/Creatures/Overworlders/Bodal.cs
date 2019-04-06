using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Bodal : Creature
    {
        public Bodal(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1,
            false, false, false, false, 0, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Caretaker)
        {
        }

        public void Ability(byte numAdjacent)
        {
            this.Wisdom += (byte)(5 * numAdjacent);
        }

        public override string Description()
        {
            return "Bodal Creature - Overworld Caretaker Courage: 40 Power: 40 Wisdom: 80 Speed: 60 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Support Wisdom 5 [This Creature gains 5 Wisdom for each adjacent OverWorld Creature you control.] " +
            "The trouble with Bodal is that Bodal is troubled.";
        }
    }
}
