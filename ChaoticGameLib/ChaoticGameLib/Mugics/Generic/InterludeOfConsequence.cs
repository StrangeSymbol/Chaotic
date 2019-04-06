using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class InterludeOfConsequence : Mugic
    {
        public InterludeOfConsequence(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        
        public override string Description()
        {
            return base.Description() + " Shuffle target Attack or Location Deck." +
                " In desparate situations, turmoil abd confusion can be one's allies.";
        }
    }
}
