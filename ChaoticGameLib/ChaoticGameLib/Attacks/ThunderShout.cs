using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class ThunderShout : Attack
    {
        public ThunderShout(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 10, 10, 0, 2, 0, 0, false, true, true, false) { }
    }
}
