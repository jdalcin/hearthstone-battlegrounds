using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class TheBeastCard : DeathrattleBattlegroundCard {
        public TheBeastCard(Entity e) : base(e) {
        }

        public TheBeastCard(Card card) : base(card) {
        }

        public TheBeastCard(TheBeastCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            Card finkleEinhorn = Utils.GetCardFromName("Finkle Einhorn");
            Deathrattle deathrattle = new Deathrattle();
            deathrattle.opponentCardIndex = opponentCards.Count;
            deathrattle.opponentCards.Add(new BattlegroundCard(finkleEinhorn));
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}