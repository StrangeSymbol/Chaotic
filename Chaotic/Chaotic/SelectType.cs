using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Chaotic
{
    class SelectType
    {
        const int cTextureWidth = 125;
        const int cTextureHeight = 125;
        const int cSpace = 20;
        const int cSpaceWidth = 21;
        int selectNumber;
        List<int> selectedIndices;
        Texture2D panel;
        Vector2 position;
        Button okButton;
        Vector2[] positions;
        SpriteFont font;
        string[] selectedText;
        List<Texture2D> pile;

        public SelectType(ContentManager content, GraphicsDeviceManager graphics, params string[] selectedText)
        {
            this.panel = content.Load<Texture2D>(@"Panel\Panel");
            this.position = new Vector2(2 * graphics.PreferredBackBufferWidth / 4 - panel.Width / 2,
                2 * graphics.PreferredBackBufferHeight / 4 - panel.Height / 2);
            this.selectedText = selectedText;
            Texture2D texture = content.Load<Texture2D>(@"Panel\OkButton");
            this.okButton = new Button(texture, new Vector2(2 * graphics.PreferredBackBufferWidth / 4 - texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"));
            positions = new Vector2[4];
            for (int i = 0; i < positions.Length; i++)
            {
                if (i == 0)
                    positions[i] = new Vector2(this.position.X + cSpace + cSpaceWidth,
                        this.position.Y + panel.Height / 2 - cTextureHeight / 2);
                else
                    positions[i] = new Vector2(positions[i - 1].X + cTextureWidth + cSpaceWidth, positions[i - 1].Y);
            }
            font = content.Load<SpriteFont>(@"Fonts\PanelFont");
            this.selectNumber = 1;
            selectedIndices = new List<int>();
            pile = new List<Texture2D>() { content.Load<Texture2D>(@"Elements\Fire"), content.Load<Texture2D>(@"Elements\Air"),
                content.Load<Texture2D>(@"Elements\Earth"), content.Load<Texture2D>(@"Elements\Water") };
        }

        public byte UpdatePanel(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < pile.Count; i++)
            {
                if (mouse.LeftButton == ButtonState.Pressed &&
                    new Rectangle((int)positions[i].X, (int)positions[i].Y, cTextureWidth,
                    cTextureHeight).Contains(mouse.X, mouse.Y) && !selectedIndices.Contains(i)
                    && selectedIndices.Count < selectNumber)
                {
                    selectedIndices.Add(i);
                    break;
                }
                else if (mouse.LeftButton == ButtonState.Pressed &&
                    new Rectangle((int)positions[i].X, (int)positions[i].Y, cTextureWidth,
                    cTextureHeight).Contains(mouse.X, mouse.Y) && !selectedIndices.Contains(i)
                    && selectedIndices.Count == selectNumber)
                {
                    selectedIndices.Clear();
                    selectedIndices.Add(i);
                    break;
                }
                else if (mouse.RightButton == ButtonState.Pressed &&
                     new Rectangle((int)positions[i].X, (int)positions[i].Y, cTextureWidth,
                    cTextureHeight).Contains(mouse.X, mouse.Y) && selectedIndices.Contains(i))
                {
                    selectedIndices.Remove(i);
                    break;
                }
            }
            byte type = 0; // Binary: 0000 for not selected type.
            if (okButton.UpdateButton(mouse, gameTime) && selectedIndices.Count == selectNumber)
            {
                switch (selectedIndices[0])
                {
                    case 0:
                        type = 8; // Binary: 1000 for Fire.
                        break;
                    case 1:
                        type = 4; // Binary: 0100 for Air.
                        break;
                    case 2:
                        type = 2; // Binary: 0010 for Earth.
                        break;
                    case 3:
                        type = 1; // Binary: 0001 for Water.
                        break;
                    default:
                        throw new IndexOutOfRangeException("This index value " + selectedIndices[0] + " shouldn't be.");
                }
                selectedIndices.Clear();
            }
            return type;
        }

        public void DrawPanel(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(panel, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);

            if (selectedIndices.Count == selectNumber)
                okButton.Draw(spriteBatch);
            string text = "Selected (" + selectedIndices.Count + "/" + selectNumber + ")";
            spriteBatch.DrawString(font, text, position, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.55f);

            for (int i = 0; i < pile.Count; i++)
            {
                spriteBatch.DrawString(font, (i + 1).ToString(),
                    new Vector2(positions[i].X + cTextureWidth / 2 - font.MeasureString((i + 1).ToString()).X / 2,
                        positions[i].Y - cTextureHeight / 4), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.55f);
                spriteBatch.Draw(pile[i], new Rectangle((int)positions[i].X, (int)positions[i].Y, cTextureWidth,
                    cTextureHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.55f);
                if (selectedIndices.Contains(i))
                {
                    int k = selectedIndices.IndexOf(i);
                    spriteBatch.DrawString(font, selectedText[k], new Vector2(positions[i].X + cTextureWidth / 2 -
                               font.MeasureString(selectedText[k]).X / 2,
                           positions[i].Y + cTextureHeight / 2 - font.MeasureString(selectedText[k]).Y / 2),
                           Color.Blue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
            }
        }
    }
}