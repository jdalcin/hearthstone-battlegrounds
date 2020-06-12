using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class KaboomBotCard : DeathrattleBattlegroundCard {
        public KaboomBotCard(Entity e) : base(e) {
        }

        public KaboomBotCard(Card card) : base(card) {
        }

        public KaboomBotCard(KaboomBotCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            for (int i = 0; i < opponentCards.Count; i++) {
                Deathrattle deathrattle = new Deathrattle();
                Debuff debuff = new Debuff(0, 4);
                debuff.opponentCardIndices.Add(i);
                deathrattle.debuffs.Add(debuff);
                deathrattles.Add(deathrattle);
            }
            return deathrattles;
        }
    }
}
