using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Yokkis : Creature, ISupport
    {
        public Yokkis(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, false, false, true, false, 0,
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
            byte amount1 = (byte)(5 * adjacentOver);
            byte amount2 = (byte)(5 * PreNumAdja);
            this.Courage += amount1;
            this.Power += amount1;
            this.Wisdom += amount1;
            this.Speed += amount1;
            this.Courage -= amount2;
            this.Power -= amount2;
            this.Wisdom -= amount2;
            this.Speed -= amount2;
            PreNumAdja = adjacentOver;
        }
    }
}
