﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class EmberSwarm : Attack
    {
        public EmberSwarm(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 5, 5, 0, 0, 0, 1, 0, 25, true, false, false, false) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            if (your.Fire)
            {
                enemy.Wisdom -= this.DisciplineAmount;
                enemy.WisdomTurn += this.DisciplineAmount;
            }
        }

        public override string Description()
        {
            return base.Description() + "Fire: Opposing Creature loses 25 Wisdom until the end of the turn.";
        }
    }
}