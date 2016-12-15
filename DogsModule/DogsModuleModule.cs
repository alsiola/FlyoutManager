using Bulldog.FlyoutManager;
using DogsModule.Views;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace DogsModule
{
    public class DogsModuleModule : IModule
    {
        IRegionManager _regionManager;
        IFlyoutManager _flyoutManager;

        public DogsModuleModule( RegionManager regionManager, IFlyoutManager flyoutManager )
        {
            _regionManager = regionManager;
            _flyoutManager = flyoutManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion( "ContentRegion", typeof( DogList ) );

            _flyoutManager.RegisterFlyout<EditDogFlyout>( "EditDogFlyout", "FlyoutsRegion" );
        }
    }
}