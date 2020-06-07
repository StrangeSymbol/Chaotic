using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ulmar : Creature, IActivateTarget<Creature>
    {
        public Ulmar(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2, true, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 10, Tribe.UnderWorld, CreatureType.Conqueror)
        {
        }

        public override string Description()
        {
            return "Ulmar Creature - UnderWorld Conqueror Courage: 40 Power: 20 Wisdom: 70 Speed: 35 Energy: 25 Mugic Ability: 2" +
                " Elemental Type: Fire Creature Ability: " +
                "Pay 1 Mugic Ability: Deal 10 damage to target Creature. " +
            "Takinom thinks that Chaor's \"go-to\" guy for BattleGear has a screw loose himself.";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost;
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
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetCreature; } }
    }
}
