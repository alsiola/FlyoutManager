﻿using Prism.Mvvm;

namespace FlyoutManager.Example.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Bulldog FlyoutManager Demo Application";

        public string Title
        {
            get { return _title; }
            set { SetProperty( ref _title, value ); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
