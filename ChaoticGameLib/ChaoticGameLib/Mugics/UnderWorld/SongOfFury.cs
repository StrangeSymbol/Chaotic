using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfFury : Mugic
    {
        public SongOfFury(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Deal 10 damage to target Creature. " +
                "Until the end of the turn, another target Creature gains \"Fire 5\". " +
                "\"My legs or my Mugician? There is no choice. Cut them off!\" -- Miklon";
        }
    }
}
