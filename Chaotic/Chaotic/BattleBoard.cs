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
    CreatureToDiscard2, BattlegearToDiscard1, BattlegearToDiscard2, Action, Combat, Initiative, EndOfCombat, MoveToCodedSpace,
    SelectingAbilities, ChangeLocation, ShuffleAtkDeck1, ShuffleAtkDeck2, ShuffleAtkDecks, EndGame, BeginningOfCombat1,
    BeginningOfCombat2, LocationStepCombat, ReturnLocationCombat,
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
        bool player1Won;
        byte numDrawed;

        GameStage nextStage;

        SpriteFont endgamefont;
        SpriteFont hubFont;

        public BattleBoard(Game game, GraphicsDeviceManager graphics, CreatureNumber creatureNumber)
            : base(game)
        {
            this.graphics = graphics;
            this.creatureNumber = creatureNumber;
            this.numDrawed = 0;
            ChaoticEngine.GStage = GameStage.BeginningOfTheGame;
            ChaoticEngine.Hive = false;
            ChaoticEngine.PrevHive = false;
            ChaoticEngine.Player1Active = true;
            ChaoticEngine.CombatThisTurn = false;
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
            endgamefont = Game.Content.Load<SpriteFont>("Fonts/EndGame");
            hubFont = Game.Content.Load<SpriteFont>("Fonts/HUB");
            base.LoadContent();
        }

        private void updateSupport()
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode is ISupport)
                {
                    byte numAdjacent = creatureSpaces[i].NumAdjacentOverWorldCreatures(creatureSpaces);
                    (creatureSpaces[i].CreatureNode as ISupport).Ability(numAdjacent);
                }
            }
        }

        private void updateHive()
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode is IHive)
                {
                    byte numMandiblor = creatureSpaces[i].NumMandiblors(creatureSpaces);
                    if (ChaoticEngine.Hive && !ChaoticEngine.PrevHive)
                        (creatureSpaces[i].CreatureNode as IHive).HiveOn(numMandiblor);
                    else if (!ChaoticEngine.Hive && ChaoticEngine.PrevHive)
                        (creatureSpaces[i].CreatureNode as IHive).HiveOff(numMandiblor);
                }
            }
            if (ChaoticEngine.Hive && !ChaoticEngine.PrevHive)
                ChaoticEngine.PrevHive = true;
            else if (!ChaoticEngine.Hive && ChaoticEngine.PrevHive)
                ChaoticEngine.PrevHive = false;
        }

        private void creatureToDiscard(GameTime gameTime, MouseState mouse, BattleBoardNode you, BattleBoardNode enemy,
            DiscardPile<ChaoticCard> discardPile, GameStage cStage, GameStage bStage)
        {
            if ((ChaoticEngine.Player1Active && you.HasBattegear()) ||
                (!ChaoticEngine.Player1Active && enemy.HasBattegear()))
            {
                nextStage = cStage;
                ChaoticEngine.GStage = bStage;
            }
            else
            {
                if (ChaoticEngine.Player1Active &&
                you.UpdateDiscardCreature(gameTime, mouse, discardPile))
                {
                    you.RemoveCreature();
                    updateSupport();
                    ChaoticEngine.GStage = GameStage.EndOfCombat;
                }
                else if (!ChaoticEngine.Player1Active &&
                    enemy.UpdateDiscardCreature(gameTime, mouse, discardPile))
                {
                    enemy.RemoveCreature();
                    updateSupport();
                    ChaoticEngine.GStage = GameStage.EndOfCombat;
                }
                if (ChaoticEngine.GStage == GameStage.EndOfCombat)
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode is IHive && ChaoticEngine.Hive)
                        {
                            byte numMandiblor = creatureSpaces[i].NumMandiblors(creatureSpaces);
                            (creatureSpaces[i].CreatureNode as IHive).HiveOn(numMandiblor);
                        }
                        else if (creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.OduBathax)
                        {
                            byte numMandiblor = creatureSpaces[i].NumMandiblors(creatureSpaces);
                            (creatureSpaces[i].CreatureNode as ChaoticGameLib.Creatures.OduBathax).Ability(numMandiblor);
                        }
                    }
                }
            }
        }

        private void battlegearToDiscard(GameTime gameTime, MouseState mouse, BattleBoardNode you, BattleBoardNode enemy,
            DiscardPile<ChaoticCard> discardPile)
        {
            if (ChaoticEngine.Player1Active && you.UpdateDiscardBattlegear(gameTime, mouse, discardPile))
            {
                you.RemoveBattlegear();
                ChaoticEngine.GStage = nextStage;
            }
            else if (!ChaoticEngine.Player1Active && enemy.UpdateDiscardBattlegear(gameTime, mouse, discardPile))
            {
                enemy.RemoveBattlegear();
                ChaoticEngine.GStage = nextStage;
            }
        }

        private void locationStep(GameTime gameTime, bool player1Active, GameStage next)
        {
            if (player1Active)
                done = locationDeck1.UpdateDeckPile(gameTime, activeLocation1);
            else
                done = locationDeck2.UpdateDeckPile(gameTime, activeLocation2);
            if (done)
            {
                Location loc = activeLocation1.LocationActive != null ? activeLocation1.LocationActive : activeLocation2.LocationActive;
                if (loc is ChaoticGameLib.Locations.LavaPond)
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.Magmon)
                            creatureSpaces[i].CreatureNode.FireDamage += 5;
                    }
                }
                else if (loc is ChaoticGameLib.Locations.GothosTower)
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode != null)
                        {
                            if (!(creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.LordVanBloot))
                                creatureSpaces[i].CreatureNode.Courage -= 10;
                            else
                                creatureSpaces[i].CreatureNode.Strike += 15;
                        }
                    }
                }
                else if (loc is ChaoticGameLib.Locations.MountPillar)
                    ChaoticEngine.Hive = true;

                ChaoticEngine.GStage = next;
            }
        }

        private void returnLocation(GameTime gameTime, GameStage combatStage, GameStage locStep)
        {
            if (activeLocation1.LocationActive != null)
                done = activeLocation1.ReturnLocationToDeck(gameTime, locationDeck1);
            else if (activeLocation2.LocationActive != null)
                done = activeLocation2.ReturnLocationToDeck(gameTime, locationDeck2);
            if (done)
            {
                if (creatureSpaces.Count(b => b.CreatureNode != null && b.IsPlayer1 == true) == 0)
                {
                    player1Won = false;
                    ChaoticEngine.GStage = GameStage.EndGame;
                }
                else if (creatureSpaces.Count(b => b.CreatureNode != null && b.IsPlayer1 == false) == 0)
                {
                    player1Won = true;
                    ChaoticEngine.GStage = GameStage.EndGame;
                }
                else if (ChaoticEngine.CombatThisTurn)
                    ChaoticEngine.GStage = combatStage;
                else
                    ChaoticEngine.GStage = locStep;
            }
        }

        private void updateCombat(GameTime gameTime, MouseState mouse, AttackHand attackHand, AttackDeck attackDeck, 
            LocationDeck locationDeck, DiscardPile<Attack> attackDiscardPile, Creature your, Creature enemy, bool player1=true)
        {
            if (attackHand.Count != 3)
                ChaoticEngine.GStage = GameStage.DrawingAttack;
            else if (attackHand.UpdateHand(gameTime, mouse, attackDiscardPile))
            {
                Location activeLocation = activeLocation1.LocationActive;
                if (activeLocation == null)
                    activeLocation = activeLocation2.LocationActive;
                if (ChaoticEngine.Player1Active)
                    attackDiscardPile[attackDiscardPile.Count - 1].Damage(your, enemy, activeLocation);
                else
                    attackDiscardPile[attackDiscardPile.Count - 1].Damage(enemy, your, activeLocation);

                if (attackDiscardPile[attackDiscardPile.Count - 1] is Windslash)
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (ChaoticEngine.Player1Strike != creatureSpaces[i].IsPlayer1 &&
                            creatureSpaces[i].CreatureNode != null)
                            creatureSpaces[i].CreatureNode.ActivateBattlegear();
                    }
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is TornadoTackle &&
                    ((ChaoticEngine.Player1Active && your.Air) ||
                    (!ChaoticEngine.Player1Active && enemy.Air)))
                {
                    attackDiscardPile1.DiscardList.AddRange(attackDeck1.Deck);
                    attackDeck1.Deck.Clear();
                    attackDeck1.ShuffleDeck(attackDiscardPile1);
                    attackDiscardPile2.DiscardList.AddRange(attackDeck2.Deck);
                    attackDeck2.Deck.Clear();
                    attackDeck2.ShuffleDeck(attackDiscardPile2);
                    ChaoticEngine.GStage = GameStage.ShuffleAtkDecks;
                    nextStage = GameStage.Combat;
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is SqueezePlay &&
                        attackDeck.Deck.Count > 1)
                {
                    ChaoticEngine.GStage = GameStage.SelectingAbilities;
                    nextStage = GameStage.Combat;
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is FlashKick &&
                        locationDeck.Deck.Count > 1)
                {
                    ChaoticEngine.GStage = GameStage.SelectingAbilities;
                    nextStage = GameStage.Combat;
                }

                else if (attackDiscardPile[attackDiscardPile.Count - 1] is CoilCrush &&
                    ((ChaoticEngine.Player1Active && your.Power >= 75
                        && enemy.Battlegear != null) ||
                    (!ChaoticEngine.Player1Active && enemy.Power >= 75
                        && your.Battlegear != null)))
                {
                    ChaoticEngine.GStage = player1 ? GameStage.BattlegearToDiscard2 : GameStage.BattlegearToDiscard1;
                    nextStage = GameStage.Combat;
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is Mirthquake &&
                    ((ChaoticEngine.Player1Active && your.Earth) ||
                    (!ChaoticEngine.Player1Active && enemy.Earth)))
                {
                    if (ChaoticEngine.Player1Active)
                        your.Earth = your.EarthCombat = false;
                    else
                        enemy.Earth = enemy.EarthCombat = false;
                    ChaoticEngine.GStage = GameStage.ReturnLocationCombat;
                    nextStage = GameStage.Combat;
                }

                ChaoticEngine.Player1Strike = !ChaoticEngine.Player1Strike;
            }
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

            if (ChaoticEngine.Hive != ChaoticEngine.PrevHive)
                updateHive();

            switch (ChaoticEngine.GStage)
            {
                case GameStage.BeginningOfTheGame:
                    attackDeck1.UpdateDeckPile(gameTime, attackHand1);
                    done = attackDeck2.UpdateDeckPile(gameTime, attackHand2);
                    if (done && numDrawed >= 1)
                    {
                        ChaoticEngine.GStage = GameStage.LocationStep;
                        for (int i = 0; i < creatureSpaces.Length; i++)
                        {
                            if (creatureSpaces[i].CreatureNode.Battlegear.RevealAtBeginning)
                                creatureSpaces[i].CreatureNode.ActivateBattlegear();
                           
                            if (creatureSpaces[i].CreatureNode is ISupport)
                            {
                                byte numAdjacent = creatureSpaces[i].NumAdjacentOverWorldCreatures(creatureSpaces);
                                (creatureSpaces[i].CreatureNode as ISupport).Ability(numAdjacent);
                            }
                            else if (creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.OduBathax)
                            {
                                byte numMandiblor = creatureSpaces[i].NumMandiblors(creatureSpaces);
                                (creatureSpaces[i].CreatureNode as ChaoticGameLib.Creatures.OduBathax).Ability(numMandiblor);
                            }
                            creatureSpaces[i].RestoreMoves();
                        }
                    }
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
                            ChaoticEngine.Hive = false;
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
                            if (attackDeck1.Count == 0)
                            {
                                ChaoticEngine.GStage = GameStage.ShuffleAtkDeck1;
                                nextStage = GameStage.Combat;
                                attackDeck1.ShuffleDeck(attackDiscardPile1);
                            }
                            else if (attackDeck2.Count == 0)
                            {
                                ChaoticEngine.GStage = GameStage.ShuffleAtkDeck2;
                                nextStage = GameStage.Combat;
                                attackDeck2.ShuffleDeck(attackDiscardPile2);
                            }
                            else
                                ChaoticEngine.GStage = GameStage.Combat;
                        }
                    }
                    break;
                case GameStage.LocationStep:
                    locationStep(gameTime, ChaoticEngine.Player1Active, GameStage.Action);
                    break;
                case GameStage.LocationStepCombat:
                    locationStep(gameTime, !ChaoticEngine.Player1Strike, nextStage);
                    break;
                case GameStage.CreatureToDiscard1:
                    creatureToDiscard(gameTime, mouse, ChaoticEngine.sYouNode, ChaoticEngine.sEnemyNode, discardPile1, 
                        GameStage.CreatureToDiscard1, GameStage.BattlegearToDiscard1);
                    break;
                case GameStage.CreatureToDiscard2:
                    creatureToDiscard(gameTime, mouse, ChaoticEngine.sEnemyNode, ChaoticEngine.sYouNode, discardPile2,
                        GameStage.CreatureToDiscard2, GameStage.BattlegearToDiscard2);
                    break;
                case GameStage.BattlegearToDiscard1:
                    battlegearToDiscard(gameTime, mouse, ChaoticEngine.sYouNode, ChaoticEngine.sEnemyNode, discardPile1);
                    break;
                case GameStage.BattlegearToDiscard2:
                    battlegearToDiscard(gameTime, mouse, ChaoticEngine.sEnemyNode, ChaoticEngine.sYouNode, discardPile2);
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
                            updateSupport();
                        }
                    }
                    break;
                case GameStage.ReturnLocation:
                    returnLocation(gameTime, GameStage.Action, GameStage.LocationStep);
                    break;
                case GameStage.ReturnLocationCombat:
                    returnLocation(gameTime, GameStage.LocationStepCombat, GameStage.LocationStepCombat);
                    break;
                case GameStage.Initiative:
                    ChaoticEngine.sYouNode.CreatureNode.ActivateBattlegear();
                    ChaoticEngine.sEnemyNode.CreatureNode.ActivateBattlegear();
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
                    
                    ChaoticEngine.GStage = GameStage.BeginningOfCombat1;
                    break;
                case GameStage.BeginningOfCombat1:
                    Location locActive = activeLocation1.LocationActive != null ? activeLocation1.LocationActive : activeLocation2.LocationActive;
                    if (locActive is ChaoticGameLib.Locations.CordacFalls)
                    {
                        ChaoticEngine.sYouNode.CreatureNode.Energy += 5;
                        ChaoticEngine.sYouNode.CreatureNode.GainedEnergyTurn += 5;
                        ChaoticEngine.sEnemyNode.CreatureNode.Energy += 5;
                        ChaoticEngine.sEnemyNode.CreatureNode.GainedEnergyTurn += 5;
                    }
                    else if (locActive is ChaoticGameLib.Locations.CordacFallsPlungepool)
                    {
                        ChaoticEngine.sYouNode.CreatureNode.Energy -= 5;
                        ChaoticEngine.sEnemyNode.CreatureNode.Energy -= 5;
                    }
                    else if (locActive is ChaoticGameLib.Locations.CastlePillar)
                    {
                        if (ChaoticEngine.sYouNode.CreatureNode.Wisdom > ChaoticEngine.sEnemyNode.CreatureNode.Wisdom)
                            ChaoticEngine.sYouNode.CreatureNode.MugicCounters += 1;
                        else if (ChaoticEngine.sYouNode.CreatureNode.Wisdom < ChaoticEngine.sEnemyNode.CreatureNode.Wisdom)
                            ChaoticEngine.sEnemyNode.CreatureNode.MugicCounters += 1;
                    }
                    else if (locActive is ChaoticGameLib.Locations.DoorsOfTheDeepmines)
                    {
                        for (int i = 0; i < creatureSpaces.Length; i++)
                        {
                            if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].CreatureNode.Water)
                            {
                                creatureSpaces[i].CreatureNode.Energy += 10;
                                creatureSpaces[i].CreatureNode.GainedEnergyTurn += 10;
                            }
                        }
                    }
                    else if (locActive is ChaoticGameLib.Locations.FearValley)
                    {
                        if (ChaoticEngine.sYouNode.CreatureNode.Courage < ChaoticEngine.sEnemyNode.CreatureNode.Courage)
                            ChaoticEngine.sYouNode.CreatureNode.Energy -= 10;
                        else if (ChaoticEngine.sYouNode.CreatureNode.Courage > ChaoticEngine.sEnemyNode.CreatureNode.Courage)
                            ChaoticEngine.sEnemyNode.CreatureNode.Energy -= 10;
                    }
                    else if (locActive is ChaoticGameLib.Locations.ForestOfLife)
                    {
                        if (ChaoticEngine.sYouNode.CreatureNode.Power > ChaoticEngine.sEnemyNode.CreatureNode.Power)
                        {
                            ChaoticEngine.sYouNode.CreatureNode.Energy += 5;
                            ChaoticEngine.sYouNode.CreatureNode.GainedEnergyTurn += 5;
                        }
                        else if (ChaoticEngine.sYouNode.CreatureNode.Power < ChaoticEngine.sEnemyNode.CreatureNode.Power)
                        {
                            ChaoticEngine.sEnemyNode.CreatureNode.Energy += 5;
                            ChaoticEngine.sEnemyNode.CreatureNode.GainedEnergyTurn += 5;
                        }
                    }
                    else if (locActive is ChaoticGameLib.Locations.Gigantempopolis)
                    {
                        if (ChaoticEngine.sYouNode.CreatureNode.CreatureTribe == Tribe.OverWorld)
                            ChaoticEngine.sYouNode.CreatureNode.MugicCounters += 1;
                        if (ChaoticEngine.sEnemyNode.CreatureNode.CreatureTribe == Tribe.OverWorld)
                            ChaoticEngine.sEnemyNode.CreatureNode.MugicCounters += 1;
                    }
                    else if (locActive is ChaoticGameLib.Locations.StrongholdMorn)
                    {
                        ChaoticEngine.sYouNode.CreatureNode.Fire = ChaoticEngine.sYouNode.CreatureNode.Air =
                            ChaoticEngine.sYouNode.CreatureNode.Earth = ChaoticEngine.sYouNode.CreatureNode.Water = true;
                        ChaoticEngine.sEnemyNode.CreatureNode.Fire = ChaoticEngine.sEnemyNode.CreatureNode.Air =
                            ChaoticEngine.sEnemyNode.CreatureNode.Earth = ChaoticEngine.sEnemyNode.CreatureNode.Water = true;
                    }
                    else if (locActive is ChaoticGameLib.Locations.KiruCity)
                    {
                        for (int i = 0; i < creatureSpaces.Length; i++)
                        {
                            if (creatureSpaces[i].CreatureNode != null && 
                                creatureSpaces[i].CreatureNode.CreatureTribe == Tribe.OverWorld)
                            {
                                creatureSpaces[i].CreatureNode.Energy += 10;
                                creatureSpaces[i].CreatureNode.GainedEnergyTurn += 10;
                            }
                        }
                    }
                    else if (locActive is ChaoticGameLib.Locations.RavanaughRidge)
                    {
                        //TODO: Implement.
                    }

                    ChaoticEngine.GStage = GameStage.Combat;
                    break;
                //case GameStage.BeginningOfCombat2:
                //    if (ChaoticEngine.sYouNode.HasBattegear() &&
                //        ChaoticEngine.sYouNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.OrbOfForesight)
                //    {
                //        ChaoticEngine.GStage = GameStage.SelectingAbilities;
                //        nextStage = GameStage.BeginningOfCombat2;
                //    }
                //    ChaoticEngine.GStage = GameStage.Combat;
                //    break;
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
                            updateCombat(gameTime, mouse, attackHand1, attackDeck1, locationDeck1, attackDiscardPile1,
                                ChaoticEngine.sYouNode.CreatureNode, ChaoticEngine.sEnemyNode.CreatureNode);
                        }
                        else
                        {
                            updateCombat(gameTime, mouse, attackHand2, attackDeck2, locationDeck2, attackDiscardPile2,
                                ChaoticEngine.sEnemyNode.CreatureNode, ChaoticEngine.sYouNode.CreatureNode, false);
                        }

                        if (ChaoticEngine.sYouNode.CreatureNode.Energy == 0 || ChaoticEngine.sEnemyNode.CreatureNode.Energy == 0)
                        {
                            if (ChaoticEngine.GStage == GameStage.ShuffleAtkDecks ||
                                ChaoticEngine.GStage == GameStage.SelectingAbilities ||
                                ChaoticEngine.GStage == GameStage.BattlegearToDiscard1 ||
                                ChaoticEngine.GStage == GameStage.BattlegearToDiscard2 ||
                                ChaoticEngine.GStage == GameStage.ReturnLocationCombat)
                                nextStage = GameStage.EndOfCombat;
                            else
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
                    {
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
                        creatureSpaces[selectedSpace].CreatureNode.MovedThisTurn = true;
                        updateSupport();
                    }
                    break;
                case GameStage.SelectingAbilities:
                    if (!ChaoticEngine.Player1Strike)
                    {
                        selectingAbilities(gameTime, attackDeck1, locationDeck1, attackDiscardPile1);
                    }
                    else
                    {
                        selectingAbilities(gameTime, attackDeck2, locationDeck2, attackDiscardPile2);
                    }
                    break;
                case GameStage.ShuffleAtkDeck1:
                    if (attackDeck1.UpdateShuffleDeck(gameTime))
                        ChaoticEngine.GStage = nextStage;
                    break;
                case GameStage.ShuffleAtkDeck2:
                    if (attackDeck2.UpdateShuffleDeck(gameTime))
                        ChaoticEngine.GStage = nextStage;
                    break;
                case GameStage.ShuffleAtkDecks:
                    if (attackDeck1.UpdateShuffleDeck(gameTime))
                        ChaoticEngine.GStage = GameStage.ShuffleAtkDeck2;
                    break;
                case GameStage.EndGame:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        ChaoticEngine.MStage = MenuStage.MainMenu;
                        Game.Components.Remove(this);
                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void selectingAbilities(GameTime gameTime, AttackDeck attackDeck, LocationDeck locationDeck,
            DiscardPile<Attack> attackDiscardPile)
        {
            if (attackDiscardPile[attackDiscardPile.Count - 1] is SqueezePlay)
            {
                if (!attackPanel1.Active)
                {
                    topTwo = new List<Attack>(){
                                attackDeck.RetrieveCard(attackDeck.Count - 1),
                                attackDeck.RetrieveCard(attackDeck.Count - 2)};
                    attackPanel1.Active = true;
                }
                List<int> indices = attackPanel1.UpdatePanel(gameTime, topTwo);
                if (indices != null)
                {
                    attackDeck.Deck.RemoveAt(attackDeck.Count - 1);
                    attackDeck.Deck.RemoveAt(attackDeck.Count - 1);

                    attackDeck.Deck.Add(topTwo[indices[0]]);
                    attackDeck.Deck.Insert(0, topTwo[indices[1]]);

                    ChaoticEngine.GStage = nextStage;
                    topTwo = null;
                }
            }
            else if (attackDiscardPile[attackDiscardPile.Count - 1] is FlashKick)
            {
                if (!locationPanel.Active)
                {
                    topTwoLoc = new List<Location>(){
                                locationDeck.RetrieveCard(locationDeck.Count - 1),
                                locationDeck.RetrieveCard(locationDeck.Count - 2)};
                    locationPanel.Active = true;
                }
                List<int> indices = locationPanel.UpdatePanel(gameTime, topTwoLoc);
                if (indices != null)
                {
                    locationDeck.Deck.RemoveAt(locationDeck.Count - 1);
                    locationDeck.Deck.RemoveAt(locationDeck.Count - 1);

                    locationDeck.Deck.Add(topTwoLoc[indices[0]]);
                    locationDeck.Deck.Insert(0, topTwoLoc[indices[1]]);

                    ChaoticEngine.GStage = nextStage;
                    topTwoLoc = null;
                }
            }
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
            attackHand1.DrawHand(spriteBatch);
            attackHand2.DrawHand(spriteBatch);
            locationDeck1.DrawDeckPile(spriteBatch);
            locationDeck2.DrawDeckPile(spriteBatch);
            activeLocation1.DrawActiveLocation(spriteBatch, true);
            activeLocation2.DrawActiveLocation(spriteBatch, false);

            if (ChaoticEngine.GStage != GameStage.ShuffleAtkDeck1 && ChaoticEngine.GStage != GameStage.ShuffleAtkDecks)
            {
                attackDeck1.DrawDeckPile(spriteBatch);
            }

            if (ChaoticEngine.GStage != GameStage.ShuffleAtkDeck2)
                attackDeck2.DrawDeckPile(spriteBatch);
    
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

            string hiveText = "Hive: " + (ChaoticEngine.Hive ? "On" : "Off");
            spriteBatch.DrawString(hubFont, hiveText, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                - endgamefont.MeasureString(hiveText).X / 2, graphics.PreferredBackBufferHeight / 2
                - endgamefont.MeasureString(hiveText).Y / 2), Color.Black);

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
                    Location loc = activeLocation1.LocationActive != null ? activeLocation1.LocationActive : activeLocation2.LocationActive;
                    if (ChaoticEngine.Player1Active)
                    {
                        attackHand1.ProjectHandDamage(spriteBatch, ChaoticEngine.sYouNode.CreatureNode,
                        ChaoticEngine.sEnemyNode.CreatureNode, loc);
                        attackHand2.ProjectHandDamage(spriteBatch, ChaoticEngine.sEnemyNode.CreatureNode,
                            ChaoticEngine.sYouNode.CreatureNode, loc);
                    }
                    else
                    {
                        attackHand1.ProjectHandDamage(spriteBatch, ChaoticEngine.sEnemyNode.CreatureNode,
                           ChaoticEngine.sYouNode.CreatureNode, loc);
                        attackHand2.ProjectHandDamage(spriteBatch, ChaoticEngine.sYouNode.CreatureNode,
                       ChaoticEngine.sEnemyNode.CreatureNode, loc);
                    }
                    break;
                case GameStage.ShuffleAtkDeck1:
                    attackDeck1.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.ShuffleAtkDeck2:
                    attackDeck2.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.ShuffleAtkDecks:
                    attackDeck1.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.EndGame:
                    string endText = (player1Won ? "Player 1 " : "Player 2 ") + "is the winner. (Press Enter)";
                    spriteBatch.DrawString(endgamefont, endText, new Vector2(graphics.PreferredBackBufferWidth / 2
                        - endgamefont.MeasureString(endText).X / 2, graphics.PreferredBackBufferHeight / 2
                        - endgamefont.MeasureString(endText).Y / 2), Color.Blue);
                    break;
                default:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}