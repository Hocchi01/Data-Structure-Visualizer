using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels
{
    internal partial class AnimationControlPanelViewModel : 
        ObservableRecipient,
        IRecipient<BeginAnyAnimationMessage>,
        IRecipient<EndAnyAnimationMessage>
    {
        private static AnimationControlPanelViewModel instance = null;
        private static readonly object padlock = new object();

        private AnimationControlPanelViewModel()
        {
            IsActive = true;
        }

        public static AnimationControlPanelViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AnimationControlPanelViewModel();
                    }
                    return instance;
                }
            }
        }

        [ObservableProperty]
        private AnimationState state = AnimationState.Stopped;

        [ObservableProperty]
        private double speedRatio = 1.0;

        partial void OnSpeedRatioChanged(double value)
        {
            WeakReferenceMessenger.Default.Send(new StoryboradSpeedRadioChangedMessage(value));
        }

        [RelayCommand]
        private void PlayOrPause()
        {
            switch (State)
            {
                case AnimationState.Running: 
                    State = AnimationState.Paused;
                    WeakReferenceMessenger.Default.Send(new PauseAnyAnimationMessage());
                    break;
                case AnimationState.Paused: 
                    State = AnimationState.Running;
                    WeakReferenceMessenger.Default.Send(new ResumeAnyAnimationMessage());
                    break;
                default: break;
            }
        }

        [RelayCommand]
        private void Skip()
        {
            if (State != AnimationState.Stopped)
            {
                WeakReferenceMessenger.Default.Send(new SkipAnyAnimationMessage());
                State = AnimationState.Stopped;
            }
        }

        

        public void Receive(BeginAnyAnimationMessage message)
        {
            SpeedRatio = 1.0;
            State = AnimationState.Running;
        }

        public void Receive(EndAnyAnimationMessage message)
        {
            State = AnimationState.Stopped;
        }
    }
}
