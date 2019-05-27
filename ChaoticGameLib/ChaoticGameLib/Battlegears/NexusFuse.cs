using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class NexusFuse : Battlegear
    {
        public NexusFuse(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 5) { }
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
            return "Nexus Fuse. Battlegear. Equipped Creature gains 5 Energy. " +
                "Sacrifice Nexus Fuse: Deal 15 damage to target Creature.";
        }
    }
}
