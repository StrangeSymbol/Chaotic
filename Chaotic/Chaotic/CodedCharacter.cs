using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    class CodedCharacter
    {
        const float rotSpeedLower = 0.3f; // 1/2 rotation per 3 secs.
        const float rotSpeedUpper = 3.14f; // 2 rotations a sec.

        const float speedLower = 20f; // 1 pixel per sec.
        const float speedUpper = 40f; // 5 pixels per sec. 

        const byte survivalTimeLower = 1;
        const byte survivalTimeUpper = 5;

        readonly char[] alphaNumeric = new char[36] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        char letter;
        Vector2 position;
        Vector2 direction;
        float rotation;
        float rotationSpeed;
        float speed;
        double time;
        double survivalTime;
        SpriteFont font;

        public bool Alive { get { return time < survivalTime; } }

        public CodedCharacter(Vector2 position, Random random, SpriteFont font)
        {
            this.time = 0f;
            this.rotation = 0f;
            this.position = position;
            this.font = font;
            int letterIndex = random.Next(alphaNumeric.Length);
            letter = alphaNumeric[letterIndex];
            this.position -= new Vector2(font.MeasureString(letter.ToString()).X / 2, font.MeasureString(letter.ToString()).Y / 2);
            survivalTime = (survivalTimeUpper - survivalTimeLower) * random.NextDouble() + survivalTimeLower;
            rotationSpeed = (rotSpeedUpper - rotSpeedLower) * (float)random.NextDouble() + rotSpeedLower;
            speed = (speedUpper - speedLower) * (float)random.NextDouble() + speedLower;
            direction = new Vector2(2 * (float)random.NextDouble() - 1, 2 * (float)random.NextDouble() - 1);
            direction.Normalize();
        }

        public void UpdateLetter(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            time += elapsedTime;
            position += direction * (float)(speed * elapsedTime);
            rotation += MathHelper.Clamp((float)(rotationSpeed * elapsedTime), 0f, 1f);
        }

        public void DrawLetter(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, letter.ToString(), position, Color.Black, rotation,
                new Vector2(font.MeasureString(letter.ToString()).X / 2, font.MeasureString(letter.ToString()).Y / 2),
                1f, SpriteEffects.None, 0.01f);
        }
    }
}
