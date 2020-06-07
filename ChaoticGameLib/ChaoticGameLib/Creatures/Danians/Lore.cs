using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Lore : Creature, IActivateTarget<Creature>
    {
        public Lore(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 3, false, false, false, false, 0, false, 0, 0, false,
            false, true, 0, 0, 0, 0, 1, 10, Tribe.Danian, CreatureType.Muge)
        {
        }

        public override string Description()
        {
            return "Lore Creature - Danian Muge Courage: 30 Power: 35 Wisdom: 70 Speed: 30 Energy: 25 Mugic Ability: 3" +
                " Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter: Deal 10 damage to target Creature and heal 10 Energy to Lore. Unique " +
            "As a spirit leader of the Danians, many believe this shaman may one day lead his tribe to the Cothica.";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost && this.CheckHealable();
        }

        public override bool CheckAbilityTarget(Creature creature, bool sameOwner)
        {
            return creature.Energy > 0;
        }

        void IActivate.PayCost()
        {
            this.MugicCounters -= this.MugicCost;
        }

        void IActivateTarget<Creature>.Ability(Creature c)
        {
            c.Energy -= this.AbilityEnergy;
            Heal(this.AbilityEnergy);
        }
        AbilityType IActivate.Type { get { return AbilityType.TargetCreature; } }
    }
}
