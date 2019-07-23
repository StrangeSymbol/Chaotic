using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Tiaane : Creature, ISacrificeTarget<Creature>
    {
        public Tiaane(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 25, Tribe.Mipedian, CreatureType.Muge)
        {
        }

        public override bool CheckSacrifice(bool hive)
        {
            return this.MugicCounters >= this.MugicCost && this.Energy > 0;
        }

        public override bool CheckSacrificeTarget(Creature creature)
        {
            return creature.Energy > 0;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            this.MugicCounters -= this.MugicCost;
            c.Energy -= this.AbilityEnergy;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return "Tiaane Creature - Mipedian Muge Courage: 65 Power: 40 Wisdom: 50 Speed: 20 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter, Sacrifice Tiaane: Deal 25 damage to target Creature. " +
                "Tiaane has studied Mugic from Najarin himself and is respected throughout the OverWorld.";
        }
    }
}
