using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DogService
    {
        IList<Dog> dogList;

        public DogService()
        {
            dogList = new List<Dog>()
            {
                new Dog() {Name = "Poochie", Breed="Poodle", Age=3 },
                new Dog() {Name = "Lassie", Breed="Collie", Age=8 },
                new Dog() {Name = "Rex", Breed="Labrador", Age=1 },
                new Dog() {Name = "Patch", Breed="Bulldog", Age=5 }
            };
        }

        public IList<Dog> GetDogs()
        {
            return dogList;
        }

        public Dog GetDog(string Name)
        {
            return dogList.Where( d => d.Name == Name ).FirstOrDefault();
        }

        public void AddDog(Dog dog)
        {
            dogList.Add( dog );
        }
    }
}
