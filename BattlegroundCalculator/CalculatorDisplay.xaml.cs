using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace BattlegroundCalculator
{
	public partial class CalculatorDisplay : UserControl
	{
		public CalculatorDisplay(){
		}

		public void Update(string text)
		{
			Visibility = Visibility.Visible;
			Log.WriteLine("UPDATE TEST.", LogType.Debug);
		}
	}
}