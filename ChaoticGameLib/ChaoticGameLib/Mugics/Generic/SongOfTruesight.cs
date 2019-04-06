using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoticGameLib.Creatures;

namespace ChaoticGameLib.Mugics
{
    public class SongOfTruesight : Mugic
    {
        public SongOfTruesight(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        public override void Ability(Creature creature)
        {
            if (creature.CreatureTribe == Tribe.Mipedian)
            {
                creature.UsedAbility = true;
                creature.Surprise = false;
            }
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature loses all Invisability abilities until the end of the turn." +
                " A melody may help you see things more clearly, so how can sound be invisible?";
        }
    }
}