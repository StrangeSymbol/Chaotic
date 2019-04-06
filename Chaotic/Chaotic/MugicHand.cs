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
        bool isPlayer1;
        bool isCardCovered;
        double elaspedTime;
        int selectedCard = -1;

        public MugicHand(bool isPlayer1, Texture2D cardBack, Vector2 discardTemplate)
        {
            this.isPlayer1 = isPlayer1;
            this.cardBack = cardBack;
            hand = new List<Mugic>();
            elaspedTime = 0.0;
            this.discardTemplate = discardTemplate;
        }

        public int Count { get { return hand.Count; } }
        public bool IsPlayer1 { get { return isPlayer1; } }

        public void UpdateCoveredCard(MouseState mouse)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                    ChaoticEngine.CoveredCard = hand[i].Texture;
            }
        }

        public bool UpdateHand(GameTime gameTime, MouseState mouse, DiscardPile<ChaoticCard> discardPile, bool selectCovered=false)
        {
            for (int i = hand.Count - 1; i >= 0; i--)
            {
                if (!ChaoticEngine.IsACardMoving && !selectCovered)
                {
                    if (mouse.LeftButton == ButtonState.Pressed &&
                    hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                    {
                        if (!isCardCovered)
                        {
                            hand[i].IsCovered = true;
                            isCardCovered = true;
                            selectedCard = i;
                        }
                        else if (isCardCovered && i != selectedCard)
                        {
                            hand[i].IsCovered = true;
                            hand[selectedCard].IsCovered = false;
                            selectedCard = i;
                        }
                    }
                    else if (mouse.RightButton == ButtonState.Pressed &&
                        hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y) && hand[i].IsCovered)
                    {
                        ChaoticEngine.IsACardMoving = true;
                        hand[i].IsCovered = false;
                        hand[i].IsMoving = true;
                        hand[i].CourseToCard(discardPile.GetDiscardTemplate().Position);
                        elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                        isCardCovered = false;
                        selectedCard = -1;
                    }
                }
                else if (!ChaoticEngine.IsACardMoving && selectCovered)
                {
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y) && hand[i].IsCovered)
                    {
                        ChaoticEngine.IsACardMoving = true;
                        hand[i].IsCovered = false;
                        hand[i].IsMoving = true;
                        hand[i].CourseToCard(discardPile.GetDiscardTemplate().Position);
                        elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else if (hand[i].Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                    hand[i].Move(gameTime);
                else if (hand[i].Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
                {
                    discardPile.DiscardList.Add(hand[i].Copy(discardPile.GetDiscardTemplate().Position));
                    ChaoticEngine.IsACardMoving = false;
                    hand[i].IsMoving = false;
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

        public void TurnOffCover()
        {
            if (selectedCard != -1)
            {
                hand[selectedCard].IsCovered = false;
                selectedCard = -1;
                isCardCovered = false;
            }
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
        }
    }
}
