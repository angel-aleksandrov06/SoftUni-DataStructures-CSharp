namespace _01.DogVet
{
    using System;
    using System.Collections.Generic;

    public class Dog : IComparable
    {
        public Dog(string id, string name, Breed breed, int age, int vaccines)
        {
            this.Id = id;
            this.Name = name;
            this.Breed = breed;
            this.Age = age;
            this.Vaccines = vaccines;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public Breed Breed { get; set; }

        public Owner Owner { get; set; }

        public int Age { get; set; }

        public int Vaccines { get; set; }

        public override bool Equals(object obj)
        {
            Dog other = (Dog)obj;
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }

        public int CompareTo(object obj)
        {
            Dog other = obj as Dog;

            int compare = this.Name.CompareTo(other.Name);

            return compare;
        }
    }
}