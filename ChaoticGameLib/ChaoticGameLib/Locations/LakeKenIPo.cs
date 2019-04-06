using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class LakeKenIPo : Location
    {
        public LakeKenIPo(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Power)
        {
        }

        public override string Description()
        {
            return "Lake Ken-I-Po. Location. Initiative: Power. Card Ability: Mugic may not be targeted. " +
                "Above the placid waters, Najarin's castle towers. A dim light flickers...and a mystery is born.";
        }
    }
}