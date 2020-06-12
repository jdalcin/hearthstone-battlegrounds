using HearthDb;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class GhastcoilerCard : DeathrattleBattlegroundCard {
        public GhastcoilerCard(Entity e) : base(e) {
        }

        public GhastcoilerCard(Card card) : base(card) {
        }

        public GhastcoilerCard(GhastcoilerCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            // TODO: Handle unstable ghoul, khadgar effects separately.
            List<string> possibleSummonNames =
                new List<string>(
                    new string[] {
                        "Selfless Hero",
                        "Mecharoo",
                        "Fiendish Servant",
                        "Spawn of N'zoth",
                        "Kindly Grandmother",
                        "Rat Pack",
                        "Harvest Golem",
                        "Kaboom Bot",
                        "Imprisoner",
                        "Unstable Ghoul",
                        "Infested Wolf",
                        "The Beast",
                        "Piloted Shredder",
                        "Replicating Menace",
                        "Mechano-Egg",
                        "Goldrinn, the Great Wolf",
                        "Savannah Highmane",
                        "Voidlord",
                        "King Bagurgle",
                        "Sneed's Old Shredder",
                        "Kangor's Apprentice",
                        "Nadina the Red"
                    });
            List<Card> possibleSummonCards = new List<Card>();
            foreach (string summonName in possibleSummonNames) {
                possibleSummonCards.Add(Utils.GetCardFromName(summonName));
            }
            List<Deathrattle> deathrattles = new List<Deathrattle>();

            // TODO: This does not consider that different orderings may lead to different results. Consider
            // whether this would change results enough to implement.
            for (int i = 0; i < possibleSummonCards.Count; i++) {
                for (int j = i + 1; j < possibleSummonCards.Count; j++) {
                    Deathrattle deathrattle = new Deathrattle();
                    deathrattle.playerCardIndex = cardIndex;
                    deathrattle.playerCards.Add(Utils.CreateBattlegroundCard(possibleSummonCards[i]));
                    deathrattle.playerCards.Add(Utils.CreateBattlegroundCard(possibleSummonCards[j]));
                    deathrattles.Add(deathrattle);
                }
            }
            return deathrattles;
        }
    }
}