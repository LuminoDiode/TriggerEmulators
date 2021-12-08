using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;
using System.Windows.Threading;

namespace Emulators
{
	class ChartToHistorySyncer
	{
		private DispatcherTimer updateChartTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500), IsEnabled = true };
		public List<(WpfPlot plot, CurrentHistory history)> Pairs { get; } = new List<(WpfPlot plot, CurrentHistory history)>();

		public ChartToHistorySyncer()
		{
			updateChartTimer.Tick += UpdateAll;
		}

		private void UpdateAll(object? sender, EventArgs e)
		{
			foreach (var pair in Pairs)
			{
				var Chart = pair.plot;
				var History = pair.history;

				Chart.Plot.Clear();

				var vects = History.GetChartLines();

				if (vects.Any())
					foreach (var vect in vects.Where(x => !float.IsNaN(x.p1.Y) && !float.IsNaN(x.p2.Y)))
						Chart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Red, lineWidth: 4);

				Chart.Refresh(true);
			}
		}
	}
}
