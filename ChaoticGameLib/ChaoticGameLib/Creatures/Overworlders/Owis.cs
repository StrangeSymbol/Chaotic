using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Owis : Creature
    {
        public Owis(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, true, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 5, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Owis Creature - Overworld Guardian Courage: 65 Power: 25 Wisdom: 55 Speed: 30 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: Water Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 5 Energy to target Creature. Pay 1 Mugic Ability: Heal 10 Energy to Owis." +
            "UnderWorlders have made countless attempts to breach Cordac Falls. Owis has repelled all but one...the one that haunts him.";
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
        public void Ability()
        {
            this.AbilityEnergy = 10;
            Ability(this);
            this.AbilityEnergy = 5;
        }
    }
}
