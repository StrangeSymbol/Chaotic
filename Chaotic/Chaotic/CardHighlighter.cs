using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Chaotic
{
    class CardHighlighter
    {
        Queue<Texture2D> cards; // All the cards to be highlighted.
        Texture2D card; // The image of the card highlighted.
        Vector2 cardPos; // The location of the card on the screen.
        Texture2D sheen; // The image of the sheen on the card.
        Vector2 sheenPos; // The location of the sheen in on the card.
        float sheenWidth; // Holds the width of sheen.
        float sheenHeight; // Holds the height of sheen.

        double elapsedTime;
        const float kWaitTime = 500f; // The amount of time the card gets highlighted.

        public CardHighlighter(ContentManager content, GraphicsDeviceManager graphics)
        {
            sheen = content.Load<Texture2D>("Sheen");
            cardPos = new Vector2((graphics.PreferredBackBufferWidth - ChaoticEngine.kCardCoveredWidth) / 2,
                (graphics.PreferredBackBufferHeight - ChaoticEngine.kCardCoveredHeight) / 2);
            cards = new Queue<Texture2D>();
        }

        public void InitializeHighlight(GameTime gameTime, ChaoticGameLib.ChaoticCard card)
        {
            this.card = card.Texture;
            sheenPos = cardPos;
            sheenWidth = 0;
            sheenHeight = 0;
            elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void AddHighlight(Texture2D texture)
        {
            cards.Enqueue(texture);
        }

        public bool UpdateHighlight(GameTime gameTime)
        {
            if (card != null)
            {
                double time = gameTime.TotalGameTime.TotalMilliseconds - elapsedTime;
                float percent = (float)(time / (kWaitTime / 2));
                if (time <= kWaitTime / 2)
                {
                    sheenWidth = percent * ChaoticEngine.kCardCoveredWidth;
                    sheenHeight = percent * ChaoticEngine.kCardCoveredHeight;
                    sheenPos = cardPos;
                }
                else if (time > kWaitTime / 2 && time <= kWaitTime)
                {
                    percent = 2 - percent;
                    sheenWidth = percent * ChaoticEngine.kCardCoveredWidth;
                    sheenHeight = percent * ChaoticEngine.kCardCoveredHeight;
                    sheenPos = cardPos + new Vector2(ChaoticEngine.kCardCoveredWidth - sheenWidth,
                        ChaoticEngine.kCardCoveredHeight - sheenHeight);
                }
                else
                {
                    card = null;
                    if (cards.Count > 0)
                    {
                        card = cards.Dequeue();
                        sheenPos = cardPos;
                        sheenWidth = 0;
                        sheenHeight = 0;
                        elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        public void DrawHighlight(SpriteBatch spriteBatch)
        {
            if (card != null)
            {
                spriteBatch.Draw(card, new Rectangle((int)cardPos.X, (int)cardPos.Y,
                    ChaoticEngine.kCardCoveredWidth, ChaoticEngine.kCardCoveredHeight),
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.15f);
                spriteBatch.Draw(sheen, new Rectangle((int)sheenPos.X, (int)sheenPos.Y,
                    (int)sheenWidth, (int)sheenHeight),
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            }
        }
    }
}