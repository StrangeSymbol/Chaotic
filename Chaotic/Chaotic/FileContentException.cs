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
