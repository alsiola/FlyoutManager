using Bulldog.FlyoutManager;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DogsModule.ViewModels
{
    public class EditDogFlyoutViewModel : FlyoutBase
    {
        Dog currentDog;

        public EditDogFlyoutViewModel()
        {
            Position = FlyoutPosition.Right;
            Theme = FlyoutTheme.Accent;
            
        }

        protected override void OnOpening( FlyoutParameters flyoutParameters )
        {
            // Because FlyoutParameters provides weakly-typed objects we need to cast the provided "dog" parameter as a Dog type
            Dog = flyoutParameters["dog"] as Dog;

            // We can set the Flyout name based on information passed via flyoutParameters
            Header = "Editing " + Dog.Name;

            // As well as setting the position
            if( Dog.Name == "Patch" )
                Position = FlyoutPosition.Left;
            else
                Position = FlyoutPosition.Right;

            // And any other property you like.  See the full list in the code wiki at flyoutmanager.codeplex.com
        }

        public Dog Dog
        {
            get { return currentDog; }
            set { SetProperty( ref currentDog, value ); }
        }
    }
}
