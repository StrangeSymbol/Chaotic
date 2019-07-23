using System;
using System.Linq;
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
        double elapsedTime;

        public DiscardPile(ContentManager content, GraphicsDeviceManager graphics, Texture2D texture, Vector2 position, bool isPlayer1)
        {
            discardPile = new List<T>();
            discardTemplate = new CardTemplate(texture, position);
            this.isPlayer1 = isPlayer1;
            discardPanel = new DiscardPanel<T>(content, graphics);
            this.elapsedTime = 0.0;
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

        public List<S> Find<S>() where S : ChaoticCard
        {
            List<T> lst = discardPile.FindAll(c => c is S);
            List<S> findLst = new List<S>();
            for (int i = 0; i < lst.Count; i++)
                findLst.Add(lst[i] as S);
            return findLst;
        }
        public List<S> Find<S>(Predicate<S> selectPred) where S : ChaoticCard
        {
            return Find<S>().FindAll(selectPred);
        }
        public void UpdateDiscardPile(GameTime gameTime, CardDescription description)
        {
            if (discardPile.Count > 0)
            {
                MouseState mouse = Mouse.GetState();
                if (mouse.LeftButton == ButtonState.Pressed && discardTemplate.CollisionRectangle.Contains(mouse.X, mouse.Y)
                    && !discardPanel.Active)
                    discardPanel.Active = true;
                else if (discardTemplate.CollisionRectangle.Contains(mouse.X, mouse.Y) && !discardPanel.Active)
                    description.CoveredCard = discardPile[discardPile.Count - 1].Texture;
            }

            discardPanel.UpdatePanel(gameTime, discardPile, description);
        }

        public bool ReturnToHand(GameTime gameTime, int i, MugicHand hand)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                discardPile[i].IsMoving = true;
                discardPile[i].CourseToCard(hand.GetNextHandPosition());
                elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (discardPile[i].Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && discardPile[i].IsMoving)
                discardPile[i].Move(gameTime);
            else if (discardPile[i].Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && discardPile[i].IsMoving)
            {
                discardPile[i].IsMoving = false;
                ChaoticEngine.IsACardMoving = false;
                elapsedTime = 0.0;
                hand.AddCardToHand(discardPile[i] as Mugic);
                discardPile.RemoveAt(i);
                return true;
            }
            return false;
        }

        public void DrawDiscardPile(SpriteBatch spriteBatch)
        {
            discardTemplate.DrawTemplate(spriteBatch, isPlayer1);

            if (discardPile.Count >= 1 && !discardPile[discardPile.Count-1].IsMoving && isPlayer1)
                spriteBatch.Draw(discardPile[discardPile.Count - 1].Texture, discardTemplate.CollisionRectangle,
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.85f);
            else if (discardPile.Count >= 1 && !discardPile[discardPile.Count - 1].IsMoving && !isPlayer1)
                spriteBatch.Draw(discardPile[discardPile.Count - 1].Texture, discardTemplate.CollisionRectangle,
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.85f);

            discardPanel.DrawPanel(spriteBatch, discardPile);
        }

        public void DrawDiscardPile(SpriteBatch spriteBatch, int i)
        {
            if (discardPile.Count - 1 != i)
                discardPile[i].Draw(spriteBatch, isPlayer1);
            else
                discardPile[i].Draw(spriteBatch, isPlayer1, 0.85f);
        }
    }
}
