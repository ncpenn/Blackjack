using System;
using Blackjack.Actors.Interfaces;
using System.Collections.Generic;

namespace Blackjack.Actors
{
    public class Shoe : IShoe
    {
        private double _whenToShuffle;
        private uint _numberOfDecks;
        private List<uint> _cardsInShoe;

        public bool ShuffleNeeded
        {
            get
            {
                return _cardsInShoe.Count / (double)(_numberOfDecks * 52) <= _whenToShuffle;
            }
        }

        public Shoe(uint numberOfDecks, double whenToShuffle)
        {
            _cardsInShoe = new List<uint>();
            _whenToShuffle = whenToShuffle;
            _numberOfDecks = numberOfDecks;
            Shuffle();
        }

        public uint CardRequest()
        {
            var card = _cardsInShoe[0];
            _cardsInShoe.RemoveAt(0);
            return card;
        }

        public uint[] CardRequest(int numberOfCardsRequested)
        {
            var cards = new uint[numberOfCardsRequested];
            for (uint i = 0; i < numberOfCardsRequested; i++)
            {
                cards[i] = CardRequest();
            }
            return cards;
        }

        public void Shuffle(uint numberOfTimesToShuffle = 5)
        {
            LoadShoe();
            for (uint i = 0; i < numberOfTimesToShuffle; i++)
            {
                var tempShoe = new uint[_cardsInShoe.Count];
                var listOfOccupiedLocationsInShoe = new HashSet<int>();

                var random = new Random();
                foreach (var card in _cardsInShoe)
                {
                    int randomLocaltion;
                    do
                    {
                        randomLocaltion = random.Next(0, _cardsInShoe.Count);
                    } while (!listOfOccupiedLocationsInShoe.Add(randomLocaltion));
                    tempShoe[randomLocaltion] = card;
                }
                _cardsInShoe.Clear();
                _cardsInShoe.AddRange(tempShoe);
            }
        }

        private void LoadShoe()
        {
            _cardsInShoe.Clear();
            for(uint numberOfDecks = 0; numberOfDecks < _numberOfDecks; numberOfDecks++)
            {
                for (uint suit = 0; suit < 4; suit++)
                {
                    for (uint card = 1; card <= 13; card++)
                    {
                        _cardsInShoe.Add(card);
                    }
                }
            }
        }
    }
}
