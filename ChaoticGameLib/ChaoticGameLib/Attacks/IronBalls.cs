using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class IronBalls : Attack
    {
        public IronBalls(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 1, 0, 0, false, false, false, false) { }

        public override string Description()
        {
            return base.Description() + "Until the end of the turn, only Generic Mugic may be cast.";
        }
    }
}
