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
using ChaoticGameLib;
using Microsoft.Xna.Framework.Audio;

namespace Chaotic
{
    class DiscardPanel<T> where T : ChaoticCard
    {
        const int cTextureWidth = 85;
        const int cTextureHeight = 133;
        const int cSpace = 20;
        const int cSpaceWidth = 21;
        int start;
        int end = -1;
        bool active;
        Texture2D panel;
        Vector2 position;
        Button leftButton;
        Button rightButton;
        Button okButton;
        Vector2[] positions;
        Vector2 pos1;
        Vector2 pos2;
        SpriteFont font;

        public DiscardPanel(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.panel = content.Load<Texture2D>(@"Panel\Panel");
            this.position = new Vector2(graphics.PreferredBackBufferWidth / 2 - panel.Width / 2,
                graphics.PreferredBackBufferHeight / 2 - panel.Height / 2);
            Texture2D texture = content.Load<Texture2D>(@"Panel\ArrowButtonL");
            this.leftButton = new Button(texture, new Vector2(position.X + 7, graphics.PreferredBackBufferHeight / 2
                - texture.Height / 2), content.Load<Texture2D>(@"Panel\ArrowButtonCoverL"),
                content.Load<SoundEffect>(@"Audio\ArrowClick"));
            texture = content.Load<Texture2D>(@"Panel\ArrowButtonR");
            this.rightButton = new Button(texture, new Vector2(position.X + panel.Width - 7 - texture.Width,
                graphics.PreferredBackBufferHeight / 2 - texture.Height / 2), content.Load<Texture2D>(@"Panel\ArrowButtonCoverR"),
                content.Load<SoundEffect>(@"Audio\ArrowClick"));
            texture = content.Load<Texture2D>(@"Panel\OkButton");
            this.okButton = new Button(texture, new Vector2(graphics.PreferredBackBufferWidth / 2 - texture.Width / 2,
                position.Y + panel.Height - texture.Height - 20), content.Load<Texture2D>("OkButtonCover"),
                content.Load<SoundEffect>(@"Audio\RightMouseClick"));
            positions = new Vector2[5];
            for (int i = 0; i < positions.Length; i++)
            {
                if (i == 0)
                    positions[i] = new Vector2(leftButton.Position.X + leftButton.Texture.Width + cSpace,
                        this.position.Y + panel.Height / 2 - cTextureHeight / 2);
                else
                    positions[i] = new Vector2(positions[i - 1].X + cTextureWidth + cSpaceWidth, positions[i - 1].Y);
            }
            pos1 = new Vector2(positions[2].X - cTextureWidth / 2, positions[2].Y);
            pos2 = new Vector2(positions[2].X + cTextureWidth / 2 + cSpaceWidth, positions[2].Y); 
            font = content.Load<SpriteFont>(@"Fonts\PanelFont");
        }

        public bool Active { get { return active; } set { active = value; } }

        public void UpdatePanel(GameTime gameTime, List<T> pile, CardDescription description)
        {
            if (active)
            {
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
                        {
                            description.CoveredCard = pile[i].Texture;
                            description.Description = pile[i].Description();
                        }
                    }
                }

                else if (start - end == 1)
                {
                    if (new Rectangle((int)pos1.X, (int)pos1.Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                    {
                        description.CoveredCard = pile[1].Texture;
                        description.Description = pile[1].Description();
                    }
                    else if (new Rectangle((int)pos2.X, (int)pos2.Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                    {
                        description.CoveredCard = pile[0].Texture;
                        description.Description = pile[0].Description();
                    }
                }

                else if (start - end == 0)
                {
                    if (new Rectangle((int)positions[2].X, (int)positions[2].Y, cTextureWidth,
                            cTextureHeight).Contains(mouse.X, mouse.Y))
                    {
                        description.CoveredCard = pile[start].Texture;
                        description.Description = pile[start].Description();
                    }
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

                else if (okButton.UpdateButton(mouse, gameTime))
                {
                    active = false;
                    end = -1;
                }
            }
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
                okButton.Draw(spriteBatch);
                string text = "Discard Pile (" + pile.Count + ")";
                spriteBatch.DrawString(font, text, position, Color.Black);

                if (start - end > 1)
                {
                    int n = 0;
                    if (start - end == 2)
                        n = 1;
                    for (int i = start, j = n; i >= end; i--, j++)
                    {
                        spriteBatch.DrawString(font, (i + 1).ToString(),
                            new Vector2(positions[j].X + cTextureWidth / 2 - font.MeasureString((i + 1).ToString()).X / 2,
                                positions[j].Y - cTextureHeight / 4), Color.Black);
                        spriteBatch.Draw(pile[i].Texture, new Rectangle((int)positions[j].X, (int)positions[j].Y, cTextureWidth,
                            cTextureHeight), Color.White);
                    }
                }
                else if (start - end == 1)
                {
                    spriteBatch.DrawString(font, 2.ToString(),
                            new Vector2(pos1.X + cTextureWidth / 2 - font.MeasureString(2.ToString()).X / 2,
                                pos1.Y - cTextureHeight / 4), Color.Black);
                    spriteBatch.Draw(pile[1].Texture, new Rectangle((int)pos1.X, (int)pos1.Y, cTextureWidth,
                            cTextureHeight), Color.White);
                    spriteBatch.DrawString(font, 1.ToString(),
                            new Vector2(pos2.X + cTextureWidth / 2 - font.MeasureString(1.ToString()).X / 2,
                                pos2.Y - cTextureHeight / 4), Color.Black);
                    spriteBatch.Draw(pile[0].Texture, new Rectangle((int)pos2.X, (int)pos2.Y, cTextureWidth,
                            cTextureHeight), Color.White);
                }
                else if (start - end == 0)
                {
                    spriteBatch.DrawString(font, 1.ToString(),
                            new Vector2(positions[2].X + cTextureWidth / 2 - font.MeasureString(1.ToString()).X / 2,
                                positions[2].Y - cTextureHeight / 4), Color.Black);
                    spriteBatch.Draw(pile[start].Texture, new Rectangle((int)positions[2].X, (int)positions[2].Y, cTextureWidth,
                            cTextureHeight), Color.White);
                }
            }
        }
    }
}
