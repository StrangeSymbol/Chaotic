using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Mommark : Creature, IActivateTarget<Creature>
    {
        public Mommark(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 3,
            false, false, false, false, 0, false, 0, 0, false, false, false,
            0, 0, 0, 0, 1, 10, Tribe.OverWorld, CreatureType.Caretaker)
        {
        }
        public override string Description()
        {
            return "Mommark Creature - Overworld Caretaker Courage: 50 Power: 20 Wisdom: 45 Speed: 35 Energy: 30 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 10 Energy to target Creature. ";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost;
        }

        public override bool CheckAbilityTarget(Creature creature, bool sameOwner)
        {
            return creature.CheckHealable();
        }

        void IActivate.PayCost()
        {
            this.MugicCounters -= this.MugicCost;
        }

        void IActivateTarget<Creature>.Ability(Creature c)
        {
            c.Heal(this.AbilityEnergy);
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetCreature; } }
    }
}
