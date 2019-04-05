using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    class CardTemplate
    {
        protected Texture2D texture;
        protected Vector2 position;
        // The collision area.
        Rectangle collisionRectangle;
        protected bool isLocation;

        public CardTemplate(Texture2D sprite, Vector2 position, bool isLocation=false)
        {
            this.texture = sprite;
            this.position = position;
            collisionRectangle = new Rectangle((int)position.X, (int)position.Y, ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight);
            this.isLocation = isLocation;
            if (isLocation)
                collisionRectangle = new Rectangle((int)position.X, (int)position.Y, ChaoticEngine.kCardHeight, ChaoticEngine.kCardWidth);
        }

        public Texture2D Texture { get { return texture; } set { this.texture = value; } }
        public Vector2 Position { get { return position; } set { this.position = value; } }
        public virtual Rectangle CollisionRectangle { get { return collisionRectangle; } }

        public virtual void DrawTemplate(SpriteBatch spriteBatch, bool isPlayer1, float layerDepth=0.95f)
        {
            if (isPlayer1)
                spriteBatch.Draw(this.texture, collisionRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
            else
                spriteBatch.Draw(this.texture, collisionRectangle, null, Color.White, 0f,
                    Vector2.Zero, SpriteEffects.FlipVertically, layerDepth);
        }
    }
}