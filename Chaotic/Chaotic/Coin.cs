/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Chaotic
{
    class Coin
    {
        // Holds the heads of the coin.
        Texture2D heads;

        // Holds the tails of the coin.
        Texture2D tails;

        // The position the coin is on screen.
        Vector2 position;

        // The initial position.
        Vector2 initPosition;

        // The direction for coin to move in.
        Vector2 direction;

        // The amount of time till movement is finished.
        double time;

        // The amount of time elapsed.
        double elapsedTime;

        // The amount of time till coin flip is finished.
        double flipTime;

        // The amount of time elapsed during flipping.
        double flipElapsedTime;

        // How fast the coin moves.
        const float speed = 600f;

        // How fast is the flipping.
        const float flipSpeed = 500f;

        // Number of flips.
        const int numFlips = 10;

        // Whether the coin is moving.
        bool isMoving;

        // Whether the coin is flipping.
        bool isFlipping;

        // whether it reached the appex.
        bool reachedAppex;

        // A flag for whether or not we are done with coin and what result is.
        bool? flippedSide;

        // The percent of completion of the transition.
        private int percent;

        // Determines whether the coin transition starts heads and ends tails.
        private bool isFromHeadsToTails = true;

        // Determines whether the coin is heads.
        private bool isHeads;

        // Controls what is visibly seen in the transition.
        Rectangle destination;

        private readonly int width;
        private readonly int height;

        private SpriteFont font;

        private SoundEffect coinEffect;

        public Coin(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.heads = content.Load<Texture2D>(@"Coin/OverWorldHeads");
            this.tails = content.Load<Texture2D>(@"Coin/OverWorldTails");
            this.font = content.Load<SpriteFont>(@"Fonts/HUB");
            width = heads.Width;
            height = heads.Height;
            this.position = this.initPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - width / 2,
                graphics.PreferredBackBufferHeight / 2 - height / 2);
            elapsedTime = 0.0;
            flipElapsedTime = 0.0;
            flippedSide = null;
            this.coinEffect = content.Load<SoundEffect>(@"Audio\CoinFlip");
        }

        private Rectangle collisionRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    width, height);
            }
        }

        private void courseToLocation(Vector2 p2)
        {
            Vector2 p1 = new Vector2(this.position.X + width / 2, this.position.Y + height / 2);
            direction = Vector2.Normalize(new Vector2(p2.X + width / 2 - p1.X,
                p2.Y + height / 2 - p1.Y));
            double distance = Vector2.Distance(p1, new Vector2(p2.X + width / 2, p2.Y + height / 2));
            time = distance / speed * 1000;
        }

        private double distanceToDestination(Vector2 position, Vector2 p2)
        {
            Vector2 p1 = new Vector2(position.X + width / 2, position.Y + height / 2);
            return Vector2.Distance(p1, new Vector2(p2.X + width / 2, p2.Y + height / 2));
        }

        private void move(GameTime gameTime)
        {
            position += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds) * direction;
        }

        private void flipCoin(GameTime gameTime)
        {
            // Calculate the completion percent of the animation
            percent = (int)((gameTime.TotalGameTime.TotalMilliseconds - flipElapsedTime) / flipTime * 100);

            if (percent >= 100)
            {
                percent = 0;
            }

            int currentPercent;
            if (percent < 50)
            {
                // On the first half of the animation the component is
                // on its initial size
                currentPercent = percent;
                isHeads = isFromHeadsToTails;
            }
            else
            {
                // On the second half of the animation the component
                // is flipped
                currentPercent = 100 - percent;
                isHeads = !isFromHeadsToTails;
            }
            // Shrink and widen the component to look like it is flipping
            destination =
                new Rectangle((int)this.position.X, (int)(this.position.Y + height * currentPercent / 100),
                    width, (int)(height - height * currentPercent / 100 * 2));
        }

        private void calcFlipTime(Vector2 p2)
        {
            float distance = p2.Length();
            flipTime = (distance / flipSpeed * 1000) / numFlips;
        }

        private void updateCoinFlipping(GameTime gameTime)
        {
            if (isMoving && flipElapsedTime == 0.0)
            {
                isFlipping = true;
                calcFlipTime(Vector2.UnitY * 4 * height);
                flipElapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (flipTime >= gameTime.TotalGameTime.TotalMilliseconds - flipElapsedTime && isFlipping)
                flipCoin(gameTime);
            else if (flipTime < gameTime.TotalGameTime.TotalMilliseconds - flipElapsedTime && isFlipping)
            {
                destination = collisionRectangle;
                isFlipping = false;
                flipElapsedTime = 0.0;
                isFromHeadsToTails = !isFromHeadsToTails;
            }
        }

        private bool updateCoinMove(GameTime gameTime, KeyboardState keyboard)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    ChaoticEngine.IsACardMoving = true;
                    isMoving = true;
                    courseToLocation(initPosition - Vector2.UnitY * 4 * height);
                    elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                    coinEffect.Play();
                }
            }
            else if (time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && isMoving)
                move(gameTime);
            else if (time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && isMoving && !reachedAppex)
            {
                position = initPosition - Vector2.UnitY * 4 * height;
                courseToLocation(initPosition);
                elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                reachedAppex = true;
            }
            else if (time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && isMoving && reachedAppex)
            {
                position = initPosition;
                ChaoticEngine.IsACardMoving = false;
                isMoving = false;
                reachedAppex = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Controls the whole process of coin flipping to determine who goes first.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        /// <returns>True if heads, False if tails, and null if the coin flip isn't finished.</returns>
        public bool? UpdateCoin(GameTime gameTime, KeyboardState keyboard)
        {
            updateCoinFlipping(gameTime);

            if (flippedSide == null && updateCoinMove(gameTime, keyboard))
            {
                Random rnd = new Random();
                int flipResult = rnd.Next(100);
                if (flipResult >= 50)
                    flippedSide = true;
                else
                    flippedSide = false;
            }
            else if (flippedSide.HasValue && keyboard.IsKeyDown(Keys.Enter))
            {
                bool flip = flippedSide.Value;
                flippedSide = null;
                return flip;
            }
            return null;
        }

        public void DrawCoin(SpriteBatch spriteBatch, float layerDepth = 0.45f)
        {
            if (isMoving)
            {
                if (isHeads)
                    spriteBatch.Draw(heads, destination, null, Color.White, 0f,
                        Vector2.Zero, SpriteEffects.None, layerDepth);
                else
                    spriteBatch.Draw(tails, destination, null, Color.White, 0f,
                        Vector2.Zero, SpriteEffects.None, layerDepth);
            }
            else
            {
                if (flippedSide == null || flippedSide.Value)
                    spriteBatch.Draw(heads, collisionRectangle, null, Color.White,
                        0f, Vector2.Zero, SpriteEffects.None, layerDepth);
                else
                    spriteBatch.Draw(tails, collisionRectangle, null, Color.White, 0f,
                        Vector2.Zero, SpriteEffects.None, layerDepth);

                if (flippedSide.HasValue)
                {
                    string instructions = (flippedSide.Value ? "Player 1" : "Player 2") + 
                        " is the Active player.\n" + "Press Enter To Start Game.";
                    spriteBatch.DrawString(font, instructions,
                        new Vector2(position.X + width / 2 - font.MeasureString(instructions).X / 2, position.Y + height),
                        Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                }
                else
                {
                    string instructions = "Press Enter To Flip Coin.\n" + "Heads - Player 1; Tails - Player 2";
                    spriteBatch.DrawString(font, instructions, 
                        new Vector2(position.X + width / 2 - font.MeasureString(instructions).X / 2, position.Y + height), 
                        Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                }
            }
        }
    }
}