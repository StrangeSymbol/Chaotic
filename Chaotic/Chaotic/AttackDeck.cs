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

        public AttackDeck(Texture2D deck, Texture2D cardBack, Texture2D overlay, Vector2 position, bool isPlayer1)
        {
            deckPile = new List<Attack>();
            attackTemplate = new CardTemplate(deck, position);
            deckCover1 = new CardTemplate(cardBack, position);
            deckCover2 = new MovingTemplate(cardBack, position, overlay);
            elapsedTime = 0.0;
            this.isPlayer1 = isPlayer1;
            this.start = false;
        }

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

        public void ShuffleDeck(List<Attack> deckHolder)
        {
            Random genDeck = new Random();
            bool[] check52 = new bool[deckHolder.Count];
            for (int count = 0; count < deckHolder.Count; count++)
            {
                bool hasCard = false;
                int deckInt = 0;
                while (!hasCard)
                {
                    deckInt = genDeck.Next(deckHolder.Count);
       
                    if (!check52[deckInt])
                        hasCard = true;
                }
                check52[deckInt] = true;
                deckPile.Add(deckHolder[deckInt].ShallowCopy() as Attack);
            }
        }

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