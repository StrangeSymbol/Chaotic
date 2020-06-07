using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Takinom : Creature, IActivateSelf
    {
        public Takinom(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, false, 1,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 0, 25, Tribe.UnderWorld, CreatureType.Commander)
        {
        }

        public override string Description()
        {
            return "Takinom Creature - UnderWorld Commander Courage: 60 Power: 65 Wisdom: 20 Speed: 95 Energy: 40 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Sacrifice an UnderWorld Creature: Heal 25 Energy to Takinom. Swift 1 " +
            "Takinom unearthed a shocking secret bond with her archenemy Intress that she's wisely kept from Chaor...";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.CheckHealable();
        }

        public override bool CheckAbilityTarget(Creature creature, bool sameOwner)
        {
            return sameOwner && this != creature && creature.CreatureTribe == Tribe.UnderWorld && creature.Energy > 0;
        }

        void IActivate.PayCost()
        {
            // No Cost paid by activator.
        }

        void IActivateSelf.Ability()
        {
            Heal(this.AbilityEnergy);
        }

        AbilityType IActivate.Type { get { return AbilityType.SacrificeTargetCreature; } }
    }
}
