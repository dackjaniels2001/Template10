﻿using System;
using System.ComponentModel;
using Template10.Services.KeyboardService;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Template10.Services.BackButtonService
{
    public class BackButtonService : IBackButtonService
    {
        public static IBackButtonService GetDefault(Container.IContainerService container = null)
        {
            container = container ?? Container.ContainerService.Default;
            return container.Resolve<IBackButtonService>();
        }

        IKeyboardService _keyboardService;
        public BackButtonService(IKeyboardService keyboardService)
        {
            _keyboardService = keyboardService;
        }

        private static bool setup = false;
        public void Setup()
        {
            if (setup) return;
            else setup = true;

            _keyboardService.Setup();
            _keyboardService.AfterKeyDown = (e) =>
            {
                e.Handled = true;

                // use this to nav back
                if (e.VirtualKey == Windows.System.VirtualKey.GoBack) e.Handled = RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft) e.Handled = RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) e.Handled = RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder) e.Handled = RaiseBackRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back) e.Handled = RaiseBackRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left) e.Handled = RaiseBackRequested().Handled;

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward) e.Handled = RaiseForwardRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight) e.Handled = RaiseForwardRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder) e.Handled = RaiseForwardRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right) e.Handled = RaiseForwardRequested().Handled;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e)
                => e.Handled = RaiseBackRequested().Handled;
        }

        /// <summary>
        /// This event allows a mechanism to intercept BackRequested and stop it. Some
        /// use cases would include the ModalDialog which would cancel the event, using
        /// it instead for itself to close the dialog - not wanting it to navigate a frame.
        /// </summary>
        public event Common.TypedEventHandler<CancelEventArgs> BeforeBackRequested;
        public event Common.TypedEventHandler<Common.HandledEventArgs> BackRequested;

        public Common.HandledEventArgs RaiseBackRequested()
        {
            var cancelEventArgs = new CancelEventArgs();
            BeforeBackRequested?.Invoke(null, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                return new Common.HandledEventArgs { Handled = true };
            }

            var handledEventArgs = new Common.HandledEventArgs();
            BackRequested?.Invoke(null, handledEventArgs);
            return handledEventArgs;
        }

        /// <summary>
        /// This event allows a mechanism to intercept ForwardRequested and stop it. 
        /// </summary>
        public event Common.TypedEventHandler<CancelEventArgs> BeforeForwardRequested;
        public event Common.TypedEventHandler<Common.HandledEventArgs> ForwardRequested;

        public Common.HandledEventArgs RaiseForwardRequested()
        {
            var cancelEventArgs = new CancelEventArgs();
            BeforeForwardRequested?.Invoke(null, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                return new Common.HandledEventArgs { Handled = true };
            }

            var handledEventArgs = new Common.HandledEventArgs();
            ForwardRequested?.Invoke(null, handledEventArgs);
            return handledEventArgs;
        }

        public event EventHandler BackButtonUpdated;

        public void UpdateBackButton(bool canGoBack, bool canGoForward = false)
        {
            switch (Settings.ShellBackButtonPreference)
            {
                case ShellBackButtonPreferences.AlwaysShowInShell when (!Settings.ShellBackButtonVisible):
                    Settings.ShellBackButtonVisible = true;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
                case ShellBackButtonPreferences.AutoShowInShell when (!Settings.ShellBackButtonVisible):
                    Settings.ShellBackButtonVisible = canGoBack;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
                case ShellBackButtonPreferences.NeverShowInShell when (Settings.ShellBackButtonVisible):
                    Settings.ShellBackButtonVisible = false;
                    BackButtonUpdated?.Invoke(null, EventArgs.Empty);
                    break;
            }
        }
    }
}
