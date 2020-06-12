using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlegroundCalculator.Cards {
    public abstract class DeathrattleBattlegroundCard : BattlegroundCard {
        public class Debuff {
            /** The amount of attack to detract from a particular card. */
            public readonly int attackToRemove;

            /** The amount of health to remove from a particular card. */
            public readonly int healthToRemove;

            public Debuff(int attackToRemove, int healthToRemove) {
                this.attackToRemove = attackToRemove;
                this.healthToRemove = healthToRemove;
            }

            /** The player card indices to debuff. */
            public List<int> playerCardIndices = new List<int>();

            /** The opponent card indices to debuff. */
            public List<int> opponentCardIndices = new List<int>();
        }

        public class Buff {
            /** The amount of attack to add to a particular card. */
            public readonly int attackToAdd;

            /** The amount of health to add to a particular card. */
            public readonly int healthToAdd;

            /** Whether to divine shield a particular card. */
            public readonly bool divineShield;

            public Buff(int attackToAdd, int healthToAdd, bool divineShield) {
                this.attackToAdd = attackToAdd;
                this.healthToAdd = healthToAdd;
                this.divineShield = divineShield;
            }

            /** The player card indices to buff. */
            public List<int> playerCardIndices = new List<int>();

            /** The opponent card indices to buff. */
            public List<int> opponentCardIndices = new List<int>();
        }

        public class Deathrattle {
            /** Cards to be added to the playerCards list. Empty if no cards are to be added. */
            public List<BattlegroundCard> playerCards = new List<BattlegroundCard>();

            /** Cards to be added to the opponentCards list. Empty if no cards are to be added. */
            public List<BattlegroundCard> opponentCards = new List<BattlegroundCard>();

            /** Buffs to be made from the deathrattle. */
            public List<Buff> buffs = new List<Buff>();

            /** Debuffs to be made from the deathrattle. */
            public List<Debuff> debuffs = new List<Debuff>();

            /** At which index to add the playerCards. Only applicable if playerCards is not empty. */
            public int playerCardIndex;

            /** At which index to add the opponentCards. Only applicable if opponentCards is not empty. */
            public int opponentCardIndex;
        }

        public DeathrattleBattlegroundCard(Entity e) : base(e) {
        }

        public DeathrattleBattlegroundCard(Entity e, IEnumerable<Entity> attachedEntities) : base(e, attachedEntities) {
        }

        public DeathrattleBattlegroundCard(Card card) : base(card) {
        }

        public DeathrattleBattlegroundCard(BattlegroundCard card) : base(card) {
        }

        /**
         * Generates all possible Deathrattles. Should be called when card is dead (i.e. health has reached <= 0). 
         * 
         * GenerateDeathrattle should only be called AFTER the card has already been removed from playerCards.
         * 
         * Note that playerCards and opponentCards should not be modified as they will be modified according to the DeathrattleOutcomes
         * by the caller of this function.
         * 
         * TODO: Explicitly disallow modification of them.
         * <param name="playerCards">The card list that used to contain this BattlegroundCard.</param>
         * <param name="opponentCards">The card list of the opponent to this BattlegroundCard.</param>
         * <param name="cardIndex">The index of where this BattlegroundCard was in the playerCards list.</param>
         * <param name="board"></param>
         */
        public abstract List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board);
    }
}
