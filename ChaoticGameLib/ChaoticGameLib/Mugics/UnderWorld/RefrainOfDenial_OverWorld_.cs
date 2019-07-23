using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class RefrainOfDenial_OverWorld_ : Mugic
    {
        public RefrainOfDenial_OverWorld_(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }

        public override string Description()
        {
            return base.Description() + " Dispel target Generic or OverWorld Mugic. " +
                "\"No such thing as Mugicians! Now fight!\" -- Dardemus.";
        }
    }
}
