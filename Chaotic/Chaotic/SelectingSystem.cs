using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    static class SelectingSystem
    {
        public static BattleBoardNode SelectingCreature(MouseState mouse, BattleBoardNode[] creatureSpaces, 
            Predicate<BattleBoardNode> selectCreature)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (selectCreature(creatureSpaces[i]))
                    creatureSpaces[i].IsSelectible = true;

                if (creatureSpaces[i].CreatureRectangle.Contains(mouse.X,mouse.Y) && mouse.LeftButton == ButtonState.Pressed &&
                    ChaoticEngine.PrevState.LeftButton != ButtonState.Pressed && creatureSpaces[i].IsSelectible)
                {
                    for (int j = 0; j < creatureSpaces.Length; j++)
                    {
                        creatureSpaces[j].IsSelectible = false;
                    }
                    return creatureSpaces[i];
                }
            }
            return null;
        }
        public static BattleBoardNode SelectingBattlegear(MouseState mouse, BattleBoardNode[] creatureSpaces,
           Predicate<BattleBoardNode> selectBattlegear)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (selectBattlegear(creatureSpaces[i]))
                    creatureSpaces[i].IsSelectible = true;

                if (creatureSpaces[i].BattlegearRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed &&
                    creatureSpaces[i].IsSelectible)
                {
                    for (int j = 0; j < creatureSpaces.Length; j++)
                    {
                        creatureSpaces[j].IsSelectible = false;
                    }
                    return creatureSpaces[i];
                }
            }
            return null;
        }

        public static BattleBoardNode SelectingOpenSpace(MouseState mouse, BattleBoardNode[] creatureSpaces)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode == null)
                    creatureSpaces[i].IsCovered = true;

                if (creatureSpaces[i].CreatureRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed &&
                    ChaoticEngine.PrevState.LeftButton != ButtonState.Pressed && creatureSpaces[i].IsCovered)
                {
                    for (int j = 0; j < creatureSpaces.Length; j++)
                    {
                        creatureSpaces[j].IsCovered = false;
                    }
                    return creatureSpaces[i];
                }
            }
            return null;
        }

        public static AttackDeck? SelectingAttackDeck(MouseState mouse, AttackDeck atkDeck1, AttackDeck atkDeck2)
        {
            atkDeck1.DeckCovered = atkDeck2.DeckCovered = true;
            if (atkDeck1.DeckRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed)
            {
                atkDeck1.DeckCovered = atkDeck2.DeckCovered = false;
                return atkDeck1;
            }
            else if (atkDeck2.DeckRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed)
            {
                atkDeck1.DeckCovered = atkDeck2.DeckCovered = false;
                return atkDeck2;
            }
            return null;
        }

        public static LocationDeck? SelectingLocationDeck(MouseState mouse, LocationDeck locDeck1, LocationDeck locDeck2)
        {
            locDeck1.DeckCovered = locDeck2.DeckCovered = true;
            if (locDeck1.DeckRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed)
            {
                locDeck1.DeckCovered = locDeck2.DeckCovered = false;
                return locDeck1;
            }
            else if (locDeck2.DeckRectangle.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed)
            {
                locDeck1.DeckCovered = locDeck2.DeckCovered = false;
                return locDeck2;
            }
            return null;
        }
    }
}
