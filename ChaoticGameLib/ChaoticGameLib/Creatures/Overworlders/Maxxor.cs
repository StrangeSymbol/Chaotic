using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Maxxor : Creature
    {
        public Maxxor(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, true, false, true, false, 0, 
            false, 0, 0, false, true, true, 0, 0, 0, 0, 1, 10, Tribe.OverWorld, CreatureType.Hero)
        {
        }

        public override string Description()
        {
            return "Maxxor Creature - Overworld Hero Courage: 100 Power: 65 Wisdom: 80 Speed: 50 Energy: 60 Mugic Ability: 2" +
                " Elemental Type: Fire Earth Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 10 Energy to target Creature. Maxxor may not enter mixed armies. Unique " +
            "No OverWorlder has ever seen Maxxor's face in battle because he is always in the frontline, leading the charge!";
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
