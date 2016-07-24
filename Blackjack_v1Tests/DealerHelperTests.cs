using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Blackjack.Actors;
using Blackjack.Helpers;
using Blackjack.Interfaces;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlackjackTests
{
    [TestClass]
    public class DealerHelperTests
    {
        private readonly MockRepository _repository;

        public DealerHelperTests()
        {
            _repository = new MockRepository(MockBehavior.Default);
        }

        [TestMethod]
        public void DealInitialCardsToDealer()
        {
            var table = _repository.Create<ITable>();
            var shoe = _repository.Create<IShoe>();
            var cards = new List<uint> {1,2,3,4,5,6};
            shoe.Setup(a => a.GiveMeSomeCards(It.Is<int>(b => b == 2))).Returns(new uint[] {1, 2});
            table.Setup(a => a.AddCardToVisibleCards(It.Is<uint>(b => b == 1)));
            var dealerHelper = new DealerHelper();
            dealerHelper.DealInitialCardsToDealer(cards, shoe.Object, table.Object);
            Assert.AreEqual(2, cards.Count);
            _repository.VerifyAll();
        }

        [TestMethod]
        public void InitRoundTest()
        {
            var table = _repository.Create<ITable>();
            var dealerHelper = new DealerHelper();
            table.Setup(a => a.ClearVisibleCardsOffTable());
            dealerHelper.InitRound(table.Object);
            table.Verify(x => x.ClearVisibleCardsOffTable(), Times.Once());
            _repository.VerifyAll();
        }

        [TestMethod]
        public void DealToDealerTest_LessThan17()
        {
            var shoe = _repository.Create<IShoe>();
            var cardHelper = _repository.Create<ICardHelper>();
            var basicStrat = _repository.Create<IBasicStrategy>();
            var dealerHand = new List<uint> {1, 2};

            cardHelper.Setup(a => a.IsBlackJack(It.Is<uint>(b => b == 1), It.Is<uint>(c => c == 2))).Returns(false);
            basicStrat.Setup(a => a.DetermineHandValue(It.Is<List<uint>>(b => b.SequenceEqual(new List<uint> {1,2})))).Returns(new HandValue {IsSoft = true, IsSplit = false, Value = 13});
            basicStrat.Setup(a => a.DetermineHandValue(It.Is<List<uint>>(b => b.SequenceEqual(new List<uint> { 1, 2, 5 })))).Returns(new HandValue { IsSoft = true, IsSplit = false, Value = 18 });
            shoe.Setup(a => a.GiveMeSomeCards(It.Is<int>(b => b == 1))).Returns(new uint[] {5});
            var dealerHelper = new DealerHelper();
            var cardsDealt = dealerHelper.DealToDealer(dealerHand, shoe.Object, cardHelper.Object, basicStrat.Object);
            Assert.AreEqual(1, cardsDealt.Count());
            Assert.AreEqual((uint)5, cardsDealt.First());
            Assert.AreEqual(3, dealerHand.Count);
            _repository.VerifyAll();
        }

        [TestMethod]
        public void DealToDealerTest_Blackjack()
        {
            var shoe = _repository.Create<IShoe>();
            var cardHelper = _repository.Create<ICardHelper>();
            var basicStrat = _repository.Create<IBasicStrategy>();
            var dealerHand = new List<uint> { 1, 11 };

            cardHelper.Setup(a => a.IsBlackJack(It.Is<uint>(b => b == 1), It.Is<uint>(c => c == 11))).Returns(true);
            var dealerHelper = new DealerHelper();
            var cardsDealt = dealerHelper.DealToDealer(dealerHand, shoe.Object, cardHelper.Object, basicStrat.Object);
            Assert.IsFalse(cardsDealt.Any());
            Assert.AreEqual(2, dealerHand.Count);
            _repository.VerifyAll();
        }

        [TestMethod]
        public void DealToDealerTest_GreaterThan17()
        {
            var shoe = _repository.Create<IShoe>();
            var cardHelper = _repository.Create<ICardHelper>();
            var basicStrat = _repository.Create<IBasicStrategy>();
            var dealerHand = new List<uint> { 13, 8 };

            cardHelper.Setup(a => a.IsBlackJack(It.Is<uint>(b => b == 13), It.Is<uint>(c => c == 8))).Returns(false);
            basicStrat.Setup(a => a.DetermineHandValue(It.Is<List<uint>>(b => b.SequenceEqual(new List<uint> { 13, 8 })))).Returns(new HandValue { IsSoft = true, IsSplit = false, Value = 18 });
            var dealerHelper = new DealerHelper();
            var cardsDealt = dealerHelper.DealToDealer(dealerHand, shoe.Object, cardHelper.Object, basicStrat.Object);
            Assert.IsFalse(cardsDealt.Any());
            Assert.AreEqual(2, dealerHand.Count);
            _repository.VerifyAll();
        }
    }
}
