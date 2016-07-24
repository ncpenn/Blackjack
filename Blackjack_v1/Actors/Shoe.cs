using System;
using System.Collections.Generic;
using Blackjack.Errors;
using Blackjack.Interfaces;
using Blackjack.Models;

namespace Blackjack.Actors
{
    public class Shoe : IShoe
    {
        private readonly Percent _whenToShuffle;
        private readonly int _numberOfDecksInShoe;
        private List<uint> _loadedShoe;
        private readonly int _initialCountOfCardsInShoe;
        public bool NeedsToBeShuffled { get; private set; }
        public delegate void TimeToShuffle();
        public event TimeToShuffle AnnounceTimeToShuffle;

        public Shoe(int numberOfDecks, Percent whenToShuffle)
        {
            _whenToShuffle = whenToShuffle;
            NeedsToBeShuffled = false;
            _numberOfDecksInShoe = numberOfDecks;
            _initialCountOfCardsInShoe = _numberOfDecksInShoe * 4 * 13;
            LoadShoe();
        }

        public uint[] GiveMeSomeCards(int numberOfCardsRequested)
        {
            var requestedCards = new uint[numberOfCardsRequested];
            if (numberOfCardsRequested <= _loadedShoe.Count)
            {
                for (var i = 0; i < numberOfCardsRequested; i++)
                {
                    var card = _loadedShoe[0];
                    requestedCards[i] = card;
                    _loadedShoe.RemoveAt(0);
                }
                if (IsTimeToShuffle())
                {
                    AnnounceTimeToShuffle?.Invoke();
                }
                return requestedCards;
            }
            throw new TooManyCardsRequestedException("Cards requested exceeds cards available");
        }

        public void Shuffle()
        {
            LoadShoe();
            NeedsToBeShuffled = false;
        }

        private bool IsTimeToShuffle()
        {
            var percentOfShoeLeft = _loadedShoe.Count / (double) _initialCountOfCardsInShoe;
            return _loadedShoe.Count < 2 || 1 - _whenToShuffle.PercentValue >= percentOfShoeLeft;
        }

        private void LoadShoe()
        {
            _loadedShoe = new List<uint>();

            for (var i = 0; i < _numberOfDecksInShoe; i++)
            {
                var deck = new Deck();
                _loadedShoe.AddRange(deck.ReadyDeck);
            }
            ShuffleDecks(5);
        }

        private void ShuffleDecks(int numberOfTimesToShuffle)
        {
            for (var i = 0; i < numberOfTimesToShuffle; i++)
            {
                var tempShoe = new uint[_initialCountOfCardsInShoe];
                var listOfOccupiedLocationsInShoe = new HashSet<int>();

                var random = new Random();
                foreach (var card in _loadedShoe)
                {
                    int randomLocaltion;
                    do
                    {
                        randomLocaltion = random.Next(0, _initialCountOfCardsInShoe);
                    } while (!listOfOccupiedLocationsInShoe.Add(randomLocaltion));
                    tempShoe[randomLocaltion] = card;
                }
                _loadedShoe.Clear();
                _loadedShoe.AddRange(tempShoe);
            }
        }
    }
}
