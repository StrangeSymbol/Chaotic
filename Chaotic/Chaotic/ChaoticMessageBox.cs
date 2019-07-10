using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
        bool active = true;
        bool clickedYes;

        public ChaoticMessageBox(string title, string description, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.title = title;
            this.description = description;
            panel = content.Load<Texture2D>(@"Panel\Panel");
            this.position = new Vector2(graphics.PreferredBackBufferWidth / 2 - panel.Width / 2,
                graphics.PreferredBackBufferHeight / 2 - panel.Height / 2);
            Texture2D texture = content.Load<Texture2D>(@"Panel\YesButton");
            this.yesButton = new Button(texture, new Vector2(graphics.PreferredBackBufferWidth / 2 - 3 * texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"));
            texture = content.Load<Texture2D>(@"Panel\NoButton");
            this.noButton = new Button(texture, new Vector2(graphics.PreferredBackBufferWidth / 2 + texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"));
            font = content.Load<SpriteFont>(@"Fonts\MessageBox");
        }

        public bool Active { get { return active; } set { active = value; } }
        public bool ClickedYes { get { return clickedYes; } }

        public void UpdateMessageBox(GameTime gameTime)
        {
            if (active)
            {
                MouseState mouse = Mouse.GetState();
                if (yesButton.UpdateButton(mouse, gameTime))
                {
                    active = false;
                    clickedYes = true;
                }
                else if (noButton.UpdateButton(mouse, gameTime))
                {
                    active = false;
                    clickedYes = false;
                }
            }
        }

        public void DrawMessageBox(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(panel, position, Color.White);
                yesButton.Draw(spriteBatch);
                noButton.Draw(spriteBatch);
                spriteBatch.DrawString(font, title, position, Color.Black);
                spriteBatch.DrawString(font, description, new Vector2(position.X + panel.Width / 2 - font.MeasureString(description).X / 2,
                    position.Y + panel.Height / 2 - font.MeasureString(description).Y / 2), Color.Black);
                spriteBatch.End();
            }
        }
    }
}
