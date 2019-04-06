using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class CoilCrush : Attack
    {
        public CoilCrush(Texture2D sprite,Texture2D overlay)
            : base(sprite, overlay, 5, 0, 0, 0, 0, 3, 0, 75, false, false, false, false) { }

        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Power >= this.DisciplineAmount && enemy.Battlegear != null)
                enemy.UnEquip();
        }

        public override string Description()
        {
            return base.Description() + "Stat Check Power 75: Destroy target Battlegear equipped to opposing Creature.";
        }
    }
}
