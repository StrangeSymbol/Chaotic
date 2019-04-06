using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfRevival_UnderWorld_ : Mugic
    {
        public SongOfRevival_UnderWorld_(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 2) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Return target UnderWorld Creature Card in your discard pile " +
                "to play in any open space. That Creature comes into play without any Mugic counters. " +
                "The Mugicians believed that anything that once was cannot be lost forever.";
        }
    }
}
