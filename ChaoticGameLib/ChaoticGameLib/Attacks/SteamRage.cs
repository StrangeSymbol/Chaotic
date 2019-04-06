using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class SteamRage : Attack
    {
        public SteamRage(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 5, 0, 0, 5, 1, 0, 0, true, false, false, true) { }
    }
}
