using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class StoryboradSpeedRadioChangedMessage
    {
        public double SpeedRatio { get; set; }
        public StoryboradSpeedRadioChangedMessage(double newSpeedRatio)
        {
            SpeedRatio = newSpeedRatio;
        }
    }
}
