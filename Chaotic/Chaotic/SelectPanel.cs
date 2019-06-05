using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Chaotic
{
    class SelectPanel<T> where T : ChaoticGameLib.ChaoticCard
    {
        readonly int cTextureWidth = 85;
        readonly int cTextureHeight = 133;
        const int cSpace = 20;
        const int cSpaceWidth = 21;
        int start;
        int end = -1;
        int selectNumber;
        List<int> selectedIndices;
        Vector2 pos1;
        Vector2 pos2;
        bool active = false;
        Texture2D panel;
        Vector2 position;
        Button leftButton;
        Button rightButton;
        Button okButton;
        Vector2[] positions;
        SpriteFont font;
        string[] selectedText;

        public SelectPanel(ContentManager content, GraphicsDeviceManager graphics, int selectNumber, params string[] selectedText)
        {
            if (typeof(T).Name == "Location")
            {
                cTextureWidth = 133;
                cTextureHeight = 85;
            }
            else
            {
                cTextureWidth = 85;
                cTextureHeight = 133;
            }
            this.panel = content.Load<Texture2D>(@"Panel\Panel");
            this.position = new Vector2(graphics.PreferredBackBufferWidth / 2 - panel.Width / 2,
                graphics.PreferredBackBufferHeight / 2 - panel.Height / 2);
            this.selectedText = selectedText;
            Texture2D texture = content.Load<Texture2D>(@"Panel\ArrowButtonL");
            this.leftButton = new Button(texture, new Vector2(position.X + 7, graphics.PreferredBackBufferHeight / 2
                - texture.Height / 2), content.Load<Texture2D>(@"Panel\ArrowButtonCoverL"));
            texture = content.Load<Texture2D>(@"Panel\ArrowButtonR");
            this.rightButton = new Button(texture, new Vector2(position.X + panel.Width - 7 - texture.Width,
                graphics.PreferredBackBufferHeight / 2 - texture.Height / 2), content.Load<Texture2D>(@"Panel\ArrowButtonCoverR"));
            texture = content.Load<Texture2D>(@"Panel\OkButton");
            this.okButton = new Button(texture, new Vector2(graphics.PreferredBackBufferWidth / 2 - texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"));
            positions = new Vector2[5];
            for (int i = 0; i < positions.Length; i++)
            {
                if (i == 0)
                    positions[i] = new Vector2(leftButton.Position.X + leftButton.Texture.Width + cSpace,
                        this.position.Y + panel.Height / 2 - cTextureHeight / 2);
                else
                    positions[i] = new Vector2(positions[i - 1].X + cTextureWidth + cSpaceWidth, positions[i - 1].Y);
            }
            font = content.Load<SpriteFont>(@"Fonts\PanelFont");
            if (typeof(T).Name == "Location")
            {
                pos1 = new Vector2(positions[2].X - 7 * cTextureWidth / 4, positions[2].Y);
                pos2 = new Vector2(positions[2].X, positions[2].Y);
            }
            else
            {
                pos1 = new Vector2(positions[2].X - cTextureWidth / 2, positions[2].Y);
                pos2 = new Vector2(positions[2].X + cTextureWidth / 2 + cSpaceWidth, positions[2].Y);
            }
            this.selectNumber = selectNumber;
            selectedIndices = new List<int>();
        }

        public bool Active { get { return active; } set { if (!active) selectedIndices = new List<int>(); active = value; } }
        public int SelectNumber { get { return selectNumber; } }

        public List<int> UpdatePanel(GameTime gameTime, List<T> pile)
        {
            if (active)
            {
                if (pile.Count < selectNumber)
                    selectNumber = pile.Count;
                if (end == -1)
                {
                    if (pile.Count < 5)
                        end = 0;
                    else
                        end = pile.Count - 5;
                    start = pile.Count - 1;
                }
                MouseState mouse = Mouse.GetState();
                if (start - end > 1)
                {
                    int n = 0;
                    if (start - end == 2)
                        n = 1;
                    for (int i = start, j = n; i >= end; i--, j++)
                    {
                        if (new Rectangle((int)positions[j].X, (int)positions[j].Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                            ChaoticEngine.CoveredCard = pile[i].Texture;

                        if (mouse.LeftButton == ButtonState.Pressed &&
                            new Rectangle((int)positions[j].X, (int)positions[j].Y, cTextureWidth,
                            cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && !selectedIndices.Contains(i)
                            && selectedIndices.Count < selectNumber)
                        {
                            selectedIndices.Add(i);
                            break;
                        }
                        else if (mouse.RightButton == ButtonState.Pressed &&
                            new Rectangle((int)positions[j].X, (int)positions[j].Y, cTextureWidth,
                            cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && selectedIndices.Contains(i))
                        {
                            selectedIndices.Remove(i);
                            break;
                        }
                    }
                }
                else if (start - end == 1)
                {
                    if (new Rectangle((int)pos1.X, (int)pos1.Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                        ChaoticEngine.CoveredCard = pile[1].Texture;
                    else if (new Rectangle((int)pos2.X, (int)pos2.Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                        ChaoticEngine.CoveredCard = pile[0].Texture;

                    if (mouse.LeftButton == ButtonState.Pressed && new Rectangle((int)pos1.X, (int)pos1.Y, cTextureWidth,
                            cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && !selectedIndices.Contains(1)
                        && selectedIndices.Count < selectNumber)
                        selectedIndices.Add(1);
                    else if (mouse.RightButton == ButtonState.Pressed &&
                            new Rectangle((int)pos1.X, (int)pos1.Y, cTextureWidth,
                            cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && selectedIndices.Contains(1))
                        selectedIndices.Remove(1);
                    else if (mouse.LeftButton == ButtonState.Pressed && new Rectangle((int)pos2.X, (int)pos2.Y, cTextureWidth,
                            cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && !selectedIndices.Contains(0)
                        && selectedIndices.Count < selectNumber)
                        selectedIndices.Add(0);
                    else if (mouse.RightButton == ButtonState.Pressed &&
                            new Rectangle((int)pos2.X, (int)pos2.Y, cTextureWidth,
                            cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && selectedIndices.Contains(0))
                        selectedIndices.Remove(0);
                }
                else if (start - end == 0)
                {
                    if (new Rectangle((int)positions[2].X, (int)positions[2].Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                        ChaoticEngine.CoveredCard = pile[start].Texture;

                    if (mouse.LeftButton == ButtonState.Pressed && new Rectangle((int)positions[2].X, (int)positions[2].Y,
                        cTextureWidth, cTextureHeight).Contains(new Point(mouse.X, mouse.Y)) && !selectedIndices.Contains(0)
                        && selectedIndices.Count < selectNumber)
                        selectedIndices.Add(0);
                }
                if (leftButton.UpdateButton(mouse, gameTime) && start != pile.Count - 1)
                {
                    if (start + 5 <= pile.Count - 1)
                    {
                        start += 5;
                        end += 5;
                    }
                    else if (start < pile.Count - 1)
                    {
                        end += pile.Count - 1 - start;
                        start = pile.Count - 1;
                    }
                }

                else if (rightButton.UpdateButton(mouse, gameTime) && end != 0)
                {
                    if (end >= 5)
                    {
                        start -= 5;
                        end -= 5;
                    }
                    else
                    {
                        start -= end;
                        end = 0;
                    }
                }

                else if (okButton.UpdateButton(mouse, gameTime) && selectedIndices.Count == selectNumber)
                {
                    active = false;
                    end = -1;
                    return selectedIndices;
                }
            }
            return null;
        }

        public void DrawPanel(SpriteBatch spriteBatch, List<T> pile)
        {
            if (active)
            {
                spriteBatch.Draw(panel, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);
                if (start != pile.Count - 1)
                    leftButton.Draw(spriteBatch);
                if (end != 0)
                    rightButton.Draw(spriteBatch);
                if (selectedIndices.Count == selectNumber)
                    okButton.Draw(spriteBatch);
                string text = "Selected (" + selectedIndices.Count + "/" + selectNumber + ")";
                spriteBatch.DrawString(font, text, position, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                

                if (start - end > 1)
                {
                    int n = 0;
                    if (start - end == 2)
                        n = 1;
                    for (int i = start, j = n; i >= end; i--, j++)
                    {
                        spriteBatch.DrawString(font, (i + 1).ToString(),
                            new Vector2(positions[j].X + cTextureWidth / 2 - font.MeasureString((i + 1).ToString()).X / 2,
                                positions[j].Y - cTextureHeight / 4), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                        spriteBatch.Draw(pile[i].Texture, new Rectangle((int)positions[j].X, (int)positions[j].Y, cTextureWidth,
                            cTextureHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                        if (selectedIndices.Contains(i))
                        {
                            int k = selectedIndices.IndexOf(i);
                            spriteBatch.DrawString(font, selectedText[k], new Vector2(positions[j].X + cTextureWidth / 2 -
                                       font.MeasureString(selectedText[k]).X / 2,
                                   positions[j].Y + cTextureHeight / 2 - font.MeasureString(selectedText[k]).Y / 2), 
                                   Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                        }
                    }
                }
                else if (start - end == 1)
                {
                    spriteBatch.DrawString(font, 2.ToString(),
                            new Vector2(pos1.X + cTextureWidth / 2 - font.MeasureString(2.ToString()).X / 2,
                                pos1.Y - cTextureHeight / 4), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                    spriteBatch.Draw(pile[1].Texture, new Rectangle((int)pos1.X, (int)pos1.Y, cTextureWidth,
                            cTextureHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                    if (selectedIndices.Contains(1))
                    {
                        int k = selectedIndices.IndexOf(1);
                        spriteBatch.DrawString(font, selectedText[k], new Vector2(pos1.X + cTextureWidth / 2 -
                                font.MeasureString(selectedText[k]).X / 2,
                            pos1.Y + cTextureHeight / 2 - font.MeasureString(selectedText[k]).Y / 2),
                            Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                    }
                    spriteBatch.DrawString(font, 1.ToString(),
                            new Vector2(pos2.X + cTextureWidth / 2 - font.MeasureString(1.ToString()).X / 2,
                                pos2.Y - cTextureHeight / 4), 
                                Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                    spriteBatch.Draw(pile[0].Texture, new Rectangle((int)pos2.X, (int)pos2.Y, cTextureWidth,
                            cTextureHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                    if (selectedIndices.Contains(0))
                    {
                        int k = selectedIndices.IndexOf(0);
                        spriteBatch.DrawString(font, selectedText[k], new Vector2(pos2.X + cTextureWidth / 2 -
                                font.MeasureString(selectedText[k]).X / 2,
                            pos2.Y + cTextureHeight / 2 - font.MeasureString(selectedText[k]).Y / 2),
                            Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                    }
                }
                else if (start - end == 0)
                {
                    spriteBatch.DrawString(font, 1.ToString(),
                            new Vector2(positions[2].X + cTextureWidth / 2 - font.MeasureString(1.ToString()).X / 2,
                                positions[2].Y - cTextureHeight / 4), 
                                Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                    spriteBatch.Draw(pile[start].Texture, new Rectangle((int)positions[2].X, (int)positions[2].Y, cTextureWidth,
                            cTextureHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                    if (selectedIndices.Contains(0))
                    {
                        spriteBatch.DrawString(font, selectedText[0], new Vector2(positions[2].X + cTextureWidth / 2 -
                                font.MeasureString(selectedText[0]).X / 2,
                            positions[2].Y + cTextureHeight / 2 - font.MeasureString(selectedText[0]).Y / 2),
                            Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
                    }
                }
            }
        }
    }
}
