/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Chaotic
{
    class ChaoticMessageBox
    {
        Texture2D panel;
        Vector2 position;
        Button yesButton;
        Button noButton;
        SpriteFont font;

        string title;
        string description;
        bool active;
        bool? clickedYes;
        bool[] prevClicks;

        public ChaoticMessageBox(string title, string description, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.title = title;
            this.description = description;
            panel = content.Load<Texture2D>(@"Panel\Panel");
            this.position = new Vector2(graphics.PreferredBackBufferWidth / 2 - panel.Width / 2,
                graphics.PreferredBackBufferHeight / 2 - panel.Height / 2);
            Texture2D texture = content.Load<Texture2D>(@"Panel\YesButton");
            this.yesButton = new Button(texture, new Vector2(graphics.PreferredBackBufferWidth / 2 - 3 * texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"),
                content.Load<SoundEffect>(@"Audio\LeftMouseClick"));
            texture = content.Load<Texture2D>(@"Panel\NoButton");
            this.noButton = new Button(texture, new Vector2(graphics.PreferredBackBufferWidth / 2 + texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"),
                content.Load<SoundEffect>(@"Audio\LeftMouseClick"));
            font = content.Load<SpriteFont>(@"Fonts\MessageBox");
            prevClicks = new bool[3]{true, true, true};
            clickedYes = null;
        }

        public bool Active { get { return active; } set { active = value; } }
        public bool? ClickedYes
        {
            get { return clickedYes; }
            set
            {
                clickedYes = value; 
                if (clickedYes.HasValue)
                {
                    prevClicks[2] = prevClicks[1];
                    prevClicks[1] = prevClicks[0];
                    prevClicks[0] = clickedYes.Value;
                }
            }
        }
        public bool PrevClick { get { return prevClicks[1]; } }

        public void UpdateMessageBox(GameTime gameTime)
        {
            if (active)
            {
                MouseState mouse = Mouse.GetState();
                if (yesButton.UpdateButton(mouse, gameTime))
                {
                    active = false;
                    ClickedYes = true;
                }
                else if (noButton.UpdateButton(mouse, gameTime))
                {
                    active = false;
                    ClickedYes = false;
                }
            }
        }

        public void Restore()
        {
            prevClicks[0] = prevClicks[1];
            prevClicks[1] = prevClicks[2];
        }

        public void Reset()
        {
            prevClicks = new bool[3] { true, true, true };
        }

        public void DrawMessageBox(SpriteBatch spriteBatch, float layerDepth=0.2f)
        {
            if (active)
            {
                spriteBatch.Draw(panel, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                yesButton.Draw(spriteBatch);
                noButton.Draw(spriteBatch);
                spriteBatch.DrawString(font, title + (Burst.Player1Turn ? " Player1" : " Player2"), position, Color.Black, 0f, 
                    Vector2.Zero, 1f, SpriteEffects.None, layerDepth - 0.05f);
                spriteBatch.DrawString(font, description, new Vector2(position.X + panel.Width / 2 - font.MeasureString(description).X / 2,
                    position.Y + panel.Height / 2 - font.MeasureString(description).Y / 2), Color.Black, 
                    0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth - 0.05f);
            }
        }
    }
}
