using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using DataStructureVisualizer.Common.Theme;
using DataStructureVisualizer.Common.Extensions;
using System.Diagnostics;
using DataStructureVisualizer.Common.Enums;
using System.Windows.Media.Animation;
using static System.Net.Mime.MediaTypeNames;
using DataStructureVisualizer.Views.Components;
using System.Net;
using DataStructureVisualizer.Views;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.Common.Structs;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    class LinkedListAlgorithmFactory : AlgorithmFactoryBase
    {
        private const string insertCodeBlock =
            "LNode* p = head;\\" +
            "int j = 0;\\" +
            "while (j++ < toIndex - 1)\\" +
            "   p = p -> next;\\" +
            "LNode* newNode = (LNode*)malloc(sizeof(LNode));\\" +
            "newNode -> data = addValue;\\" +
            "newNode -> next = p -> next;\\" +
            "p -> next = newNode;\\";

        private const string removeCodeBlock =
            "LNode* p = head;\\" +
            "int j = 0;\\" +
            "while (j++ < toIndex - 1)\\" +
            "   p = p -> next;\\" +
            "LNode* rmvNode = p -> next;\\" +
            "p -> next = p -> next -> next;\\" +
            "free(rmvNode);\\";

        public ObservableCollection<int> Values { get; set; }

        public LinkedListAlgorithmFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<LinkedListItemViewModel> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems.RevertElems())
        {
            //table = new List<int>();
            //for (int i = 1; i < last; i++) // 忽略链表中非数据结点
            //{
            //    table.Add(i);
            //}
        }

        public void InitPointer()
        {
            var head = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, 0);
            head.SetInBrush(ref head.borderBrush);
            head.borderBrush.EndPoint = new Point(1, 0);
        }

        public void PointerNext(int toIndex, Action? before = null, Action? after = null, LogViewModel? log = null)
        {
            ++toIndex;
            int preIndex = toIndex - 1;

            var preItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, preIndex);
            var toItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, toIndex);

            var preBorderOut = preItem.GetBorderOutAnimation();
            var preArrowIn = preItem.GetPointerInAnimation();
            var preArrowOut = preItem.GetPointerOutAnimation(true);
            var toBorderIn = toItem.GetBorderInAnimation(true);

            MainStoryboard.AddAsyncAnimations(new List<MyPointAnimationBase> { preBorderOut, preArrowIn }, before, log: log);
            MainStoryboard.AddAsyncAnimations(new List<MyPointAnimationBase> { preArrowOut, toBorderIn }, null, after);
            MainStoryboard.Delay(200);
        }

        public void FindElem(int elemIndex, CodeInfo w, CodeInfo next)
        {
            InitPointer();
            CodeBlockPanel.AddAnimation(w);
            for (int i = 0; i <= elemIndex; i++)
            {
                CodeBlockPanel.AddAnimation(next, MainStoryboard.Offset);
                PointerNext(i, log: new LogViewModel("Next elem", "p = p -> next;"));
                CodeBlockPanel.AddAnimation(w, MainStoryboard.Offset);
                MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
            }
        }

        private LinkedListItemUserControl GenerateVirtualElem(int elemIndex, DataItemViewModelBase viewModel, double initPoinerLen = 35)
        {
            return GenerateVirtualElem(new TranslateTransform() { X = elemIndex * (50 + 35) }, viewModel, initPoinerLen);
        }

        private LinkedListItemUserControl GenerateVirtualElem(TranslateTransform axis, DataItemViewModelBase viewModel, double initPoinerLen = 35)
        {
            var virtualElem = new LinkedListItemUserControl(initPoinerLen)
            {
                Margin = new Thickness(8),
                VerticalAlignment = VerticalAlignment.Center,
                RenderTransform = axis,
                Visibility = Visibility.Hidden,
                DataContext = viewModel
            };
            Canvas.Children.Add(virtualElem);

            return virtualElem;
        }

        public void RemoveElemInLinkedList(int elemIndex)
        {
            ++elemIndex;
            var preItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, elemIndex - 1);
            var rmvItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, elemIndex);

            var virtualPrePointer = GenerateVirtualElem(elemIndex - 1, new LinkedListItemViewModel()
            {
                EditType = DataItemEditType.None,
            });

            var virtualRmvItem = GenerateVirtualElem(elemIndex, rmvItem.DataContext as LinkedListItemViewModel);

            CodeBlockPanel.AddAnimation(codeInfos["op1"], MainStoryboard.Offset);
            MainStoryboard.InsertLog(new LogViewModel("Pre-node next to successor", "p -> next = p -> next -> next;"));
            var towardNextOfNextAnims = virtualPrePointer.next.TowardNextOfNext(() =>
            {
                preItem.next.Visibility = Visibility.Hidden;
                rmvItem.Visibility = Visibility.Hidden;
                virtualPrePointer.OnlyShowPointer();
                virtualRmvItem.Visibility = Visibility.Visible;
            });
            foreach (var anims in towardNextOfNextAnims)
            {
                MainStoryboard.AddAsyncAnimations(anims);
            }

            CodeBlockPanel.AddAnimation(codeInfos["op2"], MainStoryboard.Offset);
            RemoveElem(elemIndex, virtualRmvItem, true, null, () => { Canvas.Children.Remove(virtualRmvItem); }, new LogViewModel("Remove node", "free(rmvNode);"));

            var preLine2MoveUpAnim = virtualPrePointer.next.GetMoveLineSegmentAnimation(2, 50, Direction.Up);
            var preLine1ShortenAnim = virtualPrePointer.next.LengthenOrShortenLineSegment(1, 0);
            var preLine3ShortenAnim = virtualPrePointer.next.LengthenOrShortenLineSegment(3, 0);

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { preLine1ShortenAnim, preLine2MoveUpAnim, preLine3ShortenAnim },
            () =>
            {
                virtualPrePointer.next.FlipDirectionOfLine(3, Direction.Up);
            },
            () =>
            {
                preItem.next.Length = 120;
                preItem.next.Visibility = Visibility.Visible;
                rmvItem.Visibility = Visibility.Collapsed;
                Canvas.Children.Remove(virtualPrePointer);
            });

            MainStoryboard.AddAsyncAnimations(preItem.next.LengthenOrShorten(35));

            endOperations.Add(() =>
            {
                if (Canvas.Children.Contains(virtualPrePointer))
                    Canvas.Children.Remove(virtualPrePointer);
                if (Canvas.Children.Contains(virtualRmvItem))
                    Canvas.Children.Remove(virtualRmvItem);
            });
        }

        public void InsertElem(int toIndex, int addValue)
        {
            ++toIndex;
            var preItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, toIndex - 1);

            MainStoryboard.AddAsyncAnimations(preItem.next.LengthenOrShorten(35 * 2 + 50));

            var newItemViewModel = new LinkedListItemViewModel()
            {
                Value = addValue,
                EditType = DataItemEditType.New,
                Color = new SolidColorBrush(ThemeHelper.NewColor)
            };

            var newItem = GenerateVirtualElem(new TranslateTransform() { X = toIndex * (50 + 35), Y = 80 }, newItemViewModel, 25);

            table.Insert(toIndex, count + newValues.Count);
            newValues.Add(count + newValues.Count, addValue);

            CodeBlockPanel.AddAnimation(codeInfos["new"], MainStoryboard.Offset);
            NewElem(newItem, () => { newItem.Visibility = Visibility.Visible; }, log: new LogViewModel("New node", "LNode* newNode = (LNode*)malloc(sizeof(LNode)); newNode -> data = addValue;"));

            CodeBlockPanel.AddAnimation(codeInfos["op1"], MainStoryboard.Offset);
            MainStoryboard.InsertLog(new LogViewModel("New node next to successor", "newNode -> next = p -> next;"));
            var newToNextAnims = newItem.next.NewItemNextPointerTowardNextItem();
            for (int i = 0; i < newToNextAnims.Count; i++)
            {
                MainStoryboard.AddAsyncAnimations(newToNextAnims[i]);
            }

            CodeBlockPanel.AddAnimation(codeInfos["op2"], MainStoryboard.Offset);
            MainStoryboard.InsertLog(new LogViewModel("Pre-node next to new node", "p -> next = newNode;"));
            Point curEndPoint = preItem.next.endPoint;
            var preToNewAnims = preItem.next.PreItemNextPointerTowardNewItem(() => { preItem.next.FixCurrentWidth(curEndPoint); });
            for (int i = 0; i < preToNewAnims.Count; i++)
            {
                MainStoryboard.AddAsyncAnimations(preToNewAnims[i]);
            }


            var preArrowMoveUpAnim = preItem.next.GetMoveArrowAnimation(80, Direction.Up);
            var preLineMoveUpAnim = preItem.next.GetMoveLineSegmentAnimation(2, 80, Direction.Up);
            var newItemMoveUpAnim = new SimulatedDoubleAnimation(by: -80f, time: 500) { TargetControl = newItem, TargetParam = AnimationHelper.VerticallyMoveParam };
            var newLineMoveDownAnim = newItem.next.GetMoveLineSegmentAnimation(2, 80, Direction.Down);
            var newArrowMoveDownAnim = newItem.next.GetMoveArrowAnimation(80, Direction.Down);
            var newLineShortenAnim = newItem.next.LengthenOrShortenLineSegment(1, 0);
            var preLineShortenAnim = preItem.next.LengthenOrShortenLineSegment(1, 0);
            var returnNewAnims = new List<AnimationTimeline>
            {
                preArrowMoveUpAnim,
                preLineMoveUpAnim,
                newItemMoveUpAnim,
                newLineShortenAnim,
                preLineShortenAnim,
                newLineMoveDownAnim,
                newArrowMoveDownAnim
            };
            MainStoryboard.AddAsyncAnimations(returnNewAnims);

            endOperations.Add(() =>
            {
                Canvas.Children.Remove(newItem);
            });
        }

        public void InsertElemAlgorithm(int toIndex, int addValue)
        {
            CodeBlockPanel.SetCodeBlock(insertCodeBlock);
            codeInfos.Add("while", new CodeInfo(2));
            codeInfos.Add("next", new CodeInfo(3));
            codeInfos.Add("new", new CodeInfo(4, 2));
            codeInfos.Add("op1", new CodeInfo(6));
            codeInfos.Add("op2", new CodeInfo(7));

            FindElem(toIndex - 1, codeInfos["while"], codeInfos["next"]);
            InsertElem(toIndex, addValue);

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        public void RemoveElemAlgorithm(int toIndex)
        {
            CodeBlockPanel.SetCodeBlock(removeCodeBlock);
            codeInfos.Add("while", new CodeInfo(2));
            codeInfos.Add("next", new CodeInfo(3));
            codeInfos.Add("op1", new CodeInfo(5));
            codeInfos.Add("op2", new CodeInfo(6));

            FindElem(toIndex - 1, codeInfos["while"], codeInfos["next"]);
            RemoveElemInLinkedList(toIndex);

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }
    }
}
