using DataStructureVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    static class AnimationTimelineExtension
    {
        public static void SetActions(this AnimationTimeline timeline, Action? before, Action? after) 
        {
            timeline.CurrentStateInvalidated += (sender, e) =>
            {
                Clock clock = (Clock)sender;
                switch (clock.CurrentState)
                {
                    case ClockState.Active:
                        if (before != null)
                        {
                            before();
                        }
                        break;
                    case ClockState.Filling:
                        if (after != null)
                        {
                            after();
                        }
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
