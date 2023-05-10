using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class BinarySearchTreeAlgorithmFactory : BinaryTreeAlgorithmFactory
    {
        public BinarySearchTreeAlgorithmFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<BinaryTreeItemViewModel> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
        }

        public void Insert(int value)
        {
            int? i = 0;
            int j;
            while (i != null)
            {
                j = i ?? -1;
                if (value == DataItems[j].Value)
                {
                    Visit_Search(j, true);
                    break;
                }
                Visit_Search(j);

                if (value < DataItems[j].Value)
                {
                    if (DataItems[j].Children[0] != null)
                    {
                        GoToLeft(j);
                        i = DataItems[j].Children[0];
                    }
                    else
                    {
                        InsertToLeft(j, value);
                        
                        break;
                    }
                }
                else
                {
                    if (DataItems[j].Children[1] != null)
                    {
                        GoToRight(j);
                        i = DataItems[j].Children[1];
                    }
                    else
                    {
                        InsertToRight(j, value);
                        
                        break;
                    }
                }
            }

            MainStoryboard.Begin_Ex(Canvas);

            CodeBlockPanel.CodeBlockStoryboard.Delay(MainStoryboard.Offset);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        public void Search(int value)
        {
            int? i = 0;
            int j;
            bool isExist = false;
            while (i != null)
            {
                j = i ?? -1;
                if (value == DataItems[j].Value)
                {
                    isExist = true;
                    Visit_Search(j, true);
                    break;
                }
                Visit_Search(j);

                if (value < DataItems[j].Value)
                {
                    GoToLeft(j);
                    i = DataItems[j].Children[0];
                }
                else
                {
                    GoToRight(j);
                    i = DataItems[j].Children[1];
                }
            }

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Delay(MainStoryboard.Offset);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        public void Remove(int value)
        {
            int? i = 0;
            int j;
            bool isExist = false;
            while (i != null)
            {
                j = i ?? -1;
                if (value == DataItems[j].Value)
                {
                    isExist = true;
                    Visit_Search(j, true);
                    if (DataItems[j].IsLeaf)
                    {
                        RemoveLeafElem(j);
                    }
                    else if (DataItems[j].GetChildrenCount() == 1)
                    {
                        RemoveSingleChildElem(j);
                    }
                    else
                    {
                        int successor = FindInOrderSuccessor(j);
                        CopyValue(successor, j);
                        i = successor;
                        value = DataItems[successor].Value ?? -1;
                        continue;
                    }
                    break;
                }
                Visit_Search(j);

                if (value < DataItems[j].Value)
                {
                    GoToLeft(j);
                    i = DataItems[j].Children[0];
                }
                else
                {
                    GoToRight(j);
                    i = DataItems[j].Children[1];
                }
            }

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Delay(MainStoryboard.Offset);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }


    }
}
