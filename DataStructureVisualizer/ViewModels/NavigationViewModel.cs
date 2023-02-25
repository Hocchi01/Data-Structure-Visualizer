using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Canvas;
using DataStructureVisualizer.Views.Canvas;
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
        public NavigationViewModel() 
        {
            selected_DS = new NavigationChildItemViewModel()
            {
                Type = DS_SecondaryType.Undefined,
            };
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
            //DS_SecondaryType oldType = Selected_DS == null ? DS_SecondaryType.Undefined : Selected_DS.Type;
            //UserControl oldCanvasView = Selected_DS == null ? new UserControl() : Selected_DS.CanvasView;

            //WeakReferenceMessenger.Default.Send(new PropertyChangedMessage<DS_SecondaryType>(this, null, oldType, value.Type));
            //WeakReferenceMessenger.Default.Send(new PropertyChangedMessage<UserControl>(this, null, oldCanvasView, value.CanvasView));

            WeakReferenceMessenger.Default.Send(new PropertyChangedMessage<NavigationChildItemViewModel>(this, null, selected_DS, value));
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
                        new NavigationChildItemViewModel() { Name="Array", Type=DS_SecondaryType.Array, CanvasViewModelType=typeof(ArrayCanvasViewModel), CanvasViewType=typeof(ArrayCanvasUserControl) },

                        new NavigationChildItemViewModel() { Name="Linked List", Type=DS_SecondaryType.LinkedList, CanvasViewModelType=typeof(LinkedListCanvasViewModel), CanvasViewType=typeof(LinkedListCanvasUserControl) },

                        new NavigationChildItemViewModel() { Name="Queue", Type=DS_SecondaryType.Queue }
                    }
                },
                new NavigationItemViewModel()
                {
                    Name = "Tree",
                    IconKind = "GraphOutline",
                    Children = new List<NavigationChildItemViewModel>()
                    {
                        new NavigationChildItemViewModel() { Name="BinaryTree", Type=DS_SecondaryType.BinaryTree },
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
