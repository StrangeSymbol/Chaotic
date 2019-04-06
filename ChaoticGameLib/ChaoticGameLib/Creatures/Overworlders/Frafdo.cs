using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Frafdo : Creature
    {
        public Frafdo(Texture2D sprite, Texture2D overlay,
            byte energy, byte courage, byte power, byte wisdom, byte speed)
            : base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            false, false, false, false, 0, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Frafdo Creature - Overworld Guardian Courage: 85 Power: 80 Wisdom: 45 Speed: 75 Energy: 45 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Support Courage 5 [This Creature gains 5 Courage for each adjacent OverWorld Creature you control.] " +
            "This winged warrior makes his nest at Castle Bodhran.";
        }

        public void Ability(byte numAdjacent)
        {
            this.Courage += (byte)(5 * numAdjacent);
        }
    }
}
