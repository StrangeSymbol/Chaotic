using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class TorrentOfFlame : Attack
    {
        public TorrentOfFlame(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 0, 10, 0, 0, 10, 2, 0, 0, true, false, false, true) { }
    }
}
