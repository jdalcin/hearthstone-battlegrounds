using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class FiendishServantCard : DeathrattleBattlegroundCard {
        public FiendishServantCard(Entity e) : base(e) {
        }

        public FiendishServantCard(Card card) : base(card) {
        }

        public FiendishServantCard(FiendishServantCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            for (int i = 0; i < playerCards.Count; i++) {
                Deathrattle deathrattle = new Deathrattle();
                Buff buff = new Buff(attack, 0, false);
                buff.playerCardIndices.Add(i);
                deathrattle.buffs.Add(buff);
                deathrattles.Add(deathrattle);
            }
            return deathrattles;
        }
    }
}
