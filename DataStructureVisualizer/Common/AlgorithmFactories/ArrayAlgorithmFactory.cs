using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class ArrayAlgorithmFactory : SuccessiveAlgorithmFactory
    {
        private const string addCodeBlock =
            "for (int i = n; i > index; i--)\\" +
            "{\\" +
            "   a[i] = a[i - 1];\\" +
            "}\\" +
            "a[index] = value;";

        private const string removeCodeBlock =
            "for (int i = index; i < n - 1; i++)\\" +
            "{\\" +
            "   a[i] = a[i + 1];\\" +
            "}\\";

        public ArrayAlgorithmFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
        }

        public void AddElem(int index, int value)
        {
            CodeBlockPanel.SetCodeBlock(addCodeBlock);
            codeInfos.Add("for", new CodeInfo(0));
            codeInfos.Add("move", new CodeInfo(2));
            codeInfos.Add("write", new CodeInfo(4));

            for (int i = count - 2; i >= index; i--)
            {
                // TODO 02: 依次后移元素
                CodeBlockPanel.AddAnimation(codeInfos["move"], MainStoryboard.Offset);
                MoveElem(elemIndex: i, toIndex: i + 1, log: new LogViewModel($"Right shift a[{i}]", $"a[{i + 1}] = a[{i}];"));
                CodeBlockPanel.AddAnimation(codeInfos["for"], MainStoryboard.Offset);
                MainStoryboard.Delay(300);
            }
            // TODO 03: 写入添加的元素
            CodeBlockPanel.AddAnimation(codeInfos["write"], MainStoryboard.Offset);
            WriteElem(index, value, new LogViewModel($"Write {value} to a[{index}]", $"a[{index}] = {value};"));

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        public void RemoveElemAlgorithm(int index)
        {
            CodeBlockPanel.SetCodeBlock(removeCodeBlock);
            codeInfos.Add("for", new CodeInfo(0));
            codeInfos.Add("move", new CodeInfo(2));

            // TODO 01: 删除元素
            RemoveElem(index);
            for (int i = index + 1; i < count; i++)
            {
                // TODO 02: 依次左移元素
                CodeBlockPanel.AddAnimation(codeInfos["move"], MainStoryboard.Offset);
                MoveElem(elemIndex: i, toIndex: i - 1, log: new LogViewModel($"Left shift a[{i}]", $"a[{i - 1}] = a[{i}];"));
                CodeBlockPanel.AddAnimation(codeInfos["for"], MainStoryboard.Offset);
                MainStoryboard.Delay(300);
            }

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }
    }
}
