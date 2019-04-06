using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfMandiblor : Mugic
    {
        public SongOfMandiblor(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature gains 5 Courage, Power, Wisdom, and Speed for each Danian " +
                " in play until the end of the turn. " +
                "Listen and learn the true strength of the Danian tribe -- when you face one, you contend with all.";
        }
    }
}