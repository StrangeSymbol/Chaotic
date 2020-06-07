using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class InterludeOfConsequence : Mugic, ICast
    {
        public InterludeOfConsequence(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Generic, 1) { }

        AbilityType ICast.Type { get { return AbilityType.ShuffleTargetDeck; } }
        
        public override string Description()
        {
            return base.Description() + " Shuffle target Attack or Location Deck." +
                " In desparate situations, turmoil and confusion can be one's allies.";
        }
    }
}
