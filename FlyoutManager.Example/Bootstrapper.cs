using Microsoft.Practices.Unity;
using Prism.Unity;
using FlyoutManager.Example.Views;
using System.Windows;
using Bulldog.FlyoutManager;
using Prism.Regions;
using MahApps.Metro.Controls;
using Prism.Modularity;
using DogsModule;

namespace FlyoutManager.Example
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog) this.ModuleCatalog;

            moduleCatalog.AddModule( typeof( DogsModuleModule ) );
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<IFlyoutManager, Bulldog.FlyoutManager.FlyoutManager>( new ContainerControlledLifetimeManager() );
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping( typeof( FlyoutsControl ), Container.Resolve<FlyoutsControlRegionAdapter>() );

            return mappings;
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
