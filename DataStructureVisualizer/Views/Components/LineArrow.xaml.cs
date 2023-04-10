using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Extensions;
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
        private const double arrowOutLength = 8;
        private const double arrowLength = 9;
        private const double arrowHalfWidth = 4;

        private List<Point> directions = new List<Point> {
            new Point(-arrowLength, 0), // left
            new Point(0, -arrowLength), // up
            new Point(arrowLength, 0), // right
            new Point(0, arrowLength), // down
        };

        private Point endPoint = new Point(arrowOutLength, 4);

        private double length = arrowOutLength;

        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                double lineLength = length - arrowOutLength;
                line.Rect = new Rect(0, 3, lineLength, 2);
                arrowTranslate.X = lineLength;
                endPoint.X = length;
            }
        }

        private Direction arrowDirection = Direction.Right;

        public Direction ArrowDirection
        {
            get { return arrowDirection; }
            set 
            {
                endPoint += directions[(int)value] - directions[(int)arrowDirection];
                arrowDirection = value;
            }
        }

        public LineArrow()
        {
            InitializeComponent();
        }

        public List<AnimationTimeline> LengthenOrShorten(double toLen, Action? before = null, Action? after = null)
        {
            double toLineLen = toLen - arrowOutLength;

            var lineAnimation = new SimulatedRectAnimation(to: new Rect(0, 3, toLineLen, 2), time: 500)
            {
                TargetControl = line,
                TargetParam = RectangleGeometry.RectProperty,
                TargetName = "line_" + Comm.GetUniqueString()
            };

            double by = toLen - Length;

            var arrowAnimation = new SimulatedDoubleAnimation(to: toLineLen, time: 500, null, () => { Length = toLen; })
            {
                TargetControl = arrow,
                TargetParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.X)",
                TargetName = "arrow_" + Comm.GetUniqueString()
            };

            return new List<AnimationTimeline> { lineAnimation, arrowAnimation };
        }

        public List<List<AnimationTimeline>> NewItemNextPointerTowardNext()
        {
            var arrowTowardUpAnim = GetRotatoArrowAnimation(Direction.Up);
            var arrowMoveUpAnims = GetMoveArrowAnimations(Direction.Up, 80);
            var arrowTowardRightAnim = GetRotatoArrowAnimation(Direction.Right);
            var arrowMoveRightAnims = GetMoveArrowAnimations(Direction.Right, 10);

            return new List<List<AnimationTimeline>>
            {
                new List<AnimationTimeline> { arrowTowardUpAnim },
                arrowMoveUpAnims,
                new List<AnimationTimeline> { arrowTowardRightAnim },
                arrowMoveRightAnims
            };
        }

        private Rect GetToRect(Rect rect, double len)
        {
            var newRect = rect;
            newRect.Width = len;
            return newRect;
        }

        private void CorrectLineTranslate(ref TranslateTransform tt, Direction direction)
        {
            if (direction.IsHorizontal())
            {
                tt.X = endPoint.X - arrowOutLength;
                tt.Y = endPoint.Y - 4;
            }
            else
            {
                tt.X = endPoint.X + 1;
                tt.Y = endPoint.Y + 5;
            }
        }

        private SimulatedDoubleAnimation GetRotatoArrowAnimation(Direction direction)
        {
            ArrowDirection = direction;

            double angle = ((int)direction - (int)Direction.Right) * 90;
            return new SimulatedDoubleAnimation(to: angle, time: 250)
            {
                TargetControl = arrow,
                TargetParam = "(Geometry.Transform).(TransformGroup.Children)[0].(RotateTransform.Angle)",
                TargetName = "arrow_" + Comm.GetUniqueString()
            };
        }

        private List<AnimationTimeline> GetMoveArrowAnimations(Direction direction, double toLength)
        {
            var line = new RectangleGeometry(new Rect(0, 3, 0, 2));
            var tg = new TransformGroup();
            var tt = new TranslateTransform();

            tg.Children.Add(new RotateTransform() 
            { 
                CenterX = -1,
                CenterY = 4,
                Angle = ((int)direction - (int)Direction.Right) * 90
            });
            
            CorrectLineTranslate(ref tt, direction);
            tg.Children.Add(tt);
            line.Transform = tg;
            geometryGroup.Children.Add(line);

            var lineAnimation = new SimulatedRectAnimation(to: new Rect(0, 3, toLength, 2), time: 500)
            {
                TargetControl = line,
                TargetParam = RectangleGeometry.RectProperty,
                TargetName = "line_" + Comm.GetUniqueString()
            };

            var by = direction.IsPositive() ? toLength : -toLength;
            var axis = direction.IsHorizontal() ? "X" : "Y";
            var arrowAnimation = new SimulatedDoubleAnimation(by: (float)by, time: 500)
            {
                TargetControl = arrow,
                TargetParam = $"(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.{axis})",
                TargetName = "arrow_" + Comm.GetUniqueString(), // 为同一个 arrow 注册了不同的 Name
            };

            switch (direction)
            {
                case Direction.Left: endPoint.X -= toLength;
                    break;
                case Direction.Up: endPoint.Y -= toLength;
                    break;
                case Direction.Right: endPoint.X += toLength;
                    break;
                case Direction.Down: endPoint.Y += toLength;
                    break;
            }

            return new List<AnimationTimeline> { lineAnimation, arrowAnimation };
        }
    }
}
