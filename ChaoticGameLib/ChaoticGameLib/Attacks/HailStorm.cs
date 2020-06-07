using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class HailStorm : Attack
    {
        public HailStorm(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 10, 0, 5, 0, 5, 3, 0, 0, false, true, false, true) { }
    }
}
