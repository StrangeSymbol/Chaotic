using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoticGameLib.Creatures;

namespace ChaoticGameLib.Mugics
{
    public class SongOfTruesight : Mugic, ICastTarget<Creature>
    {
        public SongOfTruesight(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Generic, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.Invisibility();
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Strike = 0;
            creature.Surprise = false;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Target Creature loses all Invisability abilities until the end of the turn." +
                " A melody may help you see things more clearly, so how can sound be invisible?";
        }
    }
}