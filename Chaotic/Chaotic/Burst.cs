using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ChaoticGameLib;

namespace Chaotic
{
    class Burst
    {
        CardHighlighter cardHighlighter; // When a card is added to burst puts a sheen on that card.
        ChaoticMessageBox msgBox; // Checks whether a player wants to add to Burst.
        Stack<Ability> burstStack; // Contains the abilities to resolve.

        public Burst(ContentManager content, GraphicsDeviceManager graphics)
        {
            cardHighlighter = new CardHighlighter(content, graphics);
            msgBox = new ChaoticMessageBox("Adding To Burst", "Do you want to add another ability to Burst?", content, graphics);
        }
    }
}
