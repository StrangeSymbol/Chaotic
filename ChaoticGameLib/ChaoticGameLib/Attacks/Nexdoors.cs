using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Nexdoors : Attack
    {
        public Nexdoors(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 0, 0, 0, 0, 0, 0, 0, 0, false, true, false, false) { }
        public override string Description()
        {
            return base.Description() + "Air: Look at the top two cards of your Attack Deck. " +
            "Put one of them on top of that deck and the other on the bottom";
        }
    }
}
