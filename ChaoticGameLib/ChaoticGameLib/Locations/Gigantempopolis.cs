using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class Gigantempopolis : Location
    {
        public Gigantempopolis(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Power)
        {
        }

        public override string Description()
        {
            return "Gigantempopolis. Location. Initiative: Power. Card Ability: At the beginning of combat, each. " +
                "engaged OverWorld Creature gains 1 mugic counter. " +
                "The ancient beings who once dwelled here faded into oblivion - proving indisputably that size doesn't matter.";
        }
    }
}