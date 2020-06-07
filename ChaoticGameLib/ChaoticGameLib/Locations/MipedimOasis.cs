using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class MipedimOasis : Location
    {
        public MipedimOasis(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "Mipedim Oasis. Location. Initiative: Courage. Card Ability: Mipedian Creatures deal an additional " +
                "10 damage on their first attack. " +
                "For those struggling to survive the Mipedian Desert, this is a welcome sight. But it is what they can't see " +
                "that will prove most dangerous.";
        }
    }
}