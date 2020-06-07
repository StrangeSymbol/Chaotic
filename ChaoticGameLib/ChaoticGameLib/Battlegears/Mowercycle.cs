using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Mowercycle : Battlegear
    {
        public Mowercycle(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 25) { }
        public override void Equip(Creature creature)
        {
            creature.Speed += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.Speed -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Mowercycle. Battlegear. Equipped Creature gains 25 Speed.";
        }
    }
}
