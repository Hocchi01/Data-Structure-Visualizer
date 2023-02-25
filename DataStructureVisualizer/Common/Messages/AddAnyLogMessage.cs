using DataStructureVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    internal class AddAnyLogMessage
    {
        public LogViewModel Log { get; }
        public AddAnyLogMessage(LogViewModel log) { Log = log; }
    }
}
