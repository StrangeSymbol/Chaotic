using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class RiverlandStar : Battlegear
    {
        public RiverlandStar(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 15) { }
        public override void Equip(Creature creature)
        {
            creature.Courage += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.Courage -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Riverland Star. Battlegear. Equipped Creature gains 15 Courage. " +
                "If equipped Creature is OverWorld, it gains \"When equipped Creature " +
                "deals Water damage, heal 5 Energy to that Creature.\"";
        }
    }
}
