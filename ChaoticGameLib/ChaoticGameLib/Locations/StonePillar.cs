using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class StonePillar : Location
    {
        public StonePillar(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Stone Pillar. Location. Initiative: Wisdom. Card Ability: The first Mugic card cast by any engaged " +
                "UnderWorld Creature costs 1 less Mugic Ability. " +
                "Without the Pillars there would be no Over and Under, but simply one World.";
        }
    }
}