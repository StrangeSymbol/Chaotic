/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    struct AttackDeck
    {
        List<Attack> deckPile;
        CardTemplate attackTemplate;
        CardTemplate deckCover1;
        MovingTemplate deckCover2;
        double elapsedTime;
        bool isPlayer1;
        bool start;
        bool shuffleMove;
        byte numShuffles;

        public AttackDeck(Texture2D deck, Texture2D cardBack, Texture2D overlay, Vector2 position, bool isPlayer1)
        {
            deckPile = new List<Attack>();
            attackTemplate = new CardTemplate(deck, position);
            deckCover1 = new CardTemplate(cardBack, position);
            deckCover2 = new MovingTemplate(cardBack, position, overlay);
            elapsedTime = 0.0;
            this.isPlayer1 = isPlayer1;
            this.start = false;
            this.shuffleMove = false;
            this.numShuffles = 0;
        }
        /// <summary>
        /// Retrieves a given card from the deck.
        /// </summary>
        /// <param name="card">The index in the deck to retrieve.</param>
        /// <returns>The selected Attack card.</returns>
        /// <exception cref="IndexOutOfRangeException">Is thrown if the index for card is outside the deck capacity.</exception>
        public Attack RetrieveCard(int card)
        {
            if (card >= 0 && card < deckPile.Count)
                return deckPile[card];
            else
                throw new IndexOutOfRangeException("Card index has to be between 0 and " + deckPile.Count);
        }

        public List<Attack> Deck { get { return deckPile; } }
        public int Count { get { return deckPile.Count; } }
        public Vector2 Position { get { return attackTemplate.Position; } }
        public bool DeckCovered { get { return deckCover2.IsCovered; } set { deckCover2.IsCovered = value; } }
        public Rectangle DeckRectangle { get { return attackTemplate.CollisionRectangle; } }
        public bool IsPlayer1 { get { return isPlayer1; } }
        /// <summary>
        /// Populates and Shuffles Attack cards to be used as a game deck.
        /// </summary>
        /// <param name="deckHolder">This is the list of attacks this player is using for their attack deck.</param>
        public void ShuffleDeck(List<Attack> deckHolder)
        {
            Random random = new Random();
            List<Attack> deckCopy = new List<Attack>();

            for (int i = 0; i < deckHolder.Count; i++)
            {
                deckCopy.Add(deckHolder[i].ShallowCopy() as Attack);
            }

            while (deckCopy.Count > 0)
            {
                Attack card = deckCopy[random.Next(deckCopy.Count)];
                deckCopy.Remove(card);
                deckPile.Add(card);
            }
        }
        /// <summary>
        /// Re-shuffles the current Attack Deck.
        /// </summary>
        public void ShuffleDeck()
        {
            Random random = new Random();
            List<Attack> deckCopy = new List<Attack>(deckPile);
            deckPile.Clear();

            while (deckCopy.Count > 0)
            {
                Attack card = deckCopy[random.Next(deckCopy.Count)];
                deckCopy.Remove(card);
                deckPile.Add(card);
            }
        }
        /// <summary>
        /// Shuffles the Attack deck when there are no more cards to draw.
        /// </summary>
        /// <param name="discardPile">The discard pile where the attack cards used are held.</param>
        public void ShuffleDeck(DiscardPile<Attack> discardPile)
        {
            if (deckPile.Count == 0)
            {
                Random random = new Random();

                while (discardPile.Count > 0)
                {
                    Attack card = discardPile[random.Next(discardPile.Count)];
                    discardPile.Remove(card);
                    deckPile.Add(card);
                }
            }
        }

        public bool UpdateDeckPile(GameTime gameTime, AttackHand hand)
        {
            isPlayer1 = hand.IsPlayer1;
            if (!start)
            {
                start = true;
                deckCover2.IsCovered = false;
                deckCover2.IsMoving = true;
                deckCover2.CourseToCard(hand.GetNextHandPosition());
                elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (deckCover2.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
                deckCover2.Move(gameTime, attackTemplate.Position, hand.GetNextHandPosition());
            else if (deckCover2.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
            {
                hand.AddCardToHand(deckPile[deckPile.Count - 1].ShallowCopy() as Attack);
                deckPile.RemoveAt(deckPile.Count - 1);
                deckCover2.Position = attackTemplate.Position;
                deckCover2.Destination = deckCover2.CollisionRectangle;
                start = false;
                deckCover2.IsMoving = false;
                elapsedTime = 0.0;
                return true;
            }
            return false;
        }

        public bool UpdateDeckPile(GameTime gameTime, MouseState mouse, AttackHand hand)
        {
            isPlayer1 = hand.IsPlayer1;
            if (!ChaoticEngine.IsACardMoving)
            {
                if (mouse.LeftButton == ButtonState.Pressed &&
                    attackTemplate.CollisionRectangle.Contains(new Point(mouse.X, mouse.Y)))
                {
                    ChaoticEngine.IsACardMoving = true;
                    deckCover2.IsMoving = true;
                    deckCover2.CourseToCard(hand.GetNextHandPosition());
                    elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else if (deckCover2.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
                deckCover2.Move(gameTime, attackTemplate.Position, hand.GetNextHandPosition());
            else if (deckCover2.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
            {
                hand.AddCardToHand(deckPile[deckPile.Count - 1].ShallowCopy() as Attack);
                deckPile.RemoveAt(deckPile.Count - 1);
                deckCover2.Position = attackTemplate.Position;
                deckCover2.Destination = deckCover2.CollisionRectangle;
                ChaoticEngine.IsACardMoving = false;
                deckCover2.IsMoving = false;
                elapsedTime = 0.0;
                return true;
            }
            return false;
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
                        deckCover2.CourseToCard(attackTemplate.Position + new Vector2(ChaoticEngine.kCardWidth, 0));
                    else
                        deckCover2.CourseToCard(attackTemplate.Position + new Vector2(-ChaoticEngine.kCardWidth, 0));
                }
                else
                {
                    deckCover2.CourseToCard(attackTemplate.Position);
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
                        deckCover2.Position = attackTemplate.Position + new Vector2(ChaoticEngine.kCardWidth, 0);
                    else
                        deckCover2.Position = attackTemplate.Position + new Vector2(-ChaoticEngine.kCardWidth, 0);
                    shuffleMove = true;
                }
                else
                {
                    deckCover2.Position = attackTemplate.Position;
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

        //public bool UpdateDeckPile(GameTime gameTime, DiscardPile<Attack> discardPile)
        //{
        //    if (!ChaoticEngine.IsACardMoving)
        //    {
        //        ChaoticEngine.IsACardMoving = true;
        //        deckCover2.IsCovered = false;
        //        deckCover2.IsMoving = true;
        //        deckCover2.CourseToCard(discardPile.GetDiscardTemplate().Position);
        //        elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
        //    }
        //    else if (deckCover2.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
        //        deckCover2.Move(gameTime, attackTemplate.Position, discardPile.GetDiscardTemplate().Position);
        //    else if (deckCover2.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && deckCover2.IsMoving)
        //    {
        //        discardPile.DiscardList.Add(RetrieveCard(3));
        //        deckPile.RemoveAt(3);
        //        deckCover2.Position = deckPile[0].Position;
        //        deckCover2.Destination = deckCover2.CollisionRectangle;
        //        ChaoticEngine.IsACardMoving = false;
        //        deckCover2.IsMoving = false;
        //        elapsedTime = 0.0;
        //        return true;
        //    }
        //    return false;
        //}

        public bool AddingCardsToHand(GameTime gameTime, DiscardPile<Attack> discardPile, AttackHand hand)
        {
            ShuffleDeck(discardPile);
            return UpdateDeckPile(gameTime, hand);
        }

        public void DrawShuffleDeck(SpriteBatch spriteBatch)
        {
            attackTemplate.DrawTemplate(spriteBatch, isPlayer1);

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

            if (deckCover2.IsMoving && isPlayer1)
                texture = deckPile[deckPile.Count - 1].Texture;

            attackTemplate.DrawTemplate(spriteBatch, isPlayer1);

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