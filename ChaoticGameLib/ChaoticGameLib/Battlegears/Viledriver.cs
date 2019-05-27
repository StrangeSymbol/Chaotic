using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Viledriver : Battlegear
    {
        public Viledriver(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 5) { }
        public override void Equip(Creature creature)
        {
            creature.FireDamage += this.DisciplineAmount;
            creature.FireDamageGained += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.FireDamage -= (byte)(creature.FireDamage == 0 ? 0 : this.DisciplineAmount);
            creature.FireDamageGained -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Viledriver. Battlegear. Equipped Creature gains \"Fire 5 " +
                "[Fire attacks made by this Creature deal an additional 5 damage.]\"";
        }
    }
}
