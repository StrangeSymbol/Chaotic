using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class PrismOfVacuity : Battlegear, ISacrificeTarget<Creature>
    {
        public PrismOfVacuity(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 20) { }
        public override void Equip(Creature creature)
        {
            creature.Energy += 5;
            creature.GainedEnergy += 5;
        }
        public override void UnEquip(Creature creature)
        {
            creature.RemoveGainedEnergy(5);
        }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp;
        }

        public override bool CheckSacrificeTarget(Creature target)
        {
            return target.Power > 0;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Power -= this.DisciplineAmount;
            c.PowerTurn += this.DisciplineAmount;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return "Prism Of Vacuity. Battlegear. Equipped Creature gains 5 Energy. " +
                "Sacrifice Prism Of Vacuity: Until the end of the turn, target Creature loses 20 Power.";
        }
    }
}
