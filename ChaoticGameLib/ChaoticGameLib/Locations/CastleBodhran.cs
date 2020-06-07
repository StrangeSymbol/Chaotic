using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class CastleBodhran : Location
    {
        public CastleBodhran(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Power)
        {
        }

        public override string Description()
        {
            return "Castle Bodhran. Location. Initiative: Power. Card Ability: At the beginning of combat, each player may return a " +
                "Mugic Card from their discard pile to their hand. " +
                "This strange stronghold is built in a style that might be best be called 'bizzarechitecture";
        }
    }
}