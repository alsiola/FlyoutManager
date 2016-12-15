using System;

namespace Bulldog.FlyoutManager
{
    /// <summary>
    /// Event arguments passed with the OnOpened, OnClosed and OnOpenChanged event delegates.
    /// Flyout action is a required parameter to the constructor.
    /// Additional FlyoutParameters can also be attached.
    /// </summary>
    public class FlyoutEventArgs : EventArgs
    {
        public FlyoutAction FlyoutAction { get; set; }

        public FlyoutEventArgs( FlyoutAction flyoutAction )
        {
            FlyoutAction = flyoutAction;
        }
    }
}
