using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Bulldog.FlyoutManager
{
    /// <summary>
    /// Manages a collection of Flyouts, registering them with specified regions and creating their views and viewmodels.
    /// Flyouts that are managed must have viewmodels that implement IFlyout.
    /// </summary>
    public class FlyoutManager : IFlyoutManager
    {
        IDictionary<string, IFlyout> flyouts;

        IUnityContainer _unityContainer;

        string defaultFlyoutRegion;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="unitycontainer">Unity container, generally passed by dependency injection.</param>
        /// <param name="iregionManager">Region manager, generally passed by dependency injection.</param>
        public FlyoutManager( IUnityContainer unitycontainer, IRegionManager iregionManager )
        {
            _unityContainer = unitycontainer;
            RegionManager = iregionManager;
            flyouts = new Dictionary<string, IFlyout>();
        }

        public void SetDefaultFlyoutRegion( string regionName )
        {
            defaultFlyoutRegion = regionName;
        }

        /// <summary>
        /// The region manager controlling the region where the Flyouts are to be displayed.
        /// Made public to allow use of sub-region managers.
        /// </summary>
        public IRegionManager RegionManager { get; set; }

        public void RegisterFlyoutWithDefaultRegion<TView>(string flyoutKey) where TView : FrameworkElement
        {
            if( String.IsNullOrEmpty( defaultFlyoutRegion ) )
                throw new NullReferenceException( "Default region not set." );
            RegisterFlyout<TView>( flyoutKey, defaultFlyoutRegion );
        }

        public void RegisterFlyoutWithDefaultRegion<TView>( string flyoutKey, IFlyout viewModel ) where TView : FrameworkElement
        {
            if( String.IsNullOrEmpty( defaultFlyoutRegion ) )
                throw new NullReferenceException( "Default region not set." );
            RegisterFlyout<TView>( flyoutKey, defaultFlyoutRegion, viewModel );
        }

        /// <summary>
        /// Register Flyout with FlyoutManager.  The viewmodel will be obtained from the view, so a view-first approach must
        /// be used.  This method is compatible with autowireviewmodel.  The viewmodel produced must derive from IFlyout.
        /// </summary>
        /// <typeparam name="TView">Type of the view used for the flyout.</typeparam>
        /// <param name="flyoutKey">A key to identify the flyout.</param>
        /// <param name="flyoutRegion">The region name in which to display the flyout.</param>
        public void RegisterFlyout<TView>( string flyoutKey, string flyoutRegion ) where TView : FrameworkElement
        {
            var flyoutView = ResolveFlyoutView<TView>();

            var flyoutViewModel = GetFlyoutViewModelFromView( flyoutView );

            AddFlyoutToCollection( flyoutKey, flyoutRegion, flyoutViewModel, flyoutView );
        }

        /// <summary>
        /// Register Flyout with FlyoutManager, a viewmodel must be supplied that derives from IFlyout.
        /// </summary>
        /// <typeparam name="TView">Type of the view used for the flyout.</typeparam>
        /// /// <param name="flyoutKey">A key to identify the flyout.</param>
        /// <param name="flyoutRegion">The region name in which to display the flyout.</param>
        /// <param name="flyoutViewModel"></param>
        public void RegisterFlyout<TView>( string flyoutKey, string flyoutRegion, IFlyout flyoutViewModel ) where TView : FrameworkElement
        {
            var flyoutView = ResolveFlyoutView<TView>();

            AddFlyoutToCollection( flyoutKey, flyoutRegion, flyoutViewModel, flyoutView );
        }

        /// <summary>
        /// Remove the specified Flyout from this FlyoutManager.
        /// </summary>
        /// <param name="key">The key that identifies the flyout.</param>
        public void UnRegisterFlyout( string key )
        {
            flyouts.Remove( key );
        }

        /// <summary>
        /// Remove the specified Flyout from this FlyoutManager
        /// </summary>
        /// <param name="flyout">The flyout to remove.</param>
        public void UnRegisterFlyout( IFlyout flyout )
        {
            var items = flyouts.Where( kvp => kvp.Value == flyout ).ToList();
            items.ForEach( item => UnRegisterFlyout( item.Key ) );
        }
        /// <summary>
        /// Attempt to open the identified flyout, passing default flyoutparameters to the flyout's CanOpen method.
        /// </summary>
        /// <param name="key">Identifies the flyout to open.</param>
        /// <returns>True if opened, false if CanOpen prevented opening.</returns>
        public bool OpenFlyout( string key )
        {
            return OpenFlyout( key, new FlyoutParameters() );
        }

        /// <summary>
        /// Attempt to open the identified flyout, passing the specified flyoutparameters to the flyout's CanOpen method.
        /// </summary>
        /// <param name="key">Identifies the flyout to open.</param>
        /// <param name="flyoutParameters">Passed to the flyouts CanOpen method, and indirectly to OnOpening.</param>
        /// <returns>True if opened, false if CanOpen prevented opening.</returns>
        public bool OpenFlyout( string key, FlyoutParameters flyoutParameters )
        {
            return OpenFlyout( key, flyoutParameters, false );
        }

        /// <summary>
        /// Attempts to open the identified flyout
        /// Default flyoutparameters are passed to the flyout's CanOpen method and the result returned,
        /// but if forceOpen is true, the flyout is opened even if CanOpen returns false.
        /// </summary>
        /// <param name="key">Identifies the flyout to open.</param>
        /// <param name="forceOpen">Whether to force open the flyout.</param>
        /// <returns>The result of the identified flyout's CanOpen method.</returns>
        public bool OpenFlyout( string key, bool forceOpen )
        {
            return OpenFlyout( key, new FlyoutParameters(), forceOpen );
        }

        /// <summary>
        /// Attempts to open the identified flyout
        /// The specified flyoutparameters are passed to the flyout's CanOpen method and the result returned,
        /// but if forceOpen is true, the flyout is opened even if CanOpen returns false.
        /// </summary>
        /// <param name="key">Identifies the flyout to open.</param>
        /// <param name="flyoutParameters">Passed to the flyouts CanOpen method, and indirectly to OnOpening.</param>
        /// <param name="forceOpen">Whether to force open the flyout.</param>
        /// <returns>The result of the identified flyout's CanOpen method.</returns>
        public bool OpenFlyout( string key, FlyoutParameters flyoutParameters, bool forceOpen )
        {
            var flyoutToActivate = flyouts[key];
            bool canOpen = flyoutToActivate.CanOpen( flyoutParameters );

            if( !forceOpen && !canOpen )
                return false;

            flyoutToActivate.Open( flyoutParameters );

            return canOpen;
        }

        /// <summary>
        /// Attempt to close the identified flyout, passing default flyoutparameters to the flyout's CanClose method.
        /// </summary>
        /// <param name="key">Identifies the flyout to close.</param>
        /// <param name="flyoutParameters">Passed to the flyouts CanClose method, and indirectly to OnClosing.</param>
        /// <returns>True if closed, false if CanClose prevented closing.</returns>
        public bool CloseFlyout( string key )
        {
            return CloseFlyout( key, new FlyoutParameters() );
        }

        /// <summary>
        /// Attempt to close the identified flyout, passing the provided flyoutparameters to the flyout's CanClose method.
        /// </summary>
        /// <param name="key">Identifies the flyout to close.</param>
        /// <param name="flyoutParameters">Passed to the flyouts CanClose method, and indirectly to OnClosing.</param>
        /// <returns>True if closed, false if CanClose prevented closing.</returns>
        public bool CloseFlyout( string key, FlyoutParameters flyoutParameters )
        {
            return CloseFlyout( key, flyoutParameters, false );
        }

        /// <summary>
        /// Closes the identified flyout, passing default flyoutparameters to the flyout's CanClose method.
        /// If forceClose is true, the flyout will be closed even if CanClose returns false.  The result of CanClose
        /// will be returned by this method even if closure is forced.
        /// </summary>
        /// <param name="key">Identifies the flyout to close.</param>
        /// <param name="forceClose">Force flyout closure even if CanClose returns false.</param>
        /// <returns>The results of the indentified flyouts CanClose method.</returns>
        public bool CloseFlyout( string key, bool forceClose )
        {
            return CloseFlyout( key, new FlyoutParameters(), forceClose );
        }

        /// <summary>
        /// Closes the identified flyout, passing the provided flyoutparameters to the flyout's CanClose method.
        /// If forceClose is true, the flyout will be closed even if CanClose returns false.  The result of CanClose
        /// will be returned by this method even if closure is forced.
        /// </summary>
        /// <param name="key">Identifies the flyout to close.</param>
        /// <param name="flyoutParameters">Passed to the flyouts CanClose method, and indirectly to OnClosing.</param>
        /// <param name="forceClose">Force flyout closure even if CanClose returns false.</param>
        /// <returns>The results of the indentified flyouts CanClose method.</returns>
        public bool CloseFlyout( string key, FlyoutParameters flyoutParameters, bool forceClose )
        {
            var flyoutToClose = flyouts[key];
            bool canClose = flyoutToClose.CanClose( flyoutParameters );

            if( !forceClose && !canClose )
                return false;

            flyoutToClose.Open( flyoutParameters );

            return canClose;
        }

        private IFlyout GetFlyoutViewModelFromView( FrameworkElement flyoutView )
        {
            IFlyout flyoutViewModel = flyoutView.DataContext as IFlyout;

            if( flyoutViewModel == null )
                throw new ArgumentException( @"Type passed must have an auto-wired view model that implements IFlyout.
                                               If auto-wiring is not used then pass the viewmodel instance to the overloaded
                                               RegisterFlyout method." );

            return flyoutViewModel;
        }

        private FrameworkElement ResolveFlyoutView<T>()
        {
            return _unityContainer.Resolve<T>() as FrameworkElement;
        }

        private void RegisterFlyoutWithRegion( FrameworkElement flyoutView, string flyoutRegion )
        {
            RegionManager.RegisterViewWithRegion( flyoutRegion, () =>
            {
                return flyoutView;
            } );
        }

        private void AddFlyoutToCollection( string flyoutKey, string flyoutRegion, IFlyout flyoutViewModel, FrameworkElement flyoutView )
        {
            RegisterFlyoutWithRegion( flyoutView, flyoutRegion );

            flyouts.Add( flyoutKey, flyoutViewModel );
        }
    }
}
