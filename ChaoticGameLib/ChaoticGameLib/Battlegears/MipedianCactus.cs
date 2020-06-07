using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class MipedianCactus : Battlegear, IActivateBattlegear
    {
        public MipedianCactus(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 15) { }
        public override void Equip(Creature creature)
        {
            creature.Wisdom += this.DisciplineAmount;
        }

        public override void UnEquip(Creature creature)
        {
            creature.Wisdom -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Mipedian Cactus. Battlegear. Equipped Creature gains 15 Wisdom. " +
                "If equipped Creature is Mipedian, it gains \"Cost one Mugic Counter: " +
                "Until the end of the turn, this Creature can move to any space on the board.\"";
        }

        void IActivateBattlegear.PayCost(Creature creature)
        {
            creature.MugicCounters -= 1;
        }

        void IActivateBattlegear.Ability(Creature creature)
        {
            creature.CanMoveAnywhere = true;
        }

        AbilityType IActivate.Type
        {
            get { return AbilityType.TargetEquippedCreature; }
        }

        void IActivate.PayCost()
        {
            throw new NotImplementedException();
        }
    }
}
