/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaotic
{
    public class FileContentException : Exception
    {
        string fileContent;

        public FileContentException(string s)
        {
            this.fileContent = s;
        }
        public string FileContent { get { return fileContent; } set { fileContent = value; } }
    }
}
