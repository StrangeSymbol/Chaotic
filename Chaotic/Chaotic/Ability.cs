﻿using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ChaoticGameLib;

namespace Chaotic
{
    enum AbilityAction : byte { Activate, Sacrifice, Cast, }
    class Ability
    {
        bool isPlayer1;
        AbilityType type;
        AbilityAction action;
        ArrayList abilityList;

        public Ability(bool isPlayer1, AbilityType type, AbilityAction action)
        {
            this.isPlayer1 = isPlayer1;
            this.type = type;
            this.action = action;
            abilityList = new ArrayList();
        }

        public bool IsPlayer1 { get { return isPlayer1; } }
        public AbilityType Type { get { return type; } }
        public AbilityAction Action { get { return action; } }

        public void Add(object obj)
        {
            abilityList.Add(obj);
        }

        public int Count { get { return abilityList.Count; } }

        public object this[int i]
        {
            get { return abilityList[i]; }
        }

        public bool IsTarget(object card)
        {
            for (int i = 1; i < abilityList.Count; i++)
            {
                if (card.Equals(abilityList[i]))
                    return true;
            }
            return false;
        }
    }
}