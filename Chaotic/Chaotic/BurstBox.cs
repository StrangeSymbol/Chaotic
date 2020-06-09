/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chaotic
{
    class BurstBox
    {
        Texture2D box;
        Vector2 boxPosition;
        Button upButton;
        Button downButton;
        SpriteFont font;
        const int kBorderWidth = 5;
        const int kPanelWidth = ChaoticEngine.kCardCoveredHeight;
        readonly int kHeight;

        public BurstBox(ContentManager content, GraphicsDeviceManager graphics)
        {
            box = content.Load<Texture2D>(@"BurstSprites/Box");
            upButton = new Button(content.Load<Texture2D>(@"BurstSprites/ArrowButtonU"), new Vector2(),
                content.Load<Texture2D>(@"BurstSprites/ArrowButtonCoverU"));
            downButton = new Button(content.Load<Texture2D>(@"BurstSprites/ArrowButtonD"), new Vector2(),
                content.Load<Texture2D>(@"BurstSprites/ArrowButtonCoverD"));
        }
    }
}
