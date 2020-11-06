namespace _01.RoyaleArena
{
    using System;
    using System.Collections.Generic;

    public class DamageIndex : Index<double>
    {
        SortedSet<double> keys = new SortedSet<double>();

        public override Func<BattleCard, double> GetKey => (card) => card.Damage;

        protected override SortedSet<double> Keys => this.keys;

        public override int Compare(BattleCard x, BattleCard y)
        {
            int compare = base.Compare(x, y);

            if (compare == 0)
            {
                compare = x.Id.CompareTo(y.Id);
            }

            return compare;
        }

    }
}
