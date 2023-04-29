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
    partial class BinaryTreeTraverseToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private TraverseMode selectedMode;

        public List<TraverseMode> Modes { get; set; }

        public BinaryTreeTraverseToolViewModel()
        {
            Modes = new List<TraverseMode>
            {
                new TraverseMode("Pre-Order", BinaryTreeTraverseMode.PreOrder),
                new TraverseMode("In-Order", BinaryTreeTraverseMode.InOrder),
                new TraverseMode("Post-Order", BinaryTreeTraverseMode.PostOrder),
            };
        }

        public override void ExecuteFunc()
        {
            WeakReferenceMessenger.Default.Send(new LoadBinaryTreeTraverseAnimationMessage(SelectedMode.Mode));
        }
    }

    struct TraverseMode
    {
        public string Name { get; set; }
        public BinaryTreeTraverseMode Mode { get; set; }

        public TraverseMode(string name, BinaryTreeTraverseMode mode)
        {
            Name = name;
            Mode = mode;
        }
    }
}
