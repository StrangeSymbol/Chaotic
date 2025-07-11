﻿/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using ChaoticGameLib;

namespace Chaotic
{
    class AttackHand : IEnumerable<Attack>
    {
        List<Attack> hand;
        Texture2D cardBack;
        Texture2D damageCover;
        Texture2D healCover;
        Vector2 cardInHandPos;
        SpriteFont spriteFont;
        readonly Vector2 attackTemplate;
        bool isPlayer1;
        double elaspedTime;
        SoundEffect setEffect;

        public AttackHand(bool isPlayer1, Texture2D cardBack, Vector2 attackTemplate, Texture2D damage,
            Texture2D heal, SpriteFont spriteFont, SoundEffect setEffect)
        {
            this.isPlayer1 = isPlayer1;
            this.cardBack = cardBack;
            this.damageCover = damage;
            this.healCover = heal;
            hand = new List<Attack>();
            elaspedTime = 0.0;
            this.attackTemplate = attackTemplate;
            this.spriteFont = spriteFont;
            this.setEffect = setEffect;
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

        //public bool UpdateHand(GameTime gameTime, MouseState mouse, DiscardPile<Attack> discardPile)
        //{
        //    for (int i = hand.Count - 1; i >= 0; i--)
        //    {
        //        if (!ChaoticEngine.IsACardMoving)
        //        {
        //            if (mouse.LeftButton == ButtonState.Pressed &&
        //            hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
        //            {
        //                if (!isCardCovered)
        //                {
        //                    hand[i].IsCovered = true;
        //                    isCardCovered = true;
        //                    selectedCard = i;
        //                }
        //                else if (isCardCovered && i != selectedCard)
        //                {
        //                    hand[i].IsCovered = true;
        //                    hand[selectedCard].IsCovered = false;
        //                    selectedCard = i;
        //                }
        //            }
        //            else if (mouse.RightButton == ButtonState.Pressed &&
        //                hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y) && hand[i].IsCovered)
        //            {
        //                ChaoticEngine.IsACardMoving = true;
        //                hand[i].IsCovered = false;
        //                hand[i].IsMoving = true;
        //                hand[i].CourseToCard(discardPile.GetDiscardTemplate().Position);
        //                elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
        //                isCardCovered = false;
        //                selectedCard = -1;
        //            }
        //        }
        //        else if (hand[i].Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
        //            hand[i].Move(gameTime);
        //        else if (hand[i].Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && hand[i].IsMoving)
        //        {
        //            hand[i].IsMoving = false;
        //            discardPile.DiscardList.Add(hand[i].Copy(discardPile.GetDiscardTemplate().Position) as Attack);
        //            ChaoticEngine.IsACardMoving = false;
        //            elaspedTime = 0.0;
        //            RemoveCardFromHand(i);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public bool UpdateHand(GameTime gameTime, MouseState mouse, DiscardPile<Attack> discardPile)
        {
            for (int i = hand.Count - 1; i >= 0; i--)
            {
                if (!ChaoticEngine.IsACardMoving)
                {
                    if (mouse.LeftButton == ButtonState.Pressed &&
                    hand[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
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
                    discardPile.DiscardList.Add(hand[i].Copy(discardPile.GetDiscardTemplate().Position) as Attack);
                    ChaoticEngine.IsACardMoving = false;
                    elaspedTime = 0.0;
                    RemoveCardFromHand(i);
                    setEffect.Play();
                    return true;
                }
            }
            return false;
        }

        public bool UpdateHand(GameTime gameTime, int i, DiscardPile<Attack> discardPile)
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
                setEffect.Play();
                return true;
            }
            return false;
        }

        public Attack this[int i]
        {
            get { return this.hand[i]; }
        }

        public void AddCardToHand(Attack card)
        {
            hand.Add(card.Copy(GetNextHandPosition()) as Attack);
        }

        public void RemoveCardFromHand(int index)
        {
            List<Attack> cards;
            if (index != hand.Count - 1)
            {
                cards = hand.GetRange(index + 1, hand.Count - 1 - index);
                hand.RemoveRange(index, hand.Count - index);
                foreach (Attack card in cards)
                    AddCardToHand(card);
            }
            else if (index == hand.Count - 1)
                hand.RemoveAt(index);
        }

        public Vector2 GetNextHandPosition()
        {
            if (hand.Count == 0 && isPlayer1)
                return new Vector2(attackTemplate.X + ChaoticEngine.kCardWidth,
                    attackTemplate.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap);
            else if (hand.Count == 0 && !isPlayer1)
                return new Vector2(attackTemplate.X - ChaoticEngine.kCardWidth,
                    attackTemplate.Y + ChaoticEngine.kCardHeight + ChaoticEngine.kCardGap + ChaoticEngine.kBattlegearGap);
            else if (hand.Count < 3 && isPlayer1)
                return new Vector2(hand[hand.Count - 1].Position.X + ChaoticEngine.kCardWidth,
                    hand[hand.Count - 1].Position.Y);
            else if (hand.Count < 3 && !isPlayer1)
                return new Vector2(hand[hand.Count - 1].Position.X - ChaoticEngine.kCardWidth,
                    hand[hand.Count - 1].Position.Y);
            else
                throw new IndexOutOfRangeException("The Attack Hand can only have up to 3 cards in hand!");

        }

        //public void TurnOffCover()
        //{
        //    if (selectedCard != -1)
        //    {
        //        hand[selectedCard].IsCovered = false;
        //        selectedCard = -1;
        //        isCardCovered = false;
        //    }
        //}

        public void EmptyHand()
        {
            hand.Clear();
        }

        IEnumerator<Attack> IEnumerable<Attack>.GetEnumerator()
        {
            foreach (Attack sprite in hand)
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
            //        foreach (Attack card in hand)
            //            card.Draw(spriteBatch, isPlayer1, 0.7f, 0.75f);
            //    else
            //        foreach (Attack card in hand)
            //            spriteBatch.Draw(cardBack, card.CollisionRectangle, null, Color.White, 0f, Vector2.Zero,
            //                SpriteEffects.FlipVertically, 0.7f);
            //}
            //else
            //{
            //    if (!ChaoticEngine.Player1First)
            //        foreach (Attack card in hand)
            //            card.Draw(spriteBatch, isPlayer1, 0.7f, 0.75f);
            //    else
            //        foreach (Attack card in hand)
            //            spriteBatch.Draw(cardBack, card.CollisionRectangle, null, Color.White, 0f, Vector2.Zero,
            //                SpriteEffects.FlipVertically, 0.7f);
            //}
            foreach (Attack card in hand)
                card.Draw(spriteBatch, isPlayer1, 0.7f, 0.75f);
        }
        public void ProjectHandDamage(SpriteBatch spriteBatch, Creature creature1, Creature creature2, Location location)
        {
            foreach (Attack atk in hand)
            {
                Tuple<short, short> damage = atk.PredictedDamage(creature1, creature2, location);
                short damage1 = damage.Item1;
                short damage2 = damage.Item2;
                damage1 -= creature1.Recklessness;
                string msg1 = damage1.ToString();
                spriteBatch.Draw(healCover, new Vector2(atk.Position.X + ChaoticEngine.kCardWidth / 4 - healCover.Width / 2,
                    atk.Position.Y + 3 * ChaoticEngine.kCardHeight / 4 - healCover.Height / 4), null,
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.65f);
                spriteBatch.DrawString(spriteFont, msg1, new Vector2(atk.Position.X + ChaoticEngine.kCardWidth / 4
                    - spriteFont.MeasureString(msg1).X / 2,
                    atk.Position.Y + 3 * ChaoticEngine.kCardHeight / 4 - healCover.Height / 4),
                    Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);
                if (creature1.FirstAttack && creature1.Invisibility() && ChaoticEngine.HasMarquisDarini)
                    damage2 -= 5;
                string msg2 = Math.Abs(damage2).ToString();
                spriteBatch.Draw(damageCover, new Vector2(atk.Position.X + 3 * ChaoticEngine.kCardWidth / 4 
                    - damageCover.Width / 2, atk.Position.Y + 3 * ChaoticEngine.kCardHeight / 4 - damageCover.Height / 4), null,
                    Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.65f);
                spriteBatch.DrawString(spriteFont, msg2, new Vector2(atk.Position.X + 3 * ChaoticEngine.kCardWidth / 4 
                    - spriteFont.MeasureString(msg2).X / 2,
                    atk.Position.Y + 3 * ChaoticEngine.kCardHeight / 4 - damageCover.Height / 4),
                    Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);
            }
        }
    }
}
