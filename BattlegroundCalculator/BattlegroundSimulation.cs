using BattlegroundCalculator.Cards;
using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattlegroundCalculator {
    public class BattlegroundSimulation {

        public class SimulationStats {
            public int totalWon;
            public int totalLost;
            public int totalDrawn;

            public SimulationStats(int totalWon, int totalLost, int totalDrawn) {
                this.totalWon = totalWon;
                this.totalLost = totalLost;
                this.totalDrawn = totalDrawn;
            }

            public void Merge(SimulationStats sim) {
                totalWon += sim.totalWon;
                totalLost += sim.totalLost;
                totalDrawn += sim.totalDrawn;
            }
        };

        /** A Deathrattle chain for multiple deathrattles occuring on the same turn. */
        private class DeathrattleChain : Deathrattle {
            private readonly List<Deathrattle> _deathrattles = new List<Deathrattle>();

            public DeathrattleChain(BattlegroundBoard board) : base(null, board, false, -1) {
            }

            public void AddDeathrattle(Deathrattle deathrattle) {
                _deathrattles.Add(deathrattle);
            }

            public override List<BattlegroundBoard> ApplyDeathrattle() {
                List<BattlegroundBoard> boards = new List<BattlegroundBoard>();
                List<BattlegroundBoard> tempBoards = new List<BattlegroundBoard>();
                foreach (Deathrattle deathrattle in _deathrattles) {
                    // If we have boards previously generated, apply the new deathrattle on those boards.
                    if (boards.Count > 0) {
                        foreach (BattlegroundBoard board in boards) {
                            Deathrattle newDeathrattle = new Deathrattle(deathrattle, board);
                            tempBoards.AddRange(newDeathrattle.ApplyDeathrattle());
                        }
                        boards = tempBoards;
                        tempBoards = new List<BattlegroundBoard>();
                    } else {
                        boards = deathrattle.ApplyDeathrattle();
                    }
                }
                return boards;
            }
        }

        private class Deathrattle {
            private readonly DeathrattleBattlegroundCard _card;
            private readonly bool _isPlayer;
            private readonly int _cardIndex;
            private readonly BattlegroundBoard _originalBoard;

            public Deathrattle(DeathrattleBattlegroundCard card, BattlegroundBoard board, bool isPlayer, int cardIndex) {
                this._card = card;
                this._originalBoard = board;
                this._isPlayer = isPlayer;
                this._cardIndex = cardIndex;
            }

            public Deathrattle(Deathrattle deathrattle, BattlegroundBoard board) {
                this._card = deathrattle._card;
                this._originalBoard = board;
                this._isPlayer = deathrattle._isPlayer;
                this._cardIndex = deathrattle._cardIndex;
            }

            /** 
             * Returns a list of cloned BattlegroundBoard's with the deathrattles applied to them. Each separate board is a separate deathrattle outcome.
             * 
             * May return an empty list if there were no cards with deathrattles.
             */
            public virtual List<BattlegroundBoard> ApplyDeathrattle() {
                return HandleDeathrattles(
                    _card.GenerateDeathrattles(
                        _isPlayer ? _originalBoard.playerCards : _originalBoard.opponentCards,
                        _isPlayer ? _originalBoard.opponentCards : _originalBoard.playerCards,
                        _cardIndex,
                        _originalBoard));
            }

            private List<BattlegroundBoard> HandleDeathrattles(List<DeathrattleBattlegroundCard.Deathrattle> deathrattles) {
                if (deathrattles.Count == 0) {
                    return new List<BattlegroundBoard>();
                }
                List<BattlegroundBoard> boards = new List<BattlegroundBoard>();
                foreach (DeathrattleBattlegroundCard.Deathrattle deathrattle in deathrattles) {
                    BattlegroundBoard board = new BattlegroundBoard(_originalBoard);
                    int index = deathrattle.playerCardIndex;
                    foreach (BattlegroundCard card in deathrattle.playerCards) {
                        MaybeAddCard(GetPlayerCards(board), card, index);
                        index++;
                    }

                    index = deathrattle.opponentCardIndex;
                    foreach (BattlegroundCard card in deathrattle.opponentCards) {
                        MaybeAddCard(GetOpponentCards(board), card, index);
                        index++;
                    }
                    
                    foreach (DeathrattleBattlegroundCard.Buff buff in deathrattle.buffs) {
                        List<BattlegroundCard> playerCards = GetPlayerCards(board);
                        List<BattlegroundCard> opponentCards = GetOpponentCards(board);
                        foreach (int i in buff.playerCardIndices) {
                            playerCards[i].attack += buff.attackToAdd;
                            playerCards[i].health += buff.healthToAdd;
                            playerCards[i].hasDivineShield = playerCards[i].hasDivineShield || buff.divineShield;
                        }
                        foreach (int i in buff.opponentCardIndices) {
                            opponentCards[i].attack += buff.attackToAdd;
                            opponentCards[i].health += buff.healthToAdd;
                            opponentCards[i].hasDivineShield = opponentCards[i].hasDivineShield || buff.divineShield;
                        }
                    }

                    foreach (DeathrattleBattlegroundCard.Debuff debuff in deathrattle.debuffs) {
                        List<BattlegroundCard> playerCards = GetPlayerCards(board);
                        List<BattlegroundCard> opponentCards = GetOpponentCards(board);

                        foreach (int i in debuff.playerCardIndices) {
                            playerCards[i].attack = Math.Max(0, playerCards[i].attack - debuff.attackToRemove);
                            if (debuff.healthToRemove > 0 && playerCards[i].hasDivineShield) {
                                playerCards[i].hasDivineShield = false;
                            } else {
                                playerCards[i].health -= debuff.healthToRemove;
                            }
                        }
                        foreach (int i in debuff.opponentCardIndices) {
                            opponentCards[i].attack = Math.Max(0, opponentCards[i].attack - debuff.attackToRemove);
                            if (debuff.healthToRemove > 0 && opponentCards[i].hasDivineShield) {
                                opponentCards[i].hasDivineShield = false;
                            } else {
                                opponentCards[i].health -= debuff.healthToRemove;
                            }
                        }
                    }
                    boards.Add(board);
                }
                return boards;
            }

            private List<BattlegroundCard> GetPlayerCards(BattlegroundBoard board) {
                return _isPlayer ? board.playerCards : board.opponentCards;
            }

            private List<BattlegroundCard> GetOpponentCards(BattlegroundBoard board) {
                return _isPlayer ? board.opponentCards : board.playerCards;
            }

            /** Maybe adds a card to the list of cards. Enforces space limitations of Battlegrounds. */
            private static void MaybeAddCard(List<BattlegroundCard> cards, BattlegroundCard card, int cardIndex) {
                // Make sure the cardIndex is still within bounds after cards were removed.
                cardIndex = Math.Min(cards.Count, cardIndex);
                if (cards.Count < BattlegroundBoard.MaxBoardSize) {
                    cards.Insert(cardIndex, card);
                }
            }
        }

        public SimulationStats simulationStats;
        private readonly BattlegroundBoard _originalBoard;
        private readonly Dictionary<int, Entity> _gameEntities;

        // Testing constructor.
        public BattlegroundSimulation(BattlegroundBoard board) {
            _originalBoard = board;
            simulationStats = RunSimulation();
        }

        public BattlegroundSimulation(List<Entity> playerBoard, List<Entity> opponentBoard, Dictionary<int, Entity> gameEntities) {
            _originalBoard = new BattlegroundBoard(CreateCards(playerBoard), CreateCards(opponentBoard));
            _gameEntities = gameEntities;
            simulationStats = RunSimulation();
        }

        private List<BattlegroundCard> CreateCards(List<Entity> board) {
            List<BattlegroundCard> cards = new List<BattlegroundCard>();
            foreach (Entity e in board) {
                cards.Add(Utils.CreateBattlegroundCard(e, _gameEntities));
            }
            return cards;
        }

        private SimulationStats RunSimulation() {
            List<BattlegroundBoard> boards = _originalBoard.Initialize();
            SimulationStats stats = new SimulationStats(0, 0, 0);
            foreach (BattlegroundBoard board in boards) {
                stats.Merge(SimulationHelper(board));
            }
            return stats;
        }

        private SimulationStats SimulationHelper(BattlegroundBoard originalBoard) {
            // Base-case. Draw, win, or loss.
            if (originalBoard.IsDraw()) {
                return new SimulationStats(0, 0, 1);
            }
            if (originalBoard.IsWin()) {
                return new SimulationStats(1, 0, 0);
            }
            if (originalBoard.IsLoss()) {
                return new SimulationStats(0, 1, 0);
            }

            // Go through every possible iteration.
            List<BattlegroundCard> opponentCards = originalBoard.playerTurn ? originalBoard.opponentCards : originalBoard.playerCards;
            int playerIndex = originalBoard.playerTurn ? originalBoard.playerIndex : originalBoard.opponentIndex;

            SimulationStats simulationStats = new SimulationStats(0, 0, 0);

            // Look for attackable opponent cards.
            List<int> opponentCardIndices = new List<int>();
            List<int> tauntedIndices = new List<int>();
            for (int i = 0; i < opponentCards.Count; i++) {
                if (opponentCards[i].hasTaunt) {
                    tauntedIndices.Add(i);
                }
                opponentCardIndices.Add(i);
            }
            if (tauntedIndices.Count > 0) {
                opponentCardIndices = tauntedIndices;
            }
            foreach (int index in opponentCardIndices) {
                BattlegroundBoard boardClone = new BattlegroundBoard(originalBoard);

                // Update playerCards and opponentCards with our new cloned board.
                List<BattlegroundCard> playerCards = boardClone.playerTurn ? boardClone.playerCards : boardClone.opponentCards;
                opponentCards = boardClone.playerTurn ? boardClone.opponentCards : boardClone.playerCards;

                BattlegroundCard playerCard = playerCards[playerIndex];

                List<int> attackableOpponentIndices = new List<int>();
                List<BattlegroundCard> attackableOpponentCards = new List<BattlegroundCard>();
                if (playerCard.hasCleave) {
                    if (index > 0) {
                        attackableOpponentIndices.Add(index - 1);
                        attackableOpponentCards.Add(opponentCards[index - 1]);
                    }
                    attackableOpponentIndices.Add(index);
                    attackableOpponentCards.Add(opponentCards[index]);
                    if (index < opponentCardIndices.Count - 1) {
                        attackableOpponentIndices.Add(index + 1);
                        attackableOpponentCards.Add(opponentCards[index + 1]);
                    }
                } else {
                    attackableOpponentIndices.Add(index);
                    attackableOpponentCards.Add(opponentCards[index]);
                }

                List<Deathrattle> deathrattles = new List<Deathrattle>();
                if (playerCard.hasDivineShield) {
                    playerCard.hasDivineShield = false;
                } else {
                    if (opponentCards[index].hasPoison) {
                        playerCard.health = 0;
                    } else {
                        playerCard.health -= opponentCards[index].attack;
                    }
                    if (playerCard.health <= 0 && playerCard is DeathrattleBattlegroundCard) {
                        // Note that the "playerCard" is actually the opponent when isPlayerTurn is false.
                        // TODO: Rename playerCard and opponentCard in this instance.
                        deathrattles.Add(new Deathrattle((DeathrattleBattlegroundCard)playerCard, boardClone, boardClone.playerTurn, playerIndex));
                    }
                }
                foreach (int opponentIndex in attackableOpponentIndices) {
                    BattlegroundCard opponentCard = opponentCards[opponentIndex];
                    if (opponentCard.hasDivineShield) {
                        opponentCard.hasDivineShield = false;
                    } else {
                        if (playerCard.hasPoison) {
                            opponentCard.health = 0;
                        } else {
                            opponentCard.health -= playerCard.attack;
                        }
                        if (opponentCard.health <= 0 && opponentCard is DeathrattleBattlegroundCard) {
                            deathrattles.Add(new Deathrattle((DeathrattleBattlegroundCard)opponentCard, boardClone, !boardClone.playerTurn, opponentIndex));
                        }
                    }
                }
                if (playerCard.health <= 0) {
                    if (boardClone.playerTurn) {
                        boardClone.RemovePlayerCard(playerCard);
                    } else {
                        boardClone.RemoveOpponentCard(playerCard);
                    }
                }
                foreach (BattlegroundCard opponentCard in attackableOpponentCards) {
                    if (opponentCard.health <= 0) {
                        if (boardClone.playerTurn) {
                            boardClone.RemoveOpponentCard(opponentCard);
                        } else {
                            boardClone.RemovePlayerCard(opponentCard);
                        }
                    }
                }
                List<BattlegroundBoard> boards = new List<BattlegroundBoard>();
                while (deathrattles.Count > 0) {
                    foreach (Deathrattle deathrattle in deathrattles) {
                        List<BattlegroundBoard> deathrattleBoards = deathrattle.ApplyDeathrattle();
                        if (deathrattleBoards != null) {
                            foreach (BattlegroundBoard deathrattleBoard in deathrattleBoards) {
                                boards.Add(deathrattleBoard);
                            }
                        }
                    }
                    deathrattles.Clear();
                    // Deathrattles could trigger other deathrattles (e.g. Kaboom Bot killing another card). We must now look for dead cards and reapply deathrattles.
                    // TODO: Modulate this logic with the logic from above.
                    List<BattlegroundBoard> boardsToRemove = new List<BattlegroundBoard>();
                    
                    foreach (BattlegroundBoard board in boards) {
                        bool triggeredDeathrattles = false;
                        List<BattlegroundCard> playerCardsToRemove = new List<BattlegroundCard>();
                        List<BattlegroundCard> opponentCardsToRemove = new List<BattlegroundCard>();
                        DeathrattleChain deathrattleChain = new DeathrattleChain(board);
                        for (int j = 0; j < board.playerCards.Count; j++) {
                            if (board.playerCards[j].health <= 0) {
                                playerCardsToRemove.Add(board.playerCards[j]);
                                if (board.playerCards[j] is DeathrattleBattlegroundCard) {
                                    triggeredDeathrattles = true;
                                    deathrattleChain.AddDeathrattle(new Deathrattle((DeathrattleBattlegroundCard)board.playerCards[j], board, true, j));
                                }
                            }
                        }
                        for (int j = 0; j < board.opponentCards.Count; j++) {
                            if (board.opponentCards[j].health <= 0) {
                                opponentCardsToRemove.Add(board.opponentCards[j]);
                                if (board.opponentCards[j] is DeathrattleBattlegroundCard) {
                                    triggeredDeathrattles = true;
                                    deathrattleChain.AddDeathrattle(new Deathrattle((DeathrattleBattlegroundCard)board.opponentCards[j], board, false, j));
                                }
                            }
                        }
                        foreach (BattlegroundCard card in playerCardsToRemove) {
                            board.RemovePlayerCard(card);
                        }
                        foreach (BattlegroundCard card in opponentCardsToRemove) {
                            board.RemoveOpponentCard(card);
                        }
                        if (triggeredDeathrattles) {
                            deathrattles.Add(deathrattleChain);
                            boardsToRemove.Add(board);
                        }
                    }
                    // Remove any boards that are going to be handled by the newly triggered deathrattles.
                    foreach (BattlegroundBoard board in boardsToRemove) {
                        boards.Remove(board);
                    }
                }
                // Continue on with the deathrattle boards, or the original boardClone if there were no deathrattles.
                if (boards.Count == 0) {
                    boards.Add(boardClone);
                }
                foreach (BattlegroundBoard board in boards) {
                    if (board.playerTurn && playerCard.health > 0) {
                        board.playerIndex = board.playerCards.Count > 0 ? ++board.playerIndex : 0;
                    }
                    if (!board.playerTurn && playerCard.health > 0) {
                        board.opponentIndex = board.opponentCards.Count > 0 ? ++board.opponentIndex : 0;
                    }
                    if (board.playerCards.Count > 0) {
                        board.playerIndex %= board.playerCards.Count;
                    }
                    if (board.opponentCards.Count > 0) {
                        board.opponentIndex %= board.opponentCards.Count;
                    }
                    board.playerTurn = !board.playerTurn;
                    simulationStats.Merge(SimulationHelper(board));
                }
            }

            return simulationStats;
        }
    }
}
