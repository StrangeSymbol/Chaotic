/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    class BattleBoardNode
    {
        Texture2D creatureSpaceSprite;
        Texture2D overlay;
        Texture2D descriptionPanel;
        SpriteFont font;
        Vector2 position;
        Creature creature;
        byte[] movableSpaces;
        int numMoves = 1;
        double elaspedTime;
        int selectedIndex = -1;
        bool isPlayer1;
        bool mouseCoveredCreature;
        bool mouseCoveredBattlegear;
        bool isMovableSpace;
        bool isCovered;
        Texture2D mugicCounter;
        Texture2D[] mugicTextures;
        Vector2[] mugicPositions;
        Color colour;
        BattleBoardMenu creatureMenu;
        BattleBoardMenu battlegearMenu;

        bool isSeen;
        Queue<int> rangeQueue;


        public BattleBoardNode(Texture2D creatureSpaceSprite, Texture2D overlay, SpriteFont font, Vector2 position, byte[] movableSpaces,
            Texture2D descriptionPanel, BattleBoardButton[] creatureButtons, BattleBoardButton button,
            Texture2D[] mugicTextures, bool isPlayer1=true)
        {
            this.creatureSpaceSprite = creatureSpaceSprite;
            this.overlay = overlay;
            this.font = font;
            this.descriptionPanel = descriptionPanel;
            this.position = position;
            this.movableSpaces = movableSpaces;
            creature = null;
            this.isPlayer1 = isPlayer1;
            this.mugicTextures = mugicTextures;
            mugicPositions = new Vector2[6];
            creatureMenu = new BattleBoardMenu(creatureButtons, button);
            battlegearMenu = new BattleBoardMenu(new BattleBoardButton[2] { 
                new BattleBoardButton(creatureButtons[1].Texture, creatureButtons[1].Overlay, 
                creatureButtons[1].Width, creatureButtons[1].Height, ActionType.ActivateBattlegear),
                new BattleBoardButton(creatureButtons[2].Texture, creatureButtons[2].Overlay, 
                creatureButtons[2].Width, creatureButtons[2].Height, ActionType.SacrificeBattlegear)}, button);
        }

        public Vector2 Position { get { return position; } }
        public Rectangle CreatureRectangle { get { return new Rectangle((int)position.X, (int)position.Y, ChaoticEngine.kCardWidth,
            ChaoticEngine.kCardHeight); } }
        public Rectangle BattlegearRectangle
        {
            get
            {
                if (isPlayer1)
                    return new Rectangle((int)position.X, (int)position.Y - ChaoticEngine.kCardHeight / 6,
                        ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight / 6);
                else
                    return new Rectangle((int)position.X, (int)position.Y + ChaoticEngine.kCardHeight, ChaoticEngine.kCardWidth,
                        ChaoticEngine.kCardHeight / 6);
            }
        }
        public Creature CreatureNode { get { return creature; } }
        public bool MouseCoveredCreature { get { return mouseCoveredCreature; } }
        public bool MouseCoveredBattlegear { get { return mouseCoveredBattlegear; } }
        public bool IsMovableSpace { get { return isMovableSpace; } set { isMovableSpace = value; } }
        public bool IsPlayer1 { get { return isPlayer1; } set { isPlayer1 = value; } }
        public bool IsSelectible { get { if (creature != null) return creature.IsCovered; else return false; }
            set { if (creature != null) creature.IsCovered = value; } }
        public bool IsCovered { get { return isCovered; } set { isCovered = value; } }

        public int NumMoves { get { return numMoves; } set { numMoves = value; } }

        public bool HasBattegear()
        {
            return creature != null && creature.Battlegear != null;
        }

        public ActionType? UpdateBattleBoardNode(GameTime gameTime, MouseState mouse, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            ActionType? action = creatureMenu.UpdateBattleBoardMenu(gameTime, mouse, creature, position,
                CreatureRectangle, mouseCoveredCreature, this.isPlayer1, creatureSpaces, discardPile, activeLoc, numMoves);
            if (!action.HasValue)
            {
                Vector2 locButtonPos = position + (this.isPlayer1 ?
                    new Vector2(0, -ChaoticEngine.kCardHeight / 4) : new Vector2(0, 3 * ChaoticEngine.kCardHeight / 4));
                return battlegearMenu.UpdateBattleBoardMenu(gameTime, mouse, creature,
                     locButtonPos, BattlegearRectangle, MouseCoveredBattlegear,
                    this.isPlayer1, creatureSpaces, discardPile, activeLoc);
            }
            else
                return action;
        }

        public void UpdateCardDescription(MouseState mouse)
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

        public bool UpdateCancel(MouseState mouse, GameTime gameTime)
        {
            if (CreatureRectangle.Contains(mouse.X, mouse.Y) && !mouseCoveredCreature)
                mouseCoveredCreature = true;
            return creatureMenu.UpdateBattleBoardCancel(gameTime, mouse, creature,
                CreatureRectangle, mouseCoveredCreature);
        }

        public bool UpdateMovableSpaces(MouseState mouse, GameTime gameTime, BattleBoardNode[] creatureSpaces)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].isMovableSpace && mouse.LeftButton == ButtonState.Pressed
                    && creatureSpaces[i].CreatureRectangle.Contains(mouse.X, mouse.Y) && !ChaoticEngine.IsACardMoving)
                {
                    if (creatureSpaces[i].creature == null)
                    {
                        for (int j = 0; j < creatureSpaces.Length; j++)
                            creatureSpaces[j].isMovableSpace = false;
                        elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                        creature.IsMoving = true;
                        creature.CourseToCard(creatureSpaces[i].Position);
                        selectedIndex = i;
                        ChaoticEngine.IsACardMoving = true;
                    }
                    else
                    {
                        ChaoticEngine.GStage = GameStage.Initiative;
                        isMovableSpace = true;
                        ChaoticEngine.sYouNode = this;
                        ChaoticEngine.sEnemyNode = creatureSpaces[i];
                        for (int j = 0; j < creatureSpaces.Length; j++)
                        {
                            if (creatureSpaces[j] != this && creatureSpaces[j] != creatureSpaces[i])
                                creatureSpaces[j].isMovableSpace = false;
                        }
                    }
                }
            }
            if (creature.Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                creature.Move(gameTime);
                if (creature.Battlegear != null)
                    creature.Battlegear.Position = new Vector2(creature.Position.X, creature.Position.Y +
                        (isPlayer1 ? -ChaoticEngine.kBattlegearGap : ChaoticEngine.kBattlegearGap));
            }
            else if (creature.Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                ChaoticEngine.IsACardMoving = false;
                creature.MovedThisTurn = !ChaoticEngine.CombatThisTurn;
                creature.IsMoving = false;
                elaspedTime = 0.0;
                creatureSpaces[selectedIndex].isPlayer1 = this.isPlayer1;
                creatureSpaces[selectedIndex].AddCreature(creature);
                creatureSpaces[selectedIndex].numMoves = this.numMoves - 1;
                selectedIndex = -1;
                creature = null;
                return true;
            }
            return false;
        }

        public bool UpdateMoveCreature(GameTime gameTime, MouseState mouse, BattleBoardNode node)
        {
            if (node.creature == null && !ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                creature.IsMoving = true;
                creature.CourseToCard(node.Position);
            }
            if (creature.Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                creature.Move(gameTime);
                if (creature.Battlegear != null)
                    creature.Battlegear.Position = new Vector2(creature.Position.X, creature.Position.Y +
                        (isPlayer1 ? -ChaoticEngine.kBattlegearGap : ChaoticEngine.kBattlegearGap));
            }
            else if (creature.Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                ChaoticEngine.IsACardMoving = false;
                creature.MovedThisTurn = !ChaoticEngine.CombatThisTurn;
                creature.IsMoving = false;
                elaspedTime = 0.0;
                node.isPlayer1 = this.isPlayer1;
                node.AddCreature(creature);
                node.numMoves = this.numMoves - 1;
                creature = null;
                return true;
            }
            return false;
        }

        public bool UpdateDiscardCreature(GameTime gameTime, MouseState mouse, DiscardPile<ChaoticCard> discardPile)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                creature.IsMoving = true;
                creature.CourseToCard(discardPile.GetDiscardTemplate().Position);
                ChaoticEngine.CodedEffects.InitializeCodedLetters(creature.Position +
                    new Vector2(ChaoticEngine.kCardWidth / 2, ChaoticEngine.kCardHeight / 2));
            }
            else if (creature.Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
                creature.Move(gameTime);
            else if (creature.Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.IsMoving)
            {
                ChaoticEngine.IsACardMoving = false;
                creature.IsMoving = false;
                elaspedTime = 0.0;
                if (creature.Battlegear != null)
                    discardPile.DiscardList.Add(creature.Battlegear);
                discardPile.DiscardList.Add(creature.OldCreature);
                return true;
            }
            return false;
        }
        public bool UpdateDiscardBattlegear(GameTime gameTime, MouseState mouse, DiscardPile<ChaoticCard> discardPile)
        {
            if (!ChaoticEngine.IsACardMoving)
            {
                ChaoticEngine.IsACardMoving = true;
                elaspedTime = gameTime.TotalGameTime.TotalMilliseconds;
                creature.Battlegear.IsMoving = true;
                creature.Battlegear.CourseToCard(discardPile.GetDiscardTemplate().Position);
            }
            else if (creature.Battlegear.Time >= gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.Battlegear.IsMoving)
                creature.Battlegear.Move(gameTime);
            else if (creature.Battlegear.Time < gameTime.TotalGameTime.TotalMilliseconds - elaspedTime && creature.Battlegear.IsMoving)
            {
                ChaoticEngine.IsACardMoving = false;
                creature.Battlegear.IsMoving = false;
                elaspedTime = 0.0;
                discardPile.DiscardList.Add(creature.Battlegear);
                return true;
            }
            return false;
        }

        public void RestoreMoves()
        {
            this.numMoves = 1;
            if (creature != null)
                this.numMoves += creature.Swift;
        }

        public void TurnOnMovableSpaces(BattleBoardNode[] creatureSpaces)
        {
            if (creature.CanMoveAnywhere)
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (creatureSpaces[i].CreatureNode == null)
                        creatureSpaces[i].isMovableSpace = true;
                    else if (creatureSpaces[i].isPlayer1 != this.isPlayer1 && !ChaoticEngine.CombatThisTurn)
                        creatureSpaces[i].isMovableSpace = true;
                }
            }
            else
            {
                if (creature.Range)
                {
                    rangeQueue = new Queue<int>();
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i] == this)
                        {
                            rangeQueue.Enqueue(i);
                            creatureSpaces[i].isSeen = true;
                        }
                    }
                    while (rangeQueue.Count != 0)
                    {
                        int i = rangeQueue.Dequeue();
                        for (int j = 0; j < creatureSpaces[i].movableSpaces.Length; j++)
                        {
                            if (creatureSpaces[creatureSpaces[i].movableSpaces[j]].creature != null
                                && !creatureSpaces[creatureSpaces[i].movableSpaces[j]].isSeen)
                            {
                                rangeQueue.Enqueue(creatureSpaces[i].movableSpaces[j]);
                                creatureSpaces[creatureSpaces[i].movableSpaces[j]].isSeen = true;
                            }
                            if (creatureSpaces[creatureSpaces[i].movableSpaces[j]].creature == null)
                                creatureSpaces[creatureSpaces[i].movableSpaces[j]].isMovableSpace = true;
                            else if (creatureSpaces[creatureSpaces[i].movableSpaces[j]].isPlayer1 != this.isPlayer1 && !ChaoticEngine.CombatThisTurn)
                                creatureSpaces[creatureSpaces[i].movableSpaces[j]].isMovableSpace = true;
                        }
                    }

                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        creatureSpaces[i].isSeen = false;
                    }
                }
                else
                {
                    for (int j = 0; j < movableSpaces.Length; j++)
                    {
                        if (creatureSpaces[movableSpaces[j]].creature == null)
                            creatureSpaces[movableSpaces[j]].isMovableSpace = true;
                        else if (creatureSpaces[movableSpaces[j]].isPlayer1 != this.isPlayer1 && !ChaoticEngine.CombatThisTurn)
                            creatureSpaces[movableSpaces[j]].isMovableSpace = true;
                    }
                }
            }
        }

        public byte NumAdjacentOverWorldCreatures(BattleBoardNode[] creatureSpaces)
        {
            byte numOverWorldCreatures = 0;
            for (int j = 0; j < movableSpaces.Length; j++)
            {
                BattleBoardNode b = creatureSpaces[movableSpaces[j]];
                if (b.creature != null && b.creature.CreatureTribe == Tribe.OverWorld && b.isPlayer1 == isPlayer1)
                    numOverWorldCreatures++;
            }
            return numOverWorldCreatures;
        }

        public byte NumMandiblors(BattleBoardNode[] creatureSpaces)
        {
            byte numMandiblors = 0;
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                BattleBoardNode b = creatureSpaces[i];
                if (b.creature != null && b.creature.CreatureTribe == Tribe.Danian &&
                    (b.creature.CardType == CreatureType.Mandiblor || b.creature.CardType == CreatureType.MandiblorMuge)
                    && b.isPlayer1 == isPlayer1)
                    numMandiblors++;
            }
            return numMandiblors;
        }

        public void TurnOnPlayableMugic(MugicHand hand, ActiveLocation activeLoc)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].CheckCanPayMugicCost(creature, activeLoc.LocationActive))
                    hand[i].IsCovered = true;
            }
        }

        public void AddCreature(Creature c)
        {
            creature = c;
            creature.Position = position;
            creature.SaveOldCreature();
            if (creature.Battlegear != null)
                creature.Battlegear.Position = new Vector2(position.X, position.Y +
                    (isPlayer1 ? -ChaoticEngine.kBattlegearGap : ChaoticEngine.kBattlegearGap));
            switch (creature.CreatureTribe)
            {
                case Tribe.OverWorld:
                    colour = Color.Blue; 
                    mugicCounter = mugicTextures[0]; break;
                case Tribe.UnderWorld:
                    colour = Color.Red;
                    mugicCounter = mugicTextures[1]; break;
                case Tribe.Mipedian:
                    colour = Color.Yellow;
                    mugicCounter = mugicTextures[2]; break;
                case Tribe.Danian:
                    colour = Color.Brown;
                    mugicCounter = mugicTextures[3]; break;
            }
            mugicPositions[0] = new Vector2(position.X + mugicCounter.Width / 2, position.Y + 
                ChaoticEngine.kCardHeight - 5 * mugicCounter.Height / 4);
            mugicPositions[1] = new Vector2(position.X + ChaoticEngine.kCardWidth - 5 * mugicCounter.Width / 4,
                position.Y + ChaoticEngine.kCardHeight * 0.25f - mugicCounter.Height);
            mugicPositions[2] = new Vector2(position.X + ChaoticEngine.kCardWidth / 2 - mugicCounter.Width / 2,
                position.Y + ChaoticEngine.kCardHeight / 2 - mugicCounter.Height / 2);
            mugicPositions[3] = new Vector2(position.X + mugicCounter.Width / 2, position.Y + ChaoticEngine.kCardHeight * 0.25f
                - mugicCounter.Height);
            mugicPositions[4] = new Vector2(position.X + ChaoticEngine.kCardWidth - 5 * mugicCounter.Width / 4,
                 position.Y + ChaoticEngine.kCardHeight / 2 - mugicCounter.Height);
            mugicPositions[5] = new Vector2(position.X + ChaoticEngine.kCardWidth - 5 * mugicCounter.Width / 4,
                position.Y + ChaoticEngine.kCardHeight * 0.75f - mugicCounter.Height / 2);
            //TODO: Add more mugic positions to draw mugics.
        }

        public void RemoveCreature()
        {
            creature = null;
        }

        public void RemoveBattlegear()
        {
            creature.Battlegear.IsFaceUp = true;
            creature.UnEquip();
        }

        public void GetCardCoveredByMouse(MouseState mouse, CardDescription description)
        {
            if (creature != null && creature.Battlegear != null && BattlegearRectangle.Contains(mouse.X, mouse.Y))
            {
                description.CoveredCard = creature.Battlegear.Texture;
                description.Description = creature.Battlegear.Description();
            }
            else if (creature != null && CreatureRectangle.Contains(mouse.X, mouse.Y))
            {
                description.CoveredCard = creature.Texture;
                description.Description = creature.Description();
            }
        }

        private string creatureDescription()
        {
            StringBuilder builder = new StringBuilder(creature.Name);
            builder.Append(" ");
            builder.Append(creature.CreatureTribe);
            builder.Append("\n");
            builder.Append("E: ");
            builder.Append(creature.Energy);
            builder.Append(" C: ");
            builder.Append(creature.Courage);
            builder.Append(" P: ");
            builder.Append(creature.Power);
            builder.Append(" W: ");
            builder.Append(creature.Wisdom);
            builder.Append(" S: ");
            builder.Append(creature.Speed);
            if (creature.Strike > 0)
            {
                builder.Append(" ");
                builder.Append("Strike ");
                builder.Append(creature.Strike);
            }
            if (creature.Surprise)
            {
                builder.Append(" ");
                builder.Append("Surprise");
            }
            if (creature.Swift > 0)
            {
                builder.Append(" ");
                builder.Append("Swift ");
                builder.Append(creature.Swift);
            }
            if (creature.Range)
            {
                builder.Append(" ");
                builder.Append("Range");
            }
            if (creature.Recklessness > 0)
            {
                builder.Append(" ");
                builder.Append("Recklessness ");
                builder.Append(creature.Recklessness);
            }
            builder.Append("\n");
           
            if (creature.Fire)
            {
                builder.Append(" ");
                builder.Append("Fire");
            }
            if (creature.Air)
            {
                builder.Append(" ");
                builder.Append("Air");
            }
            if (creature.Earth)
            {
                builder.Append(" ");
                builder.Append("Earth");
            }
            if (creature.Water)
            {
                builder.Append(" ");
                builder.Append("Water");
            }

            if (creature.FireDamage > 0)
            {
                builder.Append(" ");
                builder.Append("Fire ");
                builder.Append(creature.FireDamage);
            }
            if (creature.AirDamage > 0)
            {
                builder.Append(" ");
                builder.Append("Air ");
                builder.Append(creature.AirDamage);
            }
            if (creature.EarthDamage > 0)
            {
                builder.Append(" ");
                builder.Append("Earth ");
                builder.Append(creature.EarthDamage);
            }
            if (creature.WaterDamage > 0)
            {
                builder.Append(" ");
                builder.Append("Water ");
                builder.Append(creature.WaterDamage);
            }

            if (creature.IntimidateCourage > 0)
            {
                builder.Append(" Intimidate Courage: ");
                builder.Append(creature.IntimidateCourage);
            }
            if (creature.IntimidatePower > 0)
            {
                builder.Append(" Intimidate Power: ");
                builder.Append(creature.IntimidatePower);
            }
            if (creature.IntimidateWisdom > 0)
            {
                builder.Append(" Intimidate Wisdom: ");
                builder.Append(creature.IntimidateWisdom);
            }
            if (creature.IntimidateSpeed > 0)
            {
                builder.Append(" Intimidate Speed: ");
                builder.Append(creature.IntimidateSpeed);
            }

            return builder.ToString();
        }

        private string battlegearDescription()
        {
            return creature.Battlegear.Name + (creature.Battlegear.Negate ? " Negated." : "");
        }

        public void DrawBattleBoardNode(SpriteBatch spriteBatch, Texture2D cardBack)
        {
            spriteBatch.Draw(creatureSpaceSprite, CreatureRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
            if (creature != null)
            {
                if (isPlayer1)
                    spriteBatch.Draw(creature.Texture, new Rectangle((int)creature.Position.X, (int)creature.Position.Y,
                        ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);
                else
                    spriteBatch.Draw(creature.Texture, new Rectangle((int)creature.Position.X, (int)creature.Position.Y,
                        ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight), null, Color.White, 0f,
                        Vector2.Zero, SpriteEffects.FlipVertically, 0.7f);

                if (creature.IsCovered)
                    spriteBatch.Draw(overlay, CreatureRectangle, null, Color.Yellow, 0f, Vector2.Zero, SpriteEffects.None, 0.55f);
                creature.DrawNegate(spriteBatch, 0.55f);

                if (creature.Battlegear != null)
                {
                    Texture2D sprite;
                    if (creature.Battlegear.IsFaceUp)
                        sprite = creature.Battlegear.Texture;
                    else
                        sprite = cardBack;

                    if (isPlayer1)
                        spriteBatch.Draw(sprite,
                        new Rectangle((int)creature.Battlegear.Position.X, (int)creature.Battlegear.Position.Y,
                            ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight),
                        null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                    else
                        spriteBatch.Draw(sprite,
                        new Rectangle((int)creature.Battlegear.Position.X, (int)creature.Battlegear.Position.Y,
                            ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight),
                        null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.8f);

                    if (mouseCoveredBattlegear)
                    {
                        Vector2 descriptionPosition;
                        if (isPlayer1)
                            descriptionPosition = new Vector2(position.X - descriptionPanel.Width, 
                                position.Y - ChaoticEngine.kCardHeight / 6);
                        else
                            descriptionPosition = new Vector2(position.X + ChaoticEngine.kCardWidth, position.Y
                            + ChaoticEngine.kCardHeight + ChaoticEngine.kCardHeight / 6 - descriptionPanel.Height);
                        spriteBatch.Draw(descriptionPanel, descriptionPosition, null, Color.White,
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 0.45f);
                        spriteBatch.DrawString(font, battlegearDescription(), descriptionPosition, Color.Black,
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);
                    }
                    if (creature.Battlegear.IsCovered)
                        spriteBatch.Draw(overlay, BattlegearRectangle, null, Color.Yellow, 0f, 
                            Vector2.Zero, SpriteEffects.None, 0.75f);
                }
                if (mouseCoveredCreature)
                {
                    Vector2 panelPosition = new Vector2(position.X - descriptionPanel.Width, position.Y);
                    spriteBatch.Draw(descriptionPanel, panelPosition, null, Color.White,
                        0f, Vector2.Zero, 1f, SpriteEffects.None, 0.45f);
                    spriteBatch.DrawString(font, creatureDescription(), panelPosition, colour, 
                        0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);
                }

                creatureMenu.DrawBattleBoardMenu(spriteBatch, mouseCoveredCreature);
                battlegearMenu.DrawBattleBoardMenu(spriteBatch, mouseCoveredBattlegear);

                for (int i = 0; i < creature.MugicCounters; i++)
                {
                    spriteBatch.Draw(mugicCounter, mugicPositions[i], null, Color.White,
                        0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
            }
            if (isMovableSpace)
            {
                spriteBatch.Draw(overlay, CreatureRectangle, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            }

            if (isCovered)
                spriteBatch.Draw(overlay, CreatureRectangle, null, Color.Yellow, 0f, Vector2.Zero, SpriteEffects.None, 0.55f);
        }
    }
}
