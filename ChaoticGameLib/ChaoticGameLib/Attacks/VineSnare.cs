using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class VineSnare : Attack
    {
        public VineSnare(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 5, 0, 0, 5, 0, 1, 0, 25, false, false, true, false) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Earth)
            {
                enemy.Speed -= this.DisciplineAmount;
                enemy.SpeedTurn += this.DisciplineAmount;
            }
        }
        public override string Description()
        {
            return base.Description() + "Earth: Opposing Creature loses 25 Speed until the end of the turn.";
        }
    }
}
