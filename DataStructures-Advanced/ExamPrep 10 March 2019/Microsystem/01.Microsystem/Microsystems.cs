namespace _01.Microsystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class Microsystems : IMicrosystem
    {
        private Dictionary<int, Computer> byNumber = new Dictionary<int, Computer>();
        private Dictionary<Brand, OrderedBag<Computer>> byBrand = new Dictionary<Brand, OrderedBag<Computer>>();
        private Dictionary<string, OrderedBag<Computer>> byColor = new Dictionary<string, OrderedBag<Computer>>();
        private Dictionary<double, OrderedBag<Computer>> byScreenSize = new Dictionary<double, OrderedBag<Computer>>();
        private OrderedBag<Computer> byRangePrice = new OrderedBag<Computer>(new sortPriceDescendingHelper());

        public bool Contains(int number)
        {
            return this.byNumber.ContainsKey(number);
        }

        public int Count()
        {
            return this.byNumber.Count();
        }

        public void CreateComputer(Computer computer)
        {
            if (this.byNumber.ContainsKey(computer.Number))
            {
                throw new ArgumentException();
            }

            this.byNumber.Add(computer.Number, computer);
            this.AddInByBrand(computer);
            this.AddInByColor(computer);
            this.AddInByScreenSize(computer);
            this.AddInByRangePrice(computer);
        }

        public IEnumerable<Computer> GetAllFromBrand(Brand brand)
        {
            if (!this.byBrand.ContainsKey(brand))
            {
                return Enumerable.Empty<Computer>();
            }
            else
            {
                return this.byBrand[brand];
            }
        }

        public IEnumerable<Computer> GetAllWithColor(string color)
        {
            if (!this.byColor.ContainsKey(color))
            {
                return Enumerable.Empty<Computer>();
            }
            else
            {
                return this.byColor[color];
            }
        }

        public IEnumerable<Computer> GetAllWithScreenSize(double screenSize)
        {
            if (!this.byScreenSize.ContainsKey(screenSize))
            {
                return Enumerable.Empty<Computer>();
            }
            else
            {
                return this.byScreenSize[screenSize];
            }
        }

        public Computer GetComputer(int number)
        {
            if (this.byNumber.ContainsKey(number))
            {
                return this.byNumber[number];
            }

            throw new ArgumentException();
        }

        public IEnumerable<Computer> GetInRangePrice(double minPrice, double maxPrice)
        {
            //return this.byNumber.Values.Where(x => x.Price >= minPrice && x.Price <= maxPrice).OrderByDescending(x => x.Price);

            var range = this.byRangePrice.Where(x => x.Price >= minPrice && x.Price <= maxPrice);

            if (range.Count() == 0)
            {
                return Enumerable.Empty<Computer>();
            }
            else
            {
                var list = new List<Computer>();

                foreach (var computer in range)
                {
                    list.Add(computer);
                }

                return list;
            }
        }

        public void Remove(int number)
        {
            if (!this.byNumber.ContainsKey(number))
            {
                throw new ArgumentException();
            }

            var computerForRemove = this.byNumber[number];

            this.byNumber.Remove(number);
            this.byBrand[computerForRemove.Brand].Remove(computerForRemove);
            if (this.byBrand[computerForRemove.Brand].Count ==0)
            {
                this.byBrand.Remove(computerForRemove.Brand);
            }
            this.byColor[computerForRemove.Color].Remove(computerForRemove);
            if (this.byColor[computerForRemove.Color].Count == 0)
            {
                this.byColor.Remove(computerForRemove.Color);
            }
            this.byScreenSize[computerForRemove.ScreenSize].Remove(computerForRemove);
            if (this.byScreenSize[computerForRemove.ScreenSize].Count == 0)
            {
                this.byScreenSize.Remove(computerForRemove.ScreenSize);
            }
            this.byRangePrice.Remove(computerForRemove);
        }

        public void RemoveWithBrand(Brand brand)
        {
            if (!this.byBrand.ContainsKey(brand))
            {
                throw new ArgumentException();
            }

            var computersForRemove = this.byBrand[brand].Select(c => c.Number).ToList();

            foreach (var computer in computersForRemove)
            {
                this.Remove(computer);
            }
        }

        public void UpgradeRam(int ram, int number)
        {
            if (!this.byNumber.ContainsKey(number))
            {
                throw new ArgumentException();
            }

            if (this.byNumber[number].RAM < ram)
            {
                this.byNumber[number].RAM = ram;
            }
        }

        private void AddInByBrand(Computer computer)
        {
            if (!this.byBrand.ContainsKey(computer.Brand))
            {
                this.byBrand[computer.Brand] = new OrderedBag<Computer>(new sortPriceDescendingHelper());
            }

            this.byBrand[computer.Brand].Add(computer);
        }

        private void AddInByColor(Computer computer)
        {
            if (!this.byColor.ContainsKey(computer.Color))
            {
                this.byColor[computer.Color] = new OrderedBag<Computer>(new sortPriceDescendingHelper());
            }
            this.byColor[computer.Color].Add(computer);
        }

        private void AddInByScreenSize(Computer computer)
        {
            if (!this.byScreenSize.ContainsKey(computer.ScreenSize))
            {
                this.byScreenSize[computer.ScreenSize] = new OrderedBag<Computer>(new sortNumberDescendingHelper());
            }
            this.byScreenSize[computer.ScreenSize].Add(computer);
        }

        private void AddInByRangePrice(Computer computer)
        {
            this.byRangePrice.Add(computer);
        }

        private class sortPriceDescendingHelper : IComparer<Computer>
        {
            public int Compare(Computer x, Computer y)
            {
                Computer c1 = (Computer)x;
                Computer c2 = (Computer)y;
                if (c1.Price < c2.Price)
                    return 1;
                if (c1.Price > c2.Price)
                    return -1;
                else
                    return 0;
            }
        }

        private class sortNumberDescendingHelper : IComparer<Computer>
        {
            public int Compare(Computer x, Computer y)
            {
                Computer c1 = (Computer)x;
                Computer c2 = (Computer)y;
                if (c1.Number < c2.Number)
                    return 1;
                if (c1.Number > c2.Number)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
