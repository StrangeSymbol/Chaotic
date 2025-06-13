/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System.Collections.Generic;
using ChaoticGameLib;

namespace Chaotic
{
    static class Burst
    {
        static Stack<Ability> burstStack; // Contains the abilities to resolve.
        static GameStage burstStart; // Contains the stage where the Burst started
        static bool player1Turn; // Holds onto whose current turn it is.
        static bool startedByAtk; // Holds whether an Attack started the burst.

        public static void InitializeBurst(GameStage startStage)
        {
            burstStack = new Stack<Ability>();
            burstStart = startStage;
            startedByAtk = false;
            ChaoticEngine.MsgBox.Reset();
        }

        public static GameStage BurstStart { get { return burstStart; } set { burstStart = value; } }
        public static bool Alive { get { return burstStack != null && burstStack.Count > 0; } }
        public static bool Player1Turn 
        { 
            get { return !(player1Turn ^ (burstStack.Count == 1 && burstStack.Peek()[0] is Attack
                && (ChaoticEngine.MsgBox.ClickedYes == null ||
                ChaoticEngine.MsgBox.ClickedYes.Value)));
            }
            set { player1Turn = value; }
        }

        public static bool StartedByAtk { get { return startedByAtk; } set { startedByAtk = value; } }

        public static void Empty()
        {
            if (burstStack != null)
            {
                burstStack.Clear();
            }
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

        public static string[] BurstBoxInfo()
        {
            if (burstStack == null)
                return new string[] { };

            Ability[] abilities = burstStack.ToArray();
            List<string> abilityList = new List<string>();

            foreach (Ability ability in abilities)
            {
                string abilityText = ability.ToString();
                abilityList.AddRange(abilityText.Split('_'));
            }
            return abilityList.ToArray();
        }
    }
}
