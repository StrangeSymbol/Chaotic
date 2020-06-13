/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
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
        const int kSpaceWidth = 3;
        const int kSpaceHeight = 6;
        const int kBorderWidth = 5;
        const int kPanelWidth = ChaoticEngine.kCardCoveredHeight;
        readonly float kboxItemHeight;
        readonly int numBoxes;

        int start;
        int end;

        public BurstBox(ContentManager content, GraphicsDeviceManager graphics)
        {
            box = content.Load<Texture2D>(@"BurstSprites/Box");
            font = content.Load<SpriteFont>(@"Fonts/BurstBoxFont");
            kboxItemHeight = font.MeasureString("dfgjl").Y + 2*kBorderWidth;
            boxPosition = new Vector2(kSpaceWidth, 3 * graphics.PreferredBackBufferHeight / 4+kSpaceHeight);
            int maxHeight = graphics.PreferredBackBufferHeight / 4 - 2 * kSpaceHeight;
            numBoxes = (int)(maxHeight / kboxItemHeight);
            Texture2D upSprite = content.Load<Texture2D>(@"BurstSprites/ArrowButtonU");
            Vector2 upPosition = new Vector2(boxPosition.X + kPanelWidth + kSpaceWidth,
                boxPosition.Y + kboxItemHeight*numBoxes / 2 - upSprite.Height / 2);
            upButton = new Button(upSprite, upPosition,
                content.Load<Texture2D>(@"BurstSprites/ArrowButtonCoverU"));
            downButton = new Button(content.Load<Texture2D>(@"BurstSprites/ArrowButtonD"), upPosition + 
                new Vector2(0, upSprite.Height + 2*kSpaceHeight),
                content.Load<Texture2D>(@"BurstSprites/ArrowButtonCoverD"));
            start = 0;
            end = numBoxes-1;
        }

        public void UpdateBox(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            string[] burstInfo = Burst.BurstBoxInfo();
            start = (burstInfo.Length > numBoxes ? start : 0);
            end = (burstInfo.Length > numBoxes ? end : numBoxes-1);

            if (upButton.UpdateButton(mouse, gameTime) && start != 0)
            {
                start--;
                end--;
            }
            else if (downButton.UpdateButton(mouse, gameTime) && end < burstInfo.Length - 1)
            {
                start++;
                end++;
            }
        }

        public void DrawBox(SpriteBatch spriteBatch)
        {
            string[] burstInfo = Burst.BurstBoxInfo();
            Vector2 currentPos = boxPosition;

            for (int i = start; i < end+1; i++)
            {
                spriteBatch.Draw(box, new Rectangle((int)currentPos.X, (int)currentPos.Y, kPanelWidth, (int)kboxItemHeight),
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.99f);
                if (i <= burstInfo.Length-1)
                    spriteBatch.DrawString(font, burstInfo[i], currentPos + new Vector2(kBorderWidth, kBorderWidth),
                        Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                currentPos += new Vector2(0, kboxItemHeight);
            }
            upButton.Draw(spriteBatch);
            downButton.Draw(spriteBatch);
        }
    }
}
