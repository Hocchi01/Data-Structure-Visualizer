﻿using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Extensions;
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

        public ObservableCollection<BinaryTreeItemViewModel> DataItems { get; set; }

        public BinaryTreeAlgorithmFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<BinaryTreeItemViewModel> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems.RevertElems())
        {
            DataItems = dataItems;
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
                anims.Add(elps1Anim);
                anims.Add(elps2Anim);
            }

            endStoryboard.AddAsyncAnimations(anims, before: () =>
            {
                foreach (var item in DataItems)
                {
                    item.State = DataItemState.Normal;
                }
            });

            endStoryboard.Begin_Ex(Canvas, false);
        }

        public void PreOrderTraverse(int? index)
        {
            if (index == null) return;
            int i = index ?? -1;

            Visit(i);

            GoToLeft(i, BinaryTreeTraverseMode.PreOrder);
            PreOrderTraverse(DataItems[i].Children[0]);

            GoToRight(i, BinaryTreeTraverseMode.PreOrder);
            PreOrderTraverse(DataItems[i].Children[1]);

            if (index != 0)
            {
                int parentIndex = DataItems[i].ParentIndex;
                if (DataItems[parentIndex].Children[0] == index)
                    ReturnFromLeft(parentIndex);
                else
                    ReturnFromRight(parentIndex);
            }
        }

        public void InOrderTraverse(int? index)
        {
            if (index == null) return;
            int i = index ?? -1;

            GoToLeft(i, BinaryTreeTraverseMode.InOrder);
            InOrderTraverse(DataItems[i].Children[0]);

            Visit(i);

            GoToRight(i, BinaryTreeTraverseMode.InOrder);
            InOrderTraverse(DataItems[i].Children[1]);

            if (index != 0)
            {
                int parentIndex = DataItems[i].ParentIndex;
                if (DataItems[parentIndex].Children[0] == index)
                    ReturnFromLeft(parentIndex);
                else
                    ReturnFromRight(parentIndex);
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

            Visit(i);

            if (index != 0)
            {
                int parentIndex = DataItems[i].ParentIndex;
                if (DataItems[parentIndex].Children[0] == index)
                    ReturnFromLeft(parentIndex);
                else
                    ReturnFromRight(parentIndex);
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

        private void Visit(int index)
        {
            CodeBlockPanel.AddAnimation(codeInfos["visit"], MainStoryboard.Offset);

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

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { elps1Anim, elps2Anim }, log: new LogViewModel("Visit Node", "visit(T);"), after: () =>
            {
                DataItems[index].State = DataItemState.Visited;
            });

            var borderAnim = new SimulatedThicknessAnimation(to: new Thickness(5), time: 50)
            {
                TargetControl = item.border,
                TargetParam = Border.BorderThicknessProperty,
                AutoReverse = true
            };

            MainStoryboard.AddSyncAnimation(borderAnim);
        }

        private void GoToLeft(int index, BinaryTreeTraverseMode mode)
        {
            CodeBlockPanel.AddAnimation(codeInfos["left"], MainStoryboard.Offset);
            CodeBlockPanel.AddAnimation(codeInfos["func"]);
            CodeBlockPanel.AddAnimation(codeInfos["if"]);

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

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { left1Anim, left2Anim }, log: new LogViewModel("Go To Left-Child", $"{mode}Traverse(T -> lchild);") );
        }

        private void GoToRight(int index, BinaryTreeTraverseMode mode)
        {
            CodeBlockPanel.AddAnimation(codeInfos["right"], MainStoryboard.Offset);
            CodeBlockPanel.AddAnimation(codeInfos["func"]);
            CodeBlockPanel.AddAnimation(codeInfos["if"]);

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

            MainStoryboard.AddAsyncAnimations(new List<AnimationTimeline> { right1Anim, right2Anim }, log: new LogViewModel("Go To Right-Child", $"{mode}Traverse(T -> rchild);"));
        }

        private void ReturnFromLeft(int index)
        {
            CodeBlockPanel.AddAnimation(codeInfos["left"], MainStoryboard.Offset);

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

        private void ReturnFromRight(int index)
        {
            CodeBlockPanel.AddAnimation(codeInfos["right"], MainStoryboard.Offset);

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
    }
}
