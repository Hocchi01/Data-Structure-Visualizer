using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataStructureVisualizer.ViewModels.Data
{
    partial class BinaryTreeItemViewModel : DataItemViewModelBase
    {
        private const double radius = 25;

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

        //public int? LeftChildIndex { get; set; } = null;
        //public int? RightChildIndex { get; set; } = null;
        public BindingList<int?> Children { get; set; } = new BindingList<int?> { null, null };

        public bool IsLeaf
        {
            get => Children[0] == null && Children[1] == null;
        }

        public double LeftMargin { get; set; }
        public int ParentIndex { get; set; }
        public double RightMargin { get; set; }

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
    }
}
