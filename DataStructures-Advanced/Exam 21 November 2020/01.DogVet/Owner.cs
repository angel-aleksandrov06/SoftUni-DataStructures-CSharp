using System;
using System.Collections.Generic;

namespace _01.DogVet
{
    public class Owner : IComparable
    {
        public Owner(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Dogs = new Dictionary<string, Dog>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, Dog> Dogs { get; set; }

        public override bool Equals(object obj)
        {
            Owner other = (Owner)obj;
            if (other == null)
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }

        public int CompareTo(object obj)
        {
            Owner other = obj as Owner;

            int compare = this.Name.CompareTo(other.Name);
            if (compare == 0)
            {
                return this.Id.CompareTo(other.Id);
            }

            return compare;
        }
    }
}