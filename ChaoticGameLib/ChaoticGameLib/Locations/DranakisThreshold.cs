using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class DranakisThreshold : Location
    {
        public DranakisThreshold(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "Dranakis Threshold. Location. Initiative: Courage. Card Ability: No Mugic or abilities may be played." +
                " The few who have survived utter nary a word of what lies beyond.";
        }
    }
}