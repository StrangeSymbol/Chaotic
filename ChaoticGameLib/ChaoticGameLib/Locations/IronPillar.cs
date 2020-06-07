/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class IronPillar : Location
    {
        public IronPillar(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "Iron Pillar. Location. Initiative: Courage. Card Ability: Battlegear loses all abilities. " +
                "The frequent quakes that rock the UnderWorld are blamed on the instability of this once mighty prop.";
        }
    }
}