using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Mirthquake : Attack
    {
        public Mirthquake(Texture2D sprite, Texture2D overlay, Texture2D negate)
            : base(sprite, overlay, negate, 0, 0, 0, 0, 0, 0, 0, 0, false, false, false, false) { }

        public override string Description()
        {
            return base.Description() + "Earth: Turn the top card of your Location Deck face up. This becomes the active Location." +
                " Your engaged Creature loses Elemental Type Earth until the end of the turn.";
        }
    }
}
