using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.ViewModels;
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
        private const string searchCodeBlock =
            "BSTNode *BST_Search(BiTree T, ElemType key, BSTNode *&p)\\" +
            "{\\" +
            "   p = NULL;\\" +
            "   while (T != NULL && key != T -> data)\\" +
            "   {\\" +
            "       p = T;\\" +
            "       if (key < T -> data)    T = T -> lchild;\\" +
            "       else                    T = T -> rchild;\\" +
            "   }\\" +
            "   return T;\\" +
            "}\\";

        private const string removeCodeBlock =
            "int BST_Remove(BiTree T, ElemType k)\\" +
            "{\\" +
            "   while (T != NULL && key != T -> data)\\" +
            "   {\\" +
            "       if (key < T -> data)    T = T -> lchild;\\" +
            "       else                    T = T -> rchild;\\" +
            "   }\\" +
            "   if (T == NULL) return 0;\\" +
            "   if (T -> lchild == NULL && T -> rchild == NULL)\\" +
            "       RemoveLeafNode(T);\\" +
            "   else if (T -> lchild == NULL || T -> rchild == NULL)\\" +
            "       RemoveSingleChildNode(T);\\" +
            "   else    RemoveDoubleChildNode(T);\\" +
            "}\\";

        private const string insertCodeBlock =
            "int BST_Insert(BiTree &T, ElemType k)\\" +
            "{\\" +
            "   if (T == NULL)\\" +
            "   {\\" +
            "       T = (BiTree)malloc(sizeof(BSTNode));\\" +
            "       T -> data = k;\\" +
            "       T -> lchild = T -> rchild = NULL;\\" +
            "       return 1;\\" +
            "   }\\" +
            "   else if (k == T -> data)    return 0;\\" +
            "   else if (k < T -> data)\\" +
            "       return BST_Insert(T -> lchild, k);\\" +
            "   else\\" +
            "       return BST_Insert(T -> rchild, k);\\" +
            "}\\";

        public BinarySearchTreeAlgorithmFactory(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<BinaryTreeItemViewModel> dataItems) : base(canvas, codeBlockPanelView, container, myStoryboard, dataItems)
        {
        }

        public void Insert(int value)
        {
            CodeBlockPanel.SetCodeBlock(insertCodeBlock);
            codeInfos.Add("func", new CodeInfo(0));
            codeInfos.Add("if", new CodeInfo(2));
            codeInfos.Add("new", new CodeInfo(4, 4));
            codeInfos.Add("eq", new CodeInfo(9));
            codeInfos.Add("less", new CodeInfo(10));
            codeInfos.Add("left", new CodeInfo(11));
            codeInfos.Add("greater", new CodeInfo(12));
            codeInfos.Add("right", new CodeInfo(13));

            int? i = 0;
            int j;
            while (i != null)
            {
                j = i ?? -1;

                if (value == DataItems[j].Value)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["eq"], MainStoryboard.Offset);
                    MainStoryboard.InsertLog(new LogViewModel($"Exists {value}", "Return 0", "return 0;"));
                    Visit_Search(j, true);
                    break;
                }
                Visit_Search(j);

                if (value < DataItems[j].Value)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["less"], MainStoryboard.Offset);
                    CodeBlockPanel.AddAnimation(codeInfos["left"]);
                    CodeBlockPanel.AddAnimation(codeInfos["func"], time: 300);
                    MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
                    if (DataItems[j].Children[0] != null)
                    {
                        GoToLeft(j, new LogViewModel($"{value} < {DataItems[j].Value}", "Visit Left Child", "BST_Insert(T -> lchild, k);"));
                        i = DataItems[j].Children[0];
                    }
                    else
                    {
                        CodeBlockPanel.AddAnimation(codeInfos["if"]);
                        CodeBlockPanel.AddAnimation(codeInfos["new"]);
                        InsertToLeft(j, value, new LogViewModel("Left is null", "Insert Node", "T = (BiTree)malloc(sizeof(BSTNode)); T -> data = k; T -> lchild = T -> rchild = NULL;"));
                        break;
                    }
                }
                else
                {
                    CodeBlockPanel.AddAnimation(codeInfos["greater"], MainStoryboard.Offset);
                    CodeBlockPanel.AddAnimation(codeInfos["right"]);
                    CodeBlockPanel.AddAnimation(codeInfos["func"], time: 300);
                    MainStoryboard.Offset = CodeBlockPanel.CodeBlockStoryboard.Offset;
                    if (DataItems[j].Children[1] != null)
                    {
                        GoToRight(j, new LogViewModel($"{value} > {DataItems[j].Value}", "Visit Right Child", "BST_Insert(T -> rchild, k);"));
                        i = DataItems[j].Children[1];
                    }
                    else
                    {
                        CodeBlockPanel.AddAnimation(codeInfos["if"]);
                        CodeBlockPanel.AddAnimation(codeInfos["new"]);
                        InsertToRight(j, value, new LogViewModel("Right is null", "Insert Node", "T = (BiTree)malloc(sizeof(BSTNode)); T -> data = k; T -> lchild = T -> rchild = NULL;"));
                        break;
                    }
                }
            }

            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        public void Search(int value)
        {
            CodeBlockPanel.SetCodeBlock(searchCodeBlock);
            codeInfos.Add("while", new CodeInfo(3));
            codeInfos.Add("left", new CodeInfo(6));
            codeInfos.Add("right", new CodeInfo(7));
            codeInfos.Add("return", new CodeInfo(9));

            int? i = 0;
            int j;
            bool isExist = false;
            while (i != null)
            {
                j = i ?? -1;

                CodeBlockPanel.AddAnimation(codeInfos["while"], MainStoryboard.Offset);
                if (value == DataItems[j].Value)
                {
                    isExist = true;
                    MainStoryboard.InsertLog(new LogViewModel($"{value} = {DataItems[j].Value}", "Return Node", "return T;"));
                    Visit_Search(j, true);
                    break;
                }
                Visit_Search(j);

                if (value < DataItems[j].Value)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["left"], MainStoryboard.Offset);

                    GoToLeft(j, new LogViewModel($"{value} < {DataItems[j].Value}", "Visit Left Child", "T = T -> lchild;"));
                    i = DataItems[j].Children[0];
                }
                else
                {
                    CodeBlockPanel.AddAnimation(codeInfos["right"], MainStoryboard.Offset);

                    GoToRight(j, new LogViewModel($"{value} > {DataItems[j].Value}", "Visit Right Child", "T = T -> rchild;"));
                    i = DataItems[j].Children[1];
                }
            }
            CodeBlockPanel.AddAnimation(codeInfos["return"]);
            if (!isExist)
            {
                MainStoryboard.InsertLog(new LogViewModel($"No {value} Exists", "Return NULL", "return NULL;"));
            }


            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }

        public void Remove(int value)
        {
            CodeBlockPanel.SetCodeBlock(removeCodeBlock);
            codeInfos.Add("while", new CodeInfo(2));
            codeInfos.Add("less", new CodeInfo(4));
            codeInfos.Add("greater", new CodeInfo(5));
            codeInfos.Add("null", new CodeInfo(7));
            codeInfos.Add("leaf", new CodeInfo(9));
            codeInfos.Add("single", new CodeInfo(11));
            codeInfos.Add("double", new CodeInfo(12));


            int? i = 0;
            int j;
            bool isExist = false;
            while (i != null)
            {
                j = i ?? -1;

                CodeBlockPanel.AddAnimation(codeInfos["while"], MainStoryboard.Offset);
                if (value == DataItems[j].Value)
                {
                    isExist = true;
                    Visit_Search(j, true);
                    if (DataItems[j].IsLeaf)
                    {
                        CodeBlockPanel.AddAnimation(codeInfos["leaf"], MainStoryboard.Offset);
                        RemoveLeafElem(j, new LogViewModel("It's leaf node", "Remove Leaf Node", "RemoveLeafNode(T);"));
                    }
                    else if (DataItems[j].GetChildrenCount() == 1)
                    {
                        CodeBlockPanel.AddAnimation(codeInfos["single"], MainStoryboard.Offset);
                        RemoveSingleChildElem(j, new LogViewModel("Have one child", "Remove Single Child Node", "RemoveSingleChildNode(T);"));
                    }
                    else
                    {
                        CodeBlockPanel.AddAnimation(codeInfos["double"], MainStoryboard.Offset);
                        MainStoryboard.InsertLog(new LogViewModel("Have two children", "Find successor", ""));
                        int successor = FindInOrderSuccessor(j);
                        CopyValue(successor, j, new LogViewModel("Copy successor"));
                        i = successor;
                        value = DataItems[successor].Value ?? -1;
                        continue;
                    }
                    break;
                }
                Visit_Search(j);

                if (value < DataItems[j].Value)
                {
                    CodeBlockPanel.AddAnimation(codeInfos["less"], MainStoryboard.Offset);
                    GoToLeft(j, new LogViewModel($"{value} < {DataItems[j].Value}", "Visit Left Child", "T = T -> lchild;"));
                    i = DataItems[j].Children[0];
                }
                else
                {
                    CodeBlockPanel.AddAnimation(codeInfos["greater"], MainStoryboard.Offset);
                    GoToRight(j, new LogViewModel($"{value} > {DataItems[j].Value}", "Visit Right Child", "T = T -> rchild;"));
                    i = DataItems[j].Children[1];
                }
            }
            if (!isExist)
            {
                CodeBlockPanel.AddAnimation(codeInfos["null"], MainStoryboard.Offset);
                MainStoryboard.InsertLog(new LogViewModel($"No {value} Exists", "Return 0", "return 0;"));
            }


            MainStoryboard.Begin_Ex(Canvas);
            CodeBlockPanel.CodeBlockStoryboard.Begin_Ex(CodeBlockPanelView);
        }
    }
}
