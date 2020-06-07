using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfEmbernova : Mugic, ICastTarget<Creature>
    {
        public SongOfEmbernova(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Generic, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.Air || creature.Fire;
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            if (creature.Fire || creature.Air)
            {
                creature.Energy -= 10;
            }
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Deal 10 damage to target Creature with Elemental Type Air or Fire." +
                " Fire consumes air, suffocating the earth.";
        }
    }
}