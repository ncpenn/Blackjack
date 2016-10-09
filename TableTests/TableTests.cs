using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Blackjack.Actors.Interfaces;
using System.Collections.Generic;
using Blackjack.Actors;
using System.Linq;

namespace TableTests
{
    [TestClass]
    public class TableTests
    {
        private MockRepository _mockRepository;
        private Mock<IDealer> _dealerMock;
        private List<IPlayer> _listPlayerMocks;
        private Mock<IShoe> _shoeMock;

        public TableTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _dealerMock = _mockRepository.Create<IDealer>();
            _shoeMock = _mockRepository.Create<IShoe>();
            _listPlayerMocks = new List<IPlayer>
            {
                _mockRepository.Create<IPlayer>().Object
            };
        }

        [TestMethod]
        public void EngageDealerTest()
        {
            _dealerMock.Setup(x => x.PlayHand(It.Is<IShoe>(y => y.Equals(_shoeMock.Object)))).Returns(new List<uint>().AsEnumerable());
            var sut = new Table(1, 2, 1, .25, _listPlayerMocks, _dealerMock.Object, _shoeMock.Object);

            sut.EngageDealer();

            _mockRepository.VerifyAll();
        }
    }
}
