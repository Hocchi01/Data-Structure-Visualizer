using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Tools
{
    partial class SortToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private Sort selectedSort;

        public List<Sort> Sorts { get; set; }

        public override void ExecuteFunc()
        {
            WeakReferenceMessenger.Default.Send(new LoadSortAnimationMessage(SelectedSort.Type));
        }

        public SortToolViewModel()
        {
            Sorts = new List<Sort>
            {
                new Sort("Selection Sort", SortType.SelectionSort),
                new Sort("Bubble Sort", SortType.BubbleSort),
                new Sort("Insertion Sort", SortType.InsertionSort),
                new Sort("Quick Sort", SortType.QuickSort),
                new Sort("Merge Sort", SortType.MergeSort),
                new Sort("[TEST]", SortType.TEST),
            };
        }
    }

    struct Sort
    {
        public string Name { get; set; }
        public SortType Type { get; set; }

        public Sort(string name, SortType type) 
        {
            Name = name;
            Type = type;
        }
    }
}
