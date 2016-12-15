using Prism.Regions;
using System.Windows;

namespace Bulldog.FlyoutManager
{
    public interface IFlyoutManager
    {
        IRegionManager RegionManager { get; set; }

        void SetDefaultFlyoutRegion( string regionName );

        void RegisterFlyoutWithDefaultRegion<TView>( string flyoutKey ) where TView : FrameworkElement;

        void RegisterFlyoutWithDefaultRegion<TView>( string flyoutKey, IFlyout viewModel ) where TView : FrameworkElement;

        void RegisterFlyout<T>( string flyoutKey, string regionName ) where T : FrameworkElement;

        void RegisterFlyout<T>( string flyoutKey, string regionName, IFlyout flyoutViewModel ) where T : FrameworkElement;

        void UnRegisterFlyout( string key );

        void UnRegisterFlyout( IFlyout flyout );

        bool OpenFlyout( string key );

        bool OpenFlyout( string key, FlyoutParameters flyoutParameters );

        bool OpenFlyout( string key, bool forceOpen );

        bool OpenFlyout( string key, FlyoutParameters flyoutParameters, bool forceOpen );

        bool CloseFlyout( string key );

        bool CloseFlyout( string key, FlyoutParameters flyoutParameters );

        bool CloseFlyout( string key, bool forceClose );

        bool CloseFlyout( string key, FlyoutParameters flyoutParameters, bool forceClose );
    }
}
