﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class NotesOfNeverwhere : Mugic
    {
        public NotesOfNeverwhere(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Location loses all abilities until the end of the turn." +
                " \"If you have all that you need, you are always home.\" -- From an ancient Mugician text";
        }
    }
}