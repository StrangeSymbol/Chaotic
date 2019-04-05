using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    struct DiscardPile<T> where T : ChaoticCard
    {
        List<T> discardPile;
        CardTemplate discardTemplate;
        DiscardPanel<T> discardPanel;
        bool isPlayer1;

        public DiscardPile(ContentManager content, GraphicsDeviceManager graphics, Texture2D texture, Vector2 position, bool isPlayer1)
        {
            discardPile = new List<T>();
            discardTemplate = new CardTemplate(texture, position);
            this.isPlayer1 = isPlayer1;
            discardPanel = new DiscardPanel<T>(content, graphics);
        }

        public List<T> DiscardList { get { return discardPile; } }
        public int Count { get { return discardPile.Count; } }
        public bool DiscardPanelActive { get { return discardPanel.Active; } }

        public T this[int i]
        {
            get { return discardPile[i]; }
        }

        public CardTemplate GetDiscardTemplate()
        {
            return discardTemplate;
        }

        public void Remove(T card)
        {
            discardPile.Remove(card);
        }
        public void UpdateDiscardPile(GameTime gameTime)
        {
            if (discardPile.Count > 0)
            {
                MouseState mouse = Mouse.GetState();
                if (mouse.LeftButton == ButtonState.Pressed && discardTemplate.CollisionRectangle.Contains(mouse.X, mouse.Y)
                    && !discardPanel.Active)
                    discardPanel.Active = true;
                else if (discardTemplate.CollisionRectangle.Contains(mouse.X, mouse.Y) && !discardPanel.Active)
                    ChaoticEngine.CoveredCard = discardPile[discardPile.Count - 1].Texture;
            }

            discardPanel.UpdatePanel(gameTime, discardPile);
        }

        public void DrawDiscardPile(SpriteBatch spriteBatch)
        {
            discardTemplate.DrawTemplate(spriteBatch, isPlayer1);

            if (discardPile.Count >= 1 && isPlayer1)
                spriteBatch.Draw(discardPile[discardPile.Count - 1].Texture, discardTemplate.CollisionRectangle,
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.85f);
            else if (discardPile.Count >= 1 && !isPlayer1)
                spriteBatch.Draw(discardPile[discardPile.Count - 1].Texture, discardTemplate.CollisionRectangle,
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.85f);

            discardPanel.DrawPanel(spriteBatch, discardPile);
        }
    }
}
