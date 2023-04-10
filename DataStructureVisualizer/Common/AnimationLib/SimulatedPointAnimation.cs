using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataStructureVisualizer.Common.AnimationLib
{
    public class SimulatedPointAnimation : MyPointAnimationBase
    {
        public SimulatedPointAnimation(Point to, double time, Point? from = null, Action? before = null, Action? after = null) : base(to, time, from, before, after)
        {
            AccelerationRatio = 0.5;
            DecelerationRatio = 0.5;
        }
    }
}
