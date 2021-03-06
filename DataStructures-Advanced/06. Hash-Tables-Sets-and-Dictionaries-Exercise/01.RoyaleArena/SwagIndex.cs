﻿namespace _01.RoyaleArena
{
    using System;
    using System.Collections.Generic;

    public class SwagIndex : Index<double>
    {
        SortedSet<double> keys = new SortedSet<double>();

        public override Func<BattleCard, double> GetKey => (card) => card.Swag;

        protected override SortedSet<double> Keys => this.keys;
    }
}
