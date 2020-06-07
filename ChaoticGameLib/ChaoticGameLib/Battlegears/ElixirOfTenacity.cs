using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class ElixirOfTenacity : Battlegear
    {
        public ElixirOfTenacity(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 10) { }

        public override void Equip(Creature creature)
        {
            creature.Courage += this.DisciplineAmount;
            creature.Speed += this.DisciplineAmount;
            if (creature.Power >= 50)
            {
                creature.GainedEnergy += this.DisciplineAmount;
                creature.Energy += this.DisciplineAmount;
            }
        }
        public override void UnEquip(Creature creature)
        {
            creature.Courage -= this.DisciplineAmount;
            creature.Speed -= this.DisciplineAmount;
            creature.RemoveGainedEnergy(this.DisciplineAmount);
        }

        public override string Description()
        {
            return "Elixir Of Tenacity. Battlegear. Equipped Creature gains 10 Courage and 10 Speed. " +
                "If equipped Creature has at least 50 Power, it gains 10 Energy.";
        }

    }
}
