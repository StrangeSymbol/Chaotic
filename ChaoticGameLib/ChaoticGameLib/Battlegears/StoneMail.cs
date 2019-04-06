using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class StoneMail : Battlegear
    {
        public StoneMail(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 50) { }
        public override void Equip(Creature creature)
        {
            creature.Energy += this.DisciplineAmount;
            creature.GainedEnergy += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.RemoveGainedEnergy(this.DisciplineAmount);
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
