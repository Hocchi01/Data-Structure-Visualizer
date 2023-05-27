using DataStructureVisualizer.Common.AlgorithmFactories;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataStructureVisualizer.Test
{
    internal class TestSort : SortFactory
    {
        public TestSort(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
        }

        //public TestSort(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, container, myStoryboard, dataItems)
        //{
        //}

        public override void Execute()
        {
            MoveElem(0, 1);

            Stick(() =>
            {
                
            });

            

            UnStick(() =>
            {
                Finish();
            });

            MainStoryboard.Begin_Ex(Canvas, true);
        }

        protected override void Init()
        {
        }
    }
}
