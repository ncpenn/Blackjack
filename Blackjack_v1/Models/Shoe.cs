using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack_v1
{
    public class Shoe
    {
        private int numberOfDecksInShoe;
        public List<Card> LoadedShoe;
        private readonly int initialCountOfCardsInShoe;
        private readonly Percent whenToShuffle;

        public Shoe(int numberOfDecks, Percent whenToShuffle)
        {
            numberOfDecksInShoe = numberOfDecks;
            LoadShoe();
            initialCountOfCardsInShoe = LoadedShoe.Count;
            this.whenToShuffle = whenToShuffle;
        }

        public IEnumerable<Card> GiveMeSomeCards(int numberOfCardsRequested)
        {
            if (CheckCardsLeftInShoe(numberOfCardsRequested))
            {
                var results = LoadedShoe.Take(numberOfCardsRequested);
                LoadedShoe.RemoveRange(0, numberOfCardsRequested);
                return results;
            }
            throw new TooManyCardsRequestedException("Cards requested exceeds cards available");
        }

        private void LoadShoe()
        {
            LoadedShoe = new List<Card>();

            for (var i = 0; i < numberOfDecksInShoe; i++)
            {
                var deck = new Deck();
                LoadedShoe.AddRange(deck.ReadyDeck);
            }
            ShuffleDecks(5);
        }

        private bool CheckCardsLeftInShoe(int numberOfCards)
        {
            var percentOfCardsLeft = (double)LoadedShoe.Count / initialCountOfCardsInShoe;
            var percentWhenToShuffle = whenToShuffle.PercentValue / (double)100;
            if (percentOfCardsLeft < percentWhenToShuffle)
            {
                LoadShoe();
            }

            if (numberOfCards < LoadedShoe.Count)
            {
                return true;
            }
            return false;
        }

        private void ShuffleDecks(int numberOfTimesToShuffle)
        {
            for (var i = 0; i < numberOfTimesToShuffle; i++)
            {
                var totalCards = numberOfDecksInShoe * Enum.GetNames(typeof(Enums.Suit)).Length * Enum.GetNames(typeof(Enums.Value)).Length;
                var tempShoe = new Card[totalCards];
                var listOfOccupiedLocationsInShoe = new List<int>();

                var random = new Random();
                foreach (var card in LoadedShoe)
                {
                    int randomLocaltion;
                    while (true)
                    {
                        randomLocaltion = random.Next(0, totalCards);
                        if (!listOfOccupiedLocationsInShoe.Contains(randomLocaltion))
                        {
                            listOfOccupiedLocationsInShoe.Add(randomLocaltion);
                            break;
                        }
                    }
                    tempShoe[randomLocaltion] = card;
                }
                LoadedShoe.Clear();
                LoadedShoe.AddRange(tempShoe);
            }
        }
    }
}
