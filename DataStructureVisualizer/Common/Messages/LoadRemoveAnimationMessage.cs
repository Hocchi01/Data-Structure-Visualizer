using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    internal class LoadRemoveAnimationMessage
    {
        public int Index { get; }
        public LoadRemoveAnimationMessage(int index)
        {
            Index = index;
        }
    }
}
