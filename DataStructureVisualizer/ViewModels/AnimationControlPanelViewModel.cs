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
        [ObservableProperty]
        private AnimationState state = AnimationState.Stopped;

        [RelayCommand]
        private void PlayOrPause()
        {
            switch (state)
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

        public AnimationControlPanelViewModel()
        {
            IsActive = true;
        }

        public void Receive(BeginAnyAnimationMessage message)
        {
            State = AnimationState.Running;
        }

        public void Receive(EndAnyAnimationMessage message)
        {
            State = AnimationState.Stopped;
        }
    }
}
