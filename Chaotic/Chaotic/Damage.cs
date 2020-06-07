using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    class Damage
    {
        const float speedLower = 20f; // 1 pixel per sec.
        const float speedUpper = 40f; // 5 pixels per sec. 

        const byte survivalTimeLower = 1;
        const byte survivalTimeUpper = 5;

        short damageAmount;
        Vector2 position;
        Vector2 direction;
        float speed;
        double time;
        double survivalTime;
        SpriteFont font;

        public bool Alive { get { return time < survivalTime; } }

        public Damage(short damageAmount, Vector2 position, SpriteFont font)
        {
            this.time = 0f;
            this.damageAmount = damageAmount;
            this.position = position;
            this.font = font;
            Random random = new Random();
            this.position -= new Vector2(font.MeasureString(damageAmount.ToString()).X / 2,
                font.MeasureString(damageAmount.ToString()).Y / 2);
            survivalTime = (survivalTimeUpper - survivalTimeLower) * random.NextDouble() + survivalTimeLower;
            speed = (speedUpper - speedLower) * (float)random.NextDouble() + speedLower;
            direction = new Vector2(2 * (float)random.NextDouble() - 1, -(float)random.NextDouble());
            direction.Normalize();
        }

        public void UpdateDamage(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            time += elapsedTime;
            position += direction * (float)(speed * elapsedTime);
        }

        public void DrawDamage(SpriteBatch spriteBatch)
        {
            Color colour;

            if (damageAmount < 0)
                colour = Color.Red;
            else
                colour = Color.Blue;

            spriteBatch.DrawString(font, damageAmount.ToString(), position, colour, 0f,
                new Vector2(font.MeasureString(damageAmount.ToString()).X / 2, 
                font.MeasureString(damageAmount.ToString()).Y / 2), 1f, SpriteEffects.None, 0.11f);
        }
    }
}
