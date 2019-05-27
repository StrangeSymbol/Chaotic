using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class RipTide : Attack
    {
        public RipTide(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 5, 0, 0, 0, 5, 1, 0, 25, false, false, false, true) { }
        public override void Damage(Creature your, Creature enemy, Location location)
        {
            base.Damage(your, enemy, location);
            if (your.Water)
            {
                enemy.Courage -= this.DisciplineAmount;
                enemy.CourageTurn += this.DisciplineAmount;
            }
        }
        public override string Description()
        {
            return base.Description() + "Water: Opposing Creature loses 25 Courage until the end of the turn.";
        }
    }
}
