using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfResurgence : Mugic
    {
        public SongOfResurgence(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }
        public override void Ability(Creature creature)
        {
            creature.Heal(20);
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Heal 20 Energy to target Creature. " +
                "\"Our understanding of Mugic is the direct inverse of our gratitude for its existence.\" -- Najarin";
        }
    }
}
