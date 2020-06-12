using System.Collections.Generic;
using BattlegroundCalculator.Cards;

namespace BattlegroundCalculator {
    public class BattlegroundBoard {
        public const int MaxBoardSize = 7;

        public List<BattlegroundCard> playerCards;
        public List<BattlegroundCard> opponentCards;

        public List<BattlegroundCard> deadPlayerCards = new List<BattlegroundCard>();
        public List<BattlegroundCard> deadOpponentCards = new List<BattlegroundCard>();

        public bool playerTurn;
        public int playerIndex;
        public int opponentIndex;

        public BattlegroundBoard(List<BattlegroundCard> playerCards, List<BattlegroundCard> opponentCards) {
            this.playerCards = playerCards;
            this.opponentCards = opponentCards;

            playerIndex = 0;
            opponentIndex = 0;
        }

        /** Copy constructor. */
        public BattlegroundBoard(BattlegroundBoard board) {
            playerCards = CloneBattlegroundCards(board.playerCards);
            opponentCards = CloneBattlegroundCards(board.opponentCards);
            deadPlayerCards = CloneBattlegroundCards(board.deadPlayerCards);
            deadOpponentCards = CloneBattlegroundCards(board.deadOpponentCards);
            playerTurn = board.playerTurn;
            playerIndex = board.playerIndex;
            opponentIndex = board.opponentIndex;
        }

        /** 
         * Called the first time using this board. Returns two copies of itself one for player start and one for opponent start.
         * 
         * Note that if a player has priority (i.e. from more cards on board), then there will be only one board returned.
         */
        public List<BattlegroundBoard> Initialize() {
            List<BattlegroundBoard> boards = new List<BattlegroundBoard>();

            if (playerCards.Count >= opponentCards.Count) {
                BattlegroundBoard playerStartBoard = new BattlegroundBoard(this);
                playerStartBoard.playerTurn = true;
                boards.Add(playerStartBoard);
            }
            if (opponentCards.Count >= playerCards.Count) {
                BattlegroundBoard opponentStartBoard = new BattlegroundBoard(this);
                opponentStartBoard.playerTurn = false;
                boards.Add(opponentStartBoard);
            }
            return boards;
        }

        public bool IsDraw() {
            return playerCards.Count == 0 && opponentCards.Count == 0;
        }

        public bool IsLoss() {
            return playerCards.Count == 0 && opponentCards.Count != 0;
        }

        public bool IsWin() {
            return playerCards.Count != 0 && opponentCards.Count == 0;
        }

        /** 
         * Removes a card from the playerCards list.
         * 
         * This should be called when a card is dead rather than removing directly from the list. 
         */
        public void RemovePlayerCard(BattlegroundCard card) {
            if (playerCards.Remove(card)) {
                deadPlayerCards.Add(card);
            }
        }
        
        /** 
         * Removes a card from the opponentCards list.
         * 
         * This should be called when a card is dead rather than removing directly from the list. 
         */
        public void RemoveOpponentCard(BattlegroundCard card) {
            if (opponentCards.Remove(card)) {
                deadOpponentCards.Add(card);
            }
        }

        /** Returns a clone of the cards list. */
        private List<BattlegroundCard> CloneBattlegroundCards(List<BattlegroundCard> cards) {
            List<BattlegroundCard> cardsClone = new List<BattlegroundCard>();
            foreach (BattlegroundCard card in cards) {
                cardsClone.Add(BattlegroundCard.CloneBattlegroundCard(card));
            }
            return cardsClone;
        }
    }
}
