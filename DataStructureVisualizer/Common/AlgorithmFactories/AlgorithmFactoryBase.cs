using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;
using DataStructureVisualizer.Common.Theme;
using DataStructureVisualizer.ViewModels;
using DataStructureVisualizer.Views;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    internal class AlgorithmFactoryBase
    {
        protected CodeBlockPanelViewModel CodeBlockPanel { get; set; }

        public string CodeBlock { get; set; }

        protected Dictionary<int, int> newValues = new Dictionary<int, int>();
        // protected List<int?> vals;
        protected List<int> table; // 用于记录当前元素实际位置
        protected int last;
        protected int count;
        protected int activeIndex = -1; // 维护一个当前唯一被激活的元素索引

        protected List<Action> endOperations = new List<Action>();

        protected AnimationTimeline lastAnimation = null;

        public Grid Canvas { get; set; }
        public CodeBlockPanelUserControl CodeBlockPanelView { get; set; }
        public ItemsControl Container { get; set; }
        public MyStoryboard MainStoryboard { get; set; }
        public ObservableCollection<DataItemViewModelBase> DataItems { get; set; }

        public AlgorithmFactoryBase(Grid canvas, CodeBlockPanelUserControl codeBlockPanelView, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems) : this(canvas, container, myStoryboard, dataItems)
        {
            CodeBlockPanelView = codeBlockPanelView;
        }

        public AlgorithmFactoryBase(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<DataItemViewModelBase> dataItems)
        {
            CodeBlockPanel = CodeBlockPanelViewModel.Instance;

            Canvas = canvas;
            Container = container;
            MainStoryboard = myStoryboard;
            DataItems = dataItems;

            count = DataItems.Count;
            last = count - 1;

            table = new List<int>();
            for (int i = 0; i < count; i++)
            {
                table.Add(i);
            }

            MainStoryboard.Completed += (_, _) => 
            {
                foreach (var op in endOperations)
                {
                    op();
                }
                UpdateValues(); 
            };
        }

        public void RemoveElem(int elemIndex, DependencyObject control, bool isChangeTable = true, Action? before = null, Action? after = null)
        {
            int elemRealIndex = table[elemIndex];
            Action afterActions = () => { DataItems[elemRealIndex].Value = null; };
            afterActions += after;

            var rmvAnim = new SimulatedDoubleAnimation(to: 0, time: 500, before: before, after: afterActions) { TargetControl = control, TargetParam = UIElement.OpacityProperty };

            MainStoryboard.AddSyncAnimation(rmvAnim);

            if (isChangeTable)
                table.RemoveAt(elemIndex);
        }

        public void RemoveElem(int elemIndex, bool isChangeTable = false, Action? before = null, Action? after = null)
        {
            int elemRealIndex = table[elemIndex];
            var control = Comm.GetItemFromItemsControlByIndex(Container, elemRealIndex).ValueItem;
            RemoveElem(elemIndex, control, isChangeTable, before, after);
        }

        public void WriteElem(int elemIndex, int elemVal, LogViewModel log = null) 
        {
            int elemRealIndex = table[elemIndex];

            var control = Comm.GetItemFromItemsControlByIndex(Container, elemRealIndex).ValueItem;

            var writeAnim = new SimulatedDoubleAnimation(from: 0, to: 1, time: 1000, before: () =>
            {
                DeactivateElem();
                DataItems[elemRealIndex].Value = elemVal;
                DataItems[elemRealIndex].Color = new SolidColorBrush(ThemeHelper.NewColor);
            }, after: null)
            { TargetControl = control, TargetParam = UIElement.OpacityProperty, Log = log };

            MainStoryboard.AddSyncAnimation(writeAnim);
        }

        public void NewElem(DependencyObject control, Action? before = null, Action? after = null)
        {
            var newAnim = new SimulatedDoubleAnimation(from: 0, to: 1, time: 1000, before: before, after: after) { TargetControl = control, TargetParam = UIElement.OpacityProperty };
            MainStoryboard.AddSyncAnimation(newAnim);
        }

        protected void SwapElemsInTable(int index1, int index2)
        {
            int tmp = table[index1];
            table[index1] = table[index2];
            table[index2] = tmp;
        }

        protected void UpdateValues()
        {
            List<int> values = new List<int>();
            for (int i = 0; i < table.Count; i++)
            {
                if (table[i] > last)
                {
                    values.Add(newValues[table[i]]);
                }
                else if (DataItems[table[i]].Value != null)
                {
                    values.Add(DataItems[table[i]].Value ?? 0);
                }
            }

            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(Comm.ListToArray(values)));
        }

        protected void ActivateElem(int elemIndex)
        {
            if (activeIndex >= 0 && activeIndex != elemIndex)
            {
                DataItems[activeIndex].Deactivate();
            }
            DataItems[elemIndex].Activate();

            activeIndex = elemIndex;
        }

        protected void DeactivateAllElems()
        {
            foreach (var item in DataItems)
            {
                item.Deactivate();
            }
        }

        protected void DeactivateElem()
        {
            DataItems[activeIndex].Deactivate();
            activeIndex = -1;
        }
    }
}
