﻿/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    class BattleBoardMenu
    {
        BattleBoardButton[] buttons;
        BattleBoardButton button;
        bool mousePressed;
        bool buttonIsCovered;

        public BattleBoardMenu(BattleBoardButton[] buttons, BattleBoardButton button)
        {
            this.buttons = buttons;
            this.button = button;
        }

        public bool MousePressed { get { return mousePressed; } }

        public ActionType? UpdateBattleBoardMenu(GameTime gameTime, MouseState mouse, Creature card, Vector2 position,
            Rectangle cardRect, bool mouseCovered, bool isPlayer1, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc, int numMoves=0)
        {
            if (card != null && mouse.LeftButton == ButtonState.Pressed && cardRect.Contains(mouse.X, mouse.Y))
            {
                int count = 0;
                mousePressed = true;
                for (int i = 0; i < buttons.Length; i++)
                {
                    switch (buttons[i].Action)
                    {
                        case ActionType.Move:
                            if (ChaoticEngine.GStage == GameStage.Action && numMoves > 0 &&
                                (!ChaoticEngine.CombatThisTurn || !card.MovedThisTurn) && !card.CannotMove) // Swift Effect.
                            {
                                buttons[i].IsActive = true;
                                buttons[i].Position = new Vector2(position.X + ChaoticEngine.kCardWidth,
                                    position.Y + count * buttons[i].Height);
                                count++;
                            }
                            break;
                        case ActionType.ActivateCreature:
                            if (CheckSystem.CheckCreatureAbility(isPlayer1, card, creatureSpaces, discardPile, activeLoc))
                            {
                                buttons[i].IsActive = true;
                                buttons[i].Position = new Vector2(position.X + ChaoticEngine.kCardWidth,
                                    position.Y + count * buttons[i].Height);
                                count++;
                            }
                            break;
                        case ActionType.ActivateBattlegear:
                            if (CheckSystem.CheckBattlegearAbility(card, creatureSpaces, activeLoc))
                            {
                                buttons[i].IsActive = true;
                                buttons[i].Position = new Vector2(position.X + ChaoticEngine.kCardWidth,
                                    position.Y + count * buttons[i].Height);
                                count++;
                            }
                            break;
                        case ActionType.SacrificeCreature:
                            if (CheckSystem.CheckCreatureSacrifice(card, creatureSpaces, discardPile, activeLoc))
                            {
                                buttons[i].IsActive = true;
                                buttons[i].Position = new Vector2(position.X + ChaoticEngine.kCardWidth,
                                    position.Y + count * buttons[i].Height);
                                count++;
                            }
                            break;
                        case ActionType.SacrificeBattlegear:
                            if (CheckSystem.CheckBattlegearSacrifice(card, creatureSpaces, discardPile, activeLoc))
                            {
                                buttons[i].IsActive = true;
                                buttons[i].Position = new Vector2(position.X + ChaoticEngine.kCardWidth,
                                    position.Y + count * buttons[i].Height);
                                count++;
                            }
                            break;
                    }
                }
            }
            buttonIsCovered = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].CollisionRectangle.Contains(mouse.X, mouse.Y))
                {
                    buttonIsCovered = true;
                    break;
                }
            }
            if (!mouseCovered && !buttonIsCovered && mousePressed)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].IsActive = false;
                }
            }
            return battleBoardButtonUpdate(mouse, gameTime);
        }

        private ActionType? battleBoardButtonUpdate(MouseState mouse, GameTime gameTime)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].UpdateButton(mouse, gameTime))
                {
                    mousePressed = false;
                    for (int j = 0; j < buttons.Length; j++)
                    {
                        buttons[j].IsActive = false;
                    }
                    return buttons[i].Action;
                }
            }
            return null;
        }

        public bool UpdateBattleBoardCancel(GameTime gameTime, MouseState mouse, Creature creature,
            Rectangle cardRect, bool mouseCovered)
        {
            if (creature != null && mouse.LeftButton == ButtonState.Pressed && cardRect.Contains(mouse.X, mouse.Y))
            {
                mousePressed = true;
                button.IsActive = true;
                button.Position = new Vector2(creature.Position.X + ChaoticEngine.kCardWidth, creature.Position.Y);
            }
            buttonIsCovered = false;
            if (button.CollisionRectangle.Contains(mouse.X, mouse.Y))
                buttonIsCovered = true;
            if (!mouseCovered && !buttonIsCovered && mousePressed)
                button.IsActive = false;
            if (button.UpdateButton(mouse, gameTime))
            {
                mousePressed = false;
                button.IsActive = false;
                return true;
            }
            return false;
        }

        public void DrawBattleBoardMenu(SpriteBatch spriteBatch, bool mouseCovered)
        {
            if (mouseCovered || buttonIsCovered)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].DrawButton(spriteBatch);
                }
                button.DrawButton(spriteBatch);
            }
        }
    }
}
