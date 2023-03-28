using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class DataSourceChangedMessage
    {
        public ObservableCollection<int> Value { get; }
        public DataSourceChangedMessage(ObservableCollection<int> ds) { Value = ds; }
    }
}
