using BattlegroundCalculator.Cards;
using HearthDb;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattlegroundCalculator {
    public class Utils {
        private const string TauntMechanic = "TAUNT";
        private const string DivineShieldMechanic = "DIVINE_SHIELD";
        private const string PoisonousMechanic = "POISONOUS";

        private const string CaveHydra = CardIds.Collectible.Hunter.CaveHydra;
        private const string FoeReaper = CardIds.Collectible.Neutral.FoeReaper4000;

        public static bool HasTaunt(Card card) {
            return Array.Exists(card.Mechanics, x => string.Equals(x, TauntMechanic, StringComparison.OrdinalIgnoreCase));
        }

        public static bool HasDivineShield(Card card) {
            return Array.Exists(card.Mechanics, x => string.Equals(x, DivineShieldMechanic, StringComparison.OrdinalIgnoreCase));
        }

        public static bool HasPoisonous(Card card) {
            return Array.Exists(card.Mechanics, x => string.Equals(x, PoisonousMechanic, StringComparison.OrdinalIgnoreCase));
        }
        public static bool HasCleave(string cardId) {
            return cardId == CaveHydra || cardId == FoeReaper;
        }

        public static Card GetCardFromName(string name) {
            // TODO: Output to debug logs that a name was not found.
            Card card = HearthDb.Cards.GetFromName(name, HearthDb.Enums.Locale.enUS);
            return card != null ? card : HearthDb.Cards.GetFromName(name, HearthDb.Enums.Locale.enUS, false);
        }

        public static BattlegroundCard CreateBattlegroundCard(Entity entity, Dictionary<int, Entity> gameEntities) {
            return CreateBattlegroundCard(entity, null, gameEntities);
        }

        public static BattlegroundCard CreateBattlegroundCard(Card card) {
            return CreateBattlegroundCard(null, card, null);
        }

        /** 
         * Create a BattlegroundCard based on the supplied Entity or HearthDb.Card. Only one may be null.
         * 
         * NOTE: If both are provided, entity will be used by default.
         */
        private static BattlegroundCard CreateBattlegroundCard(Entity entity, Card card, Dictionary<int, Entity> gameEntities) {
            Card summonCard = null;
            int summonAmount = 1;
            string cardId = entity != null ? entity.CardId : card.Id; 
            switch (cardId) {
                case CardIds.Collectible.Neutral.Mecharoo:
                    summonCard = GetCardFromName("Jo-E Bot");
                    break;
                case CardIds.Collectible.Neutral.HarvestGolem:
                    summonCard = GetCardFromName("Damaged Golem");
                    break;
                case CardIds.NonCollectible.Neutral.Imprisoner:
                    summonCard = GetCardFromName("Imp");
                    break;
                case CardIds.Collectible.Hunter.KindlyGrandmother:
                    summonCard = GetCardFromName("Big Bad Wolf");
                    break;
                case CardIds.Collectible.Paladin.MechanoEgg:
                    summonCard = GetCardFromName("Robosaur");
                    break;
                case CardIds.Collectible.Hunter.InfestedWolf:
                    summonCard = GetCardFromName("Spider");
                    summonAmount = 2;
                    break;
                case CardIds.Collectible.Neutral.ReplicatingMenace:
                    summonCard = GetCardFromName("Microbot");
                    summonAmount = 3;
                    break;
                case CardIds.Collectible.Warlock.Voidlord:
                    summonCard = GetCardFromName("Voidwalker");
                    summonAmount = 3;
                    break;
                case CardIds.Collectible.Neutral.TheBeast:
                    return entity != null ? new TheBeastCard(entity) : new TheBeastCard(card);
                case CardIds.Collectible.Hunter.RatPack:
                    return entity != null ? new RatPackCard(entity) : new RatPackCard(card);
                case CardIds.Collectible.Neutral.PilotedShredder:
                    return entity != null ? new PilotedShredderCard(entity) : new PilotedShredderCard(card);
                case CardIds.Collectible.Paladin.SelflessHero:
                    return entity != null ? new SelflessHeroCard(entity) : new SelflessHeroCard(card);
                case CardIds.Collectible.Warlock.FiendishServant:
                    return entity != null ? new FiendishServantCard(entity) : new FiendishServantCard(card);
                case CardIds.Collectible.Neutral.SpawnOfNzoth:
                    return entity != null ? new SpawnOfNzothCard(entity) : new SpawnOfNzothCard(card);
                case CardIds.NonCollectible.Neutral.GoldrinnTheGreatWolf:
                    return entity != null ? new GoldrinnCard(entity) : new GoldrinnCard(card);
                case CardIds.NonCollectible.Neutral.KingBagurgle:
                    return entity != null ? new KingBagurgleCard(entity) : new KingBagurgleCard(card);
                case CardIds.NonCollectible.Neutral.NadinaTheRed:
                    return entity != null ? new NadinaCard(entity) : new NadinaCard(card);
                case CardIds.Collectible.Neutral.KaboomBot:
                    return entity != null ? new KaboomBotCard(entity) : new KaboomBotCard(card);
                case CardIds.Collectible.Neutral.UnstableGhoul:
                    return entity != null ? new UnstableGhoulCard(entity) : new UnstableGhoulCard(card);
                case CardIds.Collectible.Neutral.SneedsOldShredder:
                    return entity != null ? new SneedsCard(entity) : new SneedsCard(card);
                case CardIds.NonCollectible.Neutral.KangorsApprentice:
                    return entity != null ? new KangorsCard(entity) : new KangorsCard(card);
                case CardIds.NonCollectible.Priest.GhastcoilerBATTLEGROUNDS:
                    return entity != null ? new GhastcoilerCard(entity) : new GhastcoilerCard(card); 
                default:
                    break;
            }
            if (entity != null) {
                return summonCard == null
                    ? new BattlegroundCard(entity, GetAttachedEntities(entity.Id, gameEntities))
                    : new BasicSummonDeathrattleCard(entity, GetAttachedEntities(entity.Id, gameEntities), summonCard, summonAmount);
            } else {
                return summonCard == null
                    ? new BattlegroundCard(card)
                    : new BasicSummonDeathrattleCard(card, summonCard, summonAmount);
            }
        }

        private static IEnumerable<Entity> GetAttachedEntities(int entityId, Dictionary<int, Entity> gameEntities) {
            return gameEntities.Values
                .Where(x => x.IsAttachedTo(entityId) && (x.IsInPlay || x.IsInSetAside || x.IsInGraveyard))
                .Select(x => x.Clone());
        }
            
    }
}
