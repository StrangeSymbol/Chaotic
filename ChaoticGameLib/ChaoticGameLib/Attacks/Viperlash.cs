using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Viperlash : Attack
    {
        public Viperlash(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 15, 0, 0, 0, 0, 3, 0, 0, false, false, false, false) { }
    }
}
