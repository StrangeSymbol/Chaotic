using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    class DeckSetupSelecter<T> where T : ChaoticCard
    {
        List<T> container;
        Button lButton;
        Button rButton;
        Vector2 position;
        SpriteFont font;

        int selectedIndex;
        
        public DeckSetupSelecter(List<string> names, Texture2D lButton, Texture2D lButtonCover,
            Texture2D rButton, Texture2D rButtonCover, Vector2 position, SpriteFont font)
        {
            this.lButton = new Button(lButton, new Vector2(position.X - lButton.Width, 
                position.Y + ChaoticEngine.kCardHeight / 2 - lButton.Height / 2), lButtonCover);
            this.rButton = new Button(rButton, new Vector2(position.X + ChaoticEngine.kCardWidth,
                this.lButton.Position.Y), rButtonCover);
            this.position = position;
            foreach (string name in names)
            {
                container.Add((ChaoticEngine.sCardDatabase.Find(c => c.Name == name) as T));
            }
            this.font = font;
            this.selectedIndex = 0;
        }

        public void AddToSelecter(T card)
        {
            container.Insert(selectedIndex, card);
        }

        public T GetSelectedCard()
        {
            if (selectedIndex != -1)
                return container[selectedIndex];
            else
                throw new IndexOutOfRangeException("No cards are in selecter.");
        }

        public void RemoveSelectedCard()
        {
            if (selectedIndex != -1)
                container.RemoveAt(selectedIndex);
            else
                throw new IndexOutOfRangeException("No cards are in selecter.");
            if (selectedIndex > 0)
                selectedIndex--;
            else if (selectedIndex < container.Count - 1)
                selectedIndex++;
            else
                selectedIndex = -1;
        }

        public void UpdateDeckSetupSelecter(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            if (lButton.UpdateButton(mouse, gameTime) && selectedIndex != 0)
                selectedIndex--;
            if (rButton.UpdateButton(mouse, gameTime) && selectedIndex != container.Count - 1)
                selectedIndex++;
        }

        public void DrawDeckSetupSelecter(SpriteBatch spriteBatch)
        {
            if (selectedIndex != -1)
            {
                if (selectedIndex != 0)
                    lButton.Draw(spriteBatch);
                if (selectedIndex != container.Count - 1)
                    rButton.Draw(spriteBatch);
            }
            string type = typeof(T).Name;
            spriteBatch.DrawString(font, type,
                new Vector2(position.X + ChaoticEngine.kCardWidth / 2 - font.MeasureString(type).X / 2,
                    position.Y - font.MeasureString(type).Y), Color.Black);
            if (selectedIndex != -1)
                container[selectedIndex].Draw(spriteBatch, true);
        }
    }
}