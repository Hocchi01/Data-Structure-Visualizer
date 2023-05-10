using DataStructureVisualizer.ViewModels.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels
{
    internal class ToolboxViewModel
    {
        public RandomGenerateToolViewModel RandomGenerateTool { get; set; }
        public BinaryTreeRandomGenerateToolViewModel BinaryTreeRandomGenerateTool { get; set; }
        public AddToolViewModel AddTool { get; set; }
        public SortToolViewModel SortTool { get; set; }
        public RemoveToolViewModel RemoveTool { get; set; }
        public RemoveByValueToolViewModel RemoveByValueTool { get; set; }

        public BinaryTreeTraverseToolViewModel BinaryTreeTraverseTool { get; set; }
        public SearchToolViewModel SearchTool { get; set; }

        public ToolboxViewModel() 
        { 
            RandomGenerateTool = new RandomGenerateToolViewModel();
            BinaryTreeRandomGenerateTool = new BinaryTreeRandomGenerateToolViewModel();
            AddTool = new AddToolViewModel();
            SortTool = new SortToolViewModel();
            RemoveTool = new RemoveToolViewModel();
            BinaryTreeTraverseTool = new BinaryTreeTraverseToolViewModel();
            SearchTool = new SearchToolViewModel();
            RemoveByValueTool = new RemoveByValueToolViewModel();
        }
    }
}
