/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    class ActiveLocation
    {
        Location location;
        Texture2D cardBack;
        Vector2 position;
        MovingTemplate locationTemp;
        double elapsedTime;

        public ActiveLocation(Texture2D cardBack, Vector2 position)
        {
            locationTemp = null;
            this.cardBack = cardBack;
            this.position = position;
            location = null;
        }

        public Vector2 Position { get { return position; } }
        public Location LocationActive { get { return location; } }

        public void SetLocation(Location location)
        {
            this.location = location;
            this.location.Position = this.position;
            ChaoticEngine.BackgroundSprite = this.location.Background;
            locationTemp = new MovingTemplate(location.Texture, position, null, true);
        }

        public void UpdateActiveLocation(MouseState mouse, CardDescription description)
        {
            if (locationTemp != null && locationTemp.CollisionRectangle.Contains(mouse.X, mouse.Y))
            {
                description.CoveredCard = location.Texture;
                description.Description = location.Description();
            }
        }

        public bool ReturnLocationToDeck(GameTime gameTime, LocationDeck deck)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                locationTemp.IsMoving = true;
                locationTemp.CourseToCard(deck.DeckPosition);
                elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (locationTemp.Time >= gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && locationTemp.IsMoving)
                locationTemp.Move(gameTime, position, deck.DeckPosition);
            else if (locationTemp.Time < gameTime.TotalGameTime.TotalMilliseconds - elapsedTime && locationTemp.IsMoving)
            {
                deck.Deck.Insert(0, location);
                location = null;
                locationTemp = null;
                ChaoticEngine.IsACardMoving = false;
                ChaoticEngine.BackgroundSprite = ChaoticEngine.OrgBackgroundSprite;
                elapsedTime = 0.0;
                return true;
            }
            return false;
        }

        public void DrawActiveLocation(SpriteBatch spriteBatch, bool isPlayer1)
        {
            if (locationTemp != null)
            {
                if (locationTemp.IsMoving && isPlayer1)
                    locationTemp.DrawTemplate(spriteBatch, cardBack, isPlayer1, 0.9f);
                else
                    locationTemp.DrawTemplate(spriteBatch, isPlayer1, 0.9f);
            }
        }
    }
}
