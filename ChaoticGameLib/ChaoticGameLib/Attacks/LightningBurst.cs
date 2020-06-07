using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class LightningBurst : Attack
    {
        public LightningBurst(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 5, 0, 5, 0, 0, 1, 0, 25, false, true, false, false) { }
        public override void Damage(Creature your, Creature enemy, Location location)
        {
            base.Damage(your, enemy, location);
            if (your.Air)
            {
                enemy.Power -= this.DisciplineAmount;
                enemy.PowerTurn += this.DisciplineAmount;
            }
        }
        public override string Description()
        {
            return base.Description() + "Air: Opposing Creature loses 25 Power until the end of the turn.";
        }
    }
}
