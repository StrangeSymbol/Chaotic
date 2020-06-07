using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class ToxicGust : Attack
    {
        public ToxicGust(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 10, 5, 5, 0, 0, 3, 0, 0, true, true, false, false) { }
    }
}
