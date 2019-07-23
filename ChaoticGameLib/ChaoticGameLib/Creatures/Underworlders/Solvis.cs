using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Solvis : Creature, IActivateTarget<Creature>
    {
        public Solvis(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 2, 10, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost;
        }

        public override bool CheckAbilityTarget(Creature creature, bool sameOwner)
        {
            return creature.Energy > 0;
        }

        public override string Description()
        {
            return "Solvis Creature - UnderWorld Taskmaster Courage: 45 Power: 60 Wisdom: 65 Speed: 35 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Pay 2 Mugic Counters: Deal 10 damage to target Creature. " +
            "The proof of the theory of natural selection or devolution defined?";
        }

        void IActivate.PayCost()
        {
            this.MugicCounters -= this.MugicCost;
        }

        void IActivateTarget<Creature>.Ability(Creature c)
        {
            c.Energy -= this.AbilityEnergy;
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetCreature; } }
    }
}
