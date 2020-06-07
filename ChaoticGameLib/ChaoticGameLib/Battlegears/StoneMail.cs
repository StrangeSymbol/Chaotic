/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
 using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class StoneMail : Battlegear
    {
        public StoneMail(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 50) { }
        public override void Equip(Creature creature)
        {
            creature.Energy += this.DisciplineAmount;
            creature.GainedEnergy += this.DisciplineAmount;
            creature.CannotMove = true;
            creature.Negate = true;
        }
        public override void UnEquip(Creature creature)
        {
            creature.RemoveGainedEnergy(this.DisciplineAmount);
            creature.CannotMove = false;
            creature.Negate = false;
        }

        public override string Description()
        {
            return "Stone Mail. Battlegear. Equipped Creature may not move. " +
                "Equipped Creature gains 50 Energy. " +
                "All damage dealt to equipped Creature is increased by 5 " +
                "Equipped Creature loses all abilities.";
        }
    }
}
