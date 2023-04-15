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
        private const string GeometryHorizontallyMoveParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.X)";
        private const string GeometryVerticallyMoveParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.Y)";

        private const double arrowOutLength = 8;
        private const double arrowLength = 9;
        private const double arrowHalfWidth = 4;

        private List<Point> directions = new List<Point> {
            new Point(-arrowLength, 0), // left
            new Point(0, -arrowLength), // up
            new Point(arrowLength, 0), // right
            new Point(0, arrowLength), // down
        };

        public List<RectangleGeometry> Lines { get; set; }

        public Point endPoint = new Point(8, 4);
        //public Point EndPoint
        //{
        //    get { return endPoint; }
        //    private set { endPoint = value; }
        //}
        private double curLength = arrowOutLength;

        private double length = arrowOutLength;

        // 当前只有一条线段才可修改
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
                curLength = length;
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
            Lines = new List<RectangleGeometry> { line };
        }

        public List<AnimationTimeline> LengthenOrShorten(double toLen, Action? before = null, Action? after = null)
        {
            double toLineLen = toLen - arrowOutLength;

            var lineAnimation = new SimulatedRectAnimation(to: new Rect(0, 3, toLineLen, 2), time: 500, before: before, after: after)
            {
                TargetControl = line,
                TargetParam = RectangleGeometry.RectProperty,
                TargetName = "line_" + Comm.GetUniqueString()
            };

            double by = toLen - curLength;

            var arrowAnimation = new SimulatedDoubleAnimation(to: toLineLen, time: 500)
            {
                TargetControl = arrow,
                TargetParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.X)",
                TargetName = "arrow_" + Comm.GetUniqueString()
            };

            endPoint.X += by;
            curLength = toLen;
            return new List<AnimationTimeline> { lineAnimation, arrowAnimation };
        }

        public SimulatedRectAnimation LengthenOrShortenLineSegment(int lineIndex, double toLen)
        {
            return new SimulatedRectAnimation(to: new Rect(0, 3, toLen, 2), time: 500)
            {
                TargetControl = Lines[lineIndex],
                TargetParam = RectangleGeometry.RectProperty,
                TargetName = "line_" + Comm.GetUniqueString()
            };
        }

        public SimulatedDoubleAnimation GetMoveLineSegmentAnimation(int lineIndex, float by, Direction direction)
        {
            if (!direction.IsPositive())
                by = -by;
            var param = direction.IsHorizontal() ? GeometryHorizontallyMoveParam : GeometryVerticallyMoveParam;

            return new SimulatedDoubleAnimation(by: by, time: 500) { TargetControl = Lines[lineIndex], TargetParam = param, TargetName = "line_" + Comm.GetUniqueString() };
        }

        public SimulatedDoubleAnimation GetMoveArrowAnimation(float by, Direction direction)
        {
            if (!direction.IsPositive())
                by = -by;
            var param = direction.IsHorizontal() ? GeometryHorizontallyMoveParam : GeometryVerticallyMoveParam;

            return new SimulatedDoubleAnimation(by: by, time: 500) { TargetControl = arrow, TargetParam = param, TargetName = "arrow_" + Comm.GetUniqueString() };
        }

        public List<List<AnimationTimeline>> NewItemNextPointerTowardNextItem()
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

        public List<List<AnimationTimeline>> PreItemNextPointerTowardNewItem(Action? before = null, Action? after = null)
        {
            var arrowShortenAnims = LengthenOrShorten(25, before, null);
            var arrowTowardDownAnim = GetRotatoArrowAnimation(Direction.Down);
            var arrowMoveDownAnims = GetMoveArrowAnimations(Direction.Down, 80);
            var arrowTowardRightAnim = GetRotatoArrowAnimation(Direction.Right);
            var arrowMoveRightAnims = GetMoveArrowAnimations(Direction.Right, 10, null, after);

            return new List<List<AnimationTimeline>>
            {
                arrowShortenAnims,
                new List<AnimationTimeline> { arrowTowardDownAnim },
                arrowMoveDownAnims,
                new List<AnimationTimeline> { arrowTowardRightAnim },
                arrowMoveRightAnims
            };
        }

        public List<List<AnimationTimeline>> TowardNextOfNext(Action? before = null, Action? after = null)
        {
            var arrowShortenAnims = LengthenOrShorten(25, before, null);
            var arrowTowardDownAnim = GetRotatoArrowAnimation(Direction.Down);
            var arrowMoveDownAnims = GetMoveArrowAnimations(Direction.Down, 50);
            var arrowTowardRightAnim = GetRotatoArrowAnimation(Direction.Right);
            var arrowMoveRightAnims = GetMoveArrowAnimations(Direction.Right, 18 + 50 + 17);
            var arrowTowardUpAnim = GetRotatoArrowAnimation(Direction.Up);
            var arrowMoveUpAnims = GetMoveArrowAnimations(Direction.Up, 50);
            var arrowTowardRight2Anim = GetRotatoArrowAnimation(Direction.Right);
            var arrowMoveRight2Anims = GetMoveArrowAnimations(Direction.Right, 10, null, after);

            return new List<List<AnimationTimeline>>
            {
                arrowShortenAnims,
                new List<AnimationTimeline> { arrowTowardDownAnim },
                arrowMoveDownAnims,
                new List<AnimationTimeline> { arrowTowardRightAnim },
                arrowMoveRightAnims,
                new List<AnimationTimeline> { arrowTowardUpAnim },
                arrowMoveUpAnims,
                new List<AnimationTimeline> { arrowTowardRight2Anim },
                arrowMoveRight2Anims
            };
        }

        public void FixCurrentWidth(Point curEndPoint)
        {
            geometryGroup.Children.Add(new RectangleGeometry(new Rect(curEndPoint.X, curEndPoint.Y, 0, 0)));
        }

        private void CorrectLineTranslate(ref TranslateTransform tt, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    tt.X = endPoint.X + 10;
                    tt.Y = endPoint.Y - 4;
                    break;
                case Direction.Up:
                    tt.X = endPoint.X + 1;
                    tt.Y = endPoint.Y + 5;
                    break;
                case Direction.Right:
                    tt.X = endPoint.X - 8;
                    tt.Y = endPoint.Y - 4;
                    break;
                case Direction.Down:
                    tt.X = endPoint.X + 1;
                    tt.Y = endPoint.Y - 13;
                    break;
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

        public void FlipDirectionOfLine(int index, Direction originalDirection)
        {
            var line = Lines[index];
            double dist = line.Rect.Width + 2;

            ((line.Transform as TransformGroup).Children[1] as TranslateTransform).Y += originalDirection == Direction.Up ? -dist : dist;

            ((line.Transform as TransformGroup).Children[0] as RotateTransform).Angle += 180;
        }

        private List<AnimationTimeline> GetMoveArrowAnimations(Direction direction, double toLength, Action? before = null, Action? after = null)
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
            Lines.Add(line);

            var lineAnimation = new SimulatedRectAnimation(to: new Rect(0, 3, toLength, 2), time: 500, before: before, after: after)
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
