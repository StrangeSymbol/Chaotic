using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class HymnOfTheElements : Mugic
    {
        public HymnOfTheElements(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature gains the Elemental Type of your choice until the end of the turn. " +
                "All elements stem from nature - as does the Cothica itself.";
        }
    }
}
