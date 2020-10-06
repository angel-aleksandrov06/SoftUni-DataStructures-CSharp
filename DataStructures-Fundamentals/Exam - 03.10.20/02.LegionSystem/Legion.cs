namespace _02.LegionSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using _02.LegionSystem.Interfaces;
    using Wintellect.PowerCollections;

    public class Legion : IArmy
    {
        private OrderedSet<IEnemy> enemies;

        public Legion()
        {
            this.enemies = new OrderedSet<IEnemy>();
        }

        public int Size => this.enemies.Count;

        public bool Contains(IEnemy enemy)
        {
            return this.enemies.Contains(enemy);
        }

        public void Create(IEnemy enemy)
        {
            this.enemies.Add(enemy);
        }

        public IEnemy GetByAttackSpeed(int speed)
        {
            var indexOfEnemy = this.GetIndexByAttSpeed(speed);

            if (indexOfEnemy == -1)
            {
                return null;
            }

            return this.enemies[indexOfEnemy];
        }

        public List<IEnemy> GetFaster(int speed)
        {
            //var result = new List<IEnemy>(this.Size);

            var result2 = this.enemies.FindAll(x => x.AttackSpeed > speed).ToList();

            //for (int i = 0; i < this.Size; i++)
            //{
            //    if (this.enemies[i].AttackSpeed > speed)
            //    {
            //        result.Add(this.enemies[i]);
            //    }
            //    else
            //    {
            //        return result;
            //    }
            //}

            return result2;
        }

        public IEnemy GetFastest()
        {
            this.EnsureNotEmpty();
            return this.enemies.GetFirst();
        }

        public IEnemy[] GetOrderedByHealth()
        {
            if (this.Size == 0)
            {
                return new IEnemy[0];
            }

            List<IEnemy> orderedByHealth = MergeSort(new List<IEnemy>(this.enemies));


            return orderedByHealth.ToArray();
        }

        public List<IEnemy> GetSlower(int speed)
        {
            var result2 = this.enemies.FindAll(x => x.AttackSpeed < speed).ToList();

            //var result = new List<IEnemy>(this.Size);

            //for (int i = this.Size - 1; i >= 0; i--)
            //{
            //    if (this.enemies[i].AttackSpeed < speed)
            //    {
            //        result.Add(this.enemies[i]);
            //    }
            //    else
            //    {
            //        return result;
            //    }
            //}

            return result2;
        }

        public IEnemy GetSlowest()
        {
            this.EnsureNotEmpty();
            return this.enemies.GetLast();
        }

        public void ShootFastest()
        {
            this.EnsureNotEmpty();
            this.enemies.RemoveFirst();
        }

        public void ShootSlowest()
        {
            this.EnsureNotEmpty();
            this.enemies.RemoveLast();
        }

        private int GetIndexByAttSpeed(int speed)
        {
            for (int i = 0; i < this.Size; i++)
            {
                if (this.enemies[i].AttackSpeed == speed)
                {
                    return i;
                }
            }

            return -1;
        }

        private void EnsureNotEmpty()
        {
            if (this.Size == 0)
            {
                throw new InvalidOperationException("Legion has no enemies!");
            }
        }

        private static List<IEnemy> MergeSort(List<IEnemy> unsorted)
        {
            if (unsorted.Count <= 1)
                return unsorted;

            List<IEnemy> left = new List<IEnemy>();
            List<IEnemy> right = new List<IEnemy>();

            int middle = unsorted.Count / 2;
            for (int i = 0; i < middle; i++)  //Dividing the unsorted list
            {
                left.Add(unsorted[i]);
            }
            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }

        private static List<IEnemy> Merge(List<IEnemy> left, List<IEnemy> right)
        {
            List<IEnemy> result = new List<IEnemy>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First().Health >= right.First().Health)  //Comparing First two elements to see which is smaller
                    {
                        result.Add(left.First());
                        left.Remove(left.First());      //Rest of the list minus the first element
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    result.Add(right.First());

                    right.Remove(right.First());
                }
            }
            return result;
        }
    }
}
