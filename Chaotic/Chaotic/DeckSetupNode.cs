using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    class DeckSetupNode
    {
        Texture2D creatureSpaceSprite;
        Texture2D overlay;
        SpriteFont font;
        Vector2 position;
        Creature creature;
        int numMoves = 1;
        double elaspedTime;
        int selectedIndex = -1;
        bool mouseCoveredCreature;
        bool mouseCoveredBattlegear;
        bool isMovableSpace;

        public DeckSetupNode(Texture2D creatureSpaceSprite, Texture2D overlay, SpriteFont font, Vector2 position)
        {
            this.creatureSpaceSprite = creatureSpaceSprite;
            this.overlay = overlay;
            this.font = font;
            this.position = position;
            creature = null;
        }

        public Vector2 Position { get { return position; } }
        protected Rectangle CreatureRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, ChaoticEngine.kCardWidth,
                    ChaoticEngine.kCardHeight);
            }
        }
        protected Rectangle BattlegearRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y - ChaoticEngine.kCardHeight / 6,
                        ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight / 6);
            }
        }
        public bool MouseCoveredCreature { get { return mouseCoveredCreature; } }
        public bool MouseCoveredBattlegear { get { return mouseCoveredBattlegear; } }
        public bool IsMovableSpace { get { return isMovableSpace; } set { isMovableSpace = value; } }

        public bool HasBattegear()
        {
            return creature.Battlegear != null;
        }

        public void UpdateBattleBoardNode(MouseState mouse, GameTime gameTime)
        {
            if (CreatureRectangle.Contains(mouse.X, mouse.Y) && !mouseCoveredCreature)
                mouseCoveredCreature = true;
            else if (!CreatureRectangle.Contains(mouse.X, mouse.Y) && mouseCoveredCreature)
                mouseCoveredCreature = false;
            if (BattlegearRectangle.Contains(mouse.X, mouse.Y) && !mouseCoveredBattlegear)
                mouseCoveredBattlegear = true;
            else if (!BattlegearRectangle.Contains(mouse.X, mouse.Y) && mouseCoveredBattlegear)
                mouseCoveredBattlegear = false;
        }

        public bool UpdateMovableSpaces(MouseState mouse, GameTime gameTime, BattleBoardNode[] creatureSpaces)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].isMovableSpace && mouse.LeftButton == ButtonState.Pressed
                    && creatureSpaces[i].CreatureRectangle.Contains(mouse.X, mouse.Y))
                {
                    for (int j = 0; j < creatureSpaces.Length; j++)
                        creatureSpaces[j].isMovableSpace = false;
                    elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                    creature.IsMoving = true;
                    creature.CourseToCard(creatureSpaces[i].Position);
                    selectedIndex = i;
                }
            }
            if (creature.Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                creature.Move(gameTime);
                if (creature.Battlegear != null)
                    creature.Battlegear.Position = new Vector2(creature.Position.X, creature.Position.Y -ChaoticEngine.kBattlegearGap);
            }
            else if (creature.Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                creature.IsMoving = false;
                elaspedTime = 0.0;
                creatureSpaces[selectedIndex].AddCreature(creature);
                selectedIndex = -1;
                creature = null;
                return true;
            }
            return false;
        }

        public void AddCreature(Creature c)
        {
            creature = c;
            creature.Position = position;
            if (creature.Battlegear != null)
                creature.Battlegear.Position = new Vector2(position.X, position.Y - ChaoticEngine.kBattlegearGap);
        }

        public void RemoveCreature()
        {
            creature = null;
        }

        public void RemoveBattlegear()
        {
            creature.UnEquip();
        }

        public void GetCardCoveredByMouse(MouseState mouse)
        {
            if (creature != null && creature.Battlegear != null && BattlegearRectangle.Contains(mouse.X, mouse.Y))
                ChaoticEngine.CoveredCard = creature.Battlegear.Texture;
            else if (creature != null && CreatureRectangle.Contains(mouse.X, mouse.Y))
                ChaoticEngine.CoveredCard = creature.Texture;
        }

        public void DrawBattleBoardNode(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(creatureSpaceSprite, CreatureRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
            if (creature != null)
            {
                spriteBatch.Draw(creature.Texture, new Rectangle((int)creature.Position.X, (int)creature.Position.Y,
                        ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);
                if (creature.Battlegear != null)
                {
                    spriteBatch.Draw(creature.Battlegear.Texture,
                        new Rectangle((int)creature.Battlegear.Position.X, (int)creature.Battlegear.Position.Y,
                            ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight),
                        null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                    if (mouseCoveredBattlegear)
                    {
                       
                    }
                }
                if (mouseCoveredCreature)
                {
                    
                }
            }
            if (isMovableSpace)
            {
                spriteBatch.Draw(overlay, CreatureRectangle,
                    null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            }
        }
    }
}