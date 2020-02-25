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
    class MugicHand : IEnumerable<Mugic>
    {
        List<Mugic> hand;
        Texture2D cardBack;
        Vector2 cardInHandPos;
        readonly Vector2 discardTemplate;
        SpriteFont font;
        int? currentCard;
        bool isPlayer1;
        double elaspedTime;

        public MugicHand(bool isPlayer1, Texture2D cardBack, Vector2 discardTemplate, SpriteFont font)
        {
            this.isPlayer1 = isPlayer1;
            this.cardBack = cardBack;
            hand = new List<Mugic>();
            elaspedTime = 0.0;
            this.discardTemplate = discardTemplate;
            this.font = font;
            this.currentCard = null;
        }

        public int Count { get { return hand.Count; } }
        public bool IsPlayer1 { get { return isPlayer1; } }

        public void UpdateCoveredCard(MouseState mouse, CardDescription description)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                {
                    description.CoveredCard = hand[i].Texture;
                    description.Description = hand[i].Description();
                }
            }
        }

        public void UpdatePlayable(MouseState mouse, BattleBoardNode[] creatureSpaces, DiscardPile<ChaoticCard> discardPile,
            ActiveLocation activeLoc)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                {
                    if (CheckSystem.CheckMugicPlayable(this.isPlayer1, hand[i], creatureSpaces, discardPile, activeLoc))
                        currentCard = i;
                }
                else if (currentCard == i)
                    currentCard = null;
            }
        }

        public bool UpdateHand(GameTime gameTime, MouseState mouse, DiscardPile<ChaoticCard> discardPile,
            BattleBoardNode[] creatureSpaces, ActiveLocation activeLoc)
        {
            for (int i = hand.Count - 1; i >= 0; i--)
            {
                if (!ChaoticEngine.IsACardMoving)
                {
                    if (mouse.LeftButton == ButtonState.Pressed && hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y) &&
                    CheckSystem.CheckMugicPlayable(this.isPlayer1, hand[i], creatureSpaces, discardPile, activeLoc))
                    {
                        ChaoticEngine.IsACardMoving = true;
                        hand[i].IsMoving = true;
                        hand[i].CourseToCard(discardPile.GetDiscardTemplate().Position);
                        elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else if (hand[i].Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                    hand[i].Move(gameTime);
                else if (hand[i].Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                {
                    hand[i].IsMoving = false;
                    discardPile.DiscardList.Add(hand[i].Copy(discardPile.GetDiscardTemplate().Position));
                    ChaoticEngine.IsACardMoving = false;
                    elaspedTime = 0.0;
                    RemoveCardFromHand(i);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateHand(GameTime gameTime, MouseState mouse, DiscardPile<ChaoticCard> discardPile)
        {
            for (int i = hand.Count - 1; i >= 0; i--)
            {
                if (!ChaoticEngine.IsACardMoving)
                {
                    if (mouse.LeftButton == ButtonState.Pressed && hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                    {
                        ChaoticEngine.IsACardMoving = true;
                        hand[i].IsMoving = true;
                        hand[i].CourseToCard(discardPile.GetDiscardTemplate().Position);
                        elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else if (hand[i].Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                    hand[i].Move(gameTime);
                else if (hand[i].Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                {
                    hand[i].IsMoving = false;
                    discardPile.DiscardList.Add(hand[i].Copy(discardPile.GetDiscardTemplate().Position));
                    ChaoticEngine.IsACardMoving = false;
                    elaspedTime = 0.0;
                    RemoveCardFromHand(i);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateHand(GameTime gameTime, int i, DiscardPile<ChaoticCard> discardPile)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                hand[i].IsMoving = true;
                hand[i].CourseToCard(discardPile.GetDiscardTemplate().Position);
                cardInHandPos = hand[i].Position;
                elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                hand[i].IsFromFaceDownToFaceUp = false;
            }
            else if (hand[i].Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                hand[i].Move(gameTime, cardInHandPos, discardPile.GetDiscardTemplate().Position);
            else if (hand[i].Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
            {
                discardPile.DiscardList.Add(hand[i].Copy(discardPile.GetDiscardTemplate().Position) as Attack);
                ChaoticEngine.IsACardMoving = false;
                hand[i].IsMoving = false;
                elaspedTime = 0.0;
                RemoveCardFromHand(i);
                return true;
            }
            return false;
        }

        public Mugic this[int i]
        {
            get { return this.hand[i]; }
        }

        public void AddCardToHand(Mugic card)
        {
            hand.Add(card.Copy(GetNextHandPosition()) as Mugic);
        }

        public void RemoveCardFromHand(int index)
        {
            List<Mugic> cards;
            if (index != hand.Count - 1)
            {
                cards = hand.GetRange(index + 1, hand.Count - 1 - index);
                hand.RemoveRange(index, hand.Count - index);
                foreach (Mugic card in cards)
                    AddCardToHand(card);
            }
            else if (index == hand.Count - 1)
                hand.RemoveAt(index);
        }

        public Vector2 GetNextHandPosition()
        {
            if (hand.Count == 0 && isPlayer1)
                return new Vector2(discardTemplate.X - 7 * ChaoticEngine.kCardWidth / 2,
                    discardTemplate.Y - ChaoticEngine.kCardHeight);
            else if (hand.Count == 0 && !isPlayer1)
                return new Vector2(discardTemplate.X + 7 * ChaoticEngine.kCardWidth / 2,
                    discardTemplate.Y + ChaoticEngine.kCardHeight);
            else if (hand.Count < 3 && isPlayer1)
                return new Vector2(hand[hand.Count - 1].Position.X + ChaoticEngine.kCardWidth,
                    hand[hand.Count - 1].Position.Y);
            else if (hand.Count < 3 && !isPlayer1)
                return new Vector2(hand[hand.Count - 1].Position.X - ChaoticEngine.kCardWidth,
                    hand[hand.Count - 1].Position.Y);
            else if (hand.Count >= 3 && hand.Count < 6 && isPlayer1)
                return new Vector2(hand[hand.Count % 3].Position.X,
                    hand[hand.Count % 3].Position.Y + ChaoticEngine.kCardHeight);
            else if (hand.Count >= 3 && hand.Count < 6 && !isPlayer1)
                return new Vector2(hand[hand.Count % 3].Position.X,
                    hand[hand.Count % 3].Position.Y - ChaoticEngine.kCardHeight);
            else
                throw new IndexOutOfRangeException("The Mugic Hand can only have up to 6 cards in hand!");
        }

        public void EmptyHand()
        {
            hand.Clear();
        }

        IEnumerator<Mugic> IEnumerable<Mugic>.GetEnumerator()
        {
            foreach (Mugic sprite in hand)
                yield return sprite;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void DrawHand(SpriteBatch spriteBatch)
        {
            //if (isPlayer1)
            //{
            //    if (ChaoticEngine.Player1First)
            //        foreach (Mugic card in hand)
            //            card.Draw(spriteBatch, isPlayer1);
            //    else
            //        foreach (Mugic card in hand)
            //            spriteBatch.Draw(cardBack, card.CollisionRectangle, Color.White);
            //}
            //else
            //{
            //    if (!ChaoticEngine.Player1First)
            //        foreach (Mugic card in hand)
            //            card.Draw(spriteBatch, isPlayer1);
            //    else
            //        foreach (Mugic card in hand)
            //            spriteBatch.Draw(cardBack, card.CollisionRectangle, null, Color.White, 0f, Vector2.Zero,
            //                SpriteEffects.FlipVertically, 0f);
            //}
            foreach (Mugic card in hand)
                card.Draw(spriteBatch, isPlayer1);

            if (currentCard.HasValue)
            {
                string canCast = "Can Cast";
                Vector2 posText;
                if (currentCard.Value / 3 == 0)
                {
                    posText = hand[currentCard.Value].Position +
                        new Vector2(ChaoticEngine.kCardWidth / 2 - font.MeasureString(canCast).X / 2,
                            -font.MeasureString(canCast).Y);
                }
                else
                {
                    posText = hand[currentCard.Value].Position + new Vector2(0, ChaoticEngine.kCardHeight) +
                        new Vector2(ChaoticEngine.kCardWidth / 2 - font.MeasureString(canCast).X / 2, 
                            -font.MeasureString(canCast).Y / 4);
                }
                spriteBatch.DrawString(font, canCast, posText, Color.Blue, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }
        }
    }
}