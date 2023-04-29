using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Tools
{
    partial class BinaryTreeRandomGenerateToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private int amount = 10;

        [ObservableProperty]
        private int depth = 1;

        [ObservableProperty]
        private int min = 0;

        [ObservableProperty]
        private int max = 100;

        [ObservableProperty]
        private BinaryTreeType selectedType = new BinaryTreeType("Normal", TreeType.NormalBinaryTree);

        public List<BinaryTreeType> Types { get; set; }

        public BinaryTreeRandomGenerateToolViewModel()
        {
            Name = "Random Generate";
            Types = new List<BinaryTreeType>
            {
                new BinaryTreeType("Normal", TreeType.NormalBinaryTree),
                new BinaryTreeType("Complete", TreeType.CompleteBinaryTree),
                new BinaryTreeType("Full", TreeType.FullBinaryTree),
            };
        }

        public override void ExecuteFunc()
        {
            int amount = SelectedType.Type == TreeType.FullBinaryTree ? (1 << Depth) - 1 : Amount;
            int[] values = new int[amount];
            Random r = new Random();
            for (int i = 0; i < amount; i++)
            {
                values[i] = r.Next(Min, Max);
            }

            WeakReferenceMessenger.Default.Send(new GenerateDataMessage(values) { TreeType = SelectedType.Type });
        }
    }

    struct BinaryTreeType
    {
        public string Name { get; set; }
        public TreeType Type { get; set; }

        public BinaryTreeType(string name, TreeType type) 
        {
            Name = name;
            Type = type;
        }
    }
}
