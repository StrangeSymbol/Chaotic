using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfRecovery : Mugic
    {
        public SongOfRecovery(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Heal 10 damage to target Creature. That Creature gains \"Air 5\" until the end of the turn." +
                " Tianne believes that each Mugic is but a small piece of a great opus.";
        }
    }
}