using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    public class BasicSummonDeathrattleCard : DeathrattleBattlegroundCard {
        private readonly Card _summonCard;
        private readonly int _summonAmount;

        public BasicSummonDeathrattleCard(Card card, Card summonCard, int summonAmount) : base(card) {
            this._summonCard = summonCard;
            this._summonAmount = summonAmount;
        }

        public BasicSummonDeathrattleCard(Entity e, IEnumerable<Entity> attachedEntities, Card summonCard, int summonAmount) : base(e, attachedEntities) {
            this._summonCard = summonCard;
            this._summonAmount = summonAmount;
        }

        /** Copy constructor. */
        public BasicSummonDeathrattleCard(BasicSummonDeathrattleCard card) : base(card) {
            this._summonCard = card._summonCard;
            this._summonAmount = card._summonAmount;
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            Deathrattle deathrattle = new Deathrattle();
            deathrattle.playerCardIndex = cardIndex;
            for (int i = 0; i < _summonAmount; i++) {
                BattlegroundCard card = new BattlegroundCard(_summonCard);
                deathrattle.playerCards.Add(card);
            }
            return new List<Deathrattle> {deathrattle};
        }
    }
}
