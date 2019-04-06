using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfSymmetry : Mugic
    {
        public SongOfSymmetry(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target engaged Creature gains 10 Energy. Deal 10 damage to another target engaged Creature." +
                " Your strength is now mine, my weakness is now yours!";
        }
    }
}