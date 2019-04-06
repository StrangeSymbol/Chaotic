using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Tartarek : Creature
    {
        public Tartarek(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 0, Tribe.OverWorld, CreatureType.Hero)
        {
        }

        public override string Description()
        {
            return "Tartarek Creature - Overworld Hero Courage: 90 Power: 45 Wisdom: 90 Speed: 25 Energy: 35 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Ability: Target Creature gains \"Swift 1\" until the end of the turn. " +
            "Brave enough to take on any battle, wise enough to avoid most.";
        }

        public void Ability(Creature c)
        {
            if (this.MugicCounters >= this.MugicCost)
            {
                this.MugicCounters -= this.MugicCost;
                c.Swift += 1;
            }
        }
    }
}
