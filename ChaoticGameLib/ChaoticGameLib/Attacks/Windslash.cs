using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Windslash : Attack
    {
        public Windslash(Texture2D sprite, Texture2D overlay) 
            : base(sprite, overlay, 5, 0, 0, 0, 0, 1, 0, 0, false, false, false, false) { }

        public override string Description()
        {
            return base.Description() + "Target opponent turns all face-down Battlegear Cards they control face up.";
        }
    }
}
