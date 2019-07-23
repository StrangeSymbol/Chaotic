using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class RefrainOfDenial : Mugic
    {
        public RefrainOfDenial(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 2) { }

        public override string Description()
        {
            return base.Description() + " Dispel target Mugic. Cacophony signaled the beginning of the end of peace for the Mugicians.";
        }
    }
}
