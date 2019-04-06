using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;

namespace Chaotic
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ChaoticDeckSetup : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DeckSetupNode[] creatureSpaces;
        CreatureNumber creatureNumber;

        MenuButton backBtn;
        Button letsGetChaoticBtn;

        DeckSetupSelecter<Creature> creatureSelecter;
        DeckSetupSelecter<Battlegear> battlegearSelecter;
        DeckSetupSelecter<Attack> attackSelecter;
        DeckSetupSelecter<Mugic> mugicSelecter;
        DeckSetupSelecter<Location> locationSelecter;

        DeckSetupPanel<Mugic> mugicPanel;
        DeckSetupPanel<Location> locationPanel;
        DeckSetupPanel<Attack> attackPanel;

        public ChaoticDeckSetup(Game game, GraphicsDeviceManager graphics, CreatureNumber creatureNumber)
            : base(game)
        {
            this.graphics = graphics;
            this.creatureNumber = creatureNumber;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Texture2D creatureSpaceSprite = Game.Content.Load<Texture2D>(@"BattleBoardSprites\CardSpace");
            Texture2D overlay = Game.Content.Load<Texture2D>("OkButtonCover");
            Texture2D spaceCover = Game.Content.Load<Texture2D>("CardOutline");
            ChaoticEngine.CoveredCard = Game.Content.Load<Texture2D>("CardBack");
            SpriteFont font = Game.Content.Load<SpriteFont>(@"Fonts\CardDescriptionFont");

            switch (creatureNumber)
            {
                case CreatureNumber.SixOnSix:
                    creatureSpaces = new DeckSetupNode[6];
                    creatureSpaces[0] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(graphics.PreferredBackBufferWidth / 2 - ChaoticEngine.kCardWidth / 2,
                            3 * ChaoticEngine.kCardHeight + 3 * ChaoticEngine.kBattlegearGap + 3 * ChaoticEngine.kCardGap));
                    creatureSpaces[1] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X - ChaoticEngine.kCardWidth,
                        creatureSpaces[0].Position.Y - ChaoticEngine.kCardHeight - 
                        ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap));
                    creatureSpaces[2] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[1].Position.Y));
                    creatureSpaces[3] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[1].Position.X - ChaoticEngine.kCardWidth,
                        creatureSpaces[1].Position.Y - ChaoticEngine.kCardHeight -
                        ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap));
                    creatureSpaces[4] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[1].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[3].Position.Y));
                    creatureSpaces[5] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[2].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[3].Position.Y));
                    break;
                case CreatureNumber.ThreeOnThree:
                    creatureSpaces = new DeckSetupNode[3];
                    creatureSpaces[0] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(graphics.PreferredBackBufferWidth / 2 -
                        ChaoticEngine.kCardWidth / 2,
                        2 * ChaoticEngine.kCardHeight + 2 * ChaoticEngine.kBattlegearGap + 2 * ChaoticEngine.kCardGap));
                    creatureSpaces[1] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X - ChaoticEngine.kCardWidth, creatureSpaces[0].Position.Y 
                        - ChaoticEngine.kCardHeight - ChaoticEngine.kCardGap - ChaoticEngine.kBattlegearGap));
                    creatureSpaces[2] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(creatureSpaces[0].Position.X + ChaoticEngine.kCardWidth, creatureSpaces[1].Position.Y));
                    break;
                case CreatureNumber.OneOnOne:
                    creatureSpaces = new DeckSetupNode[1];
                    creatureSpaces[0] = new DeckSetupNode(creatureSpaceSprite, spaceCover, font,
                        new Vector2(graphics.PreferredBackBufferWidth / 2 -
                        ChaoticEngine.kCardWidth / 2,
                        ChaoticEngine.kCardHeight + ChaoticEngine.kBattlegearGap + ChaoticEngine.kCardGap));
                    break;
            }

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                creatureSpaces[i].GetCardCoveredByMouse(mouse);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
   
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                creatureSpaces[i].DrawBattleBoardNode(spriteBatch);
            }

            creatureSelecter.DrawDeckSetupSelecter(spriteBatch);
            battlegearSelecter.DrawDeckSetupSelecter(spriteBatch);

            if (creatureNumber == CreatureNumber.OneOnOne || creatureNumber == CreatureNumber.ThreeOnThree)
            {
                mugicSelecter.DrawDeckSetupSelecter(spriteBatch);
                mugicPanel.DrawPanel(spriteBatch);
                locationSelecter.DrawDeckSetupSelecter(spriteBatch);
                locationPanel.DrawPanel(spriteBatch);
                attackSelecter.DrawDeckSetupSelecter(spriteBatch);
                attackPanel.DrawPanel(spriteBatch);
            }
                //spriteBatch.Draw(ChaoticEngine.CoveredCard, new Rectangle(3, 6, 245, 175), Color.White);
                //spriteBatch.Draw(ChaoticEngine.CoveredCard, new Rectangle(3, 6, 175, 245), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}