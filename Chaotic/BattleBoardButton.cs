using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chaotic
{
    public enum ActionType : byte { Move, Activate, Cast, SacrificeCreature, SacrificeBattlegear, Cancel }
    struct BattleBoardButton
    {
        #region Fields
        Texture2D sprite;
        Vector2 position;
        Texture2D overlay;
        double elapsedTime;
        const float ButtonWait = 100f;
        Rectangle collisionRectangle;
        int width;
        int height;
        bool isActive;
        bool isCovered;
        bool isClicked;
        ActionType action;
        #endregion
        #region Properties
        public bool IsClicked { get { return isClicked; } set { isClicked = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public Vector2 Position { get { return position; } 
            set { position = value; collisionRectangle = new Rectangle((int)position.X, (int)position.Y, width, height); } }
        public Texture2D Texture { get { return sprite; } }
        public bool IsCovered { get { return isCovered; } set { isCovered = value; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Rectangle CollisionRectangle { get { return collisionRectangle; } }
        public ActionType Action { get { return action; } }
        public Texture2D Overlay { get { return overlay; } }
        #endregion
        #region Constructors
        public BattleBoardButton(Texture2D sprite, Texture2D overlay, int width, int height, ActionType action)
        {
            this.sprite = sprite;
            this.position = Vector2.Zero;
            this.overlay = overlay;
            this.width = width;
            this.height = height;
            this.action = action;
            this.elapsedTime = 0.0;
            collisionRectangle = Rectangle.Empty;
            isCovered = false;
            isClicked = false;
            isActive = false;
        }
        #endregion
        #region Methods
        public bool UpdateButton(MouseState mouse, GameTime gameTime)
        {
            if (isActive)
            {
                if (mouse.LeftButton == ButtonState.Pressed && collisionRectangle.Contains(new Point(mouse.X, mouse.Y)))
                {
                    isClicked = true;
                    isCovered = true;
                    elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else
                    isCovered = false;

                if (elapsedTime != 0 && gameTime.TotalGameTime.TotalMilliseconds - elapsedTime >= ButtonWait)
                {
                    isClicked = false;
                    elapsedTime = 0f;
                    return true;
                }
            }
            return false;
        }
        public void DrawButton(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(sprite, collisionRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                if (isCovered)
                    spriteBatch.Draw(overlay, collisionRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.05f);
            }
        }
        #endregion
    }
}
