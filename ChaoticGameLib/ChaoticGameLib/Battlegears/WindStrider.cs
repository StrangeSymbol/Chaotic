using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class WindStrider : Battlegear
    {
        public WindStrider(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 2, true) { }
        public override void Equip(Creature creature)
        {
            creature.Swift += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.Swift -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Wind Strider. Battlegear. Equipped Creature gains \"Swift 2\". " +
                "Reveal this Battlegear at beginning of game.";
        }
    }
}
