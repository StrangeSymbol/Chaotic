/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;
using ChaoticGameLib.Attacks;

namespace Chaotic
{
    public enum CreatureNumber { SixOnSix, ThreeOnThree, OneOnOne };
    enum GameStage { Shuffles, BeginningOfTheGame, CoinFlip, LocationStep, Moving, ReturnLocation, 
    CreatureToDiscard1, CreatureToDiscard2, BattlegearToDiscard1, BattlegearToDiscard2, Action, Combat, Initiative, EndOfCombat,
    MoveToCodedSpace, AttackSelectAbility, ChangeLocation, ShuffleAtkDeck1, ShuffleAtkDeck2, EndGame, BeginningOfCombat,
    LocationStepCombat, ReturnLocationCombat, LocationSelectAbility1, LocationSelectAbility2, 
    BattlegearSelectAbility1, BattlegearSelectAbility2, SelectingCreature1, SelectingCreature2, Showdown, ShowdownCoinFlip,
    ShuffleLocDeck1, ShuffleLocDeck2, SelectMugic1, SelectMugic2, ReturnMugic1, ReturnMugic2, DiscardMugic1, DiscardMugic2,
    SelCreatureToDiscard, SelBattlegearToDiscard, TargetCreature, SelMugicInDiscard, SelCreatureInDiscard, TargetBattlegear,
    SelAttackDeck, SelLocationDeck, SelAttackLocationDeck, SelElemental, SelReturnMugic, SelReturnCreature, DecideToBurst,
    AddingToBurst, RunBurst, HighLight, SelLocationAny, SelLocationOrder, SelAttackLocationOrder, SelUnoccupiedSpace,
    SelCreatureReturn, AttackDamage, DiscardCreatures, StrikePhase, TargetEngaged, TriggeredSelectAP, TriggeredSelectDP,
    HiveCallDecision, TargetAttack, ChangeLocationStep,};

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
        MugicHand mugicHand1;
        MugicHand mugicHand2;
        LocationDeck locationDeck1;
        LocationDeck locationDeck2;
        ActiveLocation activeLocation1;
        ActiveLocation activeLocation2;
        int selectedSpace = -1;

        Button endTurnButton;
        Button backButton;

        Coin coin;

        Texture2D cardBack;

        Rectangle backgroundRect;

        SelectPanel<Attack> attackPanel1;
        SelectPanel<Attack> attackPanel2;
        List<Attack> topAtk;
        SelectPanel<Location> locationPanel1;
        SelectPanel<Location> locationAnyPanel;
        List<Location> topLoc;
        SelectPanel<Mugic> mugicPanel;
        List<Mugic> discardedMugics;
        SelectPanel<Creature> creaturePanel;
        List<Creature> discardedCreature;
        TriggeredPanel triggerPanel;
        List<Tuple<Creature, TriggeredType>> triggeredListAP;
        List<Tuple<Creature, TriggeredType>> triggeredListDP;

        SelectType elementPanel;

        CreatureNumber creatureNumber;
        bool done;
        bool? player1Won;
        byte numDrawed;

        Queue<GameStage> stageQueue; // Keeps track of next stages.

        SpriteFont endgamefont;
        SpriteFont hubFont;

        string currentStage;

        bool[] hadCombat; // keeps track of whether a showdown should happen.

        CardDescription description;
        SelectingDescription selectingDesription;

        ChaoticMessageBox attackMsgBox;

