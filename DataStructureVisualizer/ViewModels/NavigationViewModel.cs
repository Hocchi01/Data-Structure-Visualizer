using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Canvas;
using DataStructureVisualizer.Views.Canvas;
using DataStructureVisualizer.Views.Toolboxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataStructureVisualizer.ViewModels
{
    internal partial class NavigationViewModel : ObservableObject
    {
        private readonly NavigationChildItemViewModel undefined = new NavigationChildItemViewModel()
        {
            Type = DS_SecondaryType.Undefined,
        };

        public NavigationViewModel() 
        {
            selected_DS = undefined;
            LoadNavigation(); 
        }

        [ObservableProperty]
        private NavigationChildItemViewModel selected_DS;

        /// <summary>
        /// 选择的 DS 改变时，通知相应 Canvas 去响应
        /// </summary>
        /// <param name="value">新值</param>
        partial void OnSelected_DSChanging(NavigationChildItemViewModel value)
        {
            WeakReferenceMessenger.Default.Send(new PropertyChangedMessage<NavigationChildItemViewModel>(this, null, selected_DS, value ?? undefined));
        }

        public List<NavigationItemViewModel> MainItems { get; set; }

        /// <summary>
        /// 初始化导航栏数据
        /// </summary>
        private void LoadNavigation()
        {
            this.MainItems = new List<NavigationItemViewModel>()
            {
                new NavigationItemViewModel()
                {
                    Name = "Linear",
                    IconKind = "ViewColumnOutline",
                    Children = new List<NavigationChildItemViewModel>()
                    {
                        new NavigationChildItemViewModel() { Name="Array", Type=DS_SecondaryType.Array, CanvasViewModelType=typeof(ArrayCanvasViewModel), CanvasViewType=typeof(ArrayCanvasUserControl), ToolboxViewType=typeof(ArrayToolboxUserControl) },

                        new NavigationChildItemViewModel() { Name="Linked List", Type=DS_SecondaryType.LinkedList, CanvasViewModelType=typeof(LinkedListCanvasViewModel), CanvasViewType=typeof(LinkedListCanvasUserControl), ToolboxViewType=typeof(LinkedListToolboxUserControl) },

                        new NavigationChildItemViewModel() { Name="Queue", Type=DS_SecondaryType.Queue }
                    }
                },

                new NavigationItemViewModel()
                {
                    Name = "Tree",
                    IconKind = "GraphOutline",
                    Children = new List<NavigationChildItemViewModel>()
                    {
                        new NavigationChildItemViewModel() { Name="BinaryTree", Type=DS_SecondaryType.BinaryTree, CanvasViewModelType=typeof(BinaryTreeCanvasViewModel), CanvasViewType=typeof(BinaryTreeCanvasUserControl), ToolboxViewType=typeof(BinaryTreeToolboxUserControl) },
                    }
                },

                new NavigationItemViewModel()
                {
                    Name = "Graph",
                    IconKind = "VectorPolyline",
                    Children = new List<NavigationChildItemViewModel>()
                    {
                        new NavigationChildItemViewModel() { Name="Directed Graph", Type=DS_SecondaryType.DirectedGraph },
                        new NavigationChildItemViewModel() { Name="Undirected Graph", Type=DS_SecondaryType.UndirectedGraph }
                    }
                },
                new NavigationItemViewModel()
                {
                    Name = "Hash",
                    IconKind = "Table",
                    Children = new List<NavigationChildItemViewModel>()
                    {
                        new NavigationChildItemViewModel() { Name="Hashtable", Type=DS_SecondaryType.Hashtable },
                    }
                }
            };
        }
    }


}
