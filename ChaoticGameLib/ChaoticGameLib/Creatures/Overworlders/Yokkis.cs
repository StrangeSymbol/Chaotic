using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Yokkis : Creature
    {
        public Yokkis(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, true, false, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Hero)
        {
        }

        public override string Description()
        {
            return "Yokkis Creature - Overworld Hero Courage: 20 Power: 20 Wisdom: 20 Speed: 20 Energy: 50 Mugic Ability: 2" +
                " Elemental Type: Earth Creature Ability: " +
                "Support 5 Courage Support 5 Power Support 5 Wisdom Support 5 Speed. " +
            "He may look funny from a distance, but get too close and the joke's on you.";
        }

        public void Ability(byte adjacentOver)
        {
            byte amount = (byte)(5 * adjacentOver);

            this.Courage += amount;
            this.Power += amount;
            this.Wisdom += amount;
            this.Speed += amount;
        }
    }
}
