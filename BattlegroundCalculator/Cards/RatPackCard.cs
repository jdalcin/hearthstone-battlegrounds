using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class RatPackCard : DeathrattleBattlegroundCard {
        public RatPackCard(Entity e) : base(e) {
        }

        public RatPackCard(Card card) : base(card) {
        }

        public RatPackCard(RatPackCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            // Summon rats based on the attack.
            Deathrattle deathrattle = new Deathrattle();
            deathrattle.playerCardIndex = cardIndex;
            deathrattle.playerCards = new List<BattlegroundCard>();
            Card rat = Utils.GetCardFromName("Rat");
            for (int i = 0; i < attack; i++) {
                BattlegroundCard summonCard = new BattlegroundCard(rat);
                deathrattle.playerCards.Add(summonCard);
            }
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}