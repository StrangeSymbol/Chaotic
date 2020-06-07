using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class CordacFallsPlungepool : Location
    {
        public CordacFallsPlungepool(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.UnderWorld)
        {
        }

        public override string Description()
        {
            return "Cordac Falls Plungepool. Location. Initiative: Underworld. Card Ability: At the beginning of combat, deal 5 damage " +
                "to all engaged Creatures. " +
                "...and strike the lake miles below, exploring outwards into the fray.";
        }
    }
}