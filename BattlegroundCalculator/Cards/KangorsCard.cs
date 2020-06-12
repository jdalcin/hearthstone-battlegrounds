using HearthDb;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class KangorsCard : DeathrattleBattlegroundCard {
        public KangorsCard(Entity e) : base(e) {
        }

        public KangorsCard(Card card) : base(card) {
        }

        public KangorsCard(KangorsCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            // Get the two mechs that died first. Note that they are the mechs in their base forms (no buffs attached).
            List<BattlegroundCard> mechsToSummon = new List<BattlegroundCard>();
            List<BattlegroundCard> cards = 
                playerCards == board.playerCards
                    ? board.deadPlayerCards
                    : board.deadOpponentCards;
            foreach (BattlegroundCard card in cards) {
                if (card.race == Race.MECHANICAL) {
                    mechsToSummon.Add(card);
                }
                if (mechsToSummon.Count == 2) {
                    break;
                }
            }
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            Deathrattle deathrattle = new Deathrattle();
            deathrattle.playerCardIndex = cardIndex;
            foreach (BattlegroundCard card in mechsToSummon) {
                deathrattle.playerCards.Add(Utils.CreateBattlegroundCard(Utils.GetCardFromName(card.name)));
            }
            deathrattles.Add(deathrattle);
            return deathrattles;
        }
    }
}