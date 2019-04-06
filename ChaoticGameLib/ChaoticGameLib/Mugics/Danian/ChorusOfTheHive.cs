using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class ChorusOfTheHive : Mugic
    {
        public ChorusOfTheHive(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Activate Hive until the end of the turn." +
                " What repels some attracts others. Is preference learned or ingrained?";
        }
    }
}