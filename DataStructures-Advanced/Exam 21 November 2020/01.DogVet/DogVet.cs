namespace _01.DogVet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class DogVet : IDogVet
    {
        Dictionary<string, Dog> dogsById = new Dictionary<string, Dog>();
        Dictionary<string, Owner> ownerById = new Dictionary<string, Owner>();
        OrderedBag<Dog> byAge = new OrderedBag<Dog>(((x, y) => x.Age.CompareTo(y.Age)));

        public int Size => this.dogsById.Count;
        public void AddDog(Dog dog, Owner owner)
        {
            if (this.dogsById.ContainsKey(dog.Id))
            {
                throw new ArgumentException();
            }

            if (this.ownerById.ContainsKey(owner.Id))
            {
                if (this.ownerById[owner.Id].Dogs.ContainsKey(dog.Name))
                {
                    throw new ArgumentException();
                }
                else
                {
                    this.ownerById[owner.Id].Dogs.Add(dog.Name, dog);
                }
            }
            else
            {
                dog.Owner = owner;
                owner.Dogs.Add(dog.Name, dog);
                this.ownerById.Add(owner.Id, owner);
            }

            dog.Owner = owner;
            this.dogsById.Add(dog.Id, dog);
            this.byAge.Add(dog);
        }

        public bool Contains(Dog dog)
        {
            return this.dogsById.ContainsKey(dog.Id);
        }

        public Dog GetDog(string name, string ownerId)
        {
            if (!this.ownerById.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }
            Owner owner = this.ownerById[ownerId];

            if (!owner.Dogs.ContainsKey(name))
            {
                throw new ArgumentException();
            }

            return owner.Dogs[name];
        }

        public Dog RemoveDog(string name, string ownerId)
        {
            Dog dog = this.GetDog(name, ownerId);
            Owner owner = this.ownerById[ownerId];

            this.dogsById.Remove(dog.Id);
            this.byAge.Remove(dog);
            owner.Dogs.Remove(name);

            return dog;
        }

        public IEnumerable<Dog> GetDogsByOwner(string ownerId)
        {
            if (!this.ownerById.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }
            Owner owner = this.ownerById[ownerId];

            return this.ownerById[owner.Id].Dogs.Values;
        }

        public IEnumerable<Dog> GetDogsByBreed(Breed breed)
        {
            var result = this.dogsById.Values.Where(x => x.Breed == breed);

            if (!result.Any())
            {
                throw new ArgumentException();
            }

            return result;
        }

        public void Vaccinate(string name, string ownerId)
        {
            Dog dog = this.GetDog(name, ownerId);
            Owner owner = this.ownerById[ownerId];

            this.ownerById[owner.Id].Dogs[name].Vaccines++;
            // TODO: Vaccinate the dog in byBreed
            // TODO: Vaccinate the dog in byAge
        }

        public void Rename(string oldName, string newName, string ownerId)
        {
            if (!this.ownerById.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }

            Dog dog = this.GetDog(oldName, ownerId);
            Owner owner = this.ownerById[ownerId];

            owner.Dogs.Remove(oldName);
            dog.Name = newName;
            owner.Dogs.Add(newName, dog);
            this.dogsById[dog.Id].Name = newName;
            // TODO: Rename the dog in byBreed
            // TODO: Rename the dog in byAge
        }

        public IEnumerable<Dog> GetAllDogsByAge(int age)
        {
            var result = this.byAge.Where(x => x.Age == age);
            if (!result.Any())
            {
                throw new ArgumentException();
            }

            return result;
        }

        public IEnumerable<Dog> GetDogsInAgeRange(int lo, int hi)
        {
            var result = this.byAge.Range(new Dog(null, null, Breed.Bulldog, lo, 0), true, new Dog(null, null, Breed.Bulldog, hi, 0), true);

            if (result.Any())
            {
                return result;
            }

            return Enumerable.Empty<Dog>();
        }

        public IEnumerable<Dog> GetAllOrderedByAgeThenByNameThenByOwnerNameAscending()
        {
            var result = this.byAge.OrderBy(x => x.Age).ThenBy(x => x.Name).ThenBy(x => x.Owner.Name);

            if (!result.Any())
            {
                return Enumerable.Empty<Dog>();
            }

            return result;
        }
    }
}