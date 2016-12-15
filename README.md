# FlyoutManager
An MVVM-spirited manager for MahApps.Metro Flyouts used within the Prism framework.

FlyoutManager provides a mechanism for managing the Flyout control provided by the MahApps.Metro library within the Prism framework. Separation of concerns and loose coupling patterns are used to provide an easy to implement and extensible solution that fits well in an MVVM paradigm.

FlyoutManager depends on the Prism library, using Unity extensions although it could easily be modified to use an alternative DI container. Obviously MahApps.Metro is also a dependency of the project.

Available via Nuget Package Manager as Bulldog.FlyoutManager

# Overview
FlyoutManager is a model-view-viewmodel compliant way to manage Flyouts provided by MahApps.Metro within the Prism framework.  Use of the FlyoutManager maintains separation of concerns and loose coupling of classes.  Currently FlyoutManager must be used with Prism's Unity extensions.

# Dependencies
* [MahApps.Metro](http://mahapps.com/)
* [Prism](https://github.com/PrismLibrary/)

# Installation
Installation is extremely simple, just add a reference to Bulldog.FlyoutManager.dll to all projects that require it's functionality.  There is also a Nuget package available for download via Nuget Package Manager within VIsual Studio, named Bulldog.FlyoutManager

# Usage
*Register type with Unity Container*

Within the Bootstrapper class of your Prism application override the "ConfigureContainer" method, and register the IFlyoutManager interface with the Unity container, to return an instance of FlyoutManager.  Use the ContainerControlledLifetimeManager overload of the RegisterType method to let FlyoutManager behave as a singleton.
```c#
protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<IFlyoutManager, FlyoutManager>( new ContainerControlledLifetimeManager() );
            
        }
```

*Register RegionAdapterMapping for FlyoutsControl*

Still within the Bootstrapper class from above, override the ConfigureRegionAdapterMappings class, and register a Mapping that allows FlyoutsControlRegionAdapter to be the RegionAdapter for FlyoutControl.

```c#
protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping( typeof( FlyoutsControl ), Container.Resolve<FlyoutsControlRegionAdapter>() );

            return mappings;
        }
```

*Create a region where Flyouts will be displayed*

Within your Shell we need to create a region where the Flyouts will be displayed.  There is an included UserControl, FlyoutContainer, that serves this purpose.  First, add an xml namespace reference to the MetroWIndow as follow:

```xaml
<metro:MetroWindow x:Class="BaseNamespace.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:prism="http://prismlibrary.com/"
        xmlns:metro="http://metro.mahapps.com/winfx/xaml/controls"
        xmlns:bulldog="clr-namespace:Bulldog.FlyoutManager;assembly=Bulldog.FlyoutManager"
        prism:ViewModelLocator.AutoWireViewModel="True"
        Title="{Binding Title}">
```

Now add the FlyoutContainer, within the MetroWIndow.Flyouts property.
```xaml
<metro:MetroWindow.Flyouts>
        <bulldog:FlyoutContainer prism:RegionManager.RegionName="FlyoutsRegion" />
</metro:MetroWindow.Flyouts>
```

*Create your Flyout view*

Within the module that will display the flyout, create a view that will be injected into the Flyout.  It is easiest to use FlyoutManager if the view is responsible for creating its own viewmodel - this is best achieved using ViewModelLocator.AutoWireViewModel = "True".  For more details see the Prism documentation, or our sample application.
The view is a UserControl and can contain anything you like.

*Create the Flyout viewmodel*

The viewmodel must implement the IFlyout interface.  This is most easily achieved by inheriting from the FlyoutBase abstract class provided by the Bulldog.FlyoutManager assembly.  FlyoutBase inherits from Prism's BindableBase to achieve property binding.  It is not necessary to write any code within the viewmodel, but usually the Flyouts position, header and theme are set within the constructor.  
Position can be easily set using the FlyoutPosition class, and theme with the FlyoutTheme class, although both are simply strings.

```c#
    public class YourFlyoutViewModel : FlyoutBase
    {
        public YourFlyoutViewModel()
        {
             Header = "Your Flyout Header";
             Position = FlyoutPosition.Right;
             Theme = FlyoutTheme.Accent;
        }
    }    
```

*Register your Flyout with the FlyoutManager*

Within the Initialize method of your modules IModule class we need to register your Flyout with the FlyoutManager.  The FlyoutManager is accessed by injecting it into the constructor, and the Flyout registered using the generic RegisterFlyout method.  There are two overloads of this method detailed below.

```c#
FlyoutManager.RegisterFlyout<FlyoutView>( string FlyoutName, string FlyoutRegionName); 
```
Use this method signature when your view creates its own viewmodel, such as through ViewModelLocator.AutoWireViewModel.
Here, the type parameter "FlyoutView" is the type of your Flyout view.  FlyoutName is a string that serves as a key to identify this flyout - there are no restrictions on its content but it should be unique Flyout.  FlyoutRegionName is the name of the region where the Flyouts will be displayed, as we created eariler in our Shell.xaml.

```c#
FlyoutManager.RegisterFlyout<FlyoutView>( string FlyoutName, string FlyoutRegionName, IFlyout FlyoutViewModel); 
```
Use this method signature if you must manually create your own ViewModel for the Flyout view.
The final parameter of the method requires an instance of a ViewModel that implements the IFlyout interface.  It is your responsibility to ensure association between View and ViewModel, i.e. setting the DataContext of the View.

With both methods it is not necessary for you to register the Flyout View with a Prism region - this is taken care of by FlyoutManager.

*Show Your Flyout!*

We are now setup and ready to use our Flyout.  This is achieved by calling the FlyoutManager.OpenFlyout method.  Usually this is done from another ViewModel, after injecting FlyoutManager via its constructor.
There are several overloads of OpenFlyout (see the Advanced Features section), but the simplest just requires you to pass a string representing the unique FlyoutName we used when registering the Flyout earlier.

```c#
public class AnotherViewModel
    {
        IFlyoutManager _flyoutManager;

        public AnotherViewModel(IFlyoutManager flyoutManager)
        {
            _flyoutManager = flyoutManager;
        }

        private void OpenEditClientFlyout()
        {
            _flyoutManager.OpenFlyout( FlyoutName );
        }

        // other ViewModel code
    }
```

*Close Your Flyout*

Once you are done with it, the Flyout can be closed programatically, as well as by the user clicking on its close arrow.  If you wish to close the Flyout programatically then just as we did with opening the Flyout, call the FlyoutManager.CloseFlyout(string FlyoutName) method.


# Advanced Features
*Controlling Flyout Actions*

FlyoutManager allows individual Flyouts to control their own opening and closing.  Within the Flyouts ViewModel we can override the CanOpen and CanClose methods as show below:

```c#
public override bool CanOpen( FlyoutParameters flyoutParameters )
        {
              if( theFlyoutCanOpen() )   // Use any logic you like to determine whether the Flyout can open
                  return true;
              else
                  return false;
        }
```

The CanClose method has the same signature.  These methods return a boolean representing whether the Flyout is able to open - for example, it may check if the ViewModel has necessary properties set.  The methods receive an instance of FlyoutParameters, which can be used, if required, to determine whether the Flyout can open.  For more information on FlyoutParameters see below.
NB. The result of CanOpen/CanClose may be ignored if you have requested Flyout Open/Close with the forceOpen parameter set to true - for more information see CanOpen Overloads below.

*Responding To Flyout Actions*

Every ViewModel implementing IFlyout has three public events:

* OnOpening
* OnClosed
* OnOpenChanged

These events are fairly self-explanatory. They are invoked with an instance of FlyoutEventArgs - see below.

*FlyoutEventArgs*

When the above events are fired, an instance of FlyoutEventArgs is passed to all event listeners.  FlyoutEventArgs derives from System.EventArgs and adds a single property - FlyoutAction.  FlyoutAction has two possible values:

* FlyoutAction.Opening
* FlyoutAction.Closing

*FlyoutParameters - Passing data to Flyouts*

It is often necessary to pass some data to the Flyout before it opens.  For example, the Flyout might contain a means to edit an object, and the object must be passed to the Flyout to populate its fields.  This is achieved through FlyoutParameters.  FlyoutParameters is basically a wrapper for an IDictionary - it can contain a collection of objects, indexed by a string key. It is passed from the method that calls OpenFlyout to the Flyout as below:

```c#
// Within ViewModel opening the Flyout

    var flyoutparameters = new FlyoutParameters();
    flyoutparameters["payloadKey"] = payloadObject;
    FlyoutManager.OpenFlyout( flyoutName, flyoutparameters );
```

The FlyoutParameters object is passed to the CanOpen method of the Flyout, and if the Flyout is subsequently opened, it can be retrieved within the Flyout by overriding the OnOpening method, as below.  NB. Payloads are weakly typed so it is likely to be necessary to cast them to their desired type.

```c#
// Within Flyout ViewModel

protected override void OnOpening( FlyoutParameters flyoutParameters )
        {
            base.OnOpening( flyoutParameters );

            var incomingPayload= flyoutParameters["payloadKey"] as payloadType;

           // do something with payload
        }
```

The situation for closing Flyouts is entirely analogous.  FlyoutParameters can be passed to the FlyoutManager.CloseFlyout method - they are then passed to the Flyouts CanClose method, and if applicable the OnClosing method, both of which can be overridden if required.

*Forcing Flyout Actions*

On some occasions we may wish to force a Flyout to open or close, ignoring the response from its CanOpen/CanClose method.  We can use overloaded versions of the FlyoutManager.OpenFlyout and CloseFlyout to achieve this.  These methods will always return the response of CanOpen/CanClose, even if they have been told to ignore it.

```c#
FlyoutManager.OpenFlyout(string flyoutName); 
```

Tries to open the specified Flyout, respecting the response of its CanOpen method.

```c#
FlyoutManager.OpenFlyout(string flyoutName, FlyoutParameters parameters); 
```
Tries to open the specified Flyout, passing the supplied FlyoutParameters to the CanOpen method and respecting its response.  If CanOpen returns true then the same FlyoutParameters are passed to the Flyouts OnOpening method.

```c#
FlyoutManager.OpenFlyout(string flyoutName, bool forceOpen); 
```
Opens the specified Flyout, ignoring the response of its CanOpen method.

```c#
FlyoutManager.OpenFlyout(string flyoutName, FlyoutParameters parameters, bool forceOpen); 
```
Opens the specified Flyout, ignoring the response of its CanOpen method. The provided FlyoutParameters are sent to both the Flyout's CanOpen and OnOpening methods.  The method will return the response of the Flyout's CanOpen method even if it is ignored. I.E. If forceOpen is set to true, and the Flyout's CanOpen method returns false, OpenFlyout will return false even though the Flyout is opened.

*UnRegistering Flyouts*
Flyouts can be removed from the FlyoutManager by calling the FlyoutManager.UnRegisterFlyout method, passing either a string representing the Flyout's name, or the instance of the FlyoutViewModel controlling it.
