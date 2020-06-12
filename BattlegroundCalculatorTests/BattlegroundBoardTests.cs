using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BattlegroundCalculator.Cards;

namespace BattlegroundCalculator.Tests {
    [TestClass()]
    public class BattlegroundBoardTests {
        private static readonly List<BattlegroundCard> TestPlayerCards = new List<BattlegroundCard>();
        private static readonly List<BattlegroundCard> TestOpponentCards = new List<BattlegroundCard>();

        [ClassInitialize()]
        public static void Initialize(TestContext context) {
            // Setup player and opponent cards.
            TestPlayerCards.Add(new BattlegroundCard(1, 1));
            TestPlayerCards.Add(new BattlegroundCard(3, 3));

            TestOpponentCards.Add(new BattlegroundCard(1, 1));
            TestOpponentCards.Add(new BattlegroundCard(2, 2));
        }

        [TestMethod()]
        public void TestCopyConstructor() {
            // Verify the copy constructor creates an identical board.
            BattlegroundBoard board = new BattlegroundBoard(TestPlayerCards, TestOpponentCards);
            board.RemovePlayerCard(TestPlayerCards[0]);
            board.RemoveOpponentCard(TestOpponentCards[0]);
            BattlegroundBoard boardCopy = new BattlegroundBoard(board);

            Assert.AreEqual(board.playerTurn, boardCopy.playerTurn);
            Assert.AreEqual(board.playerIndex, boardCopy.playerIndex);
            Assert.AreEqual(board.opponentIndex, boardCopy.opponentIndex);

            // The cards should be cloned and not the same instance.
            Assert.AreNotEqual(board.playerCards, boardCopy.playerCards);
            Assert.AreNotEqual(board.opponentCards, boardCopy.opponentCards);
            Assert.AreNotEqual(board.deadPlayerCards, boardCopy.deadPlayerCards);
            Assert.AreNotEqual(board.deadOpponentCards, boardCopy.deadOpponentCards);

            Assert.AreEqual(1, board.playerCards.Count);
            Assert.AreEqual(1, board.opponentCards.Count);
            Assert.AreEqual(1, board.deadPlayerCards.Count);
            Assert.AreEqual(1, board.deadOpponentCards.Count);

            Assert.AreEqual(board.playerCards.Count, boardCopy.playerCards.Count);
            Assert.AreEqual(board.opponentCards.Count, boardCopy.opponentCards.Count);
            Assert.AreEqual(board.deadPlayerCards.Count, boardCopy.deadPlayerCards.Count);
            Assert.AreEqual(board.deadOpponentCards.Count, boardCopy.deadOpponentCards.Count);

            for (int i = 0; i < board.playerCards.Count; i++) {
                BattlegroundCard a = board.playerCards[i];
                BattlegroundCard b = boardCopy.playerCards[i];
                Assert.AreEqual(a.attack, b.attack);
                Assert.AreEqual(a.health, b.health);
            }

            for (int i = 0; i < board.opponentCards.Count; i++) {
                BattlegroundCard a = board.opponentCards[i];
                BattlegroundCard b = boardCopy.opponentCards[i];
                Assert.AreEqual(a.attack, b.attack);
                Assert.AreEqual(a.health, b.health);
            }

            for (int i = 0; i < board.deadPlayerCards.Count; i++) {
                BattlegroundCard a = board.deadPlayerCards[i];
                BattlegroundCard b = boardCopy.deadPlayerCards[i];
                Assert.AreEqual(a.attack, b.attack);
                Assert.AreEqual(a.health, b.health);
            }

            for (int i = 0; i < board.deadOpponentCards.Count; i++) {
                BattlegroundCard a = board.deadOpponentCards[i];
                BattlegroundCard b = boardCopy.deadOpponentCards[i];
                Assert.AreEqual(a.attack, b.attack);
                Assert.AreEqual(a.health, b.health);
            }
        }

        [TestMethod()]
        public void TestInitialize() {
            BattlegroundBoard board = new BattlegroundBoard(TestPlayerCards, TestOpponentCards);

            List<BattlegroundBoard> res = board.Initialize();
            Assert.AreEqual(res.Count, 2);
            Assert.IsTrue(res[0].playerTurn);
            Assert.IsFalse(res[1].playerTurn);
        }
    }
}