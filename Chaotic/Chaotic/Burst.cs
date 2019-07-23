using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ChaoticGameLib;

namespace Chaotic
{
    static class Burst
    {
        static Stack<Ability> burstStack; // Contains the abilities to resolve.
        static GameStage burstStart; // Contains the stage where the Burst started
        static bool player1Turn; // Holds onto whose current turn it is.

        public static void InitializeBurst(GameStage startStage)
        {
            burstStack = new Stack<Ability>();
            burstStart = startStage;
        }

        public static GameStage BurstStart { get { return burstStart; } }
        public static bool Alive { get { return burstStack != null && burstStack.Count > 0; } }
        public static bool Player1Turn 
        { 
            get { return !(player1Turn ^ (burstStack.Count == 1 && burstStack.Peek()[0] is Attack)); }
            set { player1Turn = value; }
        }

        public static Ability Peek()
        {
            return burstStack.Peek();
        }

        public static Ability NextAbility()
        {
            return burstStack.Pop();
        }

        public static void Push(Ability abil)
        {
            burstStack.Push(abil);
            player1Turn = abil.IsPlayer1;
        }
    }
}
