using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Bulldog.FlyoutManager
{
    /// <summary>
    /// Base class that provides and implementation of IFlyout.
    /// Inherits from BindableBase to allow property binding.
    /// Recommended to perform the majority of ViewModel initialization within an override of OnOpening to prevent memory leaks,
    /// and clean up as many resources as possible within OnClosing as the instance of the ViewModel will remain in memory from the
    /// moment of registration with the FlyoutManager until the moment of unregistration, or program exit.
    /// </summary>
    public abstract class FlyoutBase : BindableBase, IFlyout
    {
        public  event  EventHandler<FlyoutEventArgs> OnClosed;
        public  event  EventHandler<FlyoutEventArgs> OnOpened;
        public  event  EventHandler<FlyoutEventArgs> OnOpenChanged;
        private bool   isOpen;
        private string position;
        private string header;
        private string theme;
        private bool isModal = false;
        private bool areAnimationsEnabled = true;
        private bool animateOpacity;
        private ICommand closeCommand;
        private MouseButton externalCloseButton;
        private bool closeButtonIsCancel;
        private bool isPinned;

        /// <summary>
        /// Bindable property to determine open/closed staus of flyout, based on private field isOpen.
        /// Generally set by FlyoutManager calling Open or Close method.
        /// Initial value can be set by concrete child class.
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                OnChanging( value );
                SetProperty( ref isOpen, value );

            }
        }

        /// <summary>
        /// Bindable property to determine position of flyout in window.
        /// Possible values are stored in FlyoutPosition class and can be used to set property in the concrete class.
        /// </summary>
        public string Position
        {
            get { return position; }
            set { SetProperty( ref position, value ); }
        }

        /// <summary>
        /// Bindable property to determine header of flyout.
        /// Value set by concrete child class.
        /// </summary>
        public string Header
        {
            get { return header; }
            set { SetProperty( ref header, value ); }
        }

        /// <summary>
        /// Bindable property to determine theme of flyout.
        /// Possible values are stored in FlyoutTheme class and can be used to set property in concrete class.
        /// </summary>
        public string Theme
        {
            get { return theme; }
            set { SetProperty( ref theme, value ); }
        }

        /// <summary>
        /// Bindable property to determine whether the Flyout is modal or not
        /// </summary>
        public bool IsModal
        {
            get { return isModal; }
            set { SetProperty( ref isModal, value ); }
        }

        /// <summary>
        /// Bindable property to determine whether flyout animations are enabled
        /// </summary>
        public bool AreAnimationsEnabled
        {
            get { return areAnimationsEnabled; }
            set { SetProperty( ref areAnimationsEnabled, value ); }
        }


        /// <summary>
        /// Is opacity animated.
        /// </summary>
        public bool AnimateOpacity
        {
            get { return animateOpacity; }
            set { SetProperty( ref animateOpacity, value ); }
        }

        /// <summary>
        /// Command to execute when the close button is pressed
        /// </summary>
        public ICommand CloseCommand
        {
            get { return closeCommand; }
            set { SetProperty( ref closeCommand, value ); }
        }

        /// <summary>
        /// Designate a mousebutton that will close the flyout when pressed outside of its bounds.
        /// </summary>
        public MouseButton ExternalCloseButton
        {
            get { return externalCloseButton; }
            set { SetProperty( ref externalCloseButton, value ); }
        }

        /// <summary>
        /// Does the close button act as a cancel button.
        /// </summary>
        public bool CloseButtonIsCancel
        {
            get { return closeButtonIsCancel; }
            set { SetProperty( ref closeButtonIsCancel, value ); }
        }

        /// <summary>
        /// Is the flyout pinned.
        /// </summary>
        public bool IsPinned
        {
            get { return isPinned; }
            set { SetProperty( ref isPinned, value ); }
        }

        /// <summary>
        /// Whether the flyout is currently able to open.
        /// Default: returns true
        /// Override in concrete child class to implement custom logic.
        /// </summary>
        /// <param name="flyoutParameters">Instance of <typeparamref name="FlyoutParameters"/> containing information on current Open request.</param>
        /// <returns>True if flyout can open, false if not.</returns>
        public virtual bool CanOpen( FlyoutParameters flyoutParameters )
        {
            return true;
        }

        /// <summary>
        /// Whether the flyout is currently able to close.
        /// Default: returns true.
        /// Override in concrete child class to implement custom logic.
        /// </summary>
        /// <param name="flyoutParameters">Instance of <typeparamref name="FlyoutParameters"/> containing information on current Close request.</param>
        /// <returns>True if flyout can close, false if not.</returns>
        public virtual bool CanClose( FlyoutParameters flyoutParameters )
        {
            return true;
        }

        /// <summary>
        /// Event delegate called on flyout closing with FlyoutEventArgs.
        /// </summary>
        /// <param name="flyoutParameters">Instance of <typeparamref name="FlyoutParameters"/> containing information on current Close request.</param>
        protected virtual void OnClosing( FlyoutParameters flyoutParameters ) { }

        /// <summary>
        /// Event delegate called on flyout closing, with FlyoutEventArgs.
        /// </summary>
        /// <param name="flyoutParameters">Instance of <typeparamref name="FlyoutParameters"/> containing information on current Close request.</param>
        protected virtual void OnOpening( FlyoutParameters flyoutParameters ) { }

        /// <summary>
        /// Event delegate called on flyout open status changed, with FlyoutEventArgs containing open/close direction.
        /// </summary>
        /// <param name="isOpening"></param>
        protected virtual void OnChanging( bool isOpening )
        {
            var flyoutAction = isOpening ? FlyoutAction.Opening : FlyoutAction.Closing;
            var flyoutEventArgs = new FlyoutEventArgs( flyoutAction );
            if( OnOpenChanged != null )
                OnOpenChanged( this, flyoutEventArgs );
            if( flyoutAction == FlyoutAction.Opening )
                if( OnOpened != null )
                    OnOpened( this, flyoutEventArgs );
            if( flyoutAction == FlyoutAction.Closing )
                if( OnClosed != null )
                    OnClosed( this, flyoutEventArgs );
        }

        /// <summary>
        /// Close the flyout.
        /// </summary>
        /// <param name="flyoutParameters"></param>
        public void Close( FlyoutParameters flyoutParameters )
        {
            OnClosing( flyoutParameters );
            IsOpen = false;
        }

        public void Close()
        {
            Close( new FlyoutParameters() );
        }

        /// <summary>
        /// Open the flyout.
        /// </summary>
        /// <param name="flyoutParameters"></param>
        public void Open( FlyoutParameters flyoutParameters )
        {
            OnOpening( flyoutParameters );
            IsOpen = true;
        }

        public void Open()
        {
            Open( new FlyoutParameters() );
        }
    }
}
