/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chaotic
{
    class MenuButton : Button
    {
        MenuStage stage;

        public MenuButton(Texture2D sprite, Vector2 position, Texture2D overlay, MenuStage stage)
            : base(sprite, position, overlay)
        {
            this.stage = stage;
        }

        public MenuStage Stage { get { return stage; } }
    }
}
