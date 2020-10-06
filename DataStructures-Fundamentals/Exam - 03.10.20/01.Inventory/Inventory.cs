namespace _01.Inventory
{
    using _01.Inventory.Interfaces;
    using _01.Inventory.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Inventory : IHolder
    {
        private List<IWeapon> weapons;

        public Inventory()
        {
            this.weapons = new List<IWeapon>();
        }

        public int Capacity => this.weapons.Count;

        public void Add(IWeapon weapon)
        {
            this.weapons.Add(weapon);
        }

        public void Clear()
        {
            this.weapons.Clear();
        }

        public bool Contains(IWeapon weapon)
        {
            return this.GetIndexById(weapon.Id) != -1;
        }

        public void EmptyArsenal(Category category)
        {
            for (int i = 0; i < this.Capacity; i++)
            {
                if (this.weapons[i].Category.Equals(category))
                {
                    this.weapons[i].Ammunition = 0;
                }
            }
        }

        public bool Fire(IWeapon weapon, int ammunition)
        {
            var index = this.GetIndexById(weapon.Id);
            this.ValidateIndex(index);

            var currAmmunition = this.weapons[index].Ammunition;

            if (currAmmunition >= ammunition)
            {
                this.weapons[index].Ammunition = currAmmunition - ammunition;
                return true;
            }

            return false;
        }

        public IWeapon GetById(int id)
        {
            var currIndex = this.GetIndexById(id);

            if (currIndex == -1)
            {
                return null;
            }
            else
            {
                var currElement = this.weapons[currIndex];
                return currElement;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.weapons.GetEnumerator();
        }

        public int Refill(IWeapon weapon, int ammunition)
        {
            var index = this.GetIndexById(weapon.Id);
            this.ValidateIndex(index);

            var maxCapacity = this.weapons[index].MaxCapacity;
            var currAmmunition = this.weapons[index].Ammunition;

            if (currAmmunition < maxCapacity)
            {
                if (currAmmunition + ammunition <= maxCapacity)
                {
                    this.weapons[index].Ammunition = currAmmunition + ammunition;
                    return this.weapons[index].Ammunition;
                }
                else
                {
                    this.weapons[index].Ammunition = maxCapacity;
                    return this.weapons[index].Ammunition;
                }
            }

            return this.weapons[index].Ammunition;
        }

        public IWeapon RemoveById(int id)
        {
            var currIndex = this.GetIndexById(id);
            this.ValidateIndex(currIndex);

            var currElement = this.weapons[currIndex];
            this.weapons.RemoveAt(currIndex);
            return currElement;
        }

        public int RemoveHeavy()
        {
            return this.weapons.RemoveAll(x => x.Category.Equals(Category.Heavy));
        }

        public List<IWeapon> RetrieveAll()
        {
            return new List<IWeapon>(this.weapons);
        }

        public List<IWeapon> RetriveInRange(Category lower, Category upper)
        {
            var result = new List<IWeapon>(this.Capacity);

            for (int i = 0; i < this.Capacity; i++)
            {
                if (this.weapons[i].Category >= lower && this.weapons[i].Category <= upper)
                {
                    result.Add(this.weapons[i]);
                }
            }

            return result;
        }

        public void Swap(IWeapon firstWeapon, IWeapon secondWeapon)
        {
            int indexOfFirstEntity = this.GetIndexById(firstWeapon.Id);
            this.ValidateIndex(indexOfFirstEntity);
            int indexOfSecondEntity = this.GetIndexById(secondWeapon.Id);
            this.ValidateIndex(indexOfSecondEntity);

            if (this.weapons[indexOfFirstEntity].Category == this.weapons[indexOfSecondEntity].Category)
            {
                var temp = this.weapons[indexOfFirstEntity];
                this.weapons[indexOfFirstEntity] = this.weapons[indexOfSecondEntity];
                this.weapons[indexOfSecondEntity] = temp;
            }
        }

        private int GetIndexById(int id)
        {
            for (int i = 0; i < this.Capacity; i++)
            {
                if (this.weapons[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        private void ValidateIndex(int currIndex)
        {
            if (currIndex < 0 || currIndex >= this.Capacity)
            {
                throw new InvalidOperationException("Weapon does not exist in inventory!");
            }
        }
    }
}
