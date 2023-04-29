using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class LoadBinaryTreeTraverseAnimationMessage
    {
        public BinaryTreeTraverseMode Mode { get; set; }

        public LoadBinaryTreeTraverseAnimationMessage(BinaryTreeTraverseMode mode)
        {
            Mode = mode;
        }
    }
}
