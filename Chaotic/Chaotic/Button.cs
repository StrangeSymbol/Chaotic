/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Chaotic
{
    class Button : ChaoticGameLib.Sprite
    {
        #region Fields
        double elapsedTime;
        const float ButtonWait = 100f;
        SoundEffect buttonEffect;
        #endregion
        #region Properties
        public bool IsClicked { get; set; }
        #endregion
        #region Constructors
        public Button(Texture2D sprite, Vector2 position, Texture2D overlay, SoundEffect buttonEffect)
            : base(sprite, position, overlay)
        {
            this.buttonEffect = buttonEffect;
        }
        #endregion
        #region Methods
        public bool UpdateButton(MouseState mouse, GameTime gameTime)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (CollisionRectangle.Contains(mouse.X, mouse.Y))
                {
                    IsClicked = true;
                    IsCovered = true;
                    elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else
                IsCovered = false;

            if (elapsedTime != 0 && gameTime.TotalGameTime.TotalMilliseconds - elapsedTime >= ButtonWait)
            {
                IsClicked = false;
                elapsedTime = 0f;
                buttonEffect.Play();
                return true;
            }
            return false;
        }
        #endregion
    }
}
