using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AnimationLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataStructureVisualizer.Views.Components
{
    /// <summary>
    /// Interaction logic for LineArrow.xaml
    /// </summary>
    public partial class LineArrow : UserControl
    {
        public LineArrow()
        {
            InitializeComponent();
        }

        //public LineArrow(double length) : this()
        //{
        //    SetLength(length);
        //}

        //private void SetLength(double length)
        //{
        //    var newRect = line.Rect;
        //    newRect.Width = length;
        //    line.Rect = newRect;
        //}

        public void LengthenOrShorten(MyStoryboard MainStoryboard, double toLen, Action? before = null, Action? after = null)
        {
            //var anim = new RectAnimation();
            //anim.To = new Rect(0, 3, 70, 2);
            //anim.Duration = TimeSpan.FromMilliseconds(500);
            //line.BeginAnimation(RectangleGeometry.RectProperty, anim);

            var anim = new MyRectAnimationBase(to: new Rect(0, 3, toLen, 2), time: 500, before: before, after: after) { TargetControl = line, TargetParam = RectangleGeometry.RectProperty };

            string uniqueStr = Comm.GetUniqueString();

            MainStoryboard.RegisterTable.Add(new KeyValuePair<string, DependencyObject>("line" + uniqueStr, line));

            MainStoryboard.Children.Add(anim);
            // Storyboard.SetTarget(anim, line);
            Storyboard.SetTargetName(anim, "line" + uniqueStr);
            Storyboard.SetTargetProperty(anim, new PropertyPath(RectangleGeometry.RectProperty));


            // MainStoryboard.Begin_Ex(this);
        }
    }
}
