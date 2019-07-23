using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfRevival_UnderWorld_ : Mugic, ICastReturn
    {
        public SongOfRevival_UnderWorld_(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 2) { }

        bool ICastReturn.CheckReturnable(Creature c)
        {
            return c.CreatureTribe == Tribe.UnderWorld;
        }

        AbilityType ICast.Type { get { return AbilityType.ReturnCreature; } }

        public override string Description()
        {
            return base.Description() + " Return target UnderWorld Creature Card in your discard pile " +
                "to play in any open space. That Creature comes into play without any Mugic counters. " +
                "The Mugicians believed that anything that once was cannot be lost forever.";
        }
    }
}
