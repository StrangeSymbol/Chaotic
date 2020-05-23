using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Chaotic
{
    class CodedManager
    {
        const byte numCharsLower = 20;
        const byte numCharsUpper = 30;

        List<CodedCharacter> codedCharacters;

        SpriteFont font;

        public CodedManager(ContentManager content)
        {
            codedCharacters = new List<CodedCharacter>();
            font = content.Load<SpriteFont>(@"Fonts\CodedFont");
        }

        public void InitializeCodedLetters(Vector2 center)
        {
            Random random = new Random();
            int numChars = random.Next(numCharsLower, numCharsUpper);
            for (int i = 0; i < numChars; i++)
                codedCharacters.Add(new CodedCharacter(center, random, font));
        }

        public void UpdateCodedLetters(GameTime gameTime)
        {
            foreach (CodedCharacter letter in codedCharacters)
                letter.UpdateLetter(gameTime);

            for (int i = 0; i < codedCharacters.Count; i++)
            {
                if (!codedCharacters[i].Alive)
                {
                    codedCharacters.RemoveAt(i);
                    i--;
                }
            }
        }

        public void DrawCodedLetters(SpriteBatch spriteBatch)
        {
            foreach (CodedCharacter letter in codedCharacters)
            {
                letter.DrawLetter(spriteBatch);
            }
        }
    }
}
