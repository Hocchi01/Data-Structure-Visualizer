using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class DataSourceChangedMessage
    {
        public List<int> Value { get; }
        public DataSourceChangedMessage(List<int> ds) { Value = ds; }
    }
}
