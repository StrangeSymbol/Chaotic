using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Tiaane : Creature
    {
        public Tiaane(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 25, Tribe.Mipedian, CreatureType.Muge)
        {
        }

        public override string Description()
        {
            return "Tiaane Creature - Mipedian Muge Courage: 65 Power: 40 Wisdom: 50 Speed: 20 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter, Sacrifice Tiaane: Deal 25 damage to target Creature. " +
                "Tiaane has studied Mugic from Najarin himself and is respected throughout the OverWorld.";
        }

        public void Ability(Creature c)
        {
            if (this.MugicCounters >= this.MugicCost)
            {
                this.MugicCounters -= this.MugicCost;
                this.Alive = false;
                c.Energy -= this.AbilityEnergy;
            }
        }
    }
}
