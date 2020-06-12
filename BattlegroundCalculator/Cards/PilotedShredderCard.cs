using HearthDb;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;

namespace BattlegroundCalculator.Cards {
    class PilotedShredderCard : DeathrattleBattlegroundCard {
        public PilotedShredderCard(Entity e) : base(e) {
        }

        public PilotedShredderCard(Card card) : base(card) {
        }

        public PilotedShredderCard(PilotedShredderCard card) : base(card) {
        }

        public override List<Deathrattle> GenerateDeathrattles(List<BattlegroundCard> playerCards,
            List<BattlegroundCard> opponentCards, int cardIndex, BattlegroundBoard board) {
            // TODO: Handle unstable ghoul, khadgar effects separately.
            List<string> possibleSummonNames = 
                new List<string>(
                    new string[] { 
                        "Dire Wolf Alpha", 
                        "Vulgar Homunculus", 
                        "Micro Machine",
                        "Murloc Tidehunter",
                        "Rockpool Hunter",
                        "Dragonspawn Lieutenant",
                        "Kindly Grandmother",
                        "Scavenging Hyena",
                        "Unstable Ghoul",
                        "Khadgar"
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