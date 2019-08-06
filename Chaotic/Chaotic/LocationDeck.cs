using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    struct LocationDeck
    {
        List<Location> deckPile;
        CardTemplate locationTemplate;
        CardTemplate deckCover1;
        MovingTemplate deckCover2;
        double elapsedTime;
        bool isPlayer1;
        bool shuffleMove;
        byte numShuffles;

        public LocationDeck(Texture2D deck, Texture2D cardBack, Texture2D overlay, Vector2 position, bool isPlayer1)
        {
            deckPile = new List<Location>();
            locationTemplate = new CardTemplate(deck, position, true);
            deckCover1 = new CardTemplate(cardBack, position, true);
            deckCover2 = new MovingTemplate(cardBack, position, overlay, true);
            elapsedTime = 0.0;
            this.isPlayer1 = isPlayer1;
            this.shuffleMove = false;
            this.numShuffles = 0;
        }

        public Location RetrieveCard(int card)
        {
            if (card >= 0 && card < deckPile.Count)
                return deckPile[card];
            else
                throw new IndexOutOfRangeException("Card index has to be between 0 and " + deckPile.Count);
        }

        public List<Location> Deck { get { return deckPile; } }
        public Vector2 DeckPosition { get { return locationTemplate.Position; } }
        public int Count { get { return Deck.Count; } }
        public bool DeckCovered { get { return deckCover2.IsCovered; } set { deckCover2.IsCovered = value; } }
        public Rectangle DeckRectangle { get { return locationTemplate.CollisionRectangle; } }
        public bool IsPlayer1 { get { return isPlayer1; } }

        public void ShuffleDeck(List<Location> deckHolder)
        {
            Random random = new Random();
            List<Location> deckCopy = new List<Location>();

            for (int i = 0; i < deckHolder.Count; i++)
            {
                deckCopy.Add(deckHolder[i].ShallowCopy() as Location);
            }

            while (deckCopy.Count > 0)
            {
                Location card = deckCopy[random.Next(deckCopy.Count)];
                deckCopy.Remove(card);
                deckPile.Add(card);
            }
        }

        /// <summary>
        /// Re-shuffles the current Location Deck.
        /// </summary>
        public void ShuffleDeck()
        {
            Random random = new Random();
            List<Location> deckCopy = new List<Location>(deckPile);
            deckPile.Clear();

            while (deckCopy.Count > 0)
            {
                Location card = deckCopy[random.Next(deckCopy.Count)];
                deckCopy.Remove(card);
                deckPile.Add(card);
            }
        }

        public bool UpdateShuffleDeck(GameTime gameTime)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                deckCover2.IsMoving = true;
                if (!shuffleMove)
                {
                    if (this.isPlayer1)
                        deckCover2.CourseToCard(locationTemplate.Position + new Vector2(0, ChaoticEngine.kCardWidth));
                    else
                        deckCover2.CourseToCard(locationTemplate.Position + new Vector2(0, -ChaoticEngine.kCardWidth));
                }
                else
                {
                    deckCover2.CourseToCard(locationTemplate.Position);
                }
                elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (deckCover2.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
                deckCover2.Move(gameTime);
            else if (deckCover2.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
            {
                ChaoticEngine.IsACardMoving = false;
                deckCover2.IsMoving = false;
                elapsedTime = 0.0;
                if (!shuffleMove)
                {
                    if (this.isPlayer1)
                        deckCover2.Position = locationTemplate.Position + new Vector2(0, ChaoticEngine.kCardWidth);
                    else
                        deckCover2.Position = locationTemplate.Position + new Vector2(0, -ChaoticEngine.kCardWidth);
                    shuffleMove = true;
                }
                else
                {
                    deckCover2.Position = locationTemplate.Position;
                    shuffleMove = false;
                    if (++numShuffles == 3)
                    {
                        numShuffles = 0;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool UpdateDeckPile(GameTime gameTime, ActiveLocation location)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                deckCover2.IsCovered = false;
                deckCover2.IsMoving = true;
                deckCover2.CourseToCard(location.Position);
                elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (deckCover2.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
                deckCover2.Move(gameTime, locationTemplate.Position, location.Position);
            else if (deckCover2.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
            {
                location.SetLocation(deckPile[deckPile.Count - 1].ShallowCopy() as Location);
                deckPile.RemoveAt(deckPile.Count - 1);
                deckCover2.Position = locationTemplate.Position;
                deckCover2.Destination = deckCover2.CollisionRectangle;
                ChaoticEngine.IsACardMoving = false;
                deckCover2.IsMoving = false;
                elapsedTime = 0.0;
                return true;
            }
            return false;
        }

        public bool UpdateDeckPile(GameTime gameTime, MouseState mouse, ActiveLocation location)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                if (mouse.LeftButton == ButtonState.Pressed &&
                    locationTemplate.CollisionRectangle.Contains(mouse.X, mouse.Y))
                {
                    deckCover2.IsCovered = true;
                }
                else if (mouse.RightButton == ButtonState.Pressed &&
                    locationTemplate.CollisionRectangle.Contains(mouse.X, mouse.Y) && deckCover2.IsCovered)
                {
                    ChaoticEngine.IsACardMoving = true;
                    deckCover2.IsCovered = false;
                    deckCover2.IsMoving = true;
                    deckCover2.CourseToCard(location.Position);
                    elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else if (deckCover2.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
                deckCover2.Move(gameTime, locationTemplate.Position, location.Position);
            else if (deckCover2.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
            {
                location.SetLocation(deckPile[deckPile.Count - 1].ShallowCopy() as Location);
                deckPile.RemoveAt(deckPile.Count - 1);
                deckCover2.Position = locationTemplate.Position;
                deckCover2.Destination = deckCover2.CollisionRectangle;
                ChaoticEngine.IsACardMoving = false;
                deckCover2.IsMoving = false;
                elapsedTime = 0.0;
                return true;
            }
            return false;
        }

        public void DrawShuffleDeck(SpriteBatch spriteBatch)
        {
            locationTemplate.DrawTemplate(spriteBatch, isPlayer1);

            if (!shuffleMove)
            {
                deckCover1.DrawTemplate(spriteBatch, isPlayer1, 0.9f);
                deckCover2.DrawTemplate(spriteBatch, isPlayer1);
            }
            else
            {
                deckCover1.DrawTemplate(spriteBatch, isPlayer1, 0.85f);
                deckCover2.DrawTemplate(spriteBatch, isPlayer1, 0.9f);
            }
        }

        public void DrawDeckPile(SpriteBatch spriteBatch)
        {
            Texture2D texture = null;

            if (deckCover2.IsMoving)
                texture = deckPile[deckPile.Count - 1].Texture;

            locationTemplate.DrawTemplate(spriteBatch, isPlayer1);

            if (deckPile.Count == 1 && texture != null)
                deckCover2.DrawTemplate(spriteBatch, texture, isPlayer1);
            else if (deckPile.Count == 1 && texture == null)
                deckCover2.DrawTemplate(spriteBatch, isPlayer1);
            else if (deckPile.Count >= 2)
            {
                deckCover1.DrawTemplate(spriteBatch, isPlayer1, 0.9f);
                if (texture != null)
                    deckCover2.DrawTemplate(spriteBatch, texture, isPlayer1);
                else
                    deckCover2.DrawTemplate(spriteBatch, isPlayer1);
            }
        }
    }
}
