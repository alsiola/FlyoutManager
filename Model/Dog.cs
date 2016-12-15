using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Dog : BindableBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty( ref name, value ); }
        }

        private string breed;
        public string Breed
        {
            get { return breed; }
            set { SetProperty( ref breed, value ); }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set { SetProperty( ref age, value ); }
        }
    }
}
