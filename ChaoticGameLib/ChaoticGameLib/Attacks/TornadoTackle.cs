using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class TornadoTackle : Attack
    {
        public TornadoTackle(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 0, 0, 0, false, true, false, false) { }
        public override string Description()
        {
            return base.Description() + "Air: Both players reshuffle their Attack Deck.";
        }
    }
}
