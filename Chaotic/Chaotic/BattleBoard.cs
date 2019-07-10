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
    enum GameStage { Shuffles, BeginningOfTheGame, CoinFlip, DrawingAttack, LocationStep, Moving, ReturnLocation, 
    CreatureToDiscard1, CreatureToDiscard2, BattlegearToDiscard1, BattlegearToDiscard2, Action, Combat, Initiative, EndOfCombat,
    MoveToCodedSpace, AttackSelectAbility, ChangeLocation, ShuffleAtkDeck1, ShuffleAtkDeck2, EndGame, BeginningOfCombat,
    LocationStepCombat, ReturnLocationCombat, LocationSelectAbility1, LocationSelectAbility2, 
    BattlegearSelectAbility1, BattlegearSelectAbility2, SelectingCreature1, SelectingCreature2, Showdown, ShowdownCoinFlip,
    ShuffleLocDeck1, ShuffleLocDeck2, SelectMugic1, SelectMugic2, ReturnMugic1, ReturnMugic2, DiscardMugic1, DiscardMugic2,
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
        SelectPanel<Location> locationPanel2;
        List<Location> topLoc;
        SelectPanel<Mugic> mugicPanel;
        List<Mugic> discardedMugics;

        CreatureNumber creatureNumber;
        bool done;
        bool player1Won;
        byte numDrawed;

        Queue<GameStage> stageQueue; // Keeps track of next stages.

        SpriteFont endgamefont;
        SpriteFont hubFont;

        bool[] hadCombat;

        CardDescription description;

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
            hadCombat = new bool[3] { false, false, true };
            topAtk = null;
            topLoc = null;
            discardedMugics = null;
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
            mugicHand1 = new MugicHand(true, cardBack, discardPosition);
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
            mugicHand2 = new MugicHand(false, cardBack, discardPosition);
            for (int i = 0; i < ChaoticEngine.sMugics2.Count; i++)
                mugicHand2.AddCardToHand(ChaoticEngine.sMugics2[i]);
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
            locationPanel1 = new SelectPanel<Location>(Game.Content, graphics, 2, "Top", "Bottom");
            locationPanel2 = new SelectPanel<Location>(Game.Content, graphics, 3, "Top", "Bottom", "2nd From\n Bottom");
            mugicPanel = new SelectPanel<Mugic>(Game.Content, graphics, 1, "Return");

            Texture2D backTexture = Game.Content.Load<Texture2D>("Menu/SuitBackButton");
            backButton = new Button(backTexture, new Vector2(graphics.PreferredBackBufferWidth - backTexture.Width - 20, 20),
                Game.Content.Load<Texture2D>("Menu/backBtnCover"));
            endTurnButton = new Button(Game.Content.Load<Texture2D>("BattleBoardSprites/EndTurnButton"),
                new Vector2(attackDeck1.Position.X + ChaoticEngine.kCardWidth, graphics.PreferredBackBufferHeight / 2),
                Game.Content.Load<Texture2D>("Menu/MenuButtonCover"));
            endgamefont = Game.Content.Load<SpriteFont>("Fonts/EndGame");
            hubFont = Game.Content.Load<SpriteFont>("Fonts/HUB");

            coin = new Coin(Game.Content, graphics);
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
                stageQueue.Enqueue(cStage);
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
                else if (loc is ChaoticGameLib.Locations.EyeOfTheMaelstrom)
                {
                    ChaoticEngine.GStage = GameStage.DiscardMugic1;
                    stageQueue.Enqueue(GameStage.DiscardMugic2);
                    stageQueue.Enqueue(next);
                }
                else if (loc is ChaoticGameLib.Locations.RunicGrove)
                    ChaoticEngine.GenericMugicOnly = true;
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
                ChaoticEngine.GStage = GameStage.ReturnLocation;
            else
                ChaoticEngine.GStage = GameStage.LocationStep;
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
                {
                    setAttackInfo(your, enemy, player1);
                    attackDiscardPile[attackDiscardPile.Count - 1].Damage(your, enemy, activeLocation);
                }
                else
                {
                    setAttackInfo(enemy, your, player1);
                    attackDiscardPile[attackDiscardPile.Count - 1].Damage(enemy, your, activeLocation);
                }

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
                    ChaoticEngine.GStage = GameStage.ShuffleAtkDeck1;
                    stageQueue.Enqueue(GameStage.ShuffleAtkDeck2);
                    stageQueue.Enqueue(GameStage.Combat);
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is SqueezePlay &&
                        attackDeck.Deck.Count > 1)
                {
                    ChaoticEngine.GStage = GameStage.AttackSelectAbility;
                    stageQueue.Enqueue(GameStage.Combat);
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is FlashKick &&
                        locationDeck.Deck.Count > 1)
                {
                    ChaoticEngine.GStage = GameStage.AttackSelectAbility;
                    stageQueue.Enqueue(GameStage.Combat);
                }

                else if (attackDiscardPile[attackDiscardPile.Count - 1] is CoilCrush &&
                    ((ChaoticEngine.Player1Active && your.Power >= 75
                        && enemy.Battlegear != null) ||
                    (!ChaoticEngine.Player1Active && enemy.Power >= 75
                        && your.Battlegear != null)))
                {
                    ChaoticEngine.GStage = player1 ? GameStage.BattlegearToDiscard2 : GameStage.BattlegearToDiscard1;
                    stageQueue.Enqueue(GameStage.Combat);
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
                    stageQueue.Enqueue(GameStage.Combat);
                }
                else if (attackDiscardPile[attackDiscardPile.Count - 1] is IronBalls)
                    ChaoticEngine.GenericMugicOnly = true;

                ChaoticEngine.Player1Strike = !ChaoticEngine.Player1Strike;
            }
        }

        private void setAttackInfo(Creature you, Creature enemy, bool isPlayer1)
        {
            if (you.FirstAttack && you.Invisibility() && creatureSpaces.Count(c => c.IsPlayer1 == isPlayer1 
                    && c.CreatureNode is ChaoticGameLib.Creatures.MarquisDarini) > 0)
                ChaoticLibEngine.HasMaquisDarini = true;
            else
                ChaoticLibEngine.HasMaquisDarini = false;
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
                case GameStage.DrawingAttack:
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
                                stageQueue.Enqueue(GameStage.Combat);
                                attackDeck1.ShuffleDeck(attackDiscardPile1);
                            }
                            else if (attackDeck2.Count == 0)
                            {
                                ChaoticEngine.GStage = GameStage.ShuffleAtkDeck2;
                                stageQueue.Enqueue(GameStage.Combat);
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
                    locationStep(gameTime, !ChaoticEngine.Player1Strike, stageQueue.Peek());
                    if (ChaoticEngine.GStage != GameStage.LocationStepCombat)
                        stageQueue.Dequeue();
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
                    
                    ChaoticEngine.GStage = GameStage.BeginningOfCombat;
                    break;
                case GameStage.BeginningOfCombat:
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
                    if (ChaoticEngine.sYouNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.OrbOfForesight ||
                        ChaoticEngine.sYouNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.FluxBauble)
                        stageQueue.Enqueue(GameStage.BattlegearSelectAbility1);
                    if (ChaoticEngine.sEnemyNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.OrbOfForesight ||
                        ChaoticEngine.sEnemyNode.CreatureNode.Battlegear is ChaoticGameLib.Battlegears.FluxBauble)
                        stageQueue.Enqueue(GameStage.BattlegearSelectAbility2);

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
                            if (ChaoticEngine.GStage == GameStage.AttackSelectAbility ||
                                ChaoticEngine.GStage == GameStage.BattlegearToDiscard1 ||
                                ChaoticEngine.GStage == GameStage.BattlegearToDiscard2 ||
                                ChaoticEngine.GStage == GameStage.ReturnLocationCombat)
                            {
                                stageQueue.Clear();
                                stageQueue.Enqueue(GameStage.EndOfCombat);
                            }
                            else if (ChaoticEngine.GStage == GameStage.ShuffleAtkDeck1)
                            {
                                stageQueue.Clear();
                                stageQueue.Enqueue(GameStage.ShuffleAtkDeck2);
                                stageQueue.Enqueue(GameStage.EndOfCombat);
                            }
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
                    else
                    {
                        ChaoticEngine.sEnemyNode.CreatureNode.RestoreCombat();
                        ChaoticEngine.GStage = GameStage.ReturnLocation;
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
                        activeLocation1.LocationActive != null ? activeLocation1.LocationActive : 
                        activeLocation2.LocationActive, ChaoticEngine.Player1Active ? attackDeck1 : attackDeck2);
                    break;
                case GameStage.LocationSelectAbility2:
                    locationSelectingAbilities(gameTime,
                        activeLocation1.LocationActive != null ? activeLocation1.LocationActive :
                        activeLocation2.LocationActive, ChaoticEngine.Player1Active ? attackDeck2 : attackDeck1);
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
                        attackDiscardPile1.DiscardList.AddRange(attackDeck1.Deck);
                        attackDeck1.Deck.Clear();
                        attackDeck1.ShuffleDeck(attackDiscardPile1);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.ShuffleAtkDeck2:
                    if (attackDeck2.UpdateShuffleDeck(gameTime))
                    {
                        attackDiscardPile2.DiscardList.AddRange(attackDeck2.Deck);
                        attackDeck2.Deck.Clear();
                        attackDeck2.ShuffleDeck(attackDiscardPile2);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.ShuffleLocDeck1:
                    if (locationDeck1.UpdateShuffleDeck(gameTime))
                    {
                        locationDeck1.Deck.Clear();
                        locationDeck1.ShuffleDeck(ChaoticEngine.sLocations1);
                        ChaoticEngine.GStage = stageQueue.Dequeue();
                    }
                    break;
                case GameStage.ShuffleLocDeck2:
                    if (locationDeck2.UpdateShuffleDeck(gameTime))
                    {
                        locationDeck2.Deck.Clear();
                        locationDeck2.ShuffleDeck(ChaoticEngine.sLocations2);
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
            }

            base.Update(gameTime);
        }

        private BattleBoardNode selectingCreature(MouseState mouse, bool isPlayer1)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].IsPlayer1 == isPlayer1)
                    creatureSpaces[i].IsMovableSpace = true;

                if (creatureSpaces[i].MouseCoveredCreature && mouse.LeftButton == ButtonState.Pressed &&
                    creatureSpaces[i].IsMovableSpace)
                {
                    for (int j = 0; j < creatureSpaces.Length; j++)
                    {
                        creatureSpaces[j].IsMovableSpace = false;
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

        private void attackSelectingAbilities(GameTime gameTime, AttackDeck attackDeck, LocationDeck locationDeck,
            DiscardPile<Attack> attackDiscardPile)
        {
            if (attackDiscardPile[attackDiscardPile.Count - 1] is SqueezePlay)
            {
                selectOrdering<Attack>(gameTime, attackPanel1, attackDeck.Deck, ref topAtk);
            }
            else if (attackDiscardPile[attackDiscardPile.Count - 1] is FlashKick)
            {
                selectOrdering<Location>(gameTime, locationPanel1, locationDeck.Deck, ref topLoc);
            }
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

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            backButton.Draw(spriteBatch);
            attackPanel1.DrawPanel(spriteBatch, topAtk);
            attackPanel2.DrawPanel(spriteBatch, topAtk);
            locationPanel1.DrawPanel(spriteBatch, topLoc);
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
            // The background image.
            spriteBatch.Draw(ChaoticEngine.BackgroundSprite, backgroundRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            // The HUB of the game.
            string hiveText = "Hive: " + (ChaoticEngine.Hive ? "On" : "Off");
            spriteBatch.DrawString(hubFont, hiveText, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                - endgamefont.MeasureString(hiveText).X / 2, graphics.PreferredBackBufferHeight / 2
                - endgamefont.MeasureString(hiveText).Y / 2), Color.Brown);
            if (ChaoticEngine.GenericMugicOnly)
            {
                string genericText = "Generic Only";
                spriteBatch.DrawString(hubFont, genericText, new Vector2(9 * graphics.PreferredBackBufferWidth / 10
                    - endgamefont.MeasureString(genericText).X / 2, graphics.PreferredBackBufferHeight / 2
                    - endgamefont.MeasureString(genericText).Y / 2 + endgamefont.MeasureString(hiveText).Y), Color.Gray);
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
                case GameStage.Combat:
                    Location loc = activeLocation1.LocationActive != null ? activeLocation1.LocationActive : activeLocation2.LocationActive;
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
                case GameStage.EndGame:
                    string endText = (player1Won ? "Player 1" : "Player 2") + " is the winner. (Press Enter)";
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