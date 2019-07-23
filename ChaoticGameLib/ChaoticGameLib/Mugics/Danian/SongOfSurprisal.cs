using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfSurprisal : Mugic, ICastTarget<Creature>
    {
        public SongOfSurprisal(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }

        void ICastTarget<Creature>.Ability(Creature card)
        {
            card.Swift += 1;
            card.Range = true;
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.TargetCreature; }
        }

        public override string Description()
        {
            return base.Description() + " Target Creature gains \"Range\" and \"Swift 1\" until the end of the turn. " +
                "The usefulness of Mugic in battle is but a side effect. Its true purpose remains unknown.";
        }
    }
}