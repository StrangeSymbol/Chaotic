using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class MugicReprise : Mugic, ICast
    {
        public MugicReprise(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 2) { }

        AbilityType ICast.Type { get { return AbilityType.ReturnMugic; } }

        public override string Description()
        {
            return base.Description() + " Return target Mugic Card from your discard pile to your hand. " +
                "Everything that was once lost will be found again.";
        }
    }
}
