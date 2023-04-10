using Microsoft.Expression.Shapes;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataStructureVisualizer.Common.AnimationLib
{
    public class SimulatedRectAnimation : MyRectAnimationBase
    {
        public SimulatedRectAnimation(Rect to, double time, Rect? from = null, Action? before = null, Action? after = null) : base(to, time, from, before, after)
        {
            AccelerationRatio = 0.5;
            DecelerationRatio = 0.5;
        }
    }
}
