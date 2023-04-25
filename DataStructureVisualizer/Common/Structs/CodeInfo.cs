using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Structs
{
    public struct CodeInfo
    {
        public int LineNum { get; set; }
        public int Height { get; set; } = 1;

        public CodeInfo(int lineNum)
        {
            LineNum = lineNum;
        }

        public CodeInfo(int lineNum, int height) : this(lineNum)
        {
            Height = height;
        }
    }
}
