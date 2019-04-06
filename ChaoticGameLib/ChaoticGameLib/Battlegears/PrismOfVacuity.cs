using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class PrismOfVacuity : Battlegear
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
            // Sacrifice Until end of turn target creature loses 20 power.
        }

        public override string Description()
        {
            return "Prism Of Vacuity. Battlegear. Equipped Creature gains 5 Energy. " +
                "Sacrifice Prism Of Vacuity: Until the end of the turn, target Creature loses 20 Power.";
        }
    }
}
