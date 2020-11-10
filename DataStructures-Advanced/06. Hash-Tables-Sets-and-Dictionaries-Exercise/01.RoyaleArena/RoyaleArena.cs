namespace _01.RoyaleArena
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class RoyaleArena : IArena
    {
        private Dictionary<int, BattleCard> cards;
        private Dictionary<CardType, Table<BattleCard>> cardTypeSortedByDamage;
        private Dictionary<string, Table<BattleCard>> cardNameSortedBySawg;
        private Table<BattleCard> cardsSortedBySwag;

        public RoyaleArena()
        {
            this.cards = new Dictionary<int, BattleCard>();
            this.cardTypeSortedByDamage = new Dictionary<CardType, Table<BattleCard>>();
            this.cardNameSortedBySawg = new Dictionary<string, Table<BattleCard>>();
            this.cardsSortedBySwag = new Table<BattleCard>(new SwagIndex());
        }

        public void Add(BattleCard card)
        {
            if (!this.Contains(card))
            {
                this.cards[card.Id] = card;
            }

            this.AddToSearchableCollection<DamageIndex>(this.cardTypeSortedByDamage, card, c => c.Type);
            this.AddToSearchableCollection<SwagIndex>(this.cardNameSortedBySawg, card, c => c.Name);
            this.cardsSortedBySwag.Add(card);
        }

        private void AddToSearchableCollection<T>(IDictionary dictionary, BattleCard card, Func<BattleCard, object> getKey)
            where T : Index<double>, new()
        {
            var key = getKey(card);

            if (dictionary[key] == null)
            {
                dictionary[key] = new Table<BattleCard>(new T());
            }

            (dictionary[key] as Table<BattleCard>).Add(card);
        }

        public bool Contains(BattleCard card)
        {
            return this.cards.ContainsKey(card.Id);
        }

        public int Count => this.cards.Count;

        public void ChangeCardType(int id, CardType type)
        {
            this.CardNotExistException(id);

            BattleCard card = this.cards[id];
            this.RemoveFromSearchableCollection(this.cardTypeSortedByDamage, card, c => c.Type);
            card.Type = type;
            this.AddToSearchableCollection<DamageIndex>(this.cardTypeSortedByDamage, card, c => c.Type);
        }

        public BattleCard GetById(int id)
        {
            this.CardNotExistException(id);

            return this.cards[id];
        }

        private void CardNotExistException(int id)
        {
            if (!this.cards.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }
        }

        public void RemoveById(int id)
        {
            this.CardNotExistException(id);
            BattleCard card = this.cards[id];

            this.RemoveFromSearchableCollection(this.cardTypeSortedByDamage, card, c => c.Type);
            this.RemoveFromSearchableCollection(this.cardNameSortedBySawg, card, c => c.Name);
            this.cardsSortedBySwag.Remove(card);
            this.cards.Remove(id);
        }

        private void RemoveFromSearchableCollection(IDictionary dictionary, BattleCard card, Func<BattleCard, object> getKey)
        {
            var key = getKey(card);

            (dictionary[key] as Table<BattleCard>).Remove(card);
            if (!(dictionary[key] as Table<BattleCard>).Any())
            {
                dictionary.Remove(key);
            }
        }

        public IEnumerable<BattleCard> GetByCardType(CardType type)
        {
            if (!this.cardTypeSortedByDamage.ContainsKey(type))
            {
                throw new InvalidOperationException();
            }

            IEnumerable<BattleCard> cardsByType = this.cardTypeSortedByDamage[type];

            this.CollectionEmtyException(cardsByType);

            return cardsByType;
        }

        public IEnumerable<BattleCard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
        {
            if (!this.cardTypeSortedByDamage.ContainsKey(type))
            {
                throw new InvalidOperationException();
            }

            IEnumerable<BattleCard> cardsByTypeInDamageRange =
                this.cardTypeSortedByDamage[type]
                .GetViewBetween(lo, hi)
                .OrderBy(c => c);

            this.CollectionEmtyException(cardsByTypeInDamageRange);

            return cardsByTypeInDamageRange;
        }

        private void CollectionEmtyException(IEnumerable<BattleCard> battleCards)
        {
            if (!battleCards.Any())
            {
                throw new InvalidOperationException();
            }
        }

        public IEnumerable<BattleCard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
        {
            if (!this.cardTypeSortedByDamage.ContainsKey(type))
            {
                throw new InvalidOperationException();
            }

            IEnumerable<BattleCard> cardsByTypeAndMaxDamage = this.cardTypeSortedByDamage[type]
                .Where(c => c.Damage <= damage)
                .OrderBy(c => c);

            this.CollectionEmtyException(cardsByTypeAndMaxDamage);

            return cardsByTypeAndMaxDamage;
        }

        public IEnumerable<BattleCard> GetByNameOrderedBySwagDescending(string name)
        {
            if (!this.cardNameSortedBySawg.ContainsKey(name))
            {
                throw new InvalidOperationException();
            }

            IEnumerable<BattleCard> cardsByName = this.cardNameSortedBySawg[name];

            this.CollectionEmtyException(cardsByName);

            return cardsByName;
        }

        public IEnumerable<BattleCard> GetByNameAndSwagRange(string name, double lo, double hi)
        {
            if (!this.cardNameSortedBySawg.ContainsKey(name))
            {
                throw new InvalidOperationException();
            }

            IEnumerable<BattleCard> cardsByNameInSwagRange = this.cardNameSortedBySawg[name]?.GetViewBetween(lo, hi);

            this.CollectionEmtyException(cardsByNameInSwagRange);

            return cardsByNameInSwagRange;
        }

        public IEnumerable<BattleCard> FindFirstLeastSwag(int n)
        {
            if (n > this.Count)
            {
                throw new InvalidOperationException();
            }

            IEnumerable<BattleCard> cardsByLeastSwag = this.cardsSortedBySwag.GetFirstN(n, c => c.Id);

            return cardsByLeastSwag;
        }

        public IEnumerable<BattleCard> GetAllInSwagRange(double lo, double hi)
        {
            IEnumerable<BattleCard> cardsInSwagRange = this.cardsSortedBySwag.GetViewBetween(lo, hi);

            return cardsInSwagRange;
        }


        public IEnumerator<BattleCard> GetEnumerator()
        {
            return this.cards.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}