using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using System.Linq;

namespace BattlegroundCalculator {
	internal class Calculator {
		private readonly CalculatorDisplay _display;

		public Calculator(CalculatorDisplay display) {
			_display = display;
		}

		/** Reset on when a new game starts. */
		internal void GameStart() {
			_display.Update("GameStart");
		}

		internal void TurnStart(ActivePlayer unusedActivePlayer) {
			string opponentBoard = string.Join(",", Core.Game.Opponent.PlayerEntities.ToList().Where(x => x.IsMinion && x.IsInPlay).Select(x => x.LocalizedName).ToArray());
			string playerBoard = string.Join(",", Core.Game.Player.PlayerEntities.ToList().Where(x => x.IsMinion && x.IsInPlay).Select(x => x.LocalizedName).ToArray());

			_display.Update("Running simulation...");
			BattlegroundSimulation simulation =
				new BattlegroundSimulation(
					Core.Game.Player.PlayerEntities.ToList().Where(x => x.IsMinion && x.IsInPlay).ToList(),
					Core.Game.Opponent.PlayerEntities.ToList().Where(x => x.IsMinion && x.IsInPlay).ToList(),
                    Core.Game.Entities);

			_display.Update("Simulation complete: win, loss, drawn: " + simulation.simulationStats.totalWon + ", " + simulation.simulationStats.totalLost + ", " + simulation.simulationStats.totalDrawn);
		}
	}
}