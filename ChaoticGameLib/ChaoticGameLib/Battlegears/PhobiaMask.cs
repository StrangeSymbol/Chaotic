using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class PhobiaMask : Battlegear
    {
        public PhobiaMask(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite,overlay, negate, 10) { }

        public override void Equip(Creature creature)
        {
            creature.IntimidateCourage += this.DisciplineAmount;
            creature.IntimidatePower += this.DisciplineAmount;
        }

        public override void UnEquip(Creature creature)
        {
            creature.IntimidateCourage -= this.DisciplineAmount;
            creature.IntimidatePower -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Phobia Mask. Battlegear. Equipped Creature gains \"Intimidate: Courage 10\"" +
                " and \"Intimidate: Power 10\".";
        }
    }
}
