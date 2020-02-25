/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    class CardDescription
    {
        Texture2D panel; // The panel to write on.
        Texture2D coveredCard;
        SpriteFont font;
        string description; // Holds the description of the card.
        const int kCardSpaceWidth = 3;
        const int kCardSpaceHeight = 6;
        const int kPanelWidth = ChaoticEngine.kCardCoveredHeight;
        readonly int kPanelHeight;

        public CardDescription(ContentManager content, GraphicsDeviceManager graphics)
        {
            panel = content.Load<Texture2D>(@"Panel/Panel");
            font = content.Load<SpriteFont>(@"Fonts/Description");
            coveredCard = content.Load<Texture2D>("CardBack");
            description = String.Empty;
            kPanelHeight = graphics.PreferredBackBufferHeight - 3 * kCardSpaceHeight - ChaoticEngine.kCardCoveredHeight;
        }

        public Texture2D CoveredCard { get { return coveredCard; } set { coveredCard = value; } }
        public string Description {
        set 
        { 
            description = value;
            int start = 0;
            int end = description.IndexOf(' ');
            while (end < description.Length - 1)
            {
                int space = description.Substring(end + 1).Contains(' ') ?
                    description.Substring(end + 1).IndexOf(' ') : description.Substring(end + 1).Length - 1;

                if (kPanelWidth - font.MeasureString(description.Substring(start, space + end - start + 1)).X < 0)
                {
                    description = description.Insert(end+1, "\n");
                    end++;
                    start = end;
                }
                else
                    end += space + 1;
            }
        } }

        public void DrawDescription(SpriteBatch spriteBatch, bool isLocation=false)
        {
            if (isLocation)
                spriteBatch.Draw(coveredCard,
                    new Rectangle(kCardSpaceWidth, kCardSpaceHeight,
                        ChaoticEngine.kCardCoveredHeight, ChaoticEngine.kCardCoveredWidth), Color.White);
            else
                spriteBatch.Draw(coveredCard,
                    new Rectangle(kCardSpaceWidth, kCardSpaceHeight,
                        ChaoticEngine.kCardCoveredWidth, ChaoticEngine.kCardCoveredHeight), Color.White);
            spriteBatch.Draw(panel, new Rectangle(kCardSpaceWidth,
                2*kCardSpaceHeight + ChaoticEngine.kCardCoveredHeight, kPanelWidth, kPanelHeight), null,
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
            spriteBatch.DrawString(font, description, new Vector2(kCardSpaceWidth, 
                2 * kCardSpaceHeight + ChaoticEngine.kCardCoveredHeight), Color.Black,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }
    }
}