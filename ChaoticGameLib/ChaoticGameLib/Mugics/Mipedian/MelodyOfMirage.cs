using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class MelodyOfMirage : Mugic
    {
        public MelodyOfMirage(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target attack deals 0 damage." +
                " Trust your ears, not your eyes.";
        }
    }
}
