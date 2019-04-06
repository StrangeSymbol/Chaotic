using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfEmbernova : Mugic
    {
        public SongOfEmbernova(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }

        public override void Ability(Creature creature)
        {
            if (creature.Air || creature.Fire)
                creature.Energy -= 10;
            base.Ability(creature);
        }

        public override string Description()
        {
            return base.Description() + " Deal 10 damage to target Creature with Elemental Type Air or Fire." +
                " Fire consumes air, suffocating the earth.";
        }
    }
}