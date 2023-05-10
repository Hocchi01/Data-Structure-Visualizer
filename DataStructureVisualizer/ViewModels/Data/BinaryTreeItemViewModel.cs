using CommunityToolkit.Mvvm.ComponentModel;
using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataStructureVisualizer.ViewModels.Data
{
    [Serializable]
    partial class BinaryTreeItemViewModel : DataItemViewModelBase
    {
        private const double layerHeight = 125;
        private const double radius = 25;
        private static double baseDz = Math.Sqrt(Math.Pow(radius * 2, 2) + Math.Pow(layerHeight, 2));
        public double BaseOffsetX = radius * radius * 2 / baseDz;
        public double BaseOffsetY = radius * layerHeight / baseDz;

        [ObservableProperty]
        private double axisX = 0;

        [ObservableProperty]
        private double axisY = 0;

        [ObservableProperty]
        private double leftLineX1 = radius;

        [ObservableProperty]
        private double leftLineX2 = radius;

        [ObservableProperty]
        private double leftLineY1 = radius;

        [ObservableProperty]
        private double leftLineY2 = radius;

        [ObservableProperty]
        private double rightLineX1 = radius;

        [ObservableProperty]
        private double rightLineX2 = radius;

        [ObservableProperty]
        private double rightLineY1 = radius;

        [ObservableProperty]
        private double rightLineY2 = radius;

        [ObservableProperty]
        private DataItemState foreState = DataItemState.Visited;

        //public int? LeftChildIndex { get; set; } = null;
        //public int? RightChildIndex { get; set; } = null;
        public BindingList<int?> Children { get; set; } = new BindingList<int?> { null, null };

        public void TraverseChildrenWithAction(Action<int> action)
        {
            foreach (var item in Children)
            {
                if (item == null) continue;
                int i = item ?? -1;
                action(i);
            }
        }

        public bool IsLeaf
        {
            get => Children[0] == null && Children[1] == null;
        }

        public double LeftMargin { get; set; }
        public int ParentIndex { get; set; } // 根结点的父索引标记为 -1
        public double RightMargin { get; set; }

        public int GetChildrenCount()
        {
            int cnt = 0;
            if (Children[0] != null)
            {
                cnt++;
            }
            if (Children[1] != null)
            {
                cnt++;
            }
            return cnt;
        }

        public void OffsetRightLine()
        {
            double dx = RightLineX2 - 25;
            double dy = RightLineY2 - 25;
            double dz = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            double offsetX = radius * dx / dz;
            double offsetY = radius * dy / dz;

            RightLineX1 += offsetX;
            RightLineY1 += offsetY;
            RightLineX2 -= offsetX;
            RightLineY2 -= offsetY;
        }

        public void OffsetLeftLine()
        {
            double dx = LeftLineX2 - 25;
            double dy = LeftLineY2 - 25;
            double dz = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            double offsetX = radius * dx / dz;
            double offsetY = radius * dy / dz;

            LeftLineX1 += offsetX;
            LeftLineY1 += offsetY;
            LeftLineX2 -= offsetX;
            LeftLineY2 -= offsetY;
        }

        public void OffsetEmptyLeftLine()
        {
            LeftLineX1 -= BaseOffsetX;
            LeftLineY1 += BaseOffsetY;
            LeftLineX2 -= BaseOffsetX;
            LeftLineY2 += BaseOffsetY;
        }

        public void OffsetEmptyRightLine()
        {
            RightLineX1 += BaseOffsetX;
            RightLineY1 += BaseOffsetY;
            RightLineX2 += BaseOffsetX;
            RightLineY2 += BaseOffsetY;
        }
    }
}
