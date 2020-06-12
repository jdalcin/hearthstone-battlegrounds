using BattlegroundCalculator.Cards;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BattlegroundCalculator.Tests {
    [TestClass()]
    public class BattlegroundSimulationTests {

        private class EntityBuilder {
            private Entity entity;

            public EntityBuilder() {
                entity = new Entity();
                entity.Name = "Rat";
            }

            public EntityBuilder Attack(int attack) {
                entity.SetTag(GameTag.ATK, attack);
                return this;
            }

            public EntityBuilder Health(int health) {
                entity.SetTag(GameTag.HEALTH, health);
                return this;
            }

            public Entity Build() {
                return entity;
            }
        }

        [TestMethod()]
        public void TestBasicBoardSimulation() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            playerCards.Add(new BattlegroundCard(1, 1));
            playerCards.Add(new BattlegroundCard(3, 3));

            opponentCards.Add(new BattlegroundCard(1, 1));
            opponentCards.Add(new BattlegroundCard(2, 2));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(2, simulation.simulationStats.totalWon);
            Assert.AreEqual(2, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestTauntedBoard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            playerCards.Add(new BattlegroundCard(2, 2, true, false, false, false, Race.INVALID, ""));
            playerCards.Add(new BattlegroundCard(1, 1));

            opponentCards.Add(new BattlegroundCard(2, 2));
            opponentCards.Add(new BattlegroundCard(1, 1));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            Assert.AreEqual(2, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]

        public void TestDivineShieldedBoard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            playerCards.Add(new BattlegroundCard(2, 2, false, true, false, false, Race.INVALID, ""));
            playerCards.Add(new BattlegroundCard(1, 1));

            opponentCards.Add(new BattlegroundCard(2, 2));
            opponentCards.Add(new BattlegroundCard(1, 1));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(5, simulation.simulationStats.totalWon);
            Assert.AreEqual(2, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestCleave() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            playerCards.Add(new BattlegroundCard(1, 2, false, false, true, false, Race.INVALID, ""));

            opponentCards.Add(new BattlegroundCard(1, 1));
            opponentCards.Add(new BattlegroundCard(1, 1));
            opponentCards.Add(new BattlegroundCard(1, 1));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            // Should never win (due to priority). 
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            // Drawn: Hit either of the two remaining units.
            Assert.AreEqual(2, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestPoisonous() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            playerCards.Add(new BattlegroundCard(1, 4, false, false, false, true, Race.INVALID, ""));

            opponentCards.Add(new BattlegroundCard(2, 4));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(2, simulation.simulationStats.totalWon);
            Assert.AreEqual(0, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestBasicSummonDeathrattle() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            Entity playerCardEntity = new EntityBuilder().Attack(1).Health(1).Build();
            playerCards.Add(new BasicSummonDeathrattleCard(playerCardEntity, Utils.GetCardFromName("Rat"), 1));

            opponentCards.Add(new BattlegroundCard(1, 1));
            opponentCards.Add(new BattlegroundCard(1, 1));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestMultipleSummonDeathrattle() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            Entity playerCardEntity = new EntityBuilder().Attack(1).Health(1).Build();
            playerCards.Add(
                new BasicSummonDeathrattleCard(
                    playerCardEntity, Utils.GetCardFromName("Rat"), 2));

            opponentCards.Add(new BattlegroundCard(1, 1));
            opponentCards.Add(new BattlegroundCard(1, 1));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            Assert.AreEqual(0, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestRatPack() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            BattlegroundCard ratPackCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Rat Pack"));
            ratPackCard.attack = 3;
            ratPackCard.health = 1;
            playerCards.Add(ratPackCard);

            for (int i = 0; i < 4; i++) {
                opponentCards.Add(new BattlegroundCard(1, 1));
            }

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(6, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestTheBeastCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // The beast is a 9/7 that summons a 3/3 Finkle Einhorn for the opponent on its deathrattle.
            BattlegroundCard theBeastCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("The Beast"));

            // The following configuration should always result in a draw.
            playerCards.Add(theBeastCard);
            playerCards.Add(new BattlegroundCard(3, 3));

            opponentCards.Add(new BattlegroundCard(9, 7, true, false, false, false, Race.INVALID, ""));
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            // Player goes first due to priority, only one possible outcome.
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestPilotedShredderCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Piloted Shredder is a complicated case since it spawns so many deathrattle outcomes. For sanity, we derive case where we 
            // expect to ALWAYS lose unless it spawns a kindly grandmother. In this case, it can do a max of 8 damage.
            BattlegroundCard pilotedShredderCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Piloted Shredder"));
            playerCards.Add(pilotedShredderCard);

            opponentCards.Add(new BattlegroundCard(9, 8, false, false, false, false, Race.INVALID, ""));
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(2, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(18, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestSelflessHeroCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Selfless hero is a 2/1. The following configuration should result in a win only if the 3/1 is divine shielded
            // on the selfless hero's deathrattle.
            BattlegroundCard selflessHeroCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Selfless Hero"));
            playerCards.Add(selflessHeroCard);
            playerCards.Add(new BattlegroundCard(3, 1));
            playerCards.Add(new BattlegroundCard(1, 1));
            opponentCards.Add(new BattlegroundCard(1, 8, false, false, false, false, Race.INVALID, ""));
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            // win:
            // 2/1 -> 1/8 (divine shield 3/1); 1/8 -> 3/1; 3/1 -> 1/8. Win with 1/1 leftover.
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            // draw:
            // 2/1 -> 1/8 (divine shield 3/1); 1/8 -> 1/1; 3/1 -> 1/8; 1/8 -> 3/1.
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(2, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestFiendishServantCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            BattlegroundCard fiendishServantCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Fiendish Servant"));
            playerCards.Add(fiendishServantCard);
            fiendishServantCard.attack = 5;
            fiendishServantCard.health = 1;
            playerCards.Add(new BattlegroundCard(1, 2));
            playerCards.Add(new BattlegroundCard(1, 1));

            opponentCards.Add(new BattlegroundCard(1, 17, true, false, false, false, Race.INVALID, ""));
            // Win: Fiendish buffs 1/2. The new 6/1 hits 1/17. 1/17 hits 6/1.
            // Draw: Fiendish buffs 1/2. The new 6/1 hits 1/17. 1/17 hits 1/1. 6/1 hits 1/17.
            // Loss: All other cases.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(2, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestSpawnOfNzothCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Spawn is a 2/2.
            BattlegroundCard spawnOfNzothCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Spawn Of N'zoth"));
            spawnOfNzothCard.hasTaunt = true;
            playerCards.Add(spawnOfNzothCard);
            playerCards.Add(new BattlegroundCard(1, 1));
            playerCards.Add(new BattlegroundCard(1, 1));

            opponentCards.Add(new BattlegroundCard(2, 2));
            opponentCards.Add(new BattlegroundCard(2, 2));
            opponentCards.Add(new BattlegroundCard(2, 2));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(8, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestGoldrinnCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Goldrinn is a 4/4 granting +4/+4 to beasts.
            BattlegroundCard goldrinnCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Goldrinn, the Great Wolf"));
            playerCards.Add(goldrinnCard);
            playerCards.Add(new BattlegroundCard(1, 1, false, false, false, false, Race.BEAST, ""));
            playerCards.Add(new BattlegroundCard(1, 1));

            opponentCards.Add(new BattlegroundCard(5, 9));

            // This will always result in either a win or a draw.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            // Win: Goldrinn dies, grants +4/+4 to the beast. The 5/5 beast attacks the 5/9, killing it.
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            // Draw: Goldrinn dies, then the 1/1 attacks the 5/9. The 5/5 then attacks the 5/9.
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestKingBagurgleCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // King Bagurgle is a 6/3 that grants +2/+2 on deathrattle to murlocs.
            BattlegroundCard kingBagurgleCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("King Bagurgle"));
            playerCards.Add(kingBagurgleCard);
            playerCards.Add(new BattlegroundCard(1, 1, false, false, false, false, Race.MURLOC, ""));
            playerCards.Add(new BattlegroundCard(1, 1));

            opponentCards.Add(new BattlegroundCard(3, 9));

            // This will always result in either a win or a draw.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            // Win: Bagurgle dies, grants +2/+2 to the murloc making it a 3/3. The 3/9 then attacks the 3/3.
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            // Draw: Bagurgle dies, grants +2/+2 to the murloc making it a 3/3. The 3/9 then attacks the 1/1. The 3/3 then attacks the 3/9.
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestNadinaCard() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Nadina is a 7/4 granting divine shield to all dragons on deathrattle.
            BattlegroundCard nadinaCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Nadina the Red"));
            playerCards.Add(nadinaCard);
            playerCards.Add(new BattlegroundCard(1, 1, false, false, false, false, Race.DRAGON, ""));
            playerCards.Add(new BattlegroundCard(1, 1, false, false, false, false, Race.DRAGON, ""));

            opponentCards.Add(new BattlegroundCard(4, 7, true, false, false, false, Race.INVALID, ""));
            opponentCards.Add(new BattlegroundCard(4, 4));

            // The following should always result in a draw.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(3, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestKaboomBot() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Kaboom bot is a 2/2 that damages a random opponent card by 4.
            BattlegroundCard kaboomBotCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Kaboom Bot"));
            playerCards.Add(kaboomBotCard);

            opponentCards.Add(new BattlegroundCard(1, 5));
            opponentCards.Add(new BattlegroundCard(2, 2));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(2, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestKaboomBotTriggeringDeathrattle() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            // Kaboom bot is a 2/2 that damages a random opponent card by 4.
            BattlegroundCard kaboomBotCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Kaboom Bot"));
            kaboomBotCard.hasTaunt = true;
            playerCards.Add(kaboomBotCard);
            playerCards.Add(new BattlegroundCard(2, 4));

            // Selfless  hero is a 2/1 that grants divine shield.
            BattlegroundCard selflessHeroCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Selfless Hero"));
            opponentCards.Add(new BattlegroundCard(2, 2));
            opponentCards.Add(selflessHeroCard);
            opponentCards.Add(new BattlegroundCard(2, 1));

            // There is a chance for the kaboom bot proc to hit the selfless hero, triggering another deathrattle from its own deathrattle.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            // Win: 2/2 hits kaboom bot. Kaboom bot hits 2/1. Selfless hero procs on nothing. 2/4 Remains alive. 
            Assert.AreEqual(1, simulation.simulationStats.totalWon);
            // Draw: 2/2 hits kaboom bot. Deathrattle hits selfless hero. Sefless hero triggers divine shield on 2/1. 2/1 ties with 2/4.
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestUnstableGhoul() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            BattlegroundCard unstableGhoulCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Unstable Ghoul"));
            playerCards.Add(unstableGhoulCard);
            playerCards.Add(new BattlegroundCard(2, 1));

            for (int i = 0; i < 7; i++) {
                opponentCards.Add(new BattlegroundCard(3, 1));
            }

            // Should always result in a draw.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(1, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestSneedsCard() {
            // Sneeds is a very complicated card to test. We give it a case where it loses always
            // unless a Foe Reaper is spawned.
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            BattlegroundCard sneedsOldShredder = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Sneed's Old Shredder"));
            playerCards.Add(sneedsOldShredder);

            opponentCards.Add(new BattlegroundCard(8, 6));
            opponentCards.Add(new BattlegroundCard(8, 6));

            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            // There are two win scenarios. Both are when sneed's spawns a foe reaper.
            Assert.AreEqual(2, simulation.simulationStats.totalWon);
            Assert.AreEqual(0, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(22, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestKangors() {
            // Kangors is a 3/6 that summons the first two mechs that died in combat.
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            BattlegroundCard kangorsCard = Utils.CreateBattlegroundCard(Utils.GetCardFromName("Kangor's Apprentice"));
            for (int i = 0; i < 3; i++) {
                playerCards.Add(new BattlegroundCard(1, 1, true, false, false, false, Race.MECHANICAL, "Micro Machine"));
            }
            playerCards.Add(kangorsCard);

            opponentCards.Add(new BattlegroundCard(10, 8));
            // We expect to always draw with this configuration.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(2, simulation.simulationStats.totalDrawn);
            Assert.AreEqual(0, simulation.simulationStats.totalLost);
        }

        [TestMethod()]
        public void TestBoardSizeDeathrattleSummons() {
            List<BattlegroundCard> playerCards = new List<BattlegroundCard>();
            List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();
            Entity playerCardEntity = new EntityBuilder().Attack(1).Health(1).Build();
            playerCards.Add(new BasicSummonDeathrattleCard(playerCardEntity, Utils.GetCardFromName("Rat"), 10));

            opponentCards.Add(new BattlegroundCard(1, 9));

            // With board size being enforced, the opponent should always win.
            BattlegroundBoard board = new BattlegroundBoard(playerCards, opponentCards);
            BattlegroundSimulation simulation = new BattlegroundSimulation(board);
            Assert.AreEqual(0, simulation.simulationStats.totalWon);
            Assert.AreEqual(0, simulation.simulationStats.totalDrawn);
            Assert.IsTrue(simulation.simulationStats.totalLost > 0);
        }

        public void TestGhastcoiler() {
            // TODO: Test Ghastcoiler...
        }
    }
}