using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class DoorsOfTheDeepmines : Location
    {
        public DoorsOfTheDeepmines(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Doors Of The Deepmines. Location. Initiative: Speed. Card Ability: At the beginning of combat, all Creatures " +
                "with Elemental Type Water gain 10 Energy. " +
                "In a rare sign of solidarity, all four tribes guard the massive doors, hoping may never open again.";
        }
    }
}