﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Lavalanche : Attack
    {
        public Lavalanche(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 10, 5, 0, 5, 0, 3, 0, 0, true, false, true, false) { }
    }
}