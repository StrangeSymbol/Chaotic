using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Donmar : Creature
    {
        public Donmar(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, 
            false, false, false, false, 0, false, 0, 0, false, false, false,
            0, 0, 0, 0, 1, 10, Tribe.OverWorld, CreatureType.Caretaker)
        {
        }
        public override string Description()
        {
            return "Donmar Creature - Overworld Caretaker Courage: 45 Power: 65 Wisdom: 65 Speed: 50 Energy: 45 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 10 Energy to target Creature. " +
            "Deciphering the Runic Grove's mysterious markings is Donmar's devotion...and the likely cause of his dementia.";
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
