﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfResistance : Mugic
    {
        public SongOfResistance(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 2) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " All damage dealt to this Creature by attacks is reduced by 5 until the end of the turn." +
                " Even the proudest Danian would nevery deny a chance to harden his shell.";
        }
    }
}