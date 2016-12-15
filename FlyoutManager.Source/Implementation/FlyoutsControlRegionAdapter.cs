using MahApps.Metro.Controls;
using Prism.Regions;
using System.Collections.Specialized;
using System.Windows;

namespace Bulldog.FlyoutManager
{
    /// <summary>
    /// Region Adapter for the Flyouts Control
    /// Uses AllActiveRegion so numerous Flyoutscan be attached to one region.
    /// </summary>
    public class FlyoutsControlRegionAdapter : RegionAdapterBase<FlyoutsControl>
    {
        public FlyoutsControlRegionAdapter( IRegionBehaviorFactory factory )
            : base( factory )
        {
        }

        protected override void Adapt( IRegion region, FlyoutsControl regionTarget )
        {
            region.ActiveViews.CollectionChanged += ( s, e ) =>
            {
                if( e.Action == NotifyCollectionChangedAction.Add )
                {
                    foreach( FrameworkElement element in e.NewItems )
                    {
                        Flyout flyout = new Flyout();
                        flyout.Content = element;
                        flyout.DataContext = element.DataContext;
                        regionTarget.Items.Add( flyout );
                    }
                }
                if( e.Action == NotifyCollectionChangedAction.Remove )
                {
                    foreach( FrameworkElement element in e.OldItems )
                    {
                        regionTarget.Items.Remove( element );
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
