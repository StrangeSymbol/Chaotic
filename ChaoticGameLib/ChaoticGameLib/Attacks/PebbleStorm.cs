using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class PebbleStorm : Attack
    {
        public PebbleStorm(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 5, 5, 0, 1, 0, 0, false, true, true, false) { }
    }
}
