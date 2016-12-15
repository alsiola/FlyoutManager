using Bulldog.FlyoutManager;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace DogsModule.ViewModels
{
    public class DogListViewModel : BindableBase
    {
        IFlyoutManager _flyoutManager;

        DogService dogService;

        IList<Dog> dogList;
        ICollectionView dogListView;

        DelegateCommand<Dog> editDogCommand;

        public DogListViewModel(IFlyoutManager flyoutManager)
        {
            _flyoutManager = flyoutManager;

            dogService = new DogService();

            dogList = dogService.GetDogs();

            dogListView = CollectionViewSource.GetDefaultView( dogList );

            editDogCommand = new DelegateCommand<Dog>( TriggerOpenFlyout );
        }

        private void TriggerOpenFlyout( Dog obj )
        {
            FlyoutParameters flyoutParameters = new FlyoutParameters();
            flyoutParameters["dog"] = obj;

            _flyoutManager.OpenFlyout( "EditDogFlyout", flyoutParameters );
        }

        public ICollectionView DogList
        {
            get { return dogListView; }
        }

        public DelegateCommand<Dog> EditDogCommand
        {
            get { return editDogCommand; }
        }
    }
}
