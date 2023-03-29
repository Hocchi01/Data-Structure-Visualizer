using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class LoadSortAnimationMessage
    {
        public SortType Type { get; set; }

        public LoadSortAnimationMessage(SortType type)
        {
            Type = type;
        }
    }
}
