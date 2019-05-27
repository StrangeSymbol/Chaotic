using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class VlaricShard : Battlegear
    {
        public VlaricShard(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 5) { }
        public override void Equip(Creature creature)
        {
            creature.EarthDamage += this.DisciplineAmount;
            creature.EarthDamageGained += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.EarthDamage -= (byte)(creature.EarthDamage == 0 ? 0 : this.DisciplineAmount);
            creature.EarthDamageGained -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Vlaric Shard. Battlegear. Equipped Creature gains \"Earth 5 " +
                "[Earth attacks made by this Creature deal an additional 5 damage.]\"";
        }
    }
}
