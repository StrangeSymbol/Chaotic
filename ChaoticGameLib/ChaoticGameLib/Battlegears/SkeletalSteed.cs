using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class SkeletalSteed : Battlegear
    {
        bool hadRange;
        public SkeletalSteed(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 1, true) { }
        public override void Equip(Creature creature)
        {
            if (!creature.Range)
                creature.Range = true;
            else
                hadRange = true;
            creature.Swift += this.DisciplineAmount;
            creature.SwiftGained += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            if (!hadRange)
                creature.Range = false;
            creature.Swift -= this.DisciplineAmount;
            creature.SwiftGained -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Skeletal Steed. Battlegear. Equipped Creature gains \"Range\" and \"Swift 1\". " +
                "Reveal this Battlegear at beginning of game.";
        }
    }
}
