using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class GloomuckSwamp : Location
    {
        public GloomuckSwamp(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "GloomuckSwamp. Location. Initiative: Courage. Card Ability: Earth attacks deal an additional 5 damage. " +
                "Damage dealt by Fire attacks is reduced by 5. " +
                "For vary different reasons both Crawsectus and Antidaeon choose to call this mucky mess home.";
        }
    }
}