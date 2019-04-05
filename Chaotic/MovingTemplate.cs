using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    class MovingTemplate : CardTemplate
    {
        // The cover over the selected card.
        Texture2D overlay;

        // The direction for card to move in.
        Vector2 direction;

        // The amount of time till movement is finished.
        double time;

        // How fast the card moves.
        const float speed = 500f;

        // The percent of completion of the transition.
        private int percent;

        // Determines whether the card transition starts face down and ends face up.
        private bool isFromFaceDownToFaceUp = true;

        // Determines whether the card is face down.
        private bool isFaceDown;

        // Controls what is visibly seen in the transition.
        Rectangle destination;

        private readonly int width;
        private readonly int height;

        public MovingTemplate(Texture2D texture, Vector2 position, Texture2D overlay, bool isLocation=false)
            : base(texture, position, isLocation)
        {
            this.overlay = overlay;
            if (isLocation)
            {
                width = ChaoticEngine.kCardHeight;
                height = ChaoticEngine.kCardWidth;
            }
            else
            {
                width = ChaoticEngine.kCardWidth;
                height = ChaoticEngine.kCardHeight;
            }
        }

        public bool IsFromFaceDownToFaceUp { get { return IsFromFaceDownToFaceUp; } set { isFromFaceDownToFaceUp = value; } }
        public Rectangle Destination { set { destination = value; } }
        public double Time { get { return time; } }
        public bool IsMoving { get; set; }
        public bool IsCovered { get; set; }
        public bool GotToDestination { get; set; }
        public override Rectangle CollisionRectangle
        {
            get
            {
                if (isLocation)
                    return new Rectangle((int)position.X, (int)position.Y, ChaoticEngine.kCardHeight, ChaoticEngine.kCardWidth);
                else
                    return new Rectangle((int)position.X, (int)position.Y, ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight);
            }
        }

        public void CourseToCard(Vector2 p2)
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

        public void Move(GameTime gameTime)
        {
            position += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds) * direction;
        }

        public void Move(GameTime gameTime, Vector2 initial, Vector2 final)
        {
            position += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds) * direction;
            double distanceFromInitToFinal = distanceToDestination(initial, final);
            double distanceFromPosToDest = distanceToDestination(position, final);
            // Calculate the completion percent of the animation
            percent = 100 - (int)(distanceFromPosToDest / distanceFromInitToFinal * 100);

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
                isFaceDown = isFromFaceDownToFaceUp;
            }
            else
            {
                // On the second half of the animation the component
                // is flipped
                currentPercent = 100 - percent;
                isFaceDown = !isFromFaceDownToFaceUp;
            }
            // Shrink and widen the component to look like it is flipping
            if (!isLocation)
                destination =
                new Rectangle((int)(this.position.X + width * currentPercent / 100), (int)this.position.Y,
                    (int)(width - width * currentPercent / 100 * 2), height);
            else
                destination =
                new Rectangle((int)this.position.X, (int)(this.position.Y + height * currentPercent / 100),
                    width, (int)(height - height * currentPercent / 100 * 2));
        }

        public override void DrawTemplate(SpriteBatch spriteBatch, bool isPlayer1, float layerDepth=0.85f)
        {
            SpriteEffects spriteEffect;
            if (isPlayer1)
                spriteEffect = SpriteEffects.None;
            else
                spriteEffect = SpriteEffects.FlipVertically;

            spriteBatch.Draw(this.texture, CollisionRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, layerDepth);
            if (IsCovered)
                spriteBatch.Draw(this.overlay, CollisionRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, layerDepth - 0.05f);
        }

        public virtual void DrawTemplate(SpriteBatch spriteBatch, Texture2D sprite, bool isPlayer1, float layerDepth=0.85f)
        {
            SpriteEffects spriteEffect;
            if (isPlayer1)
                spriteEffect = SpriteEffects.None;
            else
                spriteEffect = SpriteEffects.FlipVertically;

            if (isFaceDown)
                spriteBatch.Draw(texture, destination, null, Color.White, 0f, Vector2.Zero, spriteEffect, layerDepth);
            else
                spriteBatch.Draw(sprite, destination, null, Color.White, 0f, Vector2.Zero, spriteEffect, layerDepth);
        }
    }
}
