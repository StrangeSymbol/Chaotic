using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Degenervate : Attack
    {
        public Degenervate(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 0, 0, 25, false, false, false, true) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Water)
            {
                your.Water = your.WaterCombat = false;
                enemy.Courage -= this.DisciplineAmount;
                enemy.Power -= this.DisciplineAmount;
                enemy.Wisdom -= this.DisciplineAmount;
                enemy.Speed -= this.DisciplineAmount;

                enemy.CourageTurn += this.DisciplineAmount;
                enemy.PowerTurn += this.DisciplineAmount;
                enemy.WisdomTurn += this.DisciplineAmount;
                enemy.SpeedTurn += this.DisciplineAmount;
            }
        }

        public override string Description()
        {
            return base.Description() + "Water: Opposing Creature loses 25 Courage, Power, Wisdom, and Speed until the end of the" +
                " turn. Your engaged Creature loses Elemental Type Water until the end of the turn.";
        }
    }
}
