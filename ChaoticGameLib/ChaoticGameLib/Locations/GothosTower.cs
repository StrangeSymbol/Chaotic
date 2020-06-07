using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class GothosTower : Location
    {
        public GothosTower(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Gothos Tower. Location. Initiative Speed. Card Ability: All Creatures not named Lord Van Bloot lose " +
                "10 Courage. Lord Van Bloot gains \"Invisibility: Strike 15\"." +
                "The horrific home of the harrowing Lord Van Bloot.";
        }
    }
}