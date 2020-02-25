/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;

namespace ChaoticGameLib
{
    public class InitiativeCheckException : Exception
    {
        string initiativeCheck;

        public InitiativeCheckException(string s)
        {
            this.initiativeCheck = s;
        }
        public string InitiativeCheck { get { return initiativeCheck; } set { initiativeCheck = value; } }
    }
}