        public BattleBoard(Game game, GraphicsDeviceManager graphics, CreatureNumber creatureNumber)
            : base(game)
        {
            this.graphics = graphics;
            this.creatureNumber = creatureNumber;
            this.numDrawed = 0;
            stageQueue = new Queue<GameStage>();
            ChaoticEngine.GStage = GameStage.Shuffles;
            ChaoticEngine.Hive = false;
            ChaoticEngine.PrevHive = false;
            ChaoticEngine.Player1Active = true;
            ChaoticEngine.CombatThisTurn = false;
            ChaoticEngine.GenericMugicOnly = false;
            ChaoticEngine.CurrentAbility = null;
            ChaoticEngine.PrevState = Mouse.GetState();
            ChaoticEngine.sYouNode = null;
            ChaoticEngine.sEnemyNode = null;
            ChaoticEngine.SelectedNode = null;
            Burst.Empty();
            hadCombat = new bool[3] { false, false, true };
            topAtk = null;
            topLoc = null;
            discardedMugics = null;
            discardedCreature = null;
            triggeredListAP = new List<Tuple<Creature, TriggeredType>>();
            triggeredListDP = new List<Tuple<Creature, TriggeredType>>();
            currentStage = "Beginning Of Game";
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
            description = new CardDescription(Game.Content, graphics);
            selectingDesription = new SelectingDescription(Game.Content, graphics);
            cardBack = description.CoveredCard = Game.Content.Load<Texture2D>("CardBack");
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
            mugicHand1 = new MugicHand(true, cardBack, discardPosition,
                Game.Content.Load<SpriteFont>(@"Fonts\CardDescriptionFont"));
            for (int i = 0; i < ChaoticEngine.sMugics1.Count; i++)
                mugicHand1.AddCardToHand(ChaoticEngine.sMugics1[i]);
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
            mugicHand2 = new MugicHand(false, cardBack, discardPosition, 
                Game.Content.Load<SpriteFont>(@"Fonts\CardDescriptionFont"));
            for (int i = 0; i < ChaoticEngine.sMugics2.Count; i++)
                mugicHand2.AddCardToHand(ChaoticEngine.sMugics2[i]);
            locationPosition = new Vector2(discardPosition.X + 2 * ChaoticEngine.kCardWidth,
                3 * ChaoticEngine.kCardHeight + 3 * ChaoticEngine.kBattlegearGap + 2 * ChaoticEngine.kCardGap - ChaoticEngine.kCardWidth);
            locationDeck2 = new LocationDeck(Game.Content.Load<Texture2D>(@"BattleBoardSprites\Location"), locationTexture, spaceCover,
                locationPosition, false);
            activeLocation2 = new ActiveLocation(locationTexture,
                new Vector2(locationPosition.X, locationPosition.Y - ChaoticEngine.kCardHeight));
            ChaoticEngine.CodedEffects = new CodedManager(Game.Content);
            ChaoticEngine.DamageEffects = new DamageManager(Game.Content);
            ChaoticEngine.BurstContents = new BurstBox(Game.Content, graphics);
            ChaoticEngine.BackgroundSprite = Game.Content.Load<Texture2D>(@"Backgrounds\ChaoticBackground");
            ChaoticEngine.OrgBackgroundSprite = ChaoticEngine.BackgroundSprite;
            backgroundRect = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            SpriteFont font = Game.Content.Load<SpriteFont>(@"Fonts\CardDescriptionFont");
            BattleBoardButton move = new BattleBoardButton(moveButtonSprite, overlay,
                ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight / 3, ActionType.Move);
            BattleBoardButton activate = new BattleBoardButton(activateButtonSprite, overlay,
                ChaoticEngine.kCardWidth, ChaoticEngine.kCardHeight / 3, ActionType.ActivateCreature);
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
                        new Vector2(creatureSpaces[0].Position.X + ChaoticEngine.kCardWidth,
                        creatureSpaces[0].Position.Y - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap),
                        new byte[4] { 0, 2, 3, 4 }, descriptionPanel,
                        new BattleBoardButton[4] { move, activate, sacrifice, cast }, cancel, mugicTextures);
                    creatureSpaces[2] = new BattleBoardNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X - ChaoticEngine.kCardWidth, creatureSpaces[1].Position.Y),
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
            locationAnyPanel = new SelectPanel<Location>(Game.Content, graphics, 3, "Top", "2nd From\n Top", "3rd From\n Top");
            locationPanel1 = new SelectPanel<Location>(Game.Content, graphics, 2, "Top", "Bottom");
            mugicPanel = new SelectPanel<Mugic>(Game.Content, graphics, 1, "Return");
            creaturePanel = new SelectPanel<Creature>(Game.Content, graphics, 1, "Return");
            elementPanel = new SelectType(Game.Content, graphics, "Select");
            triggerPanel = new TriggeredPanel(Game.Content, graphics);

            Texture2D backTexture = Game.Content.Load<Texture2D>("Menu/SuitBackButton");
            backButton = new Button(backTexture, new Vector2(graphics.PreferredBackBufferWidth - backTexture.Width - 20, 20),
                Game.Content.Load<Texture2D>("Menu/backBtnCover"));
            endTurnButton = new Button(Game.Content.Load<Texture2D>("BattleBoardSprites/EndTurnButton"),
                new Vector2(attackDeck1.Position.X + ChaoticEngine.kCardWidth, graphics.PreferredBackBufferHeight / 2),
                Game.Content.Load<Texture2D>("Menu/MenuButtonCover"));
            endgamefont = Game.Content.Load<SpriteFont>("Fonts/EndGame");
            hubFont = Game.Content.Load<SpriteFont>("Fonts/HUB");

            coin = new Coin(Game.Content, graphics);

            ChaoticEngine.Highlighter = new CardHighlighter(Game.Content, graphics);
            ChaoticEngine.MsgBox = new ChaoticMessageBox("Adding To Burst", "Do you want to add an ability to Burst?", 
                Game.Content, graphics);
            attackMsgBox = new ChaoticMessageBox("Hive Call: Turn on Hive", "Do you want to activate Hive Call to turn on Hive?",
                Game.Content, graphics);
            base.LoadContent();
        }

        private void updateSupport()
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode is ISupport)
                {
                    byte numAdjacent = creatureSpaces[i].NumAdjacentOverWorldCreatures(creatureSpaces);
                    numAdjacent = creatureSpaces[i].CreatureNode.Negate ? (byte)0 : numAdjacent;
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
                    numMandiblor = creatureSpaces[i].CreatureNode.Negate ? (byte)0 : numMandiblor;
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

        private void hiveUpdate()
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode is IHive && ChaoticEngine.Hive)
                {
                    byte numMandiblor = creatureSpaces[i].NumMandiblors(creatureSpaces);
                    numMandiblor = creatureSpaces[i].CreatureNode.Negate ? (byte)0 : numMandiblor;
                    (creatureSpaces[i].CreatureNode as IHive).HiveOn(numMandiblor);
                }
                else if (creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.OduBathax)
                {
                    byte numMandiblor = creatureSpaces[i].NumMandiblors(creatureSpaces);
                    numMandiblor = creatureSpaces[i].CreatureNode.Negate ? (byte)0 : numMandiblor;
                    (creatureSpaces[i].CreatureNode as ChaoticGameLib.Creatures.OduBathax).Ability(numMandiblor);
                }
            }
        }

        private void selectedCreatureToDiscard(GameTime gameTime, MouseState mouse, BattleBoardNode selectedCreature,
            DiscardPile<ChaoticCard> discardPile)
        {
            if (selectedCreature.HasBattegear())
            {
                stageQueue.Enqueue(GameStage.SelCreatureToDiscard);
                while (stageQueue.Peek() != GameStage.SelCreatureToDiscard)
                    stageQueue.Enqueue(stageQueue.Dequeue());
                ChaoticEngine.GStage = GameStage.SelBattlegearToDiscard;
            }
            else
            {
                if (selectedCreature.UpdateDiscardCreature(gameTime, mouse, discardPile))
                {
                    selectedCreature.RemoveCreature();
                    updateSupport();
                    ChaoticEngine.GStage = stageQueue.Dequeue();
                }
                if (ChaoticEngine.GStage != GameStage.SelCreatureToDiscard)
                {
                    hiveUpdate();
                }
            }
        }

        private void selectedBattlegearToDiscard(GameTime gameTime, MouseState mouse, BattleBoardNode selectedBattlegear,
            DiscardPile<ChaoticCard> discardPile)
        {
            if (selectedBattlegear.UpdateDiscardBattlegear(gameTime, mouse, discardPile))
            {
                selectedBattlegear.RemoveBattlegear();
                ChaoticEngine.GStage = stageQueue.Dequeue();
            }
        }

        private void creatureToDiscard(GameTime gameTime, MouseState mouse, BattleBoardNode you, BattleBoardNode enemy,
            DiscardPile<ChaoticCard> discardPile, GameStage bStage)
        {
            if ((ChaoticEngine.Player1Active && you.HasBattegear()) ||
                (!ChaoticEngine.Player1Active && enemy.HasBattegear()))
            {
                stageQueue.Enqueue(ChaoticEngine.GStage);
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
                    hiveUpdate();
                }
            }
        }

        private void battlegearToDiscard(GameTime gameTime, MouseState mouse, BattleBoardNode you, BattleBoardNode enemy,
            DiscardPile<ChaoticCard> discardPile)
        {
            if (ChaoticEngine.Player1Active && you.UpdateDiscardBattlegear(gameTime, mouse, discardPile))
            {
                you.RemoveBattlegear();
                ChaoticEngine.GStage = stageQueue.Dequeue();
            }
            else if (!ChaoticEngine.Player1Active && enemy.UpdateDiscardBattlegear(gameTime, mouse, discardPile))
            {
                enemy.RemoveBattlegear();
                ChaoticEngine.GStage = stageQueue.Dequeue();
            }
        }

        private void mugicToDiscard(GameTime gameTime, MouseState mouse, MugicHand hand, DiscardPile<ChaoticCard> discardPile)
        {
            for (int j = 0; j < hand.Count; j++)
                hand[j].IsCovered = true;
            if (hand.UpdateHand(gameTime, mouse, discardPile))
            {
                ChaoticEngine.GStage = stageQueue.Dequeue();
                discardPile[discardPile.Count - 1].IsCovered = false;
                for (int j = 0; j < hand.Count; j++)
                    hand[j].IsCovered = false;
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
                ChaoticEngine.GStage = next;
                Location loc = activeLocation1.LocationActive ?? activeLocation2.LocationActive;
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
                else if (loc is ChaoticGameLib.Locations.EyeOfTheMaelstrom)
                {
                    ChaoticEngine.GStage = GameStage.DiscardMugic1;
                    stageQueue.Enqueue(GameStage.DiscardMugic2);
                    stageQueue.Enqueue(next);
                }
                else if (loc is ChaoticGameLib.Locations.RunicGrove)
                    ChaoticEngine.GenericMugicOnly = true;
                else if (loc is ChaoticGameLib.Locations.KiruCity)
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode != null &&
                            creatureSpaces[i].CreatureNode.CreatureTribe == Tribe.OverWorld)
                        {
                            creatureSpaces[i].CreatureNode.Energy += 10;
                            creatureSpaces[i].CreatureNode.GainedEnergyTurn += 10;

                            ChaoticEngine.DamageEffects.AddDamageAmount(10, creatureSpaces[i].CreatureNode.Position);
                        }
                    }
                }
                else if (loc is ChaoticGameLib.Locations.IronPillar)
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode != null)
                            creatureSpaces[i].CreatureNode.NegateBattlegear();
                    }
                }
            }
        }

        /// <summary>
        /// Returns active location to deck, and checks if there was a winner or tie.
        /// </summary>
        /// <param name="gameTime">Controls the time of the game.</param>
        /// <param name="combatStage">Next stage if there was combat this turn.</param>
        /// <param name="locStep">Next stage if there wasn't combat this turn.</param>
        private void returnLocation(GameTime gameTime, GameStage combatStage, GameStage locStep)
        {
            if (activeLocation1.LocationActive != null)
                done = activeLocation1.ReturnLocationToDeck(gameTime, locationDeck1);
            else if (activeLocation2.LocationActive != null)
                done = activeLocation2.ReturnLocationToDeck(gameTime, locationDeck2);
            if (done)
            {
                // If both players have no Creatures left, they will tie.
                if (creatureSpaces.Count(b => b.CreatureNode != null && b.IsPlayer1 == true) == 0 &&
                    creatureSpaces.Count(b => b.CreatureNode != null && b.IsPlayer1 == false) == 0)
                {
                    player1Won = null;
                    ChaoticEngine.GStage = GameStage.EndGame;
                }
                else if (creatureSpaces.Count(b => b.CreatureNode != null && b.IsPlayer1 == true) == 0) // Player 2 Won.
                {
                    player1Won = false;
                    ChaoticEngine.GStage = GameStage.EndGame;
                }
                else if (creatureSpaces.Count(b => b.CreatureNode != null && b.IsPlayer1 == false) == 0) // Player 1 Won.
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

        private void endTurn()
        {
            ChaoticEngine.Player1Active = !ChaoticEngine.Player1Active;
            ChaoticEngine.Hive = false;
            ChaoticEngine.CombatThisTurn = false;
            ChaoticEngine.GenericMugicOnly = false;
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
            {
                ChaoticEngine.GStage = GameStage.ReturnLocation;
                turnOffLocation(); // Removes location effects.
            }
            else
                ChaoticEngine.GStage = GameStage.LocationStep;
        }

        private void updateAttackDamage(Attack attack, Creature your, Creature enemy,
            AttackDeck attackDeck, LocationDeck locationDeck, bool isPlayer1)
        {
            Location activeLocation = activeLocation1.LocationActive ?? activeLocation2.LocationActive;

            short prevEnergy1 = your.Energy;
            short prevEnergy2 = enemy.Energy;

            if (ChaoticEngine.Player1Active)
                attack.Damage(your, enemy, activeLocation);
            else
                attack.Damage(enemy, your, activeLocation);

            if (Math.Abs(your.Energy - prevEnergy1) != 0)
                ChaoticEngine.DamageEffects.AddDamageAmount((short)(your.Energy - prevEnergy1), your.Position);
            if (Math.Abs(enemy.Energy - prevEnergy2) != 0)
                ChaoticEngine.DamageEffects.AddDamageAmount((short)(enemy.Energy - prevEnergy2), enemy.Position);

            if (attack is Windslash)
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (ChaoticEngine.Player1Strike != creatureSpaces[i].IsPlayer1 &&
                        creatureSpaces[i].CreatureNode != null)
                        creatureSpaces[i].CreatureNode.ActivateBattlegear();
                }
            }
            else if (attack is TornadoTackle &&
                ((ChaoticEngine.Player1Active && your.Air) ||
                (!ChaoticEngine.Player1Active && enemy.Air)))
            {
                stageQueue.Enqueue(GameStage.ShuffleAtkDeck1);
                stageQueue.Enqueue(GameStage.ShuffleAtkDeck2);
            }
            else if (attack is Nexdoors && ((ChaoticEngine.Player1Active && your.Air) ||
                (!ChaoticEngine.Player1Active && enemy.Air)) &&
                    attackDeck.Deck.Count > 1)
            {
                stageQueue.Enqueue(GameStage.AttackSelectAbility);
            }
            else if (attack is SqueezePlay &&
                    attackDeck.Deck.Count > 1)
            {
                stageQueue.Enqueue(GameStage.AttackSelectAbility);
            }
            else if (attack is FlashKick &&
                    locationDeck.Deck.Count > 1)
            {
                stageQueue.Enqueue(GameStage.AttackSelectAbility);
            }

            else if (attack is CoilCrush &&
                ((ChaoticEngine.Player1Active && your.Power >= 75
                    && enemy.Battlegear != null) ||
                (!ChaoticEngine.Player1Active && enemy.Power >= 75
                    && your.Battlegear != null)))
            {
                stageQueue.Enqueue(isPlayer1 ? GameStage.BattlegearToDiscard2 : GameStage.BattlegearToDiscard1);
            }
            else if (attack is Mirthquake &&
                ((ChaoticEngine.Player1Active && your.Earth) ||
                (!ChaoticEngine.Player1Active && enemy.Earth)))
            {
                if (ChaoticEngine.Player1Active)
                    your.Earth = your.EarthCombat = false;
                else
                    enemy.Earth = enemy.EarthCombat = false;

                turnOffLocation(); // Removes location effects.
                stageQueue.Enqueue(GameStage.ReturnLocationCombat);
            }
            else if (attack is IronBalls)
                ChaoticEngine.GenericMugicOnly = true;

            ChaoticEngine.Player1Strike = !ChaoticEngine.Player1Strike;
        }

        private void updateCombat(GameTime gameTime, MouseState mouse, AttackHand attackHand, DiscardPile<Attack> attackDiscardPile, bool player1=true)
        {
            if (attackHand.UpdateHand(gameTime, mouse, attackDiscardPile))
            {
                Attack attack = attackDiscardPile[attackDiscardPile.Count - 1] as Attack;
                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, attack);
                stageQueue.Enqueue(GameStage.HighLight);

                ChaoticEngine.CurrentAbility = new Ability(player1, AbilityType.Attack, AbilityAction.Activate);
                ChaoticEngine.CurrentAbility.Add(attack);
                if (ChaoticEngine.Player1Strike)
                {
                    ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sYouNode.CreatureNode);
                    ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sEnemyNode.CreatureNode);
                }
                else
                {
                    ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sEnemyNode.CreatureNode);
                    ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sYouNode.CreatureNode);
                }
                if (attack is HiveCall && !ChaoticEngine.Hive &&
                    creatureSpaces.Count(c => c.IsPlayer1 == player1 && c.CreatureNode != null && c.CreatureNode.MugicCounters >= 1) > 0)
                    stageQueue.Enqueue(GameStage.HiveCallDecision);
                if (!Burst.Alive)
                    Burst.InitializeBurst(GameStage.Combat);

                Burst.Push(ChaoticEngine.CurrentAbility);
                stageQueue.Enqueue(GameStage.DecideToBurst);
                ChaoticEngine.GStage = stageQueue.Dequeue();
            }
        }
        
        private void updateMugicCast(GameTime gameTime, MouseState mouse, MugicHand mugicHand,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            mugicHand.UpdatePlayable(mouse, creatureSpaces, discardPile, activeLoc);
            if (mugicHand.UpdateHand(gameTime, mouse, discardPile, creatureSpaces, activeLoc))
            {
                Mugic mugic = discardPile[discardPile.Count - 1] as Mugic;
                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, mugic);
                stageQueue.Enqueue(GameStage.HighLight);
                if (mugic is ICast)
                {
                    stageQueue.Enqueue(GameStage.TargetCreature);
                    ICast cast = mugic as ICast;
                    setAbilityPath(mugicHand.IsPlayer1, cast.Type, AbilityAction.Cast);
                    ChaoticEngine.CurrentAbility.Add(mugic);
                }
            }
        }

        private void updateRecklessness(GameTime gameTime, Creature creature, bool isPlayer1)
        {
            ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creature);
            stageQueue.Enqueue(GameStage.HighLight);
            setAbilityPath(isPlayer1, AbilityType.Recklessness, AbilityAction.Activate);
            ChaoticEngine.CurrentAbility.Add(creature);
        }

        private void setAttackInfo(Creature you, Creature enemy, bool isPlayer1)
        {
            if (you.FirstAttack && you.Invisibility() && creatureSpaces.Count(c => c.IsPlayer1 == isPlayer1
                    && c.CreatureNode is ChaoticGameLib.Creatures.MarquisDarini) > 0)
                ChaoticEngine.HasMarquisDarini = true;
            else
                ChaoticEngine.HasMarquisDarini = false;
        }

        private void updatePlayableAbilities(MouseState mouse, GameTime gameTime, bool playerOneTurn)
        {
            ActiveLocation activeLoc = activeLocation1.LocationActive != null ? activeLocation1 : activeLocation2;
            if (playerOneTurn)
            {
                updateMugicCast(gameTime, mouse, mugicHand1, discardPile1, activeLoc);
            }
            else
            {
                updateMugicCast(gameTime, mouse, mugicHand2, discardPile2, activeLoc);
            }

            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].IsPlayer1 == playerOneTurn)
                {
                    ActionType? action = creatureSpaces[i].UpdateBattleBoardNode(gameTime, mouse, creatureSpaces,
                        creatureSpaces[i].IsPlayer1 ? discardPile1 : discardPile2, activeLoc);

                    switch (action)
                    {
                        case ActionType.Move:
                            creatureSpaces[i].TurnOnMovableSpaces(creatureSpaces);
                            selectedSpace = i;
                            ChaoticEngine.GStage = GameStage.Moving;
                            break;
                        case ActionType.ActivateCreature:
                            ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creatureSpaces[i].CreatureNode);
                            stageQueue.Enqueue(GameStage.HighLight);
                            IActivate active = creatureSpaces[i].CreatureNode as IActivate;
                            active.PayCost();
                            setAbilityPath(creatureSpaces[i].IsPlayer1, active.Type, AbilityAction.Activate);
                            ChaoticEngine.CurrentAbility.Add(creatureSpaces[i].CreatureNode);
                            break;
                        case ActionType.ActivateBattlegear:
                            ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creatureSpaces[i].CreatureNode);
                            stageQueue.Enqueue(GameStage.HighLight);
                            IActivateBattlegear actBattlegear = creatureSpaces[i].CreatureNode.Battlegear as IActivateBattlegear;
                            actBattlegear.PayCost(creatureSpaces[i].CreatureNode);
                            setAbilityPath(creatureSpaces[i].IsPlayer1, actBattlegear.Type, AbilityAction.Activate);
                            ChaoticEngine.CurrentAbility.Add(creatureSpaces[i].CreatureNode.Battlegear);
                            ChaoticEngine.CurrentAbility.Add(creatureSpaces[i].CreatureNode);
                            break;
                        case ActionType.SacrificeCreature:
                            ChaoticEngine.SelectedNode = creatureSpaces[i];
                            ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creatureSpaces[i].CreatureNode);
                            stageQueue.Enqueue(GameStage.HighLight);
                            stageQueue.Enqueue(GameStage.SelCreatureToDiscard);
                            setAbilityPath(creatureSpaces[i].IsPlayer1, (creatureSpaces[i].CreatureNode as ISacrifice).Type,
                                AbilityAction.Sacrifice);
                            creatureSpaces[i].CreatureNode.Energy = 0;
                            ChaoticEngine.CurrentAbility.Add(creatureSpaces[i].CreatureNode);
                            break;
                        case ActionType.SacrificeBattlegear:
                            ChaoticEngine.SelectedNode = creatureSpaces[i];
                            if (creatureSpaces[i].CreatureNode.Battlegear is ChaoticGameLib.Battlegears.TalismanOfTheMandiblor)
                                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creatureSpaces[i].CreatureNode);
                            else
                                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creatureSpaces[i].CreatureNode.Battlegear);
                            stageQueue.Enqueue(GameStage.HighLight);
                            if (creatureSpaces[i].CreatureNode.Battlegear is ChaoticGameLib.Battlegears.TalismanOfTheMandiblor)
                                stageQueue.Enqueue(GameStage.SelCreatureToDiscard);
                            else
                                stageQueue.Enqueue(GameStage.SelBattlegearToDiscard);

                            setAbilityPath(creatureSpaces[i].IsPlayer1, 
                                (creatureSpaces[i].CreatureNode.Battlegear as ISacrifice).Type, AbilityAction.Sacrifice);
                            ChaoticEngine.CurrentAbility.Add(creatureSpaces[i].CreatureNode.Battlegear);
                            if (creatureSpaces[i].CreatureNode.Battlegear is ChaoticGameLib.Battlegears.TalismanOfTheMandiblor)
                                creatureSpaces[i].CreatureNode.Energy = 0;
                            else
                                ChaoticEngine.CurrentAbility.Add(creatureSpaces[i].CreatureNode);
                            break;
                    }
                }
            }
        }

        private void setAbilityPath(bool isPlayer1, AbilityType abilityType, AbilityAction action)
        {
            ChaoticEngine.CurrentAbility = new Ability(isPlayer1, abilityType, action);
            switch (abilityType)
            {
                case AbilityType.TargetDanianCount:
                case AbilityType.TargetCreature:
                    stageQueue.Enqueue(GameStage.TargetCreature);
                    break;
                case AbilityType.TargetCreatureTwo:
                    stageQueue.Enqueue(GameStage.TargetCreature);
                    stageQueue.Enqueue(GameStage.TargetCreature);
                    break;
                case AbilityType.SacrificeTargetCreature:
                    stageQueue.Enqueue(GameStage.TargetCreature);
                    stageQueue.Enqueue(GameStage.SelCreatureToDiscard);
                    break;
                case AbilityType.DestroyTargetBattlegear:
                    stageQueue.Enqueue(GameStage.TargetBattlegear);
                    break;
                case AbilityType.ReturnMugic:
                    stageQueue.Enqueue(GameStage.SelMugicInDiscard);
                    break;
                case AbilityType.ReturnCreature:
                    stageQueue.Enqueue(GameStage.SelCreatureInDiscard);
                    break;
                case AbilityType.TargetLocationDeck:
                    stageQueue.Enqueue(GameStage.SelLocationDeck);
                    break;
                case AbilityType.ShuffleTargetDeck:
                case AbilityType.TargetAttackLocationDeck:
                    stageQueue.Enqueue(GameStage.SelAttackLocationDeck);
                    break;
                case AbilityType.TargetSelectElemental:
                    stageQueue.Enqueue(GameStage.TargetCreature);
                    stageQueue.Enqueue(GameStage.SelElemental);
                    break;
                case AbilityType.TargetEngaged:
                    stageQueue.Enqueue(GameStage.TargetEngaged);
                    break;
                case AbilityType.TargetAttack:
                    stageQueue.Enqueue(GameStage.TargetAttack);
                    break;
                case AbilityType.ChangeLocation:
                    break;
                default:
                    break;
            }
            if (!Burst.Alive)
                Burst.InitializeBurst(ChaoticEngine.GStage);

            Burst.Push(ChaoticEngine.CurrentAbility);
            stageQueue.Enqueue(GameStage.DecideToBurst);
            ChaoticEngine.GStage = stageQueue.Dequeue();
        }

        private void setAbilityRun(GameTime gameTime)
        {
            ChaoticEngine.CurrentAbility = Burst.NextAbility();
            ChaoticCard card = ChaoticEngine.CurrentAbility[0] as ChaoticCard;
            ChaoticEngine.Highlighter.InitializeHighlight(gameTime, card);
            stageQueue.Enqueue(GameStage.HighLight);

            if (card.Negate)
            {
                ChaoticEngine.GStage = stageQueue.Dequeue();
                return;
            }

            switch (ChaoticEngine.CurrentAbility.Type)
            {
                case AbilityType.TargetSelf:
                    if (ChaoticEngine.CurrentAbility.Action == AbilityAction.Activate)
	                {
                        IActivateSelf actSelf = ChaoticEngine.CurrentAbility[0] as IActivateSelf;
                        Creature actCreature = ChaoticEngine.CurrentAbility[0] as Creature;
                        short prevEner = actCreature.Energy;
                        actSelf.Ability();
                        if (Math.Abs(actCreature.Energy - prevEner) != 0)
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)(actCreature.Energy - prevEner), actCreature.Position);
                    }
                    else if (ChaoticEngine.CurrentAbility.Action == AbilityAction.Cast)
                    {
                        ICastTarget<Creature> castSelf = ChaoticEngine.CurrentAbility[0] as ICastTarget<Creature>;
                        Creature c = ChaoticEngine.CurrentAbility[1] as Creature;
                        short prevEner = c.Energy;
                        castSelf.Ability(c);
                        if (Math.Abs(c.Energy - prevEner) != 0)
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)(c.Energy - prevEner), c.Position);
                    }
                    break;
                case AbilityType.TargetSelfChange:
                    switch (ChaoticEngine.CurrentAbility.Action)
	                {
                        case AbilityAction.Activate:
                            IActivateChange actChange = ChaoticEngine.CurrentAbility[0] as IActivateChange;
                            ChaoticEngine.Hive = actChange.Ability();
                            break;
                        case AbilityAction.Sacrifice:
                            IActivateChange sacChange = ChaoticEngine.CurrentAbility[0] as IActivateChange;
                            ChaoticEngine.Hive = sacChange.Ability();
                            break;
                        case AbilityAction.Cast:
                            ICastChange castChange = ChaoticEngine.CurrentAbility[0] as ICastChange;
                            ChaoticEngine.Hive = castChange.Ability();
                            break;
	                }
                    break;
                case AbilityType.TargetCreature:
                    switch (ChaoticEngine.CurrentAbility.Action)
	                {
                        case AbilityAction.Activate:
                            IActivateTarget<Creature> actTar = ChaoticEngine.CurrentAbility[0] as IActivateTarget<Creature>;
                            Creature cre = ChaoticEngine.CurrentAbility[1] as Creature;
                            byte swift = cre.Swift;
                            short prevEner = cre.Energy;
                            actTar.Ability(cre);
                            if (cre.Swift > swift)
                            {
                                BattleBoardNode space = creatureSpaces.Single(c => c.CreatureNode == cre);
                                space.NumMoves++;
                            }
                            if (Math.Abs(cre.Energy- prevEner) != 0)
                                ChaoticEngine.DamageEffects.AddDamageAmount((short)(cre.Energy - prevEner), cre.Position);
                            break;
                        case AbilityAction.Sacrifice:
                            ISacrificeTarget<Creature> sacTar = ChaoticEngine.CurrentAbility[0] as ISacrificeTarget<Creature>;
                            cre = ChaoticEngine.CurrentAbility[0] is Battlegear ? ChaoticEngine.CurrentAbility[2] as Creature  : ChaoticEngine.CurrentAbility[1] as Creature;
                            prevEner = cre.Energy;
                            sacTar.Ability(cre);
                            if (Math.Abs(cre.Energy - prevEner) != 0)
                                ChaoticEngine.DamageEffects.AddDamageAmount((short)(cre.Energy - prevEner), cre.Position);
                            break;
                        case AbilityAction.Cast:
                            ICastTarget<Creature> castTar = ChaoticEngine.CurrentAbility[0] as ICastTarget<Creature>;
                            cre = ChaoticEngine.CurrentAbility[2] as Creature;
                            prevEner = cre.Energy;
                            byte prevSwift = cre.Swift;
                            castTar.Ability(cre);
                            if (prevEner > cre.Energy && creatureSpaces.Count(c => 
                                c.IsPlayer1 == ChaoticEngine.CurrentAbility.IsPlayer1 && !c.CreatureNode.Negate
                                && c.CreatureNode is ChaoticGameLib.Creatures.Drakness) == 1)
                            {
                                Ability abil = new Ability(ChaoticEngine.CurrentAbility.IsPlayer1, 
                                    AbilityType.TargetCreature, AbilityAction.Activate);
                                Creature drakness = creatureSpaces.First(c => c.IsPlayer1 == abil.IsPlayer1 &&
                                    c.CreatureNode is ChaoticGameLib.Creatures.Drakness).CreatureNode;
                                abil.Add(drakness);
                                abil.Add(cre);
                                Burst.Push(abil);
                                ChaoticEngine.Highlighter.AddHighlight(drakness.Texture);
                                stageQueue.Enqueue(GameStage.HighLight);
                                stageQueue.Enqueue(GameStage.DecideToBurst);
                                ChaoticEngine.GStage = stageQueue.Dequeue();
                                return;
                            }
                            if (cre.Swift > prevSwift)
                            {
                                BattleBoardNode space = creatureSpaces.Single(c => c.CreatureNode == cre);
                                space.NumMoves++;
                            }
                            if (Math.Abs(cre.Energy - prevEner) != 0)
                                ChaoticEngine.DamageEffects.AddDamageAmount((short)(cre.Energy - prevEner), cre.Position);
                            break;
	                }
                    break;
                case AbilityType.TargetEngaged:
                case AbilityType.TargetCreatureTwo:
                    if (ChaoticEngine.CurrentAbility.Action == AbilityAction.Cast)
                    {
                        ICastTargetTwo<Creature> castTar2 = ChaoticEngine.CurrentAbility[0] as ICastTargetTwo<Creature>;
                        Creature creature1 = ChaoticEngine.CurrentAbility[2] as Creature;
                        Creature creature2 = ChaoticEngine.CurrentAbility[3] as Creature;
                        short prevEnergy1 = creature1.Energy;
                        short prevEnergy2 = creature2.Energy;
                        castTar2.Ability(creature1, creature2);
                        if (creatureSpaces.Count(c => 
                                c.IsPlayer1 == ChaoticEngine.CurrentAbility.IsPlayer1 && !c.CreatureNode.Negate
                                && c.CreatureNode is ChaoticGameLib.Creatures.Drakness) == 1)
                        {
                            Ability abil = new Ability(ChaoticEngine.CurrentAbility.IsPlayer1,
                                    AbilityType.TargetCreature, AbilityAction.Activate);
                            Creature drakness = creatureSpaces.First(c => c.IsPlayer1 == abil.IsPlayer1 &&
                                c.CreatureNode is ChaoticGameLib.Creatures.Drakness).CreatureNode;
                            abil.Add(drakness);
                            if (prevEnergy1 > creature1.Energy)
                                abil.Add(creature1);
                            else if (prevEnergy2 > creature2.Energy)
                                abil.Add(creature2);

                            if (prevEnergy1 > creature1.Energy || prevEnergy2 > creature2.Energy)
                            {
                                Burst.Push(abil);
                                ChaoticEngine.Highlighter.AddHighlight(drakness.Texture);
                                stageQueue.Enqueue(GameStage.HighLight);
                                stageQueue.Enqueue(GameStage.DecideToBurst);
                                ChaoticEngine.GStage = stageQueue.Dequeue();
                                return;
                            }
                        }
                        if (Math.Abs(creature1.Energy - prevEnergy1) != 0)
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)(creature1.Energy - prevEnergy1), creature1.Position);
                        if (Math.Abs(creature2.Energy - prevEnergy2) != 0)
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)(creature2.Energy - prevEnergy2), creature2.Position);
                    }
                    break;
                case AbilityType.DestroyTargetBattlegear:
                    ChaoticEngine.SelectedNode = ChaoticEngine.CurrentAbility[2] as BattleBoardNode;
                    stageQueue.Enqueue(GameStage.SelBattlegearToDiscard);
                    break;
                case AbilityType.SacrificeTargetCreature:
                    IActivateSelf actSac = ChaoticEngine.CurrentAbility[0] as IActivateSelf;
                    Creature creature = ChaoticEngine.CurrentAbility[0] as Creature;
                    short prevEnergy = creature.Energy;
                    actSac.Ability();
                    if (Math.Abs(creature.Energy - prevEnergy) != 0)
                        ChaoticEngine.DamageEffects.AddDamageAmount((short)(creature.Energy - prevEnergy), creature.Position);
                    break;
                case AbilityType.TargetEquipped:
                    switch (ChaoticEngine.CurrentAbility.Action)
	                {
                        case AbilityAction.Activate:
                            break;
                        case AbilityAction.Sacrifice:
                            ISacrificeTarget<Creature> sacTarEquip = ChaoticEngine.CurrentAbility[0] as ISacrificeTarget<Creature>;
                            Creature equipped = ChaoticEngine.CurrentAbility[1] as Creature;
                            prevEnergy = equipped.Energy;
                            sacTarEquip.Ability(equipped);
                            if (Math.Abs(equipped.Energy - prevEnergy) != 0)
                                ChaoticEngine.DamageEffects.AddDamageAmount((short)(equipped.Energy - prevEnergy), equipped.Position);
                            break;
	                }
                    break;
                case AbilityType.ReturnCreature:
                    DiscardPile<ChaoticCard> retDiscardPile = ChaoticEngine.CurrentAbility.IsPlayer1 ? discardPile1 : discardPile2;
                    ChaoticEngine.ReturnSelectedIndex1 = ChaoticEngine.CurrentAbility.Action == AbilityAction.Activate ||
                        ChaoticEngine.CurrentAbility.Action == AbilityAction.Sacrifice ?
                        retDiscardPile.DiscardList.IndexOf(ChaoticEngine.CurrentAbility[1] as ChaoticCard) :
                        retDiscardPile.DiscardList.IndexOf(ChaoticEngine.CurrentAbility[2] as ChaoticCard);
                    stageQueue.Enqueue(GameStage.SelUnoccupiedSpace);
                    stageQueue.Enqueue(GameStage.SelCreatureReturn);
                    break;
                case AbilityType.ReturnMugic:
                    DiscardPile<ChaoticCard> discardPile = ChaoticEngine.CurrentAbility.IsPlayer1 ? discardPile1 : discardPile2;
                    ChaoticEngine.ReturnSelectedIndex1 = ChaoticEngine.CurrentAbility.Action == AbilityAction.Activate ?
                        discardPile.DiscardList.IndexOf(ChaoticEngine.CurrentAbility[1] as ChaoticCard) :
                        discardPile.DiscardList.IndexOf(ChaoticEngine.CurrentAbility[2] as ChaoticCard);
                    stageQueue.Enqueue(GameStage.SelReturnMugic);
                    break;
                case AbilityType.TargetLocationDeck:
                    if (ChaoticEngine.CurrentAbility[0] is ChaoticGameLib.Creatures.Hearring)
                        stageQueue.Enqueue(GameStage.SelLocationAny);
                    else
                        stageQueue.Enqueue(GameStage.SelLocationOrder);
                    break;
                case AbilityType.TargetAttackLocationDeck:
                    stageQueue.Enqueue(GameStage.SelAttackLocationOrder);
                    break;
                case AbilityType.ShuffleTargetDeck:
                    if (ChaoticEngine.CurrentAbility[2] is AttackDeck)
                    {
                        AttackDeck atkDeck = (AttackDeck)ChaoticEngine.CurrentAbility[2];
                        if (atkDeck.IsPlayer1)
                            stageQueue.Enqueue(GameStage.ShuffleAtkDeck1);
                        else
                            stageQueue.Enqueue(GameStage.ShuffleAtkDeck2);
                    }
                    else
                    {
                        LocationDeck locDeck = (LocationDeck)ChaoticEngine.CurrentAbility[2];
                        if (locDeck.IsPlayer1)
                            stageQueue.Enqueue(GameStage.ShuffleLocDeck1);
                        else
                            stageQueue.Enqueue(GameStage.ShuffleLocDeck2);
                    }
                    break;
                case AbilityType.DispelMugic:
                    Burst.NextAbility(); // Dispel the Mugic next on the burst.
                    break;
                case AbilityType.Attack:
                    if (ChaoticEngine.CurrentAbility[0] is Attack)
                    {
                        Attack atk = ChaoticEngine.CurrentAbility[0] as Attack;
                        Creature your = ChaoticEngine.CurrentAbility[1] as Creature;
                        Creature enemy = ChaoticEngine.CurrentAbility[2] as Creature;
                        bool firstAttack = ChaoticEngine.Player1Active ? your.FirstAttack : enemy.FirstAttack;
                        updateAttackDamage(atk, your, enemy, attackDeck1, locationDeck1, ChaoticEngine.Player1Strike);

                        if ((ChaoticEngine.CurrentAbility[1] as Creature).Energy == 0 || (ChaoticEngine.CurrentAbility[2] as Creature).Energy == 0)
                        {
                            Burst.BurstStart = GameStage.EndOfCombat;
                            ChaoticEngine.CombatThisTurn = true;
                        }

                        if (atk is HiveCall && (bool)ChaoticEngine.CurrentAbility[3])
                            ChaoticEngine.Hive = true;

                        if (firstAttack && (ChaoticEngine.Player1Active ? your.Invisibility() : enemy.Invisibility()) &&
                            creatureSpaces.Count(c => c.IsPlayer1 == ChaoticEngine.CurrentAbility.IsPlayer1
                            && c.CreatureNode is ChaoticGameLib.Creatures.MarquisDarini && !c.CreatureNode.Negate) == 1)
                        {
                            Ability abil = new Ability(ChaoticEngine.CurrentAbility.IsPlayer1,
                                AbilityType.TargetCreature, AbilityAction.Activate);
                            Creature marquisDarini = creatureSpaces.First(c => c.IsPlayer1 == abil.IsPlayer1 &&
                                c.CreatureNode is ChaoticGameLib.Creatures.MarquisDarini).CreatureNode;
                            abil.Add(marquisDarini);
                            if (ChaoticEngine.Player1Active)
                                abil.Add(enemy);
                            else
                                abil.Add(your);
                            Burst.Push(abil);
                            ChaoticEngine.Highlighter.AddHighlight(marquisDarini.Texture);
                            stageQueue.Enqueue(GameStage.HighLight);
                            stageQueue.Enqueue(GameStage.DecideToBurst);
                            ChaoticEngine.GStage = stageQueue.Dequeue();
                            return;
                        }
                    }
                    else
                        throw new Exception("Was supposed to be Attack in AbilityType.Attack.");
                    break;
                case AbilityType.Recklessness:
                    if (ChaoticEngine.CurrentAbility[0] is Creature)
                    {
                        Creature recklessCreature = ChaoticEngine.CurrentAbility[0] as Creature;
                        recklessCreature.Energy -= recklessCreature.Recklessness;
                        ChaoticEngine.DamageEffects.AddDamageAmount((short)-recklessCreature.Recklessness,
                            recklessCreature.Position);
                    }
                    else
                        throw new Exception("Was suppose to be a Creature in AbilityType.Recklessness.");
                    break;
                case AbilityType.TargetSelectElemental:
                    if (ChaoticEngine.CurrentAbility[2] is Creature)
                    {
                        byte element = (byte)ChaoticEngine.CurrentAbility[3];
                        (ChaoticEngine.CurrentAbility[2] as Creature).Fire |= ((element >> 3) & 1) == 1;
                        (ChaoticEngine.CurrentAbility[2] as Creature).Air |= ((element >> 2) & 1) == 1;
                        (ChaoticEngine.CurrentAbility[2] as Creature).Earth |= ((element >> 1) & 1) == 1;
                        (ChaoticEngine.CurrentAbility[2] as Creature).Water |= (element & 1) == 1;
                    }
                    else
                        throw new Exception("Was suppose to be a Creature in AbilityType.TargetSelectElemental.");
                    break;
                case AbilityType.TargetDanianCount:
                    if (ChaoticEngine.CurrentAbility[0] is ICast && ChaoticEngine.CurrentAbility[2] is Creature)
                    {
                        ICastTargetCount<Creature> tarDanCount = ChaoticEngine.CurrentAbility[0] as ICastTargetCount<Creature>;
                        creature = ChaoticEngine.CurrentAbility[2] as Creature;
                        prevEnergy = creature.Energy;
                        tarDanCount.Ability(creature,
                            (byte)creatureSpaces.Count(c => c.CreatureNode != null && 
                                c.CreatureNode.CreatureTribe == Tribe.Danian));
                        if (Math.Abs(creature.Energy - prevEnergy) != 0)
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)(creature.Energy - prevEnergy), creature.Position);
                    }
                    break;
                case AbilityType.TargetEquippedCreature:
                    if (ChaoticEngine.CurrentAbility[0] is IActivateBattlegear && ChaoticEngine.CurrentAbility[1] is Creature)
                    {
                        IActivateBattlegear actBattleagear = ChaoticEngine.CurrentAbility[0] as IActivateBattlegear;
                        creature = ChaoticEngine.CurrentAbility[1] as Creature;
                        prevEnergy = creature.Energy;
                        actBattleagear.Ability(creature);
                        if (Math.Abs(creature.Energy - prevEnergy) != 0)
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)(creature.Energy - prevEnergy), creature.Position);
                    }
                    break;
                case AbilityType.ChangeTarget:
                    ChaoticEngine.CurrentAbility = Burst.Peek();
                    ChaoticEngine.CurrentAbility.RemoveTarget();
                    switch (ChaoticEngine.CurrentAbility.Type)
                    {
                        case AbilityType.TargetDanianCount:
                        case AbilityType.TargetCreature:
                            stageQueue.Enqueue(GameStage.TargetCreature);
                            break;
                        case AbilityType.DestroyTargetBattlegear:
                            stageQueue.Enqueue(GameStage.TargetBattlegear);
                            break;
                        case AbilityType.ReturnMugic:
                            stageQueue.Enqueue(GameStage.SelMugicInDiscard);
                            break;
                        case AbilityType.ReturnCreature:
                            stageQueue.Enqueue(GameStage.SelCreatureInDiscard);
                            break;
                        case AbilityType.TargetLocationDeck:
                            stageQueue.Enqueue(GameStage.SelLocationDeck);
                            break;
                        case AbilityType.ShuffleTargetDeck:
                        case AbilityType.TargetAttackLocationDeck:
                            stageQueue.Enqueue(GameStage.SelAttackLocationDeck);
                            break;
                        case AbilityType.TargetSelectElemental:
                            stageQueue.Enqueue(GameStage.TargetCreature);
                            stageQueue.Enqueue(GameStage.SelElemental);
                            break;
                        case AbilityType.TargetEngaged:
                            stageQueue.Enqueue(GameStage.TargetEngaged);
                            break;
                        default:
                            break;
                    }
                    break;
                case AbilityType.Triggered:
                    switch ((TriggeredType)ChaoticEngine.CurrentAbility[1])
                    {
                        case TriggeredType.IntimidateCourage:
                            Creature intimidator = ChaoticEngine.CurrentAbility[0] as Creature;
                            Creature intimidated = ChaoticEngine.CurrentAbility[2] as Creature;
                            intimidated.Courage -= intimidator.IntimidateCourage;
                            intimidated.CourageCombat += intimidator.IntimidateCourage;
                            break;
                        case TriggeredType.IntimidatePower:
                            intimidator = ChaoticEngine.CurrentAbility[0] as Creature;
                            intimidated = ChaoticEngine.CurrentAbility[2] as Creature;
                            intimidated.Power -= intimidator.IntimidatePower;
                            intimidated.PowerCombat += intimidator.IntimidatePower;
                            break;
                        case TriggeredType.IntimidateWisdom:
                            intimidator = ChaoticEngine.CurrentAbility[0] as Creature;
                            intimidated = ChaoticEngine.CurrentAbility[2] as Creature;
                            intimidated.Wisdom -= intimidator.IntimidateWisdom;
                            intimidated.WisdomCombat += intimidator.IntimidateWisdom;
                            break;
                        case TriggeredType.IntimidateSpeed:
                            intimidator = ChaoticEngine.CurrentAbility[0] as Creature;
                            intimidated = ChaoticEngine.CurrentAbility[2] as Creature;
                            intimidated.Speed -= intimidator.IntimidateSpeed;
                            intimidated.SpeedCombat += intimidator.IntimidateSpeed;
                            break;
                        case TriggeredType.LordVanBloot:
                            creature = ChaoticEngine.CurrentAbility[2] as Creature;
                            creature.Energy -= 10;
                            ChaoticEngine.DamageEffects.AddDamageAmount((short)-10, creature.Position);
                            break;
                        case TriggeredType.Recklessness:
                            break;
                        default:
                            break;
                    }
                    break;
                case AbilityType.TargetAttack:
                    if (ChaoticEngine.CurrentAbility[0] is ICastTarget<Attack>)
                    {
                        ICastTarget<Attack> castTarAtk = ChaoticEngine.CurrentAbility[0] as ICastTarget<Attack>;
                        Attack attack = ChaoticEngine.CurrentAbility[2] as Attack;
                        castTarAtk.Ability(attack);
                    }
                    if (ChaoticEngine.CurrentAbility[0] is ISacrificeTarget<Attack>)
                    {
                        ISacrificeTarget<Attack> castTarAtk = ChaoticEngine.CurrentAbility[0] as ISacrificeTarget<Attack>;
                        Attack attack = ChaoticEngine.CurrentAbility[2] as Attack;
                        castTarAtk.Ability(attack);
                    }
                    break;
                case AbilityType.TargetLocation:
                    ICastTarget<Location> castTarLoc = ChaoticEngine.CurrentAbility[0] as ICastTarget<Location>;
                    Location location = activeLocation1.LocationActive ?? activeLocation2.LocationActive;
                    castTarLoc.Ability(location);
                    break;
                case AbilityType.ChangeLocation:
                    turnOffLocation(); // Removes location effects.
                    stageQueue.Enqueue(GameStage.ChangeLocation); // TODO: Fix code.
                    break;
            }
            stageQueue.Enqueue(GameStage.RunBurst);
            ChaoticEngine.GStage = stageQueue.Dequeue();
        }

        private void triggeredHighlight(GameTime gameTime, Creature creature)
        {
            if (ChaoticEngine.Highlighter.IsEmpty())
                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, creature);
            else
                ChaoticEngine.Highlighter.AddHighlight(creature.Texture);
            stageQueue.Enqueue(GameStage.HighLight);
        }

        private void updateTriggered(GameTime gameTime, Creature your, Creature enemy, List<Tuple<Creature, TriggeredType>> triggeredList)
        {
            if (your.IntimidateCourage > 0)
            {
                triggeredList.Add(new Tuple<Creature, TriggeredType>(your, TriggeredType.IntimidateCourage));
                triggeredHighlight(gameTime, your);
            }
            if (your.IntimidatePower > 0)
            {
                triggeredList.Add(new Tuple<Creature, TriggeredType>(your, TriggeredType.IntimidatePower));
                triggeredHighlight(gameTime, your);
            }
            if (your.IntimidateWisdom > 0)
            {
                triggeredList.Add(new Tuple<Creature, TriggeredType>(your, TriggeredType.IntimidateWisdom));
                triggeredHighlight(gameTime, your);
            }
            if (your.IntimidateSpeed > 0)
            {
                triggeredList.Add(new Tuple<Creature, TriggeredType>(your, TriggeredType.IntimidateSpeed));
                triggeredHighlight(gameTime, your);
            }
            if (your is ChaoticGameLib.Creatures.LordVanBloot && enemy.Courage < 65)
            {
                triggeredList.Add(new Tuple<Creature, TriggeredType>(your, TriggeredType.LordVanBloot));
                triggeredHighlight(gameTime, your);
            }
        }

        private bool updateTriggeredBegCombat(GameTime gameTime)
        {
            if (!ChaoticEngine.sYouNode.CreatureNode.Negate)
                updateTriggered(gameTime, ChaoticEngine.sYouNode.CreatureNode, ChaoticEngine.sEnemyNode.CreatureNode, triggeredListAP);
            if (!ChaoticEngine.sEnemyNode.CreatureNode.Negate)
                updateTriggered(gameTime, ChaoticEngine.sEnemyNode.CreatureNode, ChaoticEngine.sYouNode.CreatureNode, triggeredListDP);

            if (triggeredListAP.Count > 1 || (triggeredListAP.Count == 1 && triggeredListDP.Count > 0))
                stageQueue.Enqueue(GameStage.TriggeredSelectAP);
            if (triggeredListDP.Count > 1 || (triggeredListDP.Count == 1 && triggeredListAP.Count > 0))
                stageQueue.Enqueue(GameStage.TriggeredSelectDP);

            if (triggeredListAP.Count == 1 && triggeredListDP.Count == 0)
            {
                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, triggeredListAP[0].Item1);
                stageQueue.Enqueue(GameStage.HighLight);
                setAbilityPath(true, AbilityType.Triggered, AbilityAction.Activate);
                ChaoticEngine.CurrentAbility.Add(triggeredListAP[0].Item1);
                ChaoticEngine.CurrentAbility.Add(triggeredListAP[0].Item2);
                ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sEnemyNode.CreatureNode);
                triggeredListAP.Clear();
                return true;
            }
            else if (triggeredListDP.Count == 1 && triggeredListAP.Count == 0)
            {
                ChaoticEngine.Highlighter.InitializeHighlight(gameTime, triggeredListDP[0].Item1);
                stageQueue.Enqueue(GameStage.HighLight);
                setAbilityPath(false, AbilityType.Triggered, AbilityAction.Activate);
                ChaoticEngine.CurrentAbility.Add(triggeredListDP[0].Item1);
                ChaoticEngine.CurrentAbility.Add(triggeredListDP[0].Item2);
                ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sYouNode.CreatureNode);
                triggeredListDP.Clear();
                return true;
            }
            else if (triggeredListAP.Count != 0 || triggeredListDP.Count != 0)
                stageQueue.Enqueue(GameStage.DecideToBurst);

            return !(triggeredListAP.Count == 0 && triggeredListDP.Count == 0);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            attackHand1.UpdateCoveredCard(mouse, description);
            attackHand2.UpdateCoveredCard(mouse, description);
            mugicHand1.UpdateCoveredCard(mouse, description);
            mugicHand2.UpdateCoveredCard(mouse, description);
            activeLocation1.UpdateActiveLocation(mouse, description);
            activeLocation2.UpdateActiveLocation(mouse, description);

            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                creatureSpaces[i].GetCardCoveredByMouse(mouse, description);
                if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    creatureSpaces[i].UpdateCardDescription(mouse);

                if (creatureSpaces[i].CreatureNode != null && 
                    creatureSpaces[i].CreatureNode.Negate != creatureSpaces[i].CreatureNode.PrevNegate)
                {
                    updateSupport();
                    hiveUpdate();

                    if (creatureSpaces[i].NumMoves - creatureSpaces[i].CreatureNode.Swift <= 0)
                        creatureSpaces[i].NumMoves = 0;
                    else
                        creatureSpaces[i].NumMoves = 1;

                    creatureSpaces[i].CreatureNode.PrevNegate = creatureSpaces[i].CreatureNode.Negate;
                }
            }

            if (backButton.UpdateButton(mouse, gameTime))
            {
                ChaoticEngine.MStage = MenuStage.MainMenu;
                Game.Components.Remove(this);
            }

            if (ChaoticEngine.Hive != ChaoticEngine.PrevHive)
                updateHive();

            Location actLoc = activeLocation1.LocationActive ?? activeLocation2.LocationActive;
            if (actLoc != null && actLoc.Negate != actLoc.PrevNegate)
            {
                turnOffLocation();
                actLoc.PrevNegate = actLoc.Negate;
            }

            ChaoticEngine.MsgBox.UpdateMessageBox(gameTime);

            ChaoticEngine.CodedEffects.UpdateCodedLetters(gameTime);
            ChaoticEngine.DamageEffects.UpdateDamageAmounts(gameTime);
            ChaoticEngine.BurstContents.UpdateBox(gameTime);

            switch (ChaoticEngine.GStage)
            {
                case GameStage.Shuffles:
                    ChaoticEngine.GStage = GameStage.ShuffleLocDeck1;
                    stageQueue.Enqueue(GameStage.ShuffleLocDeck2);
                    stageQueue.Enqueue(GameStage.ShuffleAtkDeck1);
                    stageQueue.Enqueue(GameStage.ShuffleAtkDeck2);
                    stageQueue.Enqueue(GameStage.BeginningOfTheGame);
                    break;
                case GameStage.BeginningOfTheGame:
                    attackDeck1.UpdateDeckPile(gameTime, attackHand1);
                    done = attackDeck2.UpdateDeckPile(gameTime, attackHand2);
                    if (done && numDrawed >= 1)
                    {
                        ChaoticEngine.GStage = GameStage.CoinFlip;

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
                case GameStage.CoinFlip:
                    bool? flip = coin.UpdateCoin(gameTime, Keyboard.GetState());

                    if (flip != null)
                    {
                        ChaoticEngine.GStage = GameStage.LocationStep;
                        ChaoticEngine.Player1Active = flip.Value;
                    }
                    break;
                case GameStage.ShowdownCoinFlip:
                    flip = coin.UpdateCoin(gameTime, Keyboard.GetState());

                    if (flip != null)
                    {
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                        ChaoticEngine.Player1Strike = flip.Value;
                    }
                    break;
                case GameStage.Action:
                    if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                        discardPile1.UpdateDiscardPile(gameTime, description);
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                        attackDiscardPile1.UpdateDiscardPile(gameTime, description);
                    if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                        discardPile2.UpdateDiscardPile(gameTime, description);
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                        attackDiscardPile2.UpdateDiscardPile(gameTime, description);

                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        updatePlayableAbilities(mouse, gameTime, ChaoticEngine.Player1Active);

                        if ((creatureSpaces.Count(b => b.CreatureNode != null && b.CreatureNode.MovedThisTurn == true) > 0 ||
                        ChaoticEngine.CombatThisTurn) && endTurnButton.UpdateButton(mouse, gameTime))
                        {
                            hadCombat[2] = hadCombat[1];
                            hadCombat[1] = hadCombat[0];
                            hadCombat[0] = ChaoticEngine.CombatThisTurn;
                            if (hadCombat[0] || hadCombat[1] || hadCombat[2])
                                endTurn();
                            else
                            {
                                ChaoticEngine.GStage = GameStage.SelectingCreature1;
                                stageQueue.Enqueue(GameStage.SelectingCreature2);
                                stageQueue.Enqueue(GameStage.ShowdownCoinFlip);
                                stageQueue.Enqueue(GameStage.Showdown);
                                stageQueue.Enqueue(GameStage.BeginningOfCombat);
                            }
                        }
                    }
                    break;
                case GameStage.LocationStep:
                    locationStep(gameTime, ChaoticEngine.Player1Active, GameStage.Action);
                    break;
                case GameStage.LocationStepCombat:
                    locationStep(gameTime, !ChaoticEngine.Player1Strike, stageQueue.Peek());
                    if (ChaoticEngine.GStage != GameStage.LocationStepCombat)
                        stageQueue.Dequeue();
                    break;
                case GameStage.ChangeLocationStep:
                    // TODO: Messes up initiative check.
                    locationStep(gameTime, Burst.Player1Turn, stageQueue.Peek());
                    if (ChaoticEngine.GStage != GameStage.ChangeLocationStep)
                        stageQueue.Dequeue();
                    break;
                case GameStage.CreatureToDiscard1:
                    creatureToDiscard(gameTime, mouse, ChaoticEngine.sYouNode, ChaoticEngine.sEnemyNode, discardPile1, 
                        GameStage.BattlegearToDiscard1);
                    break;
                case GameStage.CreatureToDiscard2:
                    creatureToDiscard(gameTime, mouse, ChaoticEngine.sEnemyNode, ChaoticEngine.sYouNode, discardPile2,
                        GameStage.BattlegearToDiscard2);
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
                            discardPile1.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime, description);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime, description);
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
                case GameStage.ChangeLocation:
                    returnLocation(gameTime, GameStage.ChangeLocationStep, GameStage.ChangeLocationStep);
                    break;
                case GameStage.Initiative:
                    ChaoticEngine.sYouNode.CreatureNode.ActivateBattlegear();
                    ChaoticEngine.sEnemyNode.CreatureNode.ActivateBattlegear();

                    Location initLoc = activeLocation1.LocationActive ?? activeLocation2.LocationActive;

                    int flag = initLoc.initiativeCheck(ChaoticEngine.sYouNode.CreatureNode,
                            ChaoticEngine.sEnemyNode.CreatureNode);

                    if (flag >= 0)
                        ChaoticEngine.Player1Strike = ChaoticEngine.Player1Active;
                    else
                        ChaoticEngine.Player1Strike = !ChaoticEngine.Player1Active;


                    // TODO: Fix SongOfTrans.
                    /*
                    if (activeLocation1.LocationActive != null)
                    {
                        int flagg = activeLocation1.LocationActive.initiativeCheck(ChaoticEngine.sYouNode.CreatureNode,
                            ChaoticEngine.sEnemyNode.CreatureNode);
                        if (flagg > 0)
                            ChaoticEngine.Player1Strike = true;
                        else if (flagg == 0)
                            ChaoticEngine.Player1Strike = ChaoticEngine.Player1Active;
                        else
                            ChaoticEngine.Player1Strike = false;
                    }
                    else if (activeLocation2.LocationActive != null)
                    {
                        int flagg = activeLocation2.LocationActive.initiativeCheck(ChaoticEngine.sYouNode.CreatureNode,
                            ChaoticEngine.sEnemyNode.CreatureNode);
                        if (flagg > 0)
                            ChaoticEngine.Player1Strike = false;
                        else if (flagg == 0)
                            ChaoticEngine.Player1Strike = ChaoticEngine.Player1Active;
                        else
                            ChaoticEngine.Player1Strike = true;
                    }
                    */
                    ChaoticEngine.GStage = GameStage.BeginningOfCombat;
                    break;
                case GameStage.BeginningOfCombat:
                    Location locActive = activeLocation1.LocationActive ?? activeLocation2.LocationActive;
                    if (!locActive.Negate)
                    {
                        if (locActive is ChaoticGameLib.Locations.CordacFalls)
                        {
                            ChaoticEngine.sYouNode.CreatureNode.Energy += 5;
                            ChaoticEngine.sYouNode.CreatureNode.GainedEnergyTurn += 5;
                            ChaoticEngine.sEnemyNode.CreatureNode.Energy += 5;
                            ChaoticEngine.sEnemyNode.CreatureNode.GainedEnergyTurn += 5;

                            ChaoticEngine.DamageEffects.AddDamageAmount(5, ChaoticEngine.sYouNode.Position);
                            ChaoticEngine.DamageEffects.AddDamageAmount(5, ChaoticEngine.sEnemyNode.Position);
                        }
                        else if (locActive is ChaoticGameLib.Locations.CordacFallsPlungepool)
                        {
                            ChaoticEngine.sYouNode.CreatureNode.Energy -= 5;
                            ChaoticEngine.sEnemyNode.CreatureNode.Energy -= 5;

                            ChaoticEngine.DamageEffects.AddDamageAmount(-5, ChaoticEngine.sYouNode.Position);
                            ChaoticEngine.DamageEffects.AddDamageAmount(-5, ChaoticEngine.sEnemyNode.Position);
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

                                    ChaoticEngine.DamageEffects.AddDamageAmount(10, creatureSpaces[i].CreatureNode.Position);
                                }
                            }
                        }
                        else if (locActive is ChaoticGameLib.Locations.FearValley)
                        {
                            if (ChaoticEngine.sYouNode.CreatureNode.Courage < ChaoticEngine.sEnemyNode.CreatureNode.Courage)
                            {
                                ChaoticEngine.sYouNode.CreatureNode.Energy -= 10;
                                ChaoticEngine.DamageEffects.AddDamageAmount(-10, ChaoticEngine.sYouNode.Position);
                            }
                            else if (ChaoticEngine.sYouNode.CreatureNode.Courage > ChaoticEngine.sEnemyNode.CreatureNode.Courage)
                            {
                                ChaoticEngine.sEnemyNode.CreatureNode.Energy -= 10;
                                ChaoticEngine.DamageEffects.AddDamageAmount(-10, ChaoticEngine.sEnemyNode.Position);
                            }
                        }
                        else if (locActive is ChaoticGameLib.Locations.ForestOfLife)
                        {
                            if (ChaoticEngine.sYouNode.CreatureNode.Power > ChaoticEngine.sEnemyNode.CreatureNode.Power)
                            {
                                ChaoticEngine.sYouNode.CreatureNode.Energy += 5;
                                ChaoticEngine.sYouNode.CreatureNode.GainedEnergyTurn += 5;

                                ChaoticEngine.DamageEffects.AddDamageAmount(5, ChaoticEngine.sYouNode.Position);
                            }
                            else if (ChaoticEngine.sYouNode.CreatureNode.Power < ChaoticEngine.sEnemyNode.CreatureNode.Power)
                            {
                                ChaoticEngine.sEnemyNode.CreatureNode.Energy += 5;
                                ChaoticEngine.sEnemyNode.CreatureNode.GainedEnergyTurn += 5;

                                ChaoticEngine.DamageEffects.AddDamageAmount(5, ChaoticEngine.sEnemyNode.Position);
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
                        else if (locActive is ChaoticGameLib.Locations.CastleBodhran)
                        {
                            int mugicDiscardCount1 = discardPile1.DiscardList.Count(c => c is Mugic);
                            int mugicDiscardCount2 = discardPile2.DiscardList.Count(c => c is Mugic);
                            if (mugicDiscardCount1 > 0)
                                stageQueue.Enqueue(GameStage.SelectMugic1);
                            if (mugicDiscardCount2 > 0)
                                stageQueue.Enqueue(GameStage.SelectMugic2);
                            if (mugicDiscardCount1 > 0)
                                stageQueue.Enqueue(GameStage.ReturnMugic1);
                            if (mugicDiscardCount2 > 0)
                                stageQueue.Enqueue(GameStage.ReturnMugic2);
                        }
                        else if (locActive is ChaoticGameLib.Locations.RavanaughRidge)
                        {
                            if (ChaoticEngine.sYouNode.CreatureNode.Air)
                                stageQueue.Enqueue(GameStage.LocationSelectAbility1);
                            if (ChaoticEngine.sEnemyNode.CreatureNode.Air)
                                stageQueue.Enqueue(GameStage.LocationSelectAbility2);
                        }
                    }

                    if ((ChaoticEngine.sYouNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.OrbOfForesight ||
                        ChaoticEngine.sYouNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.FluxBauble)
                        && !ChaoticEngine.sYouNode.CreatureNode.Battlegear.Negate)
                        stageQueue.Enqueue(GameStage.BattlegearSelectAbility1);

                    if ((ChaoticEngine.sEnemyNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.OrbOfForesight ||
                        ChaoticEngine.sEnemyNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.FluxBauble)
                        && !ChaoticEngine.sEnemyNode.CreatureNode.Battlegear.Negate)
                        stageQueue.Enqueue(GameStage.BattlegearSelectAbility2);

                    if (updateTriggeredBegCombat(gameTime))
                        Burst.BurstStart = GameStage.Combat;
                    else
                        stageQueue.Enqueue(GameStage.Combat);
                    ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.Combat:
                    if (!ChaoticEngine.IsACardMoving)
                    {
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            discardPile1.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime, description);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime, description);
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
                                stageQueue.Enqueue(GameStage.StrikePhase);
                            }
                            else if (attackDeck2.Count == 0)
                            {
                                ChaoticEngine.GStage = GameStage.ShuffleAtkDeck2;
                                stageQueue.Enqueue(GameStage.StrikePhase);
                            }
                            else
                                ChaoticEngine.GStage = GameStage.StrikePhase;
                        }
                        updatePlayableAbilities(mouse, gameTime, ChaoticEngine.Player1Strike);
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
                        ChaoticEngine.sYouNode.CreatureNode.RestoreCombat();
                        if (hadCombat[2] || hadCombat[1] || hadCombat[0]) // Not in Showdown.
                        {
                            ChaoticEngine.GStage = GameStage.MoveToCodedSpace;
                            for (int i = 0; i < creatureSpaces.Length; i++)
                            {
                                if (creatureSpaces[i] == ChaoticEngine.sEnemyNode)
                                    selectedSpace = i;
                            }
                        }
                        else // Showdown.
                        {
                            hadCombat[0] = ChaoticEngine.CombatThisTurn;
                            endTurn();
                        }
                    }
                    else if (ChaoticEngine.sEnemyNode.CreatureNode != null && ChaoticEngine.sYouNode.CreatureNode == null)
                    {
                        ChaoticEngine.sEnemyNode.CreatureNode.RestoreCombat();
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
                        turnOffLocation(); // Removes location effects.
                        if (!hadCombat[2] && !hadCombat[1] && !hadCombat[0])
                        {
                            hadCombat[0] = ChaoticEngine.CombatThisTurn;
                            endTurn();
                        }
                    }
                    else
                    {
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
                        turnOffLocation(); // Removes location effects.
                        if (!hadCombat[2] && !hadCombat[1] && !hadCombat[0])
                        {
                            hadCombat[0] = ChaoticEngine.CombatThisTurn;
                            endTurn();
                        }
                    }
                    ChaoticEngine.sYouNode.IsMovableSpace = false;
                    ChaoticEngine.sEnemyNode.IsMovableSpace = false;
                    break;
                case GameStage.MoveToCodedSpace:
                    if (ChaoticEngine.sYouNode.UpdateMoveCreature(gameTime, mouse, creatureSpaces[selectedSpace]))
                    {
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
                        turnOffLocation(); // Removes location effects.
                        creatureSpaces[selectedSpace].CreatureNode.MovedThisTurn = true;
                        updateSupport();
                    }
                    break;
                case GameStage.AttackSelectAbility:
                    if (!ChaoticEngine.Player1Strike)
                        attackSelectingAbilities(gameTime, attackDeck1, locationDeck1, attackDiscardPile1);
                    else
                        attackSelectingAbilities(gameTime, attackDeck2, locationDeck2, attackDiscardPile2);
                    break;
                case GameStage.LocationSelectAbility1:
                    locationSelectingAbilities(gameTime,
                        activeLocation1.LocationActive ?? activeLocation2.LocationActive, ChaoticEngine.Player1Active ? attackDeck1 : attackDeck2);
                    break;
                case GameStage.LocationSelectAbility2:
                    locationSelectingAbilities(gameTime,
                        activeLocation1.LocationActive ?? activeLocation2.LocationActive, ChaoticEngine.Player1Active ? attackDeck2 : attackDeck1);
                    break;
                case GameStage.BattlegearSelectAbility1:
                    battlegearSelectingAbilities(gameTime, ChaoticEngine.sYouNode.CreatureNode.Battlegear,
                        ChaoticEngine.Player1Active ? attackDeck1 : attackDeck2, 
                        ChaoticEngine.Player1Active ? locationDeck1 : locationDeck2);
                    break;
                case GameStage.BattlegearSelectAbility2:
                    battlegearSelectingAbilities(gameTime, ChaoticEngine.sEnemyNode.CreatureNode.Battlegear,
                        ChaoticEngine.Player1Active ? attackDeck2 : attackDeck1,
                        ChaoticEngine.Player1Active ? locationDeck2 : locationDeck1);
                    break;
                case GameStage.ShuffleAtkDeck1:
                    if (attackDeck1.UpdateShuffleDeck(gameTime))
                    {
                        if (attackDeck1.Count == 0) // This Attack deck has no cards left to draw.
                            attackDeck1.ShuffleDeck(attackDiscardPile1);
                        else // An Ability cause the shuffle.
                            attackDeck1.ShuffleDeck(); 
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.ShuffleAtkDeck2:
                    if (attackDeck2.UpdateShuffleDeck(gameTime))
                    {
                        if (attackDeck2.Count == 0) // This Attack deck has no cards left to draw.
                            attackDeck2.ShuffleDeck(attackDiscardPile2);
                        else // An Ability cause the shuffle.
                            attackDeck2.ShuffleDeck(); 
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.ShuffleLocDeck1:
                    if (locationDeck1.UpdateShuffleDeck(gameTime))
                    {
                        locationDeck1.ShuffleDeck();
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.ShuffleLocDeck2:
                    if (locationDeck2.UpdateShuffleDeck(gameTime))
                    {
                        locationDeck2.ShuffleDeck();
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.SelectingCreature1:
                    ChaoticEngine.sEnemyNode = selectingCreature(mouse, !ChaoticEngine.Player1Active);
                    break;
                case GameStage.SelectingCreature2:
                    ChaoticEngine.sYouNode = selectingCreature(mouse, ChaoticEngine.Player1Active);
                    break;
                case GameStage.DiscardMugic1:
                    if (activeLocation1.LocationActive != null)
                        mugicToDiscard(gameTime, mouse, mugicHand1, discardPile1);
                    else
                        mugicToDiscard(gameTime, mouse, mugicHand2, discardPile2);
                    break;
                case GameStage.DiscardMugic2:
                    if (activeLocation1.LocationActive != null)
                        mugicToDiscard(gameTime, mouse, mugicHand2, discardPile2);
                    else
                        mugicToDiscard(gameTime, mouse, mugicHand1, discardPile1);
                    break;
                case GameStage.SelectMugic1:
                    ChaoticEngine.ReturnSelectedIndex1 = selectMugicReturn(gameTime,
                        ChaoticEngine.Player1Active ? discardPile1 : discardPile2);
                    break;
                case GameStage.SelectMugic2:
                    ChaoticEngine.ReturnSelectedIndex2 = selectMugicReturn(gameTime,
                        ChaoticEngine.Player1Active ? discardPile2 : discardPile1);
                    break;
                case GameStage.ReturnMugic1:
                    if ((ChaoticEngine.Player1Active && 
                        discardPile1.ReturnToHand(gameTime, ChaoticEngine.ReturnSelectedIndex1, mugicHand1)) ||
                        (!ChaoticEngine.Player1Active &&
                        discardPile2.ReturnToHand(gameTime, ChaoticEngine.ReturnSelectedIndex1, mugicHand2)))
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.ReturnMugic2:
                    if ((ChaoticEngine.Player1Active &&
                        discardPile2.ReturnToHand(gameTime, ChaoticEngine.ReturnSelectedIndex2, mugicHand2)) ||
                        (!ChaoticEngine.Player1Active &&
                        discardPile1.ReturnToHand(gameTime, ChaoticEngine.ReturnSelectedIndex2, mugicHand1)))
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.Showdown:
                    ChaoticEngine.sYouNode.IsMovableSpace = true;
                    ChaoticEngine.sEnemyNode.IsMovableSpace = true;
                    ChaoticEngine.sYouNode.CreatureNode.ActivateBattlegear();
                    ChaoticEngine.sEnemyNode.CreatureNode.ActivateBattlegear();
                    ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.EndGame:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        ChaoticEngine.MStage = MenuStage.MainMenu;
                        Game.Components.Remove(this);
                    }
                    break;
                case GameStage.TargetCreature:
                    BattleBoardNode targetNode = null;
                    if (ChaoticEngine.CurrentAbility[0] is Creature)
                    {
                        Creature creature = ChaoticEngine.CurrentAbility[0] as Creature;
                        switch (ChaoticEngine.CurrentAbility.Action)
                        {
                            case AbilityAction.Activate:
                                targetNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces, 
                                    b => b.CreatureNode != null && creature.CheckAbilityTarget(b.CreatureNode, 
                                        b.IsPlayer1 == ChaoticEngine.CurrentAbility.IsPlayer1));
                                if (targetNode != null && ChaoticEngine.CurrentAbility.Type == AbilityType.SacrificeTargetCreature)
                                    ChaoticEngine.SelectedNode = targetNode;
                                break;
                            case AbilityAction.Sacrifice:
                                targetNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces,
                                    b => b.CreatureNode != null && creature.CheckSacrificeTarget(b.CreatureNode));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (ChaoticEngine.CurrentAbility[0] is Battlegear)
                    {
                        Battlegear battlegear = ChaoticEngine.CurrentAbility[0] as Battlegear;
                        Creature creatureEquipped = ChaoticEngine.CurrentAbility[1] as Creature;

                        switch (ChaoticEngine.CurrentAbility.Action)
                        {
                            case AbilityAction.Sacrifice:
                                targetNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces,
                                    b => b.CreatureNode != null && battlegear.CheckSacrificeTarget(b.CreatureNode));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (ChaoticEngine.CurrentAbility[0] is Mugic && ChaoticEngine.CurrentAbility.Count > 1)
                    {
                        Mugic mugic = ChaoticEngine.CurrentAbility[0] as Mugic;
                        Creature caster = ChaoticEngine.CurrentAbility[1] as Creature;

                        switch (ChaoticEngine.CurrentAbility.Action)
                        {
                            case AbilityAction.Cast:
                                targetNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces,
                                    b => b.CreatureNode != null && mugic.CheckPlayable(b.CreatureNode));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (ChaoticEngine.CurrentAbility[0] is Mugic)
                    {
                        Mugic mugic = ChaoticEngine.CurrentAbility[0] as Mugic;
                        Location activeLoc = activeLocation1.LocationActive ?? activeLocation2.LocationActive;

                        targetNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces,
                                    b => b.CreatureNode != null && b.IsPlayer1 == ChaoticEngine.CurrentAbility.IsPlayer1 &&
                                    mugic.CheckCanPayMugicCost(b.CreatureNode, activeLoc));
                        if (targetNode != null)
                            mugic.PayCost(targetNode.CreatureNode, activeLoc);
                    }
                    else if (ChaoticEngine.CurrentAbility[0] is Attack)
                    {
                        Attack attack = ChaoticEngine.CurrentAbility[0] as Attack;

                        targetNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces,
                                    b => b.CreatureNode != null && b.IsPlayer1 == ChaoticEngine.CurrentAbility.IsPlayer1 &&
                                    b.CreatureNode.MugicCounters >= 1);
                        if (targetNode != null)
                            targetNode.CreatureNode.MugicCounters -= 1;
                    }
                    if (targetNode != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(targetNode.CreatureNode);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.TargetBattlegear:
                    targetNode = SelectingSystem.SelectingBattlegear(mouse, creatureSpaces, b => b.HasBattegear());
                    if (targetNode != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(targetNode);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.SelCreatureToDiscard:
                    selectedCreatureToDiscard(gameTime, mouse, ChaoticEngine.SelectedNode,
                        ChaoticEngine.SelectedNode.IsPlayer1 ? discardPile1 : discardPile2);
                    break;
                case GameStage.SelBattlegearToDiscard:
                    selectedBattlegearToDiscard(gameTime, mouse, ChaoticEngine.SelectedNode,
                        ChaoticEngine.SelectedNode.IsPlayer1 ? discardPile1 : discardPile2);
                    break;
                case GameStage.SelCreatureInDiscard:
                    Predicate<Creature> selCreaturePred = c => c is Creature;
                    if (ChaoticEngine.CurrentAbility[0] is ICastReturn)
                    {
                        ICastReturn castReturn = ChaoticEngine.CurrentAbility[0] as ICastReturn;
                        selCreaturePred = c => castReturn.CheckReturnable(c);
                    }
                    else if (ChaoticEngine.CurrentAbility[0] is ISacrificeReturn)
                    {
                        ISacrificeReturn castReturn = ChaoticEngine.CurrentAbility[0] as ISacrificeReturn;
                        selCreaturePred = c => castReturn.CheckReturnable(c);
                    }
                    Creature selCreature = selectCreatureReturn(gameTime,
                        ChaoticEngine.CurrentAbility.IsPlayer1 ? discardPile1 : discardPile2, selCreaturePred);
                    if (selCreature != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(selCreature);
                    }
                    break;
                case GameStage.SelMugicInDiscard:
                    Predicate<Mugic> selMugicPred = m => m is Mugic;

                    Mugic selMugic = selectMugicReturn(gameTime,
                        ChaoticEngine.CurrentAbility.IsPlayer1 ? discardPile1 : discardPile2, selMugicPred);
                    if (selMugic != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(selMugic);
                    }
                    break;
                case GameStage.SelAttackDeck:
                    AttackDeck? atkDeck = SelectingSystem.SelectingAttackDeck(mouse, attackDeck1, attackDeck2);
                    if (atkDeck != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(atkDeck.Value);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.SelLocationDeck:
                    LocationDeck? locDeck = SelectingSystem.SelectingLocationDeck(mouse, locationDeck1, locationDeck2);
                    if (locDeck != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(locDeck.Value);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.SelAttackLocationDeck:
                    atkDeck = SelectingSystem.SelectingAttackDeck(mouse, attackDeck1, attackDeck2);
                    locDeck = SelectingSystem.SelectingLocationDeck(mouse, locationDeck1, locationDeck2);
                    if (atkDeck != null || locDeck != null)
                    {
                        if (atkDeck != null)
                        {
                            locationDeck1.DeckCovered = locationDeck2.DeckCovered = false;
                            ChaoticEngine.CurrentAbility.Add(atkDeck.Value);
                        }
                        else
                        {
                            attackDeck1.DeckCovered = attackDeck2.DeckCovered = false;
                            ChaoticEngine.CurrentAbility.Add(locDeck.Value);
                        }
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.SelCreatureReturn:
                    if ((ChaoticEngine.CurrentAbility.IsPlayer1 &&
                        discardPile1.ReturnToSpace(gameTime, ChaoticEngine.ReturnSelectedIndex1, ChaoticEngine.SelectedNode)) ||
                        (!ChaoticEngine.CurrentAbility.IsPlayer1 &&
                        discardPile2.ReturnToSpace(gameTime, ChaoticEngine.ReturnSelectedIndex1, ChaoticEngine.SelectedNode)))
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.SelReturnMugic:
                    if ((ChaoticEngine.CurrentAbility.IsPlayer1 &&
                        discardPile1.ReturnToHand(gameTime, ChaoticEngine.ReturnSelectedIndex1, mugicHand1)) ||
                        (!ChaoticEngine.CurrentAbility.IsPlayer1 &&
                        discardPile2.ReturnToHand(gameTime, ChaoticEngine.ReturnSelectedIndex1, mugicHand2)))
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.DecideToBurst:
                    if (!ChaoticEngine.MsgBox.Active && !ChaoticEngine.MsgBox.ClickedYes.HasValue)
                    {
                        ActiveLocation activeLoc = activeLocation1.LocationActive != null ? activeLocation1 : activeLocation2;
                         // Recklessness ability.
                        if (Burst.Alive && Burst.Peek()[0] is Attack)
                        {
                            Burst.StartedByAtk = true; // Telling us an Attack started the burst.

                            if (ChaoticEngine.Player1Active)
                            {
                                if (ChaoticEngine.Player1Strike && !ChaoticEngine.sYouNode.CreatureNode.Negate &&
                                    ChaoticEngine.sYouNode.CreatureNode.HasRecklessness())
                                {
                                    updateRecklessness(gameTime, ChaoticEngine.sYouNode.CreatureNode, true);
                                    break;
                                }
                                else if (!ChaoticEngine.Player1Strike && !ChaoticEngine.sEnemyNode.CreatureNode.Negate
                                    && ChaoticEngine.sEnemyNode.CreatureNode.HasRecklessness())
                                {
                                    updateRecklessness(gameTime, ChaoticEngine.sEnemyNode.CreatureNode, false);
                                    break;
                                }
                            }
                            else
                            {
                                if (ChaoticEngine.Player1Strike && !ChaoticEngine.sEnemyNode.CreatureNode.Negate
                                    && ChaoticEngine.sEnemyNode.CreatureNode.HasRecklessness())
                                {
                                    updateRecklessness(gameTime, ChaoticEngine.sEnemyNode.CreatureNode, true);
                                    break;
                                }
                                else if (!ChaoticEngine.Player1Strike && !ChaoticEngine.sYouNode.CreatureNode.Negate
                                    && ChaoticEngine.sYouNode.CreatureNode.HasRecklessness())
                                {
                                    updateRecklessness(gameTime, ChaoticEngine.sYouNode.CreatureNode, false);
                                    break;
                                }
                            }
                        }

                        if (Burst.Player1Turn &&
                            (CheckSystem.CheckAnyMugicPlayable(mugicHand1, creatureSpaces, discardPile1, activeLoc) ||
                            CheckSystem.CheckAnyCreature(true, creatureSpaces, discardPile1, activeLoc) ||
                            CheckSystem.CheckAnyBattlegearAbility(true, creatureSpaces, activeLoc) ||
                            CheckSystem.CheckAnyBattlegearSacrifice(true, creatureSpaces, discardPile1, activeLoc)))
                        {
                            ChaoticEngine.MsgBox.Active = true;
                        }
                        else if (!Burst.Player1Turn &&
                            (CheckSystem.CheckAnyMugicPlayable(mugicHand2, creatureSpaces, discardPile2, activeLoc) ||
                            CheckSystem.CheckAnyCreature(false, creatureSpaces, discardPile2, activeLoc) ||
                            CheckSystem.CheckAnyBattlegearAbility(false, creatureSpaces, activeLoc) ||
                            CheckSystem.CheckAnyBattlegearSacrifice(false, creatureSpaces, discardPile2, activeLoc)))
                        {
                            ChaoticEngine.MsgBox.Active = true;
                        }
                        else
                        {
                            ChaoticEngine.MsgBox.ClickedYes = false;
                        }
                    }
                    else if (ChaoticEngine.MsgBox.ClickedYes.HasValue)
                    {
                        if (ChaoticEngine.MsgBox.ClickedYes.Value)
                            ChaoticEngine.GStage = GameStage.AddingToBurst;
                        else
                        {
                            if (!ChaoticEngine.MsgBox.PrevClick)
                            {
                                ChaoticEngine.GStage = GameStage.RunBurst;
                            }
                            else
                                Burst.Player1Turn = Burst.Player1Turn;
                        }
                        ChaoticEngine.MsgBox.ClickedYes = null;
                    }
                    break;
                case GameStage.AddingToBurst:
                    if (!ChaoticEngine.IsACardMoving)
                    {
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            discardPile1.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime, description);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime, description);
                    }
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        updatePlayableAbilities(mouse, gameTime, Burst.Player1Turn);
                        if (mouse.RightButton == ButtonState.Pressed)
                        {
                            ChaoticEngine.GStage = GameStage.DecideToBurst;
                            ChaoticEngine.MsgBox.Restore();
                        }
                    }
                    
                    break;
                case GameStage.RunBurst:
                    if (Burst.Alive)
                    {
                        setAbilityRun(gameTime);
                    }
                    else
                    {
                        //ChaoticEngine.GStage = Burst.BurstStart;
                        ChaoticEngine.GStage = GameStage.DiscardCreatures;
                    }
                    break;
                case GameStage.HighLight:
                    if (ChaoticEngine.Highlighter.UpdateHighlight(gameTime))
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.SelLocationAny:
                    LocationDeck locationDeck = (LocationDeck)ChaoticEngine.CurrentAbility[1];
                    selectAnyOrdering<Location>(gameTime, locationAnyPanel,
                        locationDeck.IsPlayer1 ? locationDeck1.Deck : locationDeck2.Deck, ref topLoc);
                    break;
                case GameStage.SelLocationOrder:
                    LocationDeck selectedLocDeck = (LocationDeck)ChaoticEngine.CurrentAbility[1];
                    creatureSelectingAbilities(gameTime, ChaoticEngine.CurrentAbility[0] as Creature,
                        selectedLocDeck.IsPlayer1 ? locationDeck1 : locationDeck2);
                    break;
                case GameStage.SelAttackLocationOrder:
                    if (ChaoticEngine.CurrentAbility[2] is LocationDeck)
                    {
                        LocationDeck locSelDeck = (LocationDeck)ChaoticEngine.CurrentAbility[2];
                        selectOrdering<Location>(gameTime, locationPanel1,
                            locSelDeck.IsPlayer1 ? locationDeck1.Deck : locationDeck2.Deck, ref topLoc);
                    }
                    else
                    {
                        AttackDeck atkSelDeck = (AttackDeck)ChaoticEngine.CurrentAbility[2];
                        selectOrdering<Attack>(gameTime, attackPanel1,
                            atkSelDeck.IsPlayer1 ? attackDeck1.Deck : attackDeck2.Deck, ref topAtk);
                    }
                    break;
                case GameStage.SelUnoccupiedSpace:
                    ChaoticEngine.SelectedNode = SelectingSystem.SelectingOpenSpace(mouse, creatureSpaces);
                    if (ChaoticEngine.SelectedNode != null)
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
                case GameStage.SelElemental:
                    byte element = elementPanel.UpdatePanel(gameTime);
                   
                    if (element != 0)
                    {
                        ChaoticEngine.CurrentAbility.Add(element);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.DiscardCreatures:
                    int idiom;
                    for (idiom = 0; idiom < creatureSpaces.Length; idiom++)
                    {
                        if (creatureSpaces[idiom] != ChaoticEngine.sYouNode && creatureSpaces[idiom] != ChaoticEngine.sEnemyNode &&
                            creatureSpaces[idiom].CreatureNode != null && creatureSpaces[idiom].CreatureNode.Energy == 0)
                        {
                            ChaoticEngine.SelectedNode = creatureSpaces[idiom];
                            stageQueue.Enqueue(ChaoticEngine.GStage);
                            while (stageQueue.Peek() != ChaoticEngine.GStage)
                                stageQueue.Enqueue(stageQueue.Dequeue());
                            ChaoticEngine.GStage = GameStage.SelCreatureToDiscard;
                            break;
                        }
                    }

                    if (idiom == creatureSpaces.Length)
                        ChaoticEngine.GStage = Burst.BurstStart;
                    break;
                case GameStage.StrikePhase:
                    if (!ChaoticEngine.IsACardMoving)
                    {
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            discardPile1.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile1.UpdateDiscardPile(gameTime, description);
                        if (!attackDiscardPile1.DiscardPanelActive && !attackDiscardPile2.DiscardPanelActive && !discardPile1.DiscardPanelActive)
                            discardPile2.UpdateDiscardPile(gameTime, description);
                        if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                            attackDiscardPile2.UpdateDiscardPile(gameTime, description);
                    }
                    if (!discardPile1.DiscardPanelActive && !attackDiscardPile1.DiscardPanelActive
                        && !attackDiscardPile2.DiscardPanelActive && !discardPile2.DiscardPanelActive)
                    {
                        if (ChaoticEngine.Player1Strike)
                            updateCombat(gameTime, mouse, attackHand1, attackDiscardPile1);
                        else
                            updateCombat(gameTime, mouse, attackHand2, attackDiscardPile2, false);
                    }
                    break;
                case GameStage.TargetEngaged:
                    BattleBoardNode engagedNode = SelectingSystem.SelectingCreature(mouse, creatureSpaces,
                        b => b == ChaoticEngine.sYouNode || b == ChaoticEngine.sEnemyNode);
                    if (engagedNode != null)
                    {
                        ChaoticEngine.CurrentAbility.Add(engagedNode.CreatureNode);
                        
                        if (engagedNode == ChaoticEngine.sYouNode)
                            ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sEnemyNode.CreatureNode);
                        else
                            ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sYouNode.CreatureNode);

                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.TriggeredSelectAP:
                    selectTriggeredOrder(true, gameTime, triggerPanel, triggeredListAP);
                    break;
                case GameStage.TriggeredSelectDP:
                    selectTriggeredOrder(false, gameTime, triggerPanel, triggeredListDP);
                    break;
                case GameStage.HiveCallDecision:
                    attackMsgBox.Active = true;
                    attackMsgBox.UpdateMessageBox(gameTime);
                    if (attackMsgBox.ClickedYes.HasValue)
                    {
                        ChaoticEngine.CurrentAbility.Add(attackMsgBox.ClickedYes.Value);
                        if (attackMsgBox.ClickedYes.Value)
                            ChaoticEngine.GStage = GameStage.TargetCreature;
                        else
                            ChaoticEngine.GStage = stageQueue.Dequeue();
                        attackMsgBox.ClickedYes = null;
                    }
                    break;
                case GameStage.TargetAttack:
                    // TODO: Fix Melody of Mirage and Droskin so attack card can be separated by another mugic.
                    Burst.NextAbility();
                    ChaoticEngine.CurrentAbility.Add(Burst.Peek()[0] as Attack);
                    Burst.Push(ChaoticEngine.CurrentAbility);
                    ChaoticEngine.GStage = stageQueue.Dequeue();
                    break;
            }
            ChaoticEngine.PrevState = mouse;

            base.Update(gameTime);
        }

        private void turnOffLocation()
        {
            Location loc = activeLocation1.LocationActive ?? activeLocation2.LocationActive;
            if (loc is ChaoticGameLib.Locations.LavaPond)
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.Magmon)
                        creatureSpaces[i].CreatureNode.FireDamage -= 5;
                }
            }
            else if (loc is ChaoticGameLib.Locations.GothosTower)
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (creatureSpaces[i].CreatureNode != null)
                    {
                        if (!(creatureSpaces[i].CreatureNode is ChaoticGameLib.Creatures.LordVanBloot))
                            creatureSpaces[i].CreatureNode.Courage += 10;
                        else
                            creatureSpaces[i].CreatureNode.Strike -= 15;
                    }
                }
            }
            else if (loc is ChaoticGameLib.Locations.RunicGrove)
                ChaoticEngine.GenericMugicOnly = false;
            else if (loc is ChaoticGameLib.Locations.KiruCity)
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (creatureSpaces[i].CreatureNode != null &&
                        creatureSpaces[i].CreatureNode.CreatureTribe == Tribe.OverWorld)
                    {
                        short energy = creatureSpaces[i].CreatureNode.Energy;
                        creatureSpaces[i].CreatureNode.RemoveGainedEnergy(10);
                        if (Math.Abs(energy - creatureSpaces[i].CreatureNode.Energy)!=0)
                            ChaoticEngine.DamageEffects.AddDamageAmount(-10, creatureSpaces[i].CreatureNode.Position);
                    }
                }
            }
            else if (loc is ChaoticGameLib.Locations.IronPillar)
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (creatureSpaces[i].CreatureNode != null)
                        creatureSpaces[i].CreatureNode.UnNegateBattlegear();
                }
            }
        }

        private BattleBoardNode selectingCreature(MouseState mouse, bool isPlayer1)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].IsPlayer1 == isPlayer1)
                    creatureSpaces[i].IsSelectible = true;

                if (creatureSpaces[i].MouseCoveredCreature && mouse.LeftButton == ButtonState.Pressed &&
                    creatureSpaces[i].IsSelectible)
                {
                    for (int j = 0; j < creatureSpaces.Length; j++)
                    {
                        creatureSpaces[j].IsSelectible = false;
                    }
                    ChaoticEngine.GStage = stageQueue.Dequeue();
                    return creatureSpaces[i];
                }
            }
            return null;
        }

        private int selectMugicReturn(GameTime gameTime, DiscardPile<ChaoticCard> discardPile)
        {
            if (!mugicPanel.Active)
            {
                discardedMugics = discardPile.Find<Mugic>();
                mugicPanel.Active = true;
            }
            List<int> indices = mugicPanel.UpdatePanel(gameTime, discardedMugics, description);
            if (indices != null)
            {
                ChaoticEngine.GStage = stageQueue.Dequeue();
                int returnIndex = discardPile.DiscardList.IndexOf(discardedMugics[indices[0]]);
                discardedMugics = null;
                return returnIndex;
            }
            return -1;
        }

        private Mugic selectMugicReturn(GameTime gameTime, DiscardPile<ChaoticCard> discardPile, Predicate<Mugic> selMugic)
        {
            if (!mugicPanel.Active)
            {
                discardedMugics = discardPile.Find<Mugic>(selMugic);
                mugicPanel.Active = true;
            }
            List<int> indices = mugicPanel.UpdatePanel(gameTime, discardedMugics, description);
            if (indices != null)
            {
                ChaoticEngine.GStage = stageQueue.Dequeue();
                Mugic retMugic = discardedMugics[indices[0]];
                discardedMugics = null;
                return retMugic;
            }
            return null;
        }

        private Creature selectCreatureReturn(GameTime gameTime, DiscardPile<ChaoticCard> discardPile, Predicate<Creature> selectPred)
        {
            if (!creaturePanel.Active)
            {
                discardedCreature = discardPile.Find<Creature>(selectPred);
                creaturePanel.Active = true;
            }
            List<int> indices = creaturePanel.UpdatePanel(gameTime, discardedCreature, description);
            if (indices != null)
            {
                ChaoticEngine.GStage = stageQueue.Dequeue();
                Creature retCreature = discardedCreature[indices[0]];
                discardedCreature = null;
                return retCreature;
            }
            return null;
        }

        private void selectOrdering<T>(GameTime gameTime, SelectPanel<T> panel, List<T> deckPile, ref List<T> topCards) where T : ChaoticCard
        {
            if (!panel.Active)
            {
                topCards = new List<T>(){
                                deckPile[deckPile.Count - 1],
                                deckPile[deckPile.Count - 2]};
                if (panel.SelectNumber == 3)
                    topCards.Add(deckPile[deckPile.Count - 3]);
                panel.Active = true;
            }
            List<int> indices = panel.UpdatePanel(gameTime, topCards, description);
            if (indices != null)
            {
                deckPile.RemoveAt(deckPile.Count - 1);
                deckPile.RemoveAt(deckPile.Count - 1);
                if (panel.SelectNumber == 3)
                    deckPile.RemoveAt(deckPile.Count - 1);

                deckPile.Add(topCards[indices[0]]);
                if (panel.SelectNumber == 3)
                    deckPile.Insert(0, topCards[indices[2]]);
                deckPile.Insert(0, topCards[indices[1]]);

                ChaoticEngine.GStage = stageQueue.Dequeue();
                topCards = null;
            }
        }

        private void selectAnyOrdering<T>(GameTime gameTime, SelectPanel<T> panel, List<T> deckPile, ref List<T> topCards) where T : ChaoticCard
        {
            if (!panel.Active)
            {
                topCards = new List<T>(){
                                deckPile[deckPile.Count - 1],
                                deckPile[deckPile.Count - 2]};
                if (panel.SelectNumber == 3)
                    topCards.Add(deckPile[deckPile.Count - 3]);
                panel.Active = true;
            }
            List<int> indices = panel.UpdatePanel(gameTime, topCards, description);
            if (indices != null)
            {
                deckPile.RemoveAt(deckPile.Count - 1);
                deckPile.RemoveAt(deckPile.Count - 1);
                if (panel.SelectNumber == 3)
                {
                    deckPile.RemoveAt(deckPile.Count - 1);
                    deckPile.Add(topCards[indices[2]]);
                }
                deckPile.Add(topCards[indices[1]]);
                deckPile.Add(topCards[indices[0]]);

                ChaoticEngine.GStage = stageQueue.Dequeue();
                topCards = null;
            }
        }

        private void selectTriggeredOrder(bool isPlayer1, GameTime gameTime, TriggeredPanel panel, 
            List<Tuple<Creature, TriggeredType>> triggeredLst)
        {
            if (!panel.Active)
            {
                panel.InitializeSelectNumber(triggeredLst.Count);
                panel.Active = true;
            }

            List<int> ordering = panel.UpdatePanel(gameTime, triggeredLst, description);

            if (ordering != null)
            {
                if (!Burst.Alive)
                    Burst.InitializeBurst(GameStage.Combat);
                for (int i = 0; i < ordering.Count; i++)
                {
                    ChaoticEngine.CurrentAbility = new Ability(isPlayer1, AbilityType.Triggered, AbilityAction.Activate);
                    
                    switch (triggeredLst[ordering[i]].Item2)
                    {
                        case TriggeredType.IntimidateCourage:
                        case TriggeredType.IntimidatePower:
                        case TriggeredType.IntimidateWisdom:
                        case TriggeredType.IntimidateSpeed:
                        case TriggeredType.LordVanBloot:
                            ChaoticEngine.CurrentAbility.Add(triggeredLst[ordering[i]].Item1);
                            ChaoticEngine.CurrentAbility.Add(triggeredLst[ordering[i]].Item2);
                            if (isPlayer1)
                                ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sEnemyNode.CreatureNode);
                            else
                                ChaoticEngine.CurrentAbility.Add(ChaoticEngine.sYouNode.CreatureNode);
                            break;
                        case TriggeredType.Recklessness:
                            break;
                        default:
                            break;
                    }
                    Burst.Push(ChaoticEngine.CurrentAbility);
                }
                triggeredLst.Clear();
                ChaoticEngine.GStage = stageQueue.Dequeue();
            }
        }

        private void attackSelectingAbilities(GameTime gameTime, AttackDeck attackDeck, LocationDeck locationDeck,
            DiscardPile<Attack> attackDiscardPile)
        {
            if (attackDiscardPile[attackDiscardPile.Count - 1] is SqueezePlay)
            {
                selectOrdering<Attack>(gameTime, attackPanel1, attackDeck.Deck, ref topAtk);
            }
            else if (attackDiscardPile[attackDiscardPile.Count - 1] is Nexdoors)
            {
                selectOrdering<Attack>(gameTime, attackPanel1, attackDeck.Deck, ref topAtk);
            }
            else if (attackDiscardPile[attackDiscardPile.Count - 1] is FlashKick)
            {
                selectOrdering<Location>(gameTime, locationPanel1, locationDeck.Deck, ref topLoc);
            }
        }

        private void creatureSelectingAbilities(GameTime gameTime, Creature creature, LocationDeck locationDeck)
        {
            if (creature is ChaoticGameLib.Creatures.Blazier)
                selectOrdering<Location>(gameTime, locationPanel1, locationDeck.Deck, ref topLoc);
        }

        private void locationSelectingAbilities(GameTime gameTime, Location location, AttackDeck attackDeck)
        {
            if (location is ChaoticGameLib.Locations.RavanaughRidge && attackDeck.Count >= 2)
            {
                if (attackDeck.Count > 2)
                    selectOrdering<Attack>(gameTime, attackPanel2, attackDeck.Deck, ref topAtk);
                else
                    selectOrdering<Attack>(gameTime, attackPanel1, attackDeck.Deck, ref topAtk);
            }
            else if (location is ChaoticGameLib.Locations.RavanaughRidge)
                ChaoticEngine.GStage = stageQueue.Dequeue();
        }

        private void battlegearSelectingAbilities(GameTime gameTime, Battlegear battlegear, AttackDeck attackDeck,
            LocationDeck locationDeck)
        {
            if (battlegear is ChaoticGameLib.Battlegears.OrbOfForesight && attackDeck.Count >= 2)
            {
                if (attackDeck.Count > 2)
                    selectOrdering<Attack>(gameTime, attackPanel2, attackDeck.Deck, ref topAtk);
                else
                    selectOrdering<Attack>(gameTime, attackPanel1, attackDeck.Deck, ref topAtk);
            }
            else if (battlegear is ChaoticGameLib.Battlegears.OrbOfForesight)
                ChaoticEngine.GStage = stageQueue.Dequeue();
            else if (battlegear is ChaoticGameLib.Battlegears.FluxBauble)
            {
                selectOrdering<Location>(gameTime, locationPanel1, locationDeck.Deck, ref topLoc);
            }
        }

        private void stageOfGame()
        {
            switch (ChaoticEngine.GStage)
            {
                case GameStage.LocationStep:
                    currentStage = "Location Step";
                    break;
                case GameStage.Action:
                    currentStage = "Action Step";
                    break;
                case GameStage.Combat:
                case GameStage.Initiative:
                case GameStage.LocationStepCombat:
                case GameStage.ReturnLocationCombat:
                case GameStage.AttackDamage:
                    currentStage = "Combat";
                    break;
                case GameStage.EndOfCombat:
                case GameStage.MoveToCodedSpace:
                    currentStage = "End Of Combat";
                    break;
                case GameStage.EndGame:
                    currentStage = "End Of Game";
                    break;
                case GameStage.BeginningOfCombat:
                    currentStage = "Beginning Of Combat";
                    break;
                case GameStage.Showdown:
                case GameStage.ShowdownCoinFlip:
                    currentStage = "Showdown";
                    break;
                case GameStage.RunBurst:
                    currentStage = "Burst";
                    break;
                case GameStage.StrikePhase:
                    currentStage = "Strike Phase";
                    break; 
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            backButton.Draw(spriteBatch);
            attackPanel1.DrawPanel(spriteBatch, topAtk);
            attackPanel2.DrawPanel(spriteBatch, topAtk);
            locationPanel1.DrawPanel(spriteBatch, topLoc);
            locationAnyPanel.DrawPanel(spriteBatch, topLoc);
            discardPile1.DrawDiscardPile(spriteBatch);
            discardPile2.DrawDiscardPile(spriteBatch);
            attackDiscardPile1.DrawDiscardPile(spriteBatch);
            attackDiscardPile2.DrawDiscardPile(spriteBatch);
            attackHand1.DrawHand(spriteBatch);
            attackHand2.DrawHand(spriteBatch);
            mugicHand1.DrawHand(spriteBatch);
            mugicHand2.DrawHand(spriteBatch);
            locationDeck1.DrawDeckPile(spriteBatch);
            locationDeck2.DrawDeckPile(spriteBatch);
            activeLocation1.DrawActiveLocation(spriteBatch, true);
            activeLocation2.DrawActiveLocation(spriteBatch, false);

            ChaoticEngine.CodedEffects.DrawCodedLetters(spriteBatch);
            ChaoticEngine.DamageEffects.DrawDamageAmounts(spriteBatch);
            ChaoticEngine.BurstContents.DrawBox(spriteBatch);

            if (ChaoticEngine.GStage != GameStage.ShuffleAtkDeck1)
                attackDeck1.DrawDeckPile(spriteBatch);

            if (ChaoticEngine.GStage != GameStage.ShuffleAtkDeck2)
                attackDeck2.DrawDeckPile(spriteBatch);

            if (ChaoticEngine.GStage != GameStage.ShuffleLocDeck1)
                locationDeck1.DrawDeckPile(spriteBatch);

            if (ChaoticEngine.GStage != GameStage.ShuffleLocDeck2)
                locationDeck2.DrawDeckPile(spriteBatch);
    
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                creatureSpaces[i].DrawBattleBoardNode(spriteBatch, cardBack);
            }
            // The card covered and description.
            if ((activeLocation1.LocationActive != null && activeLocation1.LocationActive.Texture == description.CoveredCard) ||
                (activeLocation2.LocationActive != null && activeLocation2.LocationActive.Texture == description.CoveredCard) ||
                (locationPanel1.Active && (topLoc[0].Texture == description.CoveredCard || topLoc[1].Texture == description.CoveredCard)))
                description.DrawDescription(spriteBatch, true);
            else
                description.DrawDescription(spriteBatch);

            selectingDesription.DrawDescription(spriteBatch);

            // The background image.
            spriteBatch.Draw(ChaoticEngine.BackgroundSprite, backgroundRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            // The HUB of the game.
            string turn = "Turn: " + (ChaoticEngine.Player1Active ? "Player 1" : "Player 2");
            spriteBatch.DrawString(hubFont, turn, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                - endgamefont.MeasureString(turn).X / 2, graphics.PreferredBackBufferHeight / 2
                - endgamefont.MeasureString(turn).Y / 2), Color.Brown);
            string hiveText = "Hive: " + (ChaoticEngine.Hive ? "On" : "Off");
            spriteBatch.DrawString(hubFont, hiveText, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                - endgamefont.MeasureString(hiveText).X / 2, graphics.PreferredBackBufferHeight / 2
                - endgamefont.MeasureString(hiveText).Y / 2 + endgamefont.MeasureString(turn).Y), Color.Brown);
            stageOfGame();
            string stageText = "Phase: " + currentStage;
            spriteBatch.DrawString(hubFont, stageText, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                - endgamefont.MeasureString(stageText).X / 2, graphics.PreferredBackBufferHeight / 2
                - endgamefont.MeasureString(stageText).Y / 2 + 2*endgamefont.MeasureString(turn).Y), Color.White);
            if (ChaoticEngine.GenericMugicOnly)
            {
                string genericText = "Generic Only";
                spriteBatch.DrawString(hubFont, genericText, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                    - endgamefont.MeasureString(genericText).X / 2, graphics.PreferredBackBufferHeight / 2
                    - endgamefont.MeasureString(genericText).Y / 2 + 3*endgamefont.MeasureString(turn).Y), Color.Gray);
            }

            switch (ChaoticEngine.GStage)
            {
                case GameStage.BeginningOfTheGame:
                    break;
                case GameStage.CoinFlip:
                case GameStage.ShowdownCoinFlip:
                    coin.DrawCoin(spriteBatch);
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
                case GameStage.StrikePhase:
                case GameStage.Combat:
                    if (ChaoticEngine.sYouNode.CreatureNode != null &&
                        ChaoticEngine.sEnemyNode.CreatureNode != null)
                    {
                        Location loc = activeLocation1.LocationActive ?? activeLocation2.LocationActive;

                        if (ChaoticEngine.Player1Active)
                        {
                            setAttackInfo(ChaoticEngine.sYouNode.CreatureNode, ChaoticEngine.sEnemyNode.CreatureNode,
                                ChaoticEngine.Player1Active);
                            attackHand1.ProjectHandDamage(spriteBatch, ChaoticEngine.sYouNode.CreatureNode,
                            ChaoticEngine.sEnemyNode.CreatureNode, loc);
                            setAttackInfo(ChaoticEngine.sEnemyNode.CreatureNode, ChaoticEngine.sYouNode.CreatureNode,
                                !ChaoticEngine.Player1Active);
                            attackHand2.ProjectHandDamage(spriteBatch, ChaoticEngine.sEnemyNode.CreatureNode,
                                ChaoticEngine.sYouNode.CreatureNode, loc);
                        }
                        else
                        {
                            setAttackInfo(ChaoticEngine.sEnemyNode.CreatureNode, ChaoticEngine.sYouNode.CreatureNode,
                                !ChaoticEngine.Player1Active);
                            attackHand1.ProjectHandDamage(spriteBatch, ChaoticEngine.sEnemyNode.CreatureNode,
                               ChaoticEngine.sYouNode.CreatureNode, loc);
                            setAttackInfo(ChaoticEngine.sYouNode.CreatureNode, ChaoticEngine.sEnemyNode.CreatureNode,
                                ChaoticEngine.Player1Active);
                            attackHand2.ProjectHandDamage(spriteBatch, ChaoticEngine.sYouNode.CreatureNode,
                           ChaoticEngine.sEnemyNode.CreatureNode, loc);
                        }
                    }
                    break;
                case GameStage.ShuffleAtkDeck1:
                    attackDeck1.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.ShuffleAtkDeck2:
                    attackDeck2.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.ShuffleLocDeck1:
                    locationDeck1.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.ShuffleLocDeck2:
                    locationDeck2.DrawShuffleDeck(spriteBatch);
                    break;
                case GameStage.SelectMugic1:
                case GameStage.SelectMugic2:
                case GameStage.SelMugicInDiscard:
                    mugicPanel.DrawPanel(spriteBatch, discardedMugics);
                    break;
                case GameStage.ReturnMugic1:
                    if (ChaoticEngine.Player1Active)
                        discardPile1.DrawDiscardPile(spriteBatch, ChaoticEngine.ReturnSelectedIndex1);
                    else
                        discardPile2.DrawDiscardPile(spriteBatch, ChaoticEngine.ReturnSelectedIndex1);
                    break;
                case GameStage.ReturnMugic2:
                    if (ChaoticEngine.Player1Active)
                        discardPile2.DrawDiscardPile(spriteBatch, ChaoticEngine.ReturnSelectedIndex2);
                    else
                        discardPile1.DrawDiscardPile(spriteBatch, ChaoticEngine.ReturnSelectedIndex2);
                    break;
                case GameStage.SelReturnMugic:
                case GameStage.SelCreatureReturn:
                    if (ChaoticEngine.CurrentAbility.IsPlayer1)
                        discardPile1.DrawDiscardPile(spriteBatch, ChaoticEngine.ReturnSelectedIndex1);
                    else
                        discardPile2.DrawDiscardPile(spriteBatch, ChaoticEngine.ReturnSelectedIndex1);
                    break;
                case GameStage.EndGame:
                    string endText = player1Won.HasValue ? (player1Won.Value ? "Player 1" : "Player 2") + " is the winner. (Press Enter)" :
                        "Tie Game. (Press Enter)";
                    spriteBatch.DrawString(endgamefont, endText, new Vector2(graphics.PreferredBackBufferWidth / 2
                        - endgamefont.MeasureString(endText).X / 2, graphics.PreferredBackBufferHeight / 2
                        - endgamefont.MeasureString(endText).Y / 2), Color.Blue);
                    break;
                case GameStage.SelCreatureInDiscard:
                    creaturePanel.DrawPanel(spriteBatch, discardedCreature);
                    break;
                case GameStage.HighLight:
                    ChaoticEngine.Highlighter.DrawHighlight(spriteBatch);
                    break;
                case GameStage.SelElemental:
                    elementPanel.DrawPanel(spriteBatch);
                    break;
                case GameStage.TriggeredSelectAP:
                    triggerPanel.DrawPanel(spriteBatch, triggeredListAP);
                    break;
                case GameStage.TriggeredSelectDP:
                    triggerPanel.DrawPanel(spriteBatch, triggeredListDP);
                    break;
                case GameStage.HiveCallDecision:
                    attackMsgBox.DrawMessageBox(spriteBatch);
                    break;
                default:
                    break;
            }
            ChaoticEngine.MsgBox.DrawMessageBox(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}