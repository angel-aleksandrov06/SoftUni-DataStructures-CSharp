using System;
using Wintellect.PowerCollections;

namespace _04.CookiesProblem
{
    public class CookiesProblem
    {
        public int Solve(int k, int[] cookies)
        {
            var bag = new OrderedBag<int>();

            foreach (var cookie in cookies)
            {
                bag.Add(cookie);
            }

            var currMinSweetCookie = bag.GetFirst();
            int steps = 0;

            while (currMinSweetCookie < k && bag.Count > 1)
            {
                var minSweetCookie = bag.RemoveFirst();
                var beforeMinSweetCookie = bag.RemoveFirst();

                var newCookie = minSweetCookie + (2 * beforeMinSweetCookie);

                bag.Add(newCookie);

                currMinSweetCookie = bag.GetFirst();
                steps++;
            }

            return currMinSweetCookie < k ? -1 : steps;
        }
    }
}
