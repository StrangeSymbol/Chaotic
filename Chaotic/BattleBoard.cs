using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;
using ChaoticGameLib.Attacks;

namespace Chaotic
{
    public enum CreatureNumber { SixOnSix, ThreeOnThree, OneOnOne };
    enum GameStage { BeginningOfTheGame, CoinFlip, DrawingAttack, LocationStep, Moving, ReturnLocation, CreatureToDiscard1,
    CreatureToDiscard2, Action, Combat, Initiative, EndOfCombat, MoveToCodedSpace, Abilities, ChangeLocation,
    };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BattleBoard : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BattleBoardNode[] creatureSpaces;
        DiscardPile<ChaoticCard> discardPile1;
        DiscardPile<ChaoticCard> discardPile2;
        DiscardPile<Attack> attackDiscardPile1;
        DiscardPile<Attack> attackDiscardPile2;
        AttackDeck attackDeck1;
        AttackDeck attackDeck2;
        AttackHand attackHand1;
        AttackHand attackHand2;
        LocationDeck locationDeck1;
        LocationDeck locationDeck2;
        ActiveLocation activeLocation1;
        ActiveLocation activeLocation2;
        int selectedSpace = -1;

        Button endTurnButton;
        Button backButton;

        Texture2D cardBack;

        Rectangle backgroundRect;

        SelectPanel<Attack> attackPanel1;
        SelectPanel<Attack> attackPanel2;
        List<Attack> topTwo;
        SelectPanel<Location> locationPanel;
        List<Location> topTwoLoc;

        CreatureNumber creatureNumber;
        bool done;
        byte numDrawed;

