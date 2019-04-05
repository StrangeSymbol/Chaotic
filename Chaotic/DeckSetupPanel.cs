using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    class DeckSetupPanel<T> where T : ChaoticCard
    {
        List<T> panel;
        SpriteFont font;
        Texture2D texture;
        Vector2 position;
        readonly byte numCards;

        public DeckSetupPanel(Texture2D texture, Vector2 position, SpriteFont font, CreatureNumber creatureNumber)
        {
            if (creatureNumber == CreatureNumber.SixOnSix)
                throw new Exception("Wrong Creature Number.");
            if (typeof(T).Name == "Mugic" && creatureNumber == CreatureNumber.ThreeOnThree)
                numCards = 3;
            else if (typeof(T).Name == "Mugic" && creatureNumber == CreatureNumber.OneOnOne)
                numCards = 1;
            else if (typeof(T).Name == "Location")
                numCards = 5;
            else if (typeof(T).Name == "Attack")
                numCards = 10;
            this.texture = texture;
            this.position = position;
            this.font = font;
            panel = new List<T>();
        }

        public void AddToPanel(T card)
        {
            if (panel.Count < 5)
                card.Position = new Vector2(position.X + panel.Count * ChaoticEngine.kCardWidth, position.Y);
            else
                card.Position = new Vector2(position.X + panel.Count * ChaoticEngine.kCardWidth, position.Y + ChaoticEngine.kCardHeight);
            panel.Add(card);
        }

        public void UpdatePanel(GameTime gameTime, DeckSetupSelecter<T> selecter)
        {
            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < panel.Count; i++)
            {
                if (mouse.RightButton == ButtonState.Pressed && panel[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                {
                    T card = panel[i];
                    panel.RemoveAt(i);
                    selecter.AddToSelecter(card);
                }
            }
            for (int i = panel.Count; i < numCards; i++)
            {
                if (mouse.LeftButton == ButtonState.Pressed && new Rectangle((int)(position.X + i * ChaoticEngine.kCardWidth),
                    (numCards < 5) ? (int)position.X : (int)(position.Y + ChaoticEngine.kCardHeight),
                    ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight).Contains(mouse.X, mouse.Y))
                {
                    panel.Add(selecter.GetSelectedCard());
                    selecter.RemoveSelectedCard();
                }
            }
        }

        public void DrawPanel(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, typeof(T).Name, 
                new Vector2(this.position.X, this.position.Y - 3 * font.MeasureString(typeof(T).Name).Y / 2), Color.Black);
            foreach (T card in panel)
                card.Draw(spriteBatch, true);
            for (int i = panel.Count; i < numCards; i++)
                spriteBatch.Draw(texture, new Rectangle((int)(position.X + i * ChaoticEngine.kCardWidth),
                    (numCards < 5) ? (int)position.X : (int)(position.Y + ChaoticEngine.kCardHeight),
                    ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight), null, Color.White);
        }
    }
}
