using System.Collections.Generic;
using System.Linq;
using HearthDb;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;

namespace BattlegroundCalculator.Cards {

    /** When adding a new subclass of BattlegroundCard, make sure to update BattlegroundCard.CloneBattlegroundCard and Utils.CreateBattlegroundCard. */
    public class BattlegroundCard {
        public int attack;
        public int health;
        public bool hasTaunt;
        public bool hasDivineShield;
        public bool hasCleave;
        public bool hasPoison;
        public Race race;
        public string name;

        /** TODO: Associate these with mech/murloc classes explicitly. */
        public int numMagnetics;
        public int numPlants;

        public BattlegroundCard(Entity e)
            : this(e, null) {
        }

        public BattlegroundCard(Entity e, IEnumerable<Entity> attachedEntities) :
            this(e.GetTag(GameTag.ATK),
                 e.GetTag(GameTag.HEALTH),
                 e.HasTag(GameTag.TAUNT),
            e.HasTag(GameTag.DIVINE_SHIELD),
                 Utils.HasCleave(e.CardId),
                 e.HasTag(GameTag.POISONOUS),
                 Utils.GetCardFromName(e.Name).Race,
                 e.Name) {
            if (attachedEntities != null) {
                numMagnetics = attachedEntities.Count(x =>
                    x.CardId == CardIds.NonCollectible.Neutral.ReplicatingMenace_ReplicatingMenaceEnchantment);
                numPlants = attachedEntities.Count(x => x.CardId == CardIds.NonCollectible.Neutral.LivingSporesToken2);
            }
        }

        public BattlegroundCard(Card card)
            : this(card.Attack,
                    card.Health,
                    Utils.HasTaunt(card),
                    Utils.HasDivineShield(card),
                    Utils.HasCleave(card.Id),
                    Utils.HasPoisonous(card),
                    card.Race,
                    card.Name) {
        }

        public BattlegroundCard(int attack, int health)
            : this(attack, health, false, false, false, false, Race.INVALID, "") {
        }

        protected BattlegroundCard(BattlegroundCard card)
            : this(card.attack, card.health, card.hasTaunt, card.hasDivineShield, card.hasCleave, card.hasPoison, card.race, card.name) {
            // TODO: Clean up constructors.
            this.numPlants = card.numPlants;
            this.numMagnetics = card.numMagnetics;
        }

        public BattlegroundCard(int attack, int health, bool hasTaunt, bool hasDivineShield, bool hasCleave, bool hasPoison, Race race, string name) {
            this.attack = attack;
            this.health = health;
            this.hasTaunt = hasTaunt;
            this.hasDivineShield = hasDivineShield;
            this.hasCleave = hasCleave;
            this.hasPoison = hasPoison;
            this.race = race;
            this.name = name;
        }

        public static BattlegroundCard CloneBattlegroundCard(BattlegroundCard card) {
            if (card is BasicSummonDeathrattleCard) {
                return new BasicSummonDeathrattleCard((BasicSummonDeathrattleCard)card);
            } else if (card is RatPackCard) {
                return new RatPackCard((RatPackCard)card);
            } else if (card is TheBeastCard) {
                return new TheBeastCard((TheBeastCard)card);
            } else if (card is PilotedShredderCard) {
                return new PilotedShredderCard((PilotedShredderCard)card);
            } else if (card is SelflessHeroCard) {
                return new SelflessHeroCard((SelflessHeroCard)card);
            } else if (card is FiendishServantCard) {
                return new FiendishServantCard((FiendishServantCard)card);
            } else if (card is SpawnOfNzothCard) {
                return new SpawnOfNzothCard((SpawnOfNzothCard)card);
            } else if (card is GoldrinnCard) {
                return new GoldrinnCard((GoldrinnCard)card);
            } else if (card is KingBagurgleCard) {
                return new KingBagurgleCard((KingBagurgleCard)card);
            } else if (card is NadinaCard) {
                return new NadinaCard((NadinaCard)card);
            } else if (card is KaboomBotCard) {
                return new KaboomBotCard((KaboomBotCard)card);
            } else if (card is UnstableGhoulCard) {
                return new UnstableGhoulCard((UnstableGhoulCard)card);
            } else if (card is SneedsCard) {
                return new SneedsCard((SneedsCard)card);
            } else if (card is KangorsCard) {
                return new KangorsCard((KangorsCard)card);
            } else if (card is GhastcoilerCard) {
                return new GhastcoilerCard((GhastcoilerCard)card);
            } else {
                return new BattlegroundCard(card);
            }
        }

        /** Copies all attributes from the supplied card. */
        public void CopyFrom(BattlegroundCard card) {
            this.attack = card.attack;
            this.health = card.health;
            this.hasTaunt = card.hasTaunt;
            this.hasDivineShield = card.hasDivineShield;
            this.hasCleave = card.hasCleave;
            this.hasPoison = card.hasPoison;
            this.race = card.race;
            this.name = card.name;
        }
    }
}
