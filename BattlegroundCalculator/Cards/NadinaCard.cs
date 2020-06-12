using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class NadinaCard : DeathrattleBattlegroundCard {
        public NadinaCard(Entity e) : base(e) {
        }

        public NadinaCard(Card card) : base(card) {
        }

        public NadinaCard(NadinaCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            Deathrattle deathrattle = new Deathrattle();
            Buff buff = new Buff(0, 0, true);
            for (int i = 0; i < playerCards.Count; i++) {
                if (playerCards[i].race == HearthDb.Enums.Race.DRAGON) {
                   buff.playerCardIndices.Add(i);
                }
            }
            deathrattle.buffs.Add(buff);
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}