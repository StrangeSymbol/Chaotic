using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class CoilCrush : Attack
    {
        public CoilCrush(Texture2D sprite,Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 5, 0, 0, 0, 0, 3, 0, 75, false, false, false, false) { }

        public override string Description()
        {
            return base.Description() + "Stat Check Power 75: Destroy target Battlegear equipped to opposing Creature.";
        }
    }
}
