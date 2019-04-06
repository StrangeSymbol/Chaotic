using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class TalismanOfTheMandiblor : Battlegear
    {
        public TalismanOfTheMandiblor(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 15) { }
        public override void Equip(Creature creature)
        {
            creature.Speed += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.Speed -= this.DisciplineAmount;
            // Gains if Danian sacrifice creature return mandiblor to any open spot on board.
        }

        public override string Description()
        {
            return "Talisman Of The Mandiblor. Battlegear. Equipped Creature gains 15 Speed. " +
                "If equipped Creature is Danian, it gains \"Sacrifice equipped Creature: " +
                "Return a Mandiblor to play in any open space.\"";
        }
    }
}
