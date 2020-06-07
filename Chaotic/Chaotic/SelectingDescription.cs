/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    class SelectingDescription
    {
        Texture2D panel; // The panel to write on.
        Vector2 position;
        SpriteFont font;
        const int kBorderWidth = 5;

        public SelectingDescription(ContentManager content, GraphicsDeviceManager graphics)
        {
            panel = content.Load<Texture2D>(@"Panel/Panel");
            font = content.Load<SpriteFont>(@"Fonts/Description");
            position = new Vector2(graphics.PreferredBackBufferWidth / 2, 0);
        }

        public void DrawDescription(SpriteBatch spriteBatch)
        {
            string description = String.Empty;
            switch (ChaoticEngine.GStage)
            {
                case GameStage.Moving:
                    description = "Left Click A Yellow Bordered Battleboard Space To Move To.";
                    break;
                case GameStage.Action:
                    description = "Move A Creature Or Play An Ability.";
                    break;
                case GameStage.Combat:
                    description = "Draw An Attack Or Play An Ability,";
                    break;
                case GameStage.ShuffleAtkDeck1:
                case GameStage.ShuffleAtkDeck2:
                    description = "Shuffling Attack Deck.";
                    break;
                case GameStage.SelectingCreature1:
                case GameStage.SelectingCreature2:
                    description = "Left Click A Red Bordered Enemy Creature Space To Start Showdown With.";
                    break;
                case GameStage.ShuffleLocDeck1:
                case GameStage.ShuffleLocDeck2:
                    description = "Shuffling Location Deck.";
                    break;
                case GameStage.SelectMugic1:
                case GameStage.SelectMugic2:
                case GameStage.SelMugicInDiscard:
                    description = "Select A Discarded Mugic To Return To Hand.";
                    break;
                case GameStage.DiscardMugic1:
                case GameStage.DiscardMugic2:
                    description = "Select A Mugic To Discard.";
                    break;
                case GameStage.TargetCreature:
                    description = "Left Click A White Bordered Creature Space.";
                    break;
                case GameStage.SelCreatureInDiscard:
                    description = "Select Coded Creature To Return To The Board.";
                    break;
                case GameStage.TargetBattlegear:
                    description = "Left Click A Yellow Bordered Battleboard Space.";
                    break;
                case GameStage.SelAttackDeck:
                    description = "Left Click One Of The Attack Decks.";
                    break;
                case GameStage.SelLocationDeck:
                    description = "Left Click One Of The Attack Decks.";
                    break;
                case GameStage.SelAttackLocationDeck:
                    description = "Left Click One Of Either Attack Or Location Decks.";
                    break;
                case GameStage.SelElemental:
                    description = "Left Click One Of The Elements To Give To Targeted Creature.";
                    break;
                case GameStage.AddingToBurst:
                    description = "Play An Ability.";
                    break;
                case GameStage.SelUnoccupiedSpace:
                    description = "Left Click A Yellow Bordered Unoccupied Space.";
                    break;
                case GameStage.StrikePhase:
                    description = "Select An Attack Card To Play.";
                    break;
                case GameStage.TargetEngaged:
                    description = "Select An Engaged Creature (Left Click A Yellow Bordered).";
                    break;
                default:
                    return;
            }
            Vector2 textDimensions = font.MeasureString(description);
            Vector2 panelPos = new Vector2(position.X - textDimensions.X / 2 - kBorderWidth,
                position.Y);

            spriteBatch.Draw(panel, new Rectangle((int)panelPos.X,
                (int)panelPos.Y, (int)textDimensions.X + 2*kBorderWidth, (int)textDimensions.Y + 2 * kBorderWidth), null,
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);

            spriteBatch.DrawString(font, description, new Vector2(panelPos.X + kBorderWidth,
                panelPos.Y + kBorderWidth), Color.Black,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 0.09f);
        }
    }
}