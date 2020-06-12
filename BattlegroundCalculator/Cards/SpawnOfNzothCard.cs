using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class SpawnOfNzothCard : DeathrattleBattlegroundCard {
        public SpawnOfNzothCard(Entity e) : base(e) {
        }

        public SpawnOfNzothCard(Card card) : base(card) {
        }

        public SpawnOfNzothCard(SpawnOfNzothCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            Deathrattle deathrattle = new Deathrattle();
            Buff buff = new Buff(1, 1, false);
            for (int i = 0; i < playerCards.Count; i++) {
                buff.playerCardIndices.Add(i);
            }
            deathrattle.buffs.Add(buff);
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}
