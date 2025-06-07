using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Agitos : Creature, IActivateTarget<Creature>
    {
        public Agitos(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 5, Tribe.UnderWorld, CreatureType.Commander)
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
            return "Agitos Creature - UnderWorld Commander Courage: 65 Power: 40 Wisdom: 85 Speed: 35 Energy: 55 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Counter: Deal 5 damage to target Creature.";
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
