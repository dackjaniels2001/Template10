﻿using Windows.ApplicationModel.Activation;

namespace Template10.Core
{
    public interface IStartArgsEx
    {
        bool ThisIsFirstStart { get; }
        StartArgsEx.StartKinds StartKind { get; }
        StartArgsEx.StartCauses StartCause { get; }
        ApplicationExecutionState PreviousExecutionState { get; }

        object EventArgs { get; }

        Windows.ApplicationModel.Activation.BackgroundActivatedEventArgs BackgroundActivatedEventArgs { get; }
        CachedFileUpdaterActivatedEventArgs CachedFileUpdaterActivatedEventArgs { get; }
        FileActivatedEventArgs FileActivatedEventArgs { get; }
        FileOpenPickerActivatedEventArgs FileOpenPickerActivatedEventArgs { get; }
        FileSavePickerActivatedEventArgs FileSavePickerActivatedEventArgs { get; }
        IActivatedEventArgs IActivatedEventArgs { get; }
        LaunchActivatedEventArgs LaunchActivatedEventArgs { get; }
        SearchActivatedEventArgs SearchActivatedEventArgs { get; }
        ShareTargetActivatedEventArgs ShareTargetActivatedEventArgs { get; }
    }
}