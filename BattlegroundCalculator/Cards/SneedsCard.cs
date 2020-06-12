using HearthDb;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class SneedsCard : DeathrattleBattlegroundCard {
        public SneedsCard(Entity e) : base(e) {
        }

        public SneedsCard(Card card) : base(card) {
        }

        public SneedsCard(SneedsCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            // TODO: Handle Khadgar effects (e.g. with micro machines).
            List<string> possibleSummonNames =
                new List<string>(
                    new string[] {
                        "Old Murk-Eye",
                        "Khadgar",
                        "Shifter Zerus",
                        "The Beast",
                        "Bolvar, Fireblood",
                        "Baron Rivendare",
                        "Brann Bronzebeard",
                        "Goldrinn, the Great Wolf",
                        "King Bagurgle",
                        "Mal'Ganis",
                        "Foe Reaper 4000",
                        "Maexxna"
                    });
            List<Card> possibleSummonCards = new List<Card>();
            foreach (string summonName in possibleSummonNames) {
                possibleSummonCards.Add(Utils.GetCardFromName(summonName));
            }
            List<Deathrattle> deathrattles = new List<Deathrattle>();
            foreach (Card card in possibleSummonCards) {
                Deathrattle deathrattle = new Deathrattle();
                deathrattle.playerCardIndex = cardIndex;
                deathrattle.playerCards = new List<BattlegroundCard>();
                BattlegroundCard summonCard = Utils.CreateBattlegroundCard(card);
                deathrattle.playerCards.Add(summonCard);
                deathrattles.Add(deathrattle);
            }
            return deathrattles;
        }
    }
}