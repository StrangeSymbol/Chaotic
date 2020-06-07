using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class RingOfNaarin : Battlegear
    {
        public RingOfNaarin(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 10) { }
        public override void Equip(Creature creature)
        {
            creature.Power += this.DisciplineAmount;
            creature.Wisdom += this.DisciplineAmount;
            if (creature.Courage >= 50)
            {
                creature.GainedEnergy += this.DisciplineAmount;
                creature.Energy += this.DisciplineAmount;
            }
        }
        public override void UnEquip(Creature creature)
        {
            creature.Power -= this.DisciplineAmount;
            creature.Wisdom -= this.DisciplineAmount;
            creature.RemoveGainedEnergy(this.DisciplineAmount);
        }

        public override string Description()
        {
            return "Ring Of Naarin. Battlegear. Equipped Creature gains 10 Power and 10 Wisdom. " +
                "If equipped Creature has at least 50 Courage, it gains 10 Energy.";
        }
    }
}
