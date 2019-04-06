using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfTreachery : Mugic
    {
        public SongOfTreachery(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature loses 15 Courage, Power, Wisdom, and Speed until the end of the turn." +
                " One cannot move forward if one's always watching one's own back.";
        }
    }
}
