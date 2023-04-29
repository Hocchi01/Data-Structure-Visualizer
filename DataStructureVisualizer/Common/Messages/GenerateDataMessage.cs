using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class GenerateDataMessage
    {
        public int[] Values { get; set; }
        public TreeType TreeType { get; set; }

        public GenerateDataMessage(int[] values)
        {
            Values = values;
        }
    }
}
