using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Extensions;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    class BinaryTreeAlgorithmFactory : AlgorithmFactoryBase
    {
        private const string inOrderTraverseCodeBlock =
            "void InOrderTraverse(BiTree T)\\" +
            "{\\" +
            "   if (T != NULL)\\" +
            "   {\\" +
            "       InOrderTraverse(T -> lchild);\\" +
            "       visit(T);\\" +
            "       InOrderTraverse(T -> rchild);\\" +
            "   }\\" +
            "}\\";

        private const string postOrderTraverseCodeBlock =
            "void PostOrderTraverse(BiTree T)\\" +
            "{\\" +
            "   if (T != NULL)\\" +
            "   {\\" +
            "       PostOrderTraverse(T -> lchild);\\" +
            "       PostOrderTraverse(T -> rchild);\\" +
            "       visit(T);\\" +
            "   }\\" +
            "}\\";

        private const string preOrderTraverseCodeBlock =
                            "void PreOrderTraverse(BiTree T)\\" +
            "{\\" +
            "   if (T != NULL)\\" +
            "   {\\" +
            "       visit(T);\\" +
            "       PreOrderTraverse(T -> lchild);\\" +
            "       PreOrderTraverse(T -> rchild);\\" +
            "   }\\" +
            "}\\";
        public BinaryTreeAlgorithmFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<BinaryTreeItemViewModel> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems.RevertElems())
        {
            DataItems = dataItems;
            //NewDataItems = new ObservableCollection<BinaryTreeItemViewModel>();
            //foreach (var item in DataItems)
            //{
            //    NewDataItems.Add(Comm.DeepCopy2(item));
            //}

            NewDataItems = Comm.DeepCopy(DataItems);
        }

        public ObservableCollection<BinaryTreeItemViewModel> DataItems { get; set; }
        public ObservableCollection<BinaryTreeItemViewModel> NewDataItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 调用前应确保处理好即将删除的结点的子结点 或 确保它是叶子结点
        /// </remarks>
        /// <param name="index"></param>
        /// <param name="hasParent"></param>
        protected void RemoveItemInNewDataItems(int index, bool hasParent = true)
        {
            int last = NewDataItems.Count - 1;

            if (hasParent)
            {
                NewDataItems[NewDataItems[index].ParentIndex].Children[GetDirection(index)] = null;
            }

            NewDataItems[index] = NewDataItems[last];

            var lastParent = NewDataItems[NewDataItems[last].ParentIndex];
            lastParent.Children[GetDirection(last)] = index;
            NewDataItems[last].TraverseChildrenWithAction((i) => { NewDataItems[i].ParentIndex = index; });

            NewDataItems.RemoveAt(last);
        }

        public void InOrderTraverse(int? index)
        {
            if (index == null) return;
            int i = index ?? -1;

            GoToLeft(i, BinaryTreeTraverseMode.InOrder);
            InOrderTraverse(DataItems[i].Children[0]);

            Visit_Traverse(i);

            GoToRight(i, BinaryTreeTraverseMode.InOrder);
            InOrderTraverse(DataItems[i].Children[1]);

            if (index != 0)
            {
                int parentIndex = DataItems[i].ParentIndex;
                if (DataItems[parentIndex].Children[0] == index)
                    ReturnFromLeft_Traverse(parentIndex);
                else
                    ReturnFromRight_Traverse(parentIndex);
            }
        }

        public void PostOrderTraverse(int? index)
        {
            if (index == null) return;
            int i = index ?? -1;

            GoToLeft(i, BinaryTreeTraverseMode.PostOrder);
            PostOrderTraverse(DataItems[i].Children[0]);

            GoToRight(i, BinaryTreeTraverseMode.PostOrder);
            PostOrderTraverse(DataItems[i].Children[1]);

            Visit_Traverse(i);

            if (index != 0)
            {
                int parentIndex = DataItems[i].ParentIndex;
                if (DataItems[parentIndex].Children[0] == index)
                    ReturnFromLeft_Traverse(parentIndex);
                else
                    ReturnFromRight_Traverse(parentIndex);
            }
        }

        public void PreOrderTraverse(int? index)
        {
            if (index == null) return;
            int i = index ?? -1;

            Visit_Traverse(i);

            GoToLeft(i, BinaryTreeTraverseMode.PreOrder);
            PreOrderTraverse(DataItems[i].Children[0]);

            GoToRight(i, BinaryTreeTraverseMode.PreOrder);
            PreOrderTraverse(DataItems[i].Children[1]);

            if (index != 0)
            {
                int parentIndex = DataItems[i].ParentIndex;
                if (DataItems[parentIndex].Children[0] == index)
                    ReturnFromLeft_Traverse(parentIndex);
                else
                    ReturnFromRight_Traverse(parentIndex);
            }
        }

        public void Traverse(BinaryTreeTraverseMode mode)
        {
            switch (mode)
            {
                case BinaryTreeTraverseMode.PreOrder:

                    CodeBlockPanel.SetCodeBlock(preOrderTraverseCodeBlock);

                    codeInfos.Add("func", new CodeInfo(0));
                    codeInfos.Add("if", new CodeInfo(2));
                    codeInfos.Add("visit", new CodeInfo(4));
                    codeInfos.Add("left", new CodeInfo(5));
                    codeInfos.Add("right", new CodeInfo(6));

                    PreOrderTraverse(0);
                    break;

                case BinaryTreeTraverseMode.InOrder:

                    CodeBlockPanel.SetCodeBlock(inOrderTraverseCodeBlock);

                    codeInfos.Add("func", new CodeInfo(0));
                    codeInfos.Add("if", new CodeInfo(2));
                    codeInfos.Add("visit", new CodeInfo(5));
                    codeInfos.Add("left", new CodeInfo(4));
                    codeInfos.Add("right", new CodeInfo(6));

                    InOrderTraverse(0);
                    break;
                case BinaryTreeTraverseMode.PostOrder:

                    CodeBlockPanel.SetCodeBlock(postOrderTraverseCodeBlock);

                    codeInfos.Add("func", new CodeInfo(0));
                    codeInfos.Add("if", new CodeInfo(2));
                    codeInfos.Add("visit", new CodeInfo(6));
                    codeInfos.Add("left", new CodeInfo(4));
                    codeInfos.Add("right", new CodeInfo(5));

                    PostOrderTraverse(0);
                    break;
            }
            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        protected void GoToLeft(int index, LogViewModel? log = null)
        {
            if (DataItems[index].Children[0] == null) return;

            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            var left1Anim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = item.left1,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };
            var left2Anim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = item.left2,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { left1Anim, left2Anim }, log: log);
        }

        protected void GoToRight(int index, LogViewModel? log = null)
        {
            if (DataItems[index].Children[1] == null) return;

            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            var right1Anim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = item.right1,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };
            var right2Anim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = item.right2,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { right1Anim, right2Anim }, log: log);
        }

        protected void ReturnFromLeft(int index, LogViewModel? log = null)
        {
            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            var left1Anim = new SimulatedDoubleAnimation(from: 1, to: 0, time: 500)
            {
                TargetControl = item.left1,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };
            var left2Anim = new SimulatedDoubleAnimation(from: 1, to: 0, time: 500)
            {
                TargetControl = item.left2,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { left1Anim, left2Anim });
        }

        protected void ReturnFromRight(int index, LogViewModel? log = null)
        {
            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            var right1Anim = new SimulatedDoubleAnimation(from: 1, to: 0, time: 500)
            {
                TargetControl = item.right1,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };
            var right2Anim = new SimulatedDoubleAnimation(from: 1, to: 0, time: 500)
            {
                TargetControl = item.right2,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { right1Anim, right2Anim });
        }

        protected void InsertToLeft(int index, int value, LogViewModel? log = null)
        {
            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            DataItems[index].OffsetEmptyLeftLine();
            var anims = item.GetInsertToLeftAnimations(value);
            MainStoryboard.AddAsyncAnimations(anims[0]);
            MainStoryboard.AddAsyncAnimations(anims[1]);

            NewDataItems.Add(new BinaryTreeItemViewModel() { Value = value, ParentIndex = index });
            NewDataItems[index].Children[0] = NewDataItems.Count - 1;
        }

        protected void InsertToRight(int index, int value, LogViewModel? log = null)
        {
            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            DataItems[index].OffsetEmptyRightLine();
            var anims = item.GetInsertToRightAnimations(value);
            MainStoryboard.AddAsyncAnimations(anims[0]);
            MainStoryboard.AddAsyncAnimations(anims[1]);

            NewDataItems.Add(new BinaryTreeItemViewModel() { Value = value, ParentIndex = index });
            NewDataItems[index].Children[1] = NewDataItems.Count - 1;
        }

        protected bool IsLeftChild(int index)
        {
            int parentIndex = NewDataItems[index].ParentIndex;
            return NewDataItems[parentIndex].Children[0] == index;
        }

        protected int GetDirection(int index)
        {
            int parentIndex = NewDataItems[index].ParentIndex;
            return NewDataItems[parentIndex].Children[0] == index ? 0 : 1;
        }

        protected void RemoveLeafElem(int index, LogViewModel? log = null)
        {
            var rmvItem = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            int parentIndex = DataItems[index].ParentIndex;
            var parentItem = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, parentIndex);

            var lineAnims = DataItems[parentIndex].Children[0] == index ? parentItem.GetRemoveLeftLineAnimations(DataItems[parentIndex].LeftLineX1, DataItems[parentIndex].LeftLineY1) : parentItem.GetRemoveRightLineAnimations(DataItems[parentIndex].RightLineX1, DataItems[parentIndex].RightLineY1);

            var nodeAnims = rmvItem.GetRemoveNodeAnimations();

            var anims = lineAnims;
            anims.AddRange(nodeAnims);

            MainStoryboard.AddAsyncAnimations(anims);

            RemoveItemInNewDataItems(index);
        }

        protected void TraverseSubtreeWithAction(int? index, Action<int> action)
        {
            if (index == null) return;
            int i = index ?? -1;
            action(i);
            TraverseSubtreeWithAction(DataItems[i].Children[0], action);
            TraverseSubtreeWithAction(DataItems[i].Children[1], action);
        }

        protected List<AnimationTimeline> GetMoveSubtreeAnimation(int rootIndex, double byX, double byY)
        {
            var anims = new List<AnimationTimeline>();
            TraverseSubtreeWithAction(rootIndex, (index) =>
            {
                var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);
                anims.AddRange(item.GetMoveElemAnimations(byX, byY));
            });
            return anims;
        }

        protected void RemoveSingleChildElem(int index, LogViewModel? log = null)
        {
            var rmvItem = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            var rmvAnims = new List<AnimationTimeline>();
            var moveAnims = new List<AnimationTimeline>();

            rmvAnims.AddRange(rmvItem.GetRemoveNodeAnimations());
            int direct = 0;
            if (DataItems[index].Children[0] != null)
            {
                rmvAnims.AddRange(rmvItem.GetRemoveLeftLineAnimations(DataItems[index].LeftLineX1, DataItems[index].LeftLineY1));
            }
            else
            {
                direct = 1;
                rmvAnims.AddRange(rmvItem.GetRemoveRightLineAnimations(DataItems[index].RightLineX1, DataItems[index].RightLineY1));
            }

            double dX = DataItems[index].AxisX - DataItems[DataItems[index].Children[direct] ?? -1].AxisX;
            double dY = DataItems[index].AxisY - DataItems[DataItems[index].Children[direct] ?? -1].AxisY;

            moveAnims.AddRange(GetMoveSubtreeAnimation(DataItems[index].Children[direct] ?? -1, dX, dY));

            var anims = rmvAnims;
            anims.AddRange(moveAnims);
            MainStoryboard.AddAsyncAnimations(anims);

            int parentDirect = IsLeftChild(index) ? 0 : 1;
            NewDataItems[DataItems[index].ParentIndex].Children[parentDirect] = DataItems[index].Children[direct];
            NewDataItems[DataItems[index].Children[direct] ?? -1].ParentIndex = DataItems[index].ParentIndex;
            RemoveItemInNewDataItems(index, false);
        }

        protected override void UpdateValues()
        {
            var endStoryboard = new MyStoryboard(false);

            var anims = new List<AnimationTimeline>();
            for (int i = 0; i < DataItems.Count; i++)
            {
                var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, i);
                var elps1Anim = new SimulatedDoubleAnimation(to: 0, time: 1000)
                {
                    TargetControl = item.elps1,
                    TargetParam = GradientStop.OffsetProperty,
                    TargetName = "gs_" + Comm.GetUniqueString()
                };
                var elps2Anim = new SimulatedDoubleAnimation(to: 0, time: 1000)
                {
                    TargetControl = item.elps2,
                    TargetParam = GradientStop.OffsetProperty,
                    TargetName = "gs_" + Comm.GetUniqueString()
                };
                var left1Anim = new SimulatedDoubleAnimation(to: 0, time: 1000)
                {
                    TargetControl = item.left1,
                    TargetParam = GradientStop.OffsetProperty,
                    TargetName = "gs_" + Comm.GetUniqueString()
                };
                var left2Anim = new SimulatedDoubleAnimation(to: 0, time: 1000)
                {
                    TargetControl = item.left2,
                    TargetParam = GradientStop.OffsetProperty,
                    TargetName = "gs_" + Comm.GetUniqueString()
                };
                var right1Anim = new SimulatedDoubleAnimation(to: 0, time: 1000)
                {
                    TargetControl = item.right1,
                    TargetParam = GradientStop.OffsetProperty,
                    TargetName = "gs_" + Comm.GetUniqueString()
                };
                var right2Anim = new SimulatedDoubleAnimation(to: 0, time: 1000)
                {
                    TargetControl = item.right2,
                    TargetParam = GradientStop.OffsetProperty,
                    TargetName = "gs_" + Comm.GetUniqueString()
                };
                anims.Add(elps1Anim);
                anims.Add(elps2Anim);
                anims.Add(left1Anim);
                anims.Add(left2Anim);
                anims.Add(right1Anim);
                anims.Add(right2Anim);
            }

            endStoryboard.AddAsyncAnimations(anims, before: () =>
            {
                foreach (var item in DataItems)
                {
                    item.State = DataItemState.Normal;
                }
            });

            endStoryboard.Completed += (_, _) =>
            {
                WeakReferenceMessenger.Default.Send(new GenerateDataMessage() { DataItems = NewDataItems.RevertElems() });
            };

            endStoryboard.Begin_Ex(Canvas, false);
        }

        protected void Visit(int index, Action? before = null, Action? after = null, LogViewModel? log = null)
        {
            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, index);

            var elps1Anim = new SimulatedDoubleAnimation(to: 1, time: 250)
            {
                TargetControl = item.elps1,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };
            var elps2Anim = new SimulatedDoubleAnimation(to: 1, time: 250)
            {
                TargetControl = item.elps2,
                TargetParam = GradientStop.OffsetProperty,
                TargetName = "gs_" + Comm.GetUniqueString()
            };

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { elps1Anim, elps2Anim }, before, after, log);

            var borderAnim = new SimulatedThicknessAnimation(to: new Thickness(5), time: 50)
            {
                TargetControl = item.border,
                TargetParam = Border.BorderThicknessProperty,
                AutoReverse = true
            };

            MainStoryboard.AddSyncAnimation(borderAnim);
        }

        protected int FindInOrderSuccessor(int index)
        {
            GoToRight(index);
            int i = DataItems[index].Children[1] ?? -1;
            
            while (DataItems[i].Children[0] != null)
            {
                Visit_Search(i);
                GoToLeft(i);
                i = DataItems[i].Children[0] ?? -1;
            }

            Visit_Search(i, true);
            return i;
        }

        protected void CopyValue(int elemIndex, int toIndex)
        {
            var rmvAnim = GetRemoveElemAniamtion(toIndex);

            var item = Comm.GetItemFromItemsControlByIndex<BinaryTreeItemUserControl>(Container, elemIndex);
            double dX = DataItems[toIndex].AxisX - DataItems[elemIndex].AxisX;
            double dY = DataItems[toIndex].AxisY - DataItems[elemIndex].AxisY;
            var copyAnims = item.GetMoveCopyValueItemAnimations(dX, dY);

            var anims = copyAnims;
            anims.Add(rmvAnim);
            MainStoryboard.AddAsyncAnimations(anims);

            NewDataItems[toIndex].Value = NewDataItems[elemIndex].Value;
        }

        protected void Visit_Search(int index, bool isMatched = false)
        {
            Visit(index, () => { DataItems[index].ForeState = isMatched ? DataItemState.Matched : DataItemState.Visited; }, () => { DataItems[index].State = isMatched ? DataItemState.Matched : DataItemState.Visited; });
        }

        private void GoToLeft(int index, BinaryTreeTraverseMode mode)
        {
            CodeBlockPanel.AddAnimation(codeInfos["left"], MainStoryboard.Offset);
            CodeBlockPanel.AddAnimation(codeInfos["func"]);
            CodeBlockPanel.AddAnimation(codeInfos["if"]);

            GoToLeft(index, new LogViewModel("Go To Left-Child", $"{mode}Traverse(T -> lchild);"));
        }

        private void GoToRight(int index, BinaryTreeTraverseMode mode)
        {
            CodeBlockPanel.AddAnimation(codeInfos["right"], MainStoryboard.Offset);
            CodeBlockPanel.AddAnimation(codeInfos["func"]);
            CodeBlockPanel.AddAnimation(codeInfos["if"]);

            GoToRight(index, new LogViewModel("Go To Right-Child", $"{mode}Traverse(T -> rchild);"));
        }

        private void ReturnFromLeft_Traverse(int index)
        {
            CodeBlockPanel.AddAnimation(codeInfos["left"], MainStoryboard.Offset);

            ReturnFromLeft(index);
        }

        private void ReturnFromRight_Traverse(int index)
        {
            CodeBlockPanel.AddAnimation(codeInfos["right"], MainStoryboard.Offset);

            ReturnFromRight(index);
        }

        private void Visit_Traverse(int index)
        {
            CodeBlockPanel.AddAnimation(codeInfos["visit"], MainStoryboard.Offset);

            Visit(index, null, () => { DataItems[index].State = DataItemState.Visited; }, new LogViewModel("Visit Node", "visit(T);"));
        }

    }
}
