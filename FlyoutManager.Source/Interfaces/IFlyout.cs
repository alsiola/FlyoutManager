using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bulldog.FlyoutManager
{
    public interface IFlyout
    {
        event  EventHandler<FlyoutEventArgs> OnClosed;

        event  EventHandler<FlyoutEventArgs> OnOpened;

        event  EventHandler<FlyoutEventArgs> OnOpenChanged;

        string Position { get; set; }

        string Header { get; set; }

        string Theme { get; set; }

        bool IsModal { get; set; }

        bool AreAnimationsEnabled { get; set; }

        bool AnimateOpacity { get; set; }

        ICommand CloseCommand { get; set; }

        MouseButton ExternalCloseButton { get; set; }

        bool CloseButtonIsCancel { get; set; }

        bool IsPinned { get; set; }

        bool CanClose( FlyoutParameters flyoutParameters );

        void Close( FlyoutParameters flyoutParameters );

        void Close();

        bool CanOpen( FlyoutParameters flyoutParameters );

        void Open( FlyoutParameters flyoutParameters );

        void Open();
    }
}
