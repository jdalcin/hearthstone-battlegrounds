using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class UnstableGhoulCard : DeathrattleBattlegroundCard {
        public UnstableGhoulCard(Entity e) : base(e) {
        }

        public UnstableGhoulCard(Card card) : base(card) {
        }

        public UnstableGhoulCard(UnstableGhoulCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            Deathrattle deathrattle = new Deathrattle();
            Debuff debuff = new Debuff(0, 1);
            for (int i = 0; i < playerCards.Count; i++) {
                debuff.playerCardIndices.Add(i);
            }
            for (int i = 0; i < opponentCards.Count; i++) {
                debuff.opponentCardIndices.Add(i);
            }
            deathrattle.debuffs.Add(debuff);
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}
