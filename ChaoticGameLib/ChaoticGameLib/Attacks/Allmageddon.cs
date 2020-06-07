using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Allmageddon : Attack
    {
        public Allmageddon(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 10, 10, 10, 10, 10, 5, 0, 0, true, true, true, true, true) { }

        public override string Description()
        {
            return base.Description() + "Unique.";
        }
    }
}
