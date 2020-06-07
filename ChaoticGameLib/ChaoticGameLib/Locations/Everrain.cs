using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class Everrain : Location
    {
        public Everrain(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "Everrain. Location. Initiative: Courage. Card Ability: Water attacks deal an additional 5 damage. " +
                "Damage dealt by Earth attacks is reduced by 5. " +
                "Lake Ken-I-Po seeps through the Underworld ceiling in an unending reminder of the Overworld's is aqueous bounty.";
        }
    }
}