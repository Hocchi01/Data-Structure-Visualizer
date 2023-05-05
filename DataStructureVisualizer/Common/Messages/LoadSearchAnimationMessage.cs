using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    internal class LoadSearchAnimationMessage
    {
        public int Value { get; set; }
        public LoadSearchAnimationMessage(int value)
        {
            Value = value;
        }
    }
}
