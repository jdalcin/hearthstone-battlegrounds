using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class SelflessHeroCard : DeathrattleBattlegroundCard {
        public SelflessHeroCard(Entity e) : base(e) {
        }

        public SelflessHeroCard(Card card) : base(card) {
        }

        public SelflessHeroCard(SelflessHeroCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            for (int i = 0; i < playerCards.Count; i++) {
                Deathrattle deathrattle = new Deathrattle();
                Buff buff = new Buff(0, 0, true);
                buff.playerCardIndices.Add(i);
                deathrattle.buffs.Add(buff);
                deathrattles.Add(deathrattle);
            }
            return deathrattles;
        }
    }
}