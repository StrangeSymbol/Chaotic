using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Vidav : Creature
    {
        public Vidav(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed)
            : base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 15, Tribe.OverWorld, CreatureType.Strategist)
        {
        }

        public override string Description()
        {
            return "Vidav Creature - Overworld Strategist Courage: 45 Power: 30 Wisdom: 50 Speed: 35 Energy: 35 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 15 Energy to target Creature. The Treaty of Vidav " +
            "could have brought peace to the warring tribes of Perim...if only its leaders had been willing to accept it.";
        }

        public void Ability(Creature c)
        {
            if (this.MugicCounters >= this.MugicCost)
            {
                // cost of activating ability
                this.MugicCounters -= this.MugicCost;
                c.Heal(this.AbilityEnergy);
            }
        }
    }
}
