using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class MugiciansLyre : Battlegear
    {
        public MugiciansLyre(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 1) { }
        public override void UnEquip(Creature creature)
        {
            creature.MugicCounters += this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Mugician's Lyre. Battlegear. Sacrifice Mugician's Lyre: Add 1 Mugic Counter to this Creature." +
                "Sacrifice Aqua Shield: Heal 15 to target Creature.";
        }
    }
}
