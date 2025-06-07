using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Catacollision : Attack
    {
        public Catacollision(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 5, 5, 5, 5, 5, 3, 0, 0, true, true, true, true, false) { }

        public override string Description()
        {
            return base.Description();
        }
    }
}
