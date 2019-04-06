using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class SqueezePlay : Attack
    {
        public SqueezePlay(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 5, 0, 0, 0, 0, 1, 0, 0, false, false, false, false) { }
        public override string Description()
        {
            return base.Description() + "Look at the top two cards of your Attack Deck. " +
            "Put one of them on top of that deck and the other on the bottom";
        }
    }
}