        public BattleBoard(Game game, GraphicsDeviceManager graphics, CreatureNumber creatureNumber)
            : base(game)
        {
            this.graphics = graphics;
            this.creatureNumber = creatureNumber;
            this.numDrawed = 0;
            ChaoticEngine.GStage = GameStage.BeginningOfTheGame;
            ChaoticEngine.Player1Active = true;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Texture2D creatureSpaceSprite = Game.Content.Load<Texture2D>(@"BattleBoardSprites\CardSpace");
            Texture2D descriptionPanel = Game.Content.Load<Texture2D>(@"BattleBoardSprites\BattleBoardDescription");
            Texture2D moveButtonSprite = Game.Content.Load<Texture2D>(@"BattleBoardButtons\MoveButton");
            Texture2D activateButtonSprite = Game.Content.Load<Texture2D>(@"BattleBoardButtons\ActivateButton"); ;
            Texture2D sacrificeButtonSprite = Game.Content.Load<Texture2D>(@"BattleBoardButtons\SacrificeButton");
            Texture2D castButtonSprite = Game.Content.Load<Texture2D>(@"BattleBoardButtons\CastButton");
            Texture2D cancelButtonSprite = Game.Content.Load<Texture2D>(@"BattleBoardButtons\CancelButton");
            Texture2D overlay = Game.Content.Load<Texture2D>("OkButtonCover");
            Texture2D spaceCover = Game.Content.Load<Texture2D>("CardOutline");
            Texture2D damageCover = Game.Content.Load<Texture2D>("damageCover");
            Texture2D healCover = Game.Content.Load<Texture2D>("healCover");
            Texture2D[] mugicTextures = new Texture2D[4] {Game.Content.Load<Texture2D>(@"BattleBoardSprites\MugicOW"),
                Game.Content.Load<Texture2D>(@"BattleBoardSprites\Mugic-UW"),
                Game.Content.Load<Texture2D>(@"BattleBoardSprites\Mugic-Mip"),
                Game.Content.Load<Texture2D>(@"BattleBoardSprites\Mugic-Dan")};
            cardBack = ChaoticEngine.CoveredCard = Game.Content.Load<Texture2D>("CardBack");
            Texture2D discardTexture = Game.Content.Load<Texture2D>(@"BattleBoardSprites\Discard");
            Vector2 discardPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 2 * ChaoticEngine.kCardWidth, graphics.PreferredBackBufferHeight
                - ChaoticEngine.kCardHeight - (graphics.PreferredBackBufferHeight - (6 * ChaoticEngine.kCardHeight + 6 * ChaoticEngine.kBattlegearGap + 5 * ChaoticEngine.kCardGap)));
            discardPile1 = new DiscardPile<ChaoticCard>(Game.Content, graphics, discardTexture, discardPosition, true);
            Texture2D attackDiscardTexture = Game.Content.Load<Texture2D>(@"BattleBoardSprites\AttackDiscard");
            Vector2 attackDiscardPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 + ChaoticEngine.kCardWidth, discardPosition.Y);
            attackDiscardPile1 = new DiscardPile<Attack>(Game.Content, graphics, attackDiscardTexture, attackDiscardPosition, true);
            Texture2D attackTexture = Game.Content.Load<Texture2D>(@"BattleBoardSprites\Attack");
            Vector2 attackPosition = new Vector2(attackDiscardPosition.X + 3 * ChaoticEngine.kCardWidth / 2, attackDiscardPosition.Y);
            attackDeck1 = new AttackDeck(attackTexture, cardBack, spaceCover, attackPosition, true);
            attackHand1 = new AttackHand(true, cardBack, attackPosition, damageCover, healCover,
                Game.Content.Load<SpriteFont>("Fonts/AttackDamage"));
            Vector2 locationPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 3 * ChaoticEngine.kCardWidth
                - ChaoticEngine.kCardHeight, 3 * ChaoticEngine.kCardHeight + 4 * ChaoticEngine.kBattlegearGap + 3 * ChaoticEngine.kCardGap);
            Texture2D locationTexture = Game.Content.Load<Texture2D>(@"BattleBoardSprites\LocationCardBack");
            locationDeck1 = new LocationDeck(Game.Content.Load<Texture2D>(@"BattleBoardSprites\Location"), 
                locationTexture, spaceCover, locationPosition, true);
            activeLocation1 = new ActiveLocation(locationTexture,
                new Vector2(locationPosition.X, locationPosition.Y + ChaoticEngine.kCardHeight));
            discardPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 + ChaoticEngine.kCardWidth, ChaoticEngine.kBattlegearGap);
            discardPile2 = new DiscardPile<ChaoticCard>(Game.Content, graphics, discardTexture, discardPosition, false);
            attackDiscardPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 2 * ChaoticEngine.kCardWidth, discardPosition.Y);
            attackDiscardPile2 = new DiscardPile<Attack>(Game.Content, graphics, attackDiscardTexture, attackDiscardPosition, false);
            attackPosition = new Vector2(attackDiscardPosition.X - 3 * ChaoticEngine.kCardWidth / 2, attackDiscardPosition.Y);
            attackDeck2 = new AttackDeck(attackTexture, cardBack, spaceCover, attackPosition, false);
            attackHand2 = new AttackHand(false, cardBack, attackPosition, damageCover, healCover,
                Game.Content.Load<SpriteFont>("Fonts/AttackDamage"));
            locationPosition = new Vector2(discardPosition.X + 2 * ChaoticEngine.kCardWidth,
                3 * ChaoticEngine.kCardHeight + 3 * ChaoticEngine.kBattlegearGap + 2 * ChaoticEngine.kCardGap - ChaoticEngine.kCardWidth);
            locationDeck2 = new LocationDeck(Game.Content.Load<Texture2D>(@"BattleBoardSprites\Location"), locationTexture, spaceCover,
                locationPosition, false);
            activeLocation2 = new ActiveLocation(locationTexture,
                new Vector2(locationPosition.X, locationPosition.Y - ChaoticEngine.kCardHeight));
            ChaoticEngine.BackgroundSprite = Game.Content.Load<Texture2D>(@"Backgrounds\ChaoticBackground");
            ChaoticEngine.OrgBackgroundSprite = ChaoticEngine.BackgroundSprite;
            backgroundRect = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            SpriteFont font = Game.Content.Load<SpriteFont>(@"Fonts\CardDescriptionFont");
            BattleBoardButton move = new BattleBoardButton(moveButtonSprite, overlay,
                ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight / 3, ActionType.Move);
            BattleBoardButton activate = new BattleBoardButton(activateButtonSprite, overlay,
                ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight / 3, ActionType.Activate);
            BattleBoardButton sacrifice = new BattleBoardButton(sacrificeButtonSprite, overlay, ChaoticEngine.kCardWidth,
                ChaoticEngine.kCardHeight / 3, ActionType.SacrificeCreature);
            BattleBoardButton cast = new BattleBoardButton(castButtonSprite, overlay, ChaoticEngine.kCardWidth,
                ChaoticEngine.kCardHeight / 3, ActionType.Cast);
            BattleBoardButton cancel = new BattleBoardButton(cancelButtonSprite, overlay, ChaoticEngine.kCardWidth,
                ChaoticEngine.kCardHeight / 3, ActionType.Cancel);

            switch (creatureNumber)
            {
                case CreatureNumber.SixOnSix:
                    creatureSpaces = new BattleBoardNode[12];
                    creatureSpaces[0] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(graphics.PreferredBackBufferWidth / 2 -
                            ChaoticEngine.kCardWidth / 2, graphics.PreferredBackBufferHeight - ChaoticEngine.kCardHeight -
                        (graphics.PreferredBackBufferHeight - (6 * ChaoticEngine.kCardHeight + 6 * ChaoticEngine.kBattlegearGap + 5 * ChaoticEngine.kCardGap))),
                        new byte[2] { 1, 2 }, descriptionPanel, new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel,
                        mugicTextures);
                    creatureSpaces[1] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X - ChaoticEngine.kCardWidth,
                        creatureSpaces[0].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[4] { 0, 2, 3, 4 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[2] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[1].Position.Y),
                        new byte[4] { 0, 1, 4, 5 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[3] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[1].Position.X - ChaoticEngine.kCardWidth,
                        creatureSpaces[1].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[4] { 1, 4, 6, 7 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[4] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[1].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[3].Position.Y),
                        new byte[7] { 1, 2, 3, 5, 6, 7, 8 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[5] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[2].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[3].Position.Y),
                        new byte[4] { 2, 4, 7, 8 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[6] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[3].Position.X,
                        creatureSpaces[3].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[4] { 3, 4, 7, 9 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[7] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[4].Position.X, creatureSpaces[6].Position.Y),
                        new byte[7] { 3, 4, 5, 6, 8, 9, 10 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[8] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[5].Position.X, creatureSpaces[6].Position.Y),
                        new byte[4] { 4, 5, 7, 10 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[9] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                    new Vector2(creatureSpaces[1].Position.X,
                        creatureSpaces[1].Position.Y - 3 * (ChaoticEngine.kCardHeight + ChaoticEngine.kCardGap + ChaoticEngine.kBattlegearGap)),
                        new byte[4] { 6, 7, 10, 11 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[10] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[2].Position.X, creatureSpaces[9].Position.Y),
                        new byte[4] { 7, 8, 9, 11 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[11] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X,
                            creatureSpaces[10].Position.Y - (ChaoticEngine.kCardHeight + ChaoticEngine.kCardGap + ChaoticEngine.kBattlegearGap)),
                         new byte[2] { 9, 10 }, descriptionPanel,
                         new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    for (int i = 5, j = 0; i >= 0; i--, j++)
                    {
                        ChaoticEngine.sCreatures1[j].Battlegear = ChaoticEngine.sBattlegears1[j];
                        creatureSpaces[i].AddCreature(ChaoticEngine.sCreatures1[j]);
                    }
                    for (int i = 6, j = 0; i < 12; i++, j++)
                    {
                        ChaoticEngine.sCreatures2[j].Battlegear = ChaoticEngine.sBattlegears2[j];
                        creatureSpaces[i].AddCreature(ChaoticEngine.sCreatures2[j]);
                    }
                    break;
                case CreatureNumber.ThreeOnThree:
                    creatureSpaces = new BattleBoardNode[6];
                    creatureSpaces[0] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(graphics.PreferredBackBufferWidth / 2 -
                        ChaoticEngine.kCardWidth / 2, 4 * ChaoticEngine.kCardHeight + 5 * ChaoticEngine.kBattlegearGap + 4 * ChaoticEngine.kCardGap),
                        new byte[2] { 1, 2 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[1] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X - ChaoticEngine.kCardWidth,
                        creatureSpaces[0].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[4] { 0, 2, 3, 4 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[2] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[1].Position.Y),
                        new byte[4] { 0, 1, 3, 4 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[3] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[1].Position.X,
                        creatureSpaces[1].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[4] { 1, 2, 4, 5 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[4] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[2].Position.X, creatureSpaces[3].Position.Y),
                        new byte[4] { 1, 2, 3, 5 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    creatureSpaces[5] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X,
                        creatureSpaces[3].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[2] { 3, 4 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures, false);
                    for (int i = 2, j = 0; i >= 0; i--, j++)
                    {
                        ChaoticEngine.sCreatures1[j].Battlegear = ChaoticEngine.sBattlegears1[j];
                        creatureSpaces[i].AddCreature(ChaoticEngine.sCreatures1[j]);
                    }
                    for (int i = 3, j = 0; i < 6; i++, j++)
                    {
                        ChaoticEngine.sCreatures2[j].Battlegear = ChaoticEngine.sBattlegears2[j];
                        creatureSpaces[i].AddCreature(ChaoticEngine.sCreatures2[j]);
                    }
                    break;
                case CreatureNumber.OneOnOne:
                    creatureSpaces = new BattleBoardNode[2];
                    creatureSpaces[0] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(graphics.PreferredBackBufferWidth / 2 -
                        ChaoticEngine.kCardWidth / 2, 3 * ChaoticEngine.kCardHeight + 4 * ChaoticEngine.kBattlegearGap + 3 * ChaoticEngine.kCardGap),
                        new byte[1] { 1 }, descriptionPanel, new BattleBoardButton[4] { move, activate, sacrifice, cast },
                        cancel, mugicTextures);
                    creatureSpaces[1] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X
                        , creatureSpaces[0].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[1] { 0 }, descriptionPanel, new BattleBoardButton[4] { move, activate, sacrifice, cast },
                        cancel, mugicTextures, false);
                    ChaoticEngine.sCreatures1[0].Battlegear = ChaoticEngine.sBattlegears1[0];
                    creatureSpaces[0].AddCreature(ChaoticEngine.sCreatures1[0]);
                    ChaoticEngine.sCreatures2[0].Battlegear = ChaoticEngine.sBattlegears2[0];
                    creatureSpaces[1].AddCreature(ChaoticEngine.sCreatures2[0]);
                    break;
            }
            attackDeck1.ShuffleDeck(ChaoticEngine.sAttacks1);
            attackDeck2.ShuffleDeck(ChaoticEngine.sAttacks2);
            locationDeck1.ShuffleDeck(ChaoticEngine.sLocations1);
            locationDeck2.ShuffleDeck(ChaoticEngine.sLocations2);

            attackPanel1 = new SelectPanel<Attack>(Game.Content, graphics, 2, "Top", "Bottom");
            attackPanel2 = new SelectPanel<Attack>(Game.Content, graphics, 3, "Top", "Bottom", "2nd From\n Bottom");
            locationPanel = new SelectPanel<Location>(Game.Content, graphics, 2, "Top", "Bottom");

            Texture2D backTexture = Game.Content.Load<Texture2D>("Menu/SuitBackButton");
            backButton = new Button(backTexture, new Vector2(graphics.PreferredBackBufferWidth - backTexture.Width - 20, 20),
                Game.Content.Load<Texture2D>("Menu/backBtnCover"));
            endTurnButton = new Button(Game.Content.Load<Texture2D>("BattleBoardSprites/EndTurnButton"),
                new Vector2(attackDeck1.Position.X + ChaoticEngine.kCardWidth, graphics.PreferredBackBufferHeight / 2),
                Game.Content.Load<Texture2D>("Menu/MenuButtonCover"));
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            attackHand1.UpdateCoveredCard(mouse);
            attackHand2.UpdateCoveredCard(mouse);
            activeLocation1.UpdateActiveLocation(mouse);
            activeLocation2.UpdateActiveLocation(mouse);

            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                creatureSpaces[i].GetCardCoveredByMouse(mouse);
                if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    creatureSpaces[i].UpdateCardDescription(mouse);
            }

            if (backButton.UpdateButton(mouse, gameTime))
            {
                ChaoticEngine.MStage = MenuStage.MainMenu;
                Game.Components.Remove(this);
            }

            switch (ChaoticEngine.GStage)
            {
                case GameStage.BeginningOfTheGame:
                    for (int i = 0; i < creatureSpaces.Length; i++)
                        if (creatureSpaces[i].CreatureNode.Battlegear.RevealAtBeginning)
                            creatureSpaces[i].CreatureNode.Battlegear.IsFaceUp = true;
                    attackDeck1.UpdateDeckPile(gameTime, attackHand1);
                    done = attackDeck2.UpdateDeckPile(gameTime, attackHand2);
                    if (done && numDrawed >= 1)
                        ChaoticEngine.GStage = GameStage.LocationStep;
                    else if (done)
                        numDrawed++;
                    break;
                case GameStage.Action:
                    if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                        discardPile1.UpdateDiscardPile(gameTime);
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                        attackDiscardPile1.UpdateDiscardPile(gameTime);
                    if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                        discardPile2.UpdateDiscardPile(gameTime);
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                        attackDiscardPile2.UpdateDiscardPile(gameTime);

                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        for (int i = 0; i < creatureSpaces.Length; i++)
                        {
                            if (creatureSpaces[i].IsPlayer1 == ChaoticEngine.Player1Active)
                            {
                                ActionType? action = creatureSpaces[i].UpdateBattleBoardNode(mouse, gameTime);

                                switch (action)
                                {
                                    case ActionType.Move:
                                        creatureSpaces[i].TurnOnMovableSpaces(creatureSpaces);
                                        selectedSpace = i;
                                        ChaoticEngine.GStage = GameStage.Moving;
                                        break;
                                }
                            }
                        }
                        if ((creatureSpaces.Count(b => b.CreatureNode != null && b.CreatureNode.MovedThisTurn == true) > 0 ||
                        ChaoticEngine.CombatThisTurn) && endTurnButton.UpdateButton(mouse, gameTime))
                        {
                            ChaoticEngine.Player1Active = !ChaoticEngine.Player1Active;
                            ChaoticEngine.CombatThisTurn = false;
                            for (int i = 0; i < creatureSpaces.Length; i++)
                            {
                                if (creatureSpaces[i].CreatureNode != null)
                                {
                                    creatureSpaces[i].CreatureNode.MovedThisTurn = false;
                                    creatureSpaces[i].CreatureNode.RestoreTurn();
                                    creatureSpaces[i].RestoreMoves();
                                }
                            }
                            if (activeLocation1.LocationActive != null || activeLocation2.LocationActive != null)
                                ChaoticEngine.GStage = GameStage.ReturnLocation;
                            else
                                ChaoticEngine.GStage = GameStage.LocationStep;
                        }
                    }
                    break;
                case GameStage.DrawingAttack:
                    if (!ChaoticEngine.IsACardMoving)
                    {
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            discardPile1.UpdateDiscardPile(gameTime);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime);
                    }

                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        if (ChaoticEngine.Player1Strike)
                            done = attackDeck1.UpdateDeckPile(gameTime, mouse, attackHand1);
                        else
                            done = attackDeck2.UpdateDeckPile(gameTime, mouse, attackHand2);
                        if (done)
                        {
                            ChaoticEngine.GStage = GameStage.Combat;
                            attackDeck1.ShuffleDeck(attackDiscardPile1);
                            attackDeck2.ShuffleDeck(attackDiscardPile2);
                        }
                    }
                    break;
                case GameStage.LocationStep:
                    if (ChaoticEngine.Player1Active)
                        done = locationDeck1.UpdateDeckPile(gameTime, activeLocation1);
                    else
                        done = locationDeck2.UpdateDeckPile(gameTime, activeLocation2);
                    if (done)
                        ChaoticEngine.GStage = GameStage.Action;
                    break;
                case GameStage.CreatureToDiscard1:
                    if (ChaoticEngine.Player1Active && 
                        ChaoticEngine.sYouNode.UpdateDiscardCreature(gameTime, mouse, discardPile1))
                    {
                        ChaoticEngine.sYouNode.RemoveCreature();
                        ChaoticEngine.GStage = GameStage.EndOfCombat;
                    }
                    else if (ChaoticEngine.sEnemyNode.UpdateDiscardCreature(gameTime, mouse, discardPile1))
                    {
                        ChaoticEngine.sEnemyNode.RemoveCreature();
                        ChaoticEngine.GStage = GameStage.EndOfCombat;
                    }
                    break;
                case GameStage.CreatureToDiscard2:
                    if (ChaoticEngine.Player1Active &&
                        ChaoticEngine.sEnemyNode.UpdateDiscardCreature(gameTime, mouse, discardPile2))
                    {
                        ChaoticEngine.sEnemyNode.RemoveCreature();
                        ChaoticEngine.GStage = GameStage.EndOfCombat;
                    }
                    else if (ChaoticEngine.sYouNode.UpdateDiscardCreature(gameTime, mouse, discardPile2))
                    {
                        ChaoticEngine.sYouNode.RemoveCreature();
                        ChaoticEngine.GStage = GameStage.EndOfCombat;
                    }
                    break;
                case GameStage.Moving:
                    if (!ChaoticEngine.IsACardMoving && !creatureSpaces[selectedSpace].MouseCoveredCreature)
                    {
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            discardPile1.UpdateDiscardPile(gameTime);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime);
                    }
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        if (creatureSpaces[selectedSpace].UpdateCancel(mouse, gameTime))
                        {
                            for (int i = 0; i < creatureSpaces.Length; i++)
                            {
                                creatureSpaces[i].IsMovableSpace = false;
                            }
                            selectedSpace = -1;
                            ChaoticEngine.GStage = GameStage.Action;
                        }
                        else if (creatureSpaces[selectedSpace].UpdateMovableSpaces(mouse, gameTime, creatureSpaces))
                        {
                            selectedSpace = -1;
                            ChaoticEngine.GStage = GameStage.Action;
                        }
                    }
                    break;
                case GameStage.ReturnLocation:
                    if (activeLocation1.LocationActive != null)
                        done = activeLocation1.ReturnLocationToDeck(gameTime, locationDeck1);
                    else if (activeLocation2.LocationActive != null)
                        done = activeLocation2.ReturnLocationToDeck(gameTime, locationDeck2);
                    if (done)
                    {
                        if (ChaoticEngine.CombatThisTurn)
                            ChaoticEngine.GStage = GameStage.Action;
                        else
                            ChaoticEngine.GStage = GameStage.LocationStep;
                    }
                    break;
                case GameStage.Initiative:
                    if (activeLocation1.LocationActive != null)
                    {
                        int flag = activeLocation1.LocationActive.initiativeCheck(ChaoticEngine.sYouNode.CreatureNode,
                            ChaoticEngine.sEnemyNode.CreatureNode);
                        if (flag > 0)
                            ChaoticEngine.Player1Strike = true;
                        else if (flag == 0)
                            ChaoticEngine.Player1Strike = ChaoticEngine.Player1Active;
                        else
                            ChaoticEngine.Player1Strike = false;
                    }
                    else if (activeLocation2.LocationActive != null)
                    {
                        int flag = activeLocation2.LocationActive.initiativeCheck(ChaoticEngine.sYouNode.CreatureNode,
                            ChaoticEngine.sEnemyNode.CreatureNode);
                        if (flag > 0)
                            ChaoticEngine.Player1Strike = false;
                        else if (flag == 0)
                            ChaoticEngine.Player1Strike = ChaoticEngine.Player1Active;
                        else
                            ChaoticEngine.Player1Strike = true;
                    }
                    ChaoticEngine.sYouNode.CreatureNode.Battlegear.IsFaceUp = true;
                    ChaoticEngine.sEnemyNode.CreatureNode.Battlegear.IsFaceUp = true;
                    ChaoticEngine.GStage = GameStage.Combat;
                    break;
                case GameStage.Combat:
                    if (!ChaoticEngine.IsACardMoving)
                    {
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            discardPile1.UpdateDiscardPile(gameTime);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime);
                    }
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        if (ChaoticEngine.Player1Strike)
                        {
                            if (attackHand1.Count != 3)
                                ChaoticEngine.GStage = GameStage.DrawingAttack;
                            else if (attackHand1.UpdateHand(gameTime, mouse, attackDiscardPile1))
                            {
                                if (ChaoticEngine.Player1Active)
                                    attackDiscardPile1[attackDiscardPile1.Count - 1].Damage(ChaoticEngine.sYouNode.CreatureNode,
                                    ChaoticEngine.sEnemyNode.CreatureNode);
                                else
                                    attackDiscardPile1[attackDiscardPile1.Count - 1].Damage(ChaoticEngine.sEnemyNode.CreatureNode,
                                        ChaoticEngine.sYouNode.CreatureNode);

                                if (attackDiscardPile1[attackDiscardPile1.Count - 1] is Windslash)
                                {
                                    for (int i = 0; i < creatureSpaces.Length; i++)
                                    {
                                        if (ChaoticEngine.Player1Strike != creatureSpaces[i].IsPlayer1 &&
                                            creatureSpaces[i].CreatureNode != null && creatureSpaces[i].HasBattegear())
                                            creatureSpaces[i].CreatureNode.Battlegear.IsFaceUp = true;
                                    }
                                }
                                else if (attackDiscardPile1[attackDiscardPile1.Count - 1] is TornadoTackle &&
                                    ((ChaoticEngine.Player1Active && ChaoticEngine.sYouNode.CreatureNode.Air) ||
                                    (!ChaoticEngine.Player1Active && ChaoticEngine.sEnemyNode.CreatureNode.Air)))
                                {
                                    attackDiscardPile1.DiscardList.AddRange(attackDeck1.Deck);
                                    attackDeck1.Deck.Clear();
                                    attackDeck1.ShuffleDeck(attackDiscardPile1);
                                    attackDiscardPile2.DiscardList.AddRange(attackDeck2.Deck);
                                    attackDeck2.Deck.Clear();
                                    attackDeck2.ShuffleDeck(attackDiscardPile2);
                                }
                                else if (attackDiscardPile1[attackDiscardPile1.Count - 1] is SqueezePlay &&
                                        attackDeck1.Deck.Count > 2)
                                {
                                    ChaoticEngine.GStage = GameStage.Abilities;
                                }
                                else if (attackDiscardPile1[attackDiscardPile1.Count - 1] is FlashKick &&
                                        locationDeck1.Deck.Count > 2)
                                {
                                    ChaoticEngine.GStage = GameStage.Abilities;
                                }

                                ChaoticEngine.Player1Strike = !ChaoticEngine.Player1Strike;
                            }
                        }
                        else
                        {
                            if (attackHand2.Count != 3)
                                ChaoticEngine.GStage = GameStage.DrawingAttack;
                            else if (attackHand2.UpdateHand(gameTime, mouse, attackDiscardPile2))
                            {
                                if (ChaoticEngine.Player1Active)
                                    attackDiscardPile2[attackDiscardPile2.Count - 1].Damage(ChaoticEngine.sEnemyNode.CreatureNode,
                                        ChaoticEngine.sYouNode.CreatureNode);
                                else
                                    attackDiscardPile2[attackDiscardPile2.Count - 1].Damage(ChaoticEngine.sYouNode.CreatureNode,
                                    ChaoticEngine.sEnemyNode.CreatureNode);

                                if (attackDiscardPile2[attackDiscardPile2.Count - 1] is Windslash)
                                {
                                    for (int i = 0; i < creatureSpaces.Length; i++)
                                    {
                                        if (ChaoticEngine.Player1Strike != creatureSpaces[i].IsPlayer1 && creatureSpaces[i].HasBattegear())
                                            creatureSpaces[i].CreatureNode.Battlegear.IsFaceUp = true;
                                    }
                                }
                                else if (attackDiscardPile2[attackDiscardPile2.Count - 1] is TornadoTackle &&
                                    ((ChaoticEngine.Player1Active && ChaoticEngine.sEnemyNode.CreatureNode.Air) ||
                                    (!ChaoticEngine.Player1Active && ChaoticEngine.sYouNode.CreatureNode.Air)))
                                {
                                    attackDiscardPile1.DiscardList.AddRange(attackDeck1.Deck);
                                    attackDeck1.Deck.Clear();
                                    attackDeck1.ShuffleDeck(attackDiscardPile1);
                                    attackDiscardPile2.DiscardList.AddRange(attackDeck2.Deck);
                                    attackDeck2.Deck.Clear();
                                    attackDeck2.ShuffleDeck(attackDiscardPile2);
                                }
                                else if (attackDiscardPile2[attackDiscardPile2.Count - 1] is SqueezePlay &&
                                        attackDeck2.Deck.Count > 2)
                                {
                                    ChaoticEngine.GStage = GameStage.Abilities;
                                }
                                ChaoticEngine.Player1Strike = !ChaoticEngine.Player1Strike;
                            }
                        }

                        if (ChaoticEngine.sYouNode.CreatureNode.Energy == 0 || ChaoticEngine.sEnemyNode.CreatureNode.Energy == 0)
                        {
                            ChaoticEngine.GStage = GameStage.EndOfCombat;
                            ChaoticEngine.CombatThisTurn = true;
                        }
                    }
                    break;
                case GameStage.EndOfCombat:
                    if (ChaoticEngine.sYouNode.CreatureNode != null && ChaoticEngine.sYouNode.CreatureNode.Energy == 0)
                    {
                        if (ChaoticEngine.Player1Active)
                            ChaoticEngine.GStage = GameStage.CreatureToDiscard1;
                        else
                            ChaoticEngine.GStage = GameStage.CreatureToDiscard2;
                    }
                    else if (ChaoticEngine.sEnemyNode.CreatureNode != null && ChaoticEngine.sEnemyNode.CreatureNode.Energy == 0)
                    {
                        if (ChaoticEngine.Player1Active)
                            ChaoticEngine.GStage = GameStage.CreatureToDiscard2;
                        else
                            ChaoticEngine.GStage = GameStage.CreatureToDiscard1;
                    }
                    else if (ChaoticEngine.sEnemyNode.CreatureNode == null && ChaoticEngine.sYouNode.CreatureNode != null)
                    {
                        ChaoticEngine.GStage = GameStage.MoveToCodedSpace;
                        for (int i = 0; i < creatureSpaces.Length; i++)
                        {
                            if (creatureSpaces[i] == ChaoticEngine.sEnemyNode)
                                selectedSpace = i;
                        }
                        ChaoticEngine.sYouNode.CreatureNode.RestoreCombat();
                    }
                    else
                    {
                        ChaoticEngine.sEnemyNode.CreatureNode.RestoreCombat();
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
                    }
                    ChaoticEngine.sYouNode.IsMovableSpace = false;
                    ChaoticEngine.sEnemyNode.IsMovableSpace = false;
                    break;
                case GameStage.MoveToCodedSpace:
                    if (ChaoticEngine.sYouNode.UpdateMoveCreature(gameTime, mouse, creatureSpaces[selectedSpace]))
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
                    break;
                case GameStage.ChangeLocation:

                    break;
                case GameStage.Abilities:
                    if (!ChaoticEngine.Player1Strike)
                    {
                        if (attackDiscardPile1[attackDiscardPile1.Count - 1] is SqueezePlay)
                        {
                            if (!attackPanel1.Active)
                            {
                                topTwo = new List<Attack>(){
                                attackDeck1.RetrieveCard(attackDeck1.Count - 1),
                                attackDeck1.RetrieveCard(attackDeck1.Count - 2)};
                                attackPanel1.Active = true;
                            }
                            List<int> indices = attackPanel1.UpdatePanel(gameTime, topTwo);
                            if (indices != null)
                            {
                                attackDeck1.Deck.RemoveAt(attackDeck1.Count - 1);
                                attackDeck1.Deck.RemoveAt(attackDeck1.Count - 1);

                                attackDeck1.Deck.Add(topTwo[indices[0]]);
                                attackDeck1.Deck.Insert(0, topTwo[indices[1]]);

                                ChaoticEngine.GStage = GameStage.Combat;
                                topTwo = null;
                            }
                        }
                        else if (attackDiscardPile1[attackDiscardPile1.Count - 1] is FlashKick)
                        {
                            if (!locationPanel.Active)
                            {
                                topTwoLoc = new List<Location>(){
                                locationDeck1.RetrieveCard(locationDeck1.Count - 1),
                                locationDeck1.RetrieveCard(locationDeck1.Count - 2)};
                                locationPanel.Active = true;
                            }
                            List<int> indices = locationPanel.UpdatePanel(gameTime, topTwoLoc);
                            if (indices != null)
                            {
                                locationDeck1.Deck.RemoveAt(locationDeck1.Count - 1);
                                locationDeck1.Deck.RemoveAt(locationDeck1.Count - 1);

                                locationDeck1.Deck.Add(topTwoLoc[indices[0]]);
                                locationDeck1.Deck.Insert(0, topTwoLoc[indices[1]]);

                                ChaoticEngine.GStage = GameStage.Combat;
                                topTwoLoc = null;
                            }
                        }
                    }
                    else
                    {
                        if (attackDiscardPile2[attackDiscardPile2.Count - 1] is SqueezePlay)
                        {
                            if (!attackPanel1.Active)
                            {
                                topTwo = new List<Attack>(){
                                attackDeck2.RetrieveCard(attackDeck2.Count - 1),
                                attackDeck2.RetrieveCard(attackDeck2.Count - 2)};
                                attackPanel1.Active = true;
                            }
                            List<int> indices = attackPanel1.UpdatePanel(gameTime, topTwo);
                            if (indices != null)
                            {
                                attackDeck2.Deck.RemoveAt(attackDeck2.Deck.Count - 1);
                                attackDeck2.Deck.RemoveAt(attackDeck2.Deck.Count - 1);

                                attackDeck2.Deck.Add(topTwo[indices[0]]);
                                attackDeck2.Deck.Insert(0, topTwo[indices[1]]);

                                ChaoticEngine.GStage = GameStage.Combat;
                                topTwo = null;
                            }
                        }
                    }
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            backButton.Draw(spriteBatch);
            attackPanel1.DrawPanel(spriteBatch, topTwo);
            locationPanel.DrawPanel(spriteBatch, topTwoLoc);
            discardPile1.DrawDiscardPile(spriteBatch);
            discardPile2.DrawDiscardPile(spriteBatch);
            attackDiscardPile1.DrawDiscardPile(spriteBatch);
            attackDiscardPile2.DrawDiscardPile(spriteBatch);
            attackDeck1.DrawDeckPile(spriteBatch);
            attackDeck2.DrawDeckPile(spriteBatch);
            attackHand1.DrawHand(spriteBatch);
            attackHand2.DrawHand(spriteBatch);
            locationDeck1.DrawDeckPile(spriteBatch);
            locationDeck2.DrawDeckPile(spriteBatch);
            activeLocation1.DrawActiveLocation(spriteBatch, true);
            activeLocation2.DrawActiveLocation(spriteBatch, false);
    
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                creatureSpaces[i].DrawBattleBoardNode(spriteBatch, cardBack);
            }
            if ((activeLocation1.LocationActive != null && activeLocation1.LocationActive.Texture == ChaoticEngine.CoveredCard) ||
                (activeLocation2.LocationActive != null && activeLocation2.LocationActive.Texture == ChaoticEngine.CoveredCard) ||
                locationPanel.Active && (topTwoLoc[0].Texture == ChaoticEngine.CoveredCard || topTwoLoc[1].Texture == ChaoticEngine.CoveredCard))
                spriteBatch.Draw(ChaoticEngine.CoveredCard, new Rectangle(3, 6, 245, 175), Color.White);
            else
                spriteBatch.Draw(ChaoticEngine.CoveredCard, new Rectangle(3, 6, 175, 245), Color.White);
            spriteBatch.Draw(ChaoticEngine.BackgroundSprite, backgroundRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            switch (ChaoticEngine.GStage)
            {
                case GameStage.BeginningOfTheGame:
                    break;
                case GameStage.CoinFlip:
                    break;
                case GameStage.LocationStep:
                    break;
                case GameStage.Moving:
                    break;
                case GameStage.CreatureToDiscard1:
                    break;
                case GameStage.Action:
                    if (creatureSpaces.Count(b => b.CreatureNode != null && b.CreatureNode.MovedThisTurn == true) > 0 ||
                        ChaoticEngine.CombatThisTurn)
                        endTurnButton.Draw(spriteBatch);
                    break;
                case GameStage.Combat:
                    if (ChaoticEngine.Player1Active)
                    {
                        attackHand1.ProjectHandDamage(spriteBatch, ChaoticEngine.sYouNode.CreatureNode,
                        ChaoticEngine.sEnemyNode.CreatureNode);
                        attackHand2.ProjectHandDamage(spriteBatch, ChaoticEngine.sEnemyNode.CreatureNode,
                            ChaoticEngine.sYouNode.CreatureNode);
                    }
                    else
                    {
                        attackHand1.ProjectHandDamage(spriteBatch, ChaoticEngine.sEnemyNode.CreatureNode,
                           ChaoticEngine.sYouNode.CreatureNode);
                        attackHand2.ProjectHandDamage(spriteBatch, ChaoticEngine.sYouNode.CreatureNode,
                       ChaoticEngine.sEnemyNode.CreatureNode);
                    }
                    break;
                default:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}