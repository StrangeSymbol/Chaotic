using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class SludgeGush : Attack
    {
        public SludgeGush(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 10, 0, 0, 5, 5, 3, 0, 0, false, false, true, true) { }
    }
}
