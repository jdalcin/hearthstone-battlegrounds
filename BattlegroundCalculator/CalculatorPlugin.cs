using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Windows.Controls;

namespace BattlegroundCalculator {
	public class CalculatorPlugin : IPlugin {
		private CalculatorDisplay _display;

		public string Author {
			get { return ""; }
		}

		public string ButtonText {
			get { return "Settings"; }
		}

		public string Description {
			get { return "A Hearthstone Battlegrounds Calculator."; }
		}

		public MenuItem MenuItem {
			get { return null; }
		}

		public string Name {
			get { return "Battlegrounds Calculator"; }
		}

		public void OnButtonPress() {
		}

		public void OnLoad() {
			_display = new CalculatorDisplay();
			Calculator calculator = new Calculator(_display);
			Core.OverlayCanvas.Children.Add(_display);
			GameEvents.OnGameStart.Add(calculator.GameStart);
			GameEvents.OnTurnStart.Add(calculator.TurnStart);
		}

		public void OnUnload() {
		}

		public void OnUpdate() {
		}

		public Version Version {
			get { return new Version(0, 1, 1); }
		}
	}
}