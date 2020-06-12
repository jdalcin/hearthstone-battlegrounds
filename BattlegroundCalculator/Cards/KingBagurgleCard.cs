using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class KingBagurgleCard : DeathrattleBattlegroundCard {
        public KingBagurgleCard(Entity e) : base(e) {
        }

        public KingBagurgleCard(Card card) : base(card) {
        }

        public KingBagurgleCard(KingBagurgleCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            Deathrattle deathrattle = new Deathrattle();
            Buff buff = new Buff(2, 2, false);
            for (int i = 0; i < playerCards.Count; i++) {
                if (playerCards[i].race == HearthDb.Enums.Race.MURLOC) {
                    buff.playerCardIndices.Add(i);
                }
            }
            deathrattle.buffs.Add(buff);
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}
