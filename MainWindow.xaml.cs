using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emulators;
using System.Timers;
using System.Windows.Threading;

namespace Emulators
{
	public partial class MainWindow : Window
	{
		public static long Timespan15secs = new TimeSpan(0, 0, 15).Ticks;
		public static long NowTicksOfTheDay => DateTime.Now.Ticks - DateTime.Today.Ticks;

		RS_Trigger trigger = new RS_Trigger();
		ICurrentSource oscill, Rgen, Sgen;
		DispatcherTimer updateChartTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500), IsEnabled = true };
		public MainWindow()
		{
			InitializeComponent();

			ClockGenerator.AutoRun = true;
			oscill = trigger.C_ChannelInput = new ClockGenerator { LowLevelDuration_sec = 5f, HighLevelDuration_sec = 5f };
			Rgen = trigger.R_ChannelInput = new ClockGenerator { LowLevelDuration_sec = 0.7f, HighLevelDuration_sec = 0.3f };
			Sgen = trigger.S_ChannelInput = new ClockGenerator { LowLevel_Volt = 0, HighLevel_Volt = 0 };
			trigger.IsInClockedMode = true;
			oscill.OutputHistory.SecondsStored = Rgen.OutputHistory.SecondsStored = Sgen.OutputHistory.SecondsStored = trigger.OutputHistory.SecondsStored = 30;


			updateChartTimer.Tick += UpdateChart;
		}
		public void UpdateChart(object? sender, EventArgs e)
		{
			//OscillatorChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			OscillatorChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -1.1, yMax: 1.1);
			//RChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			RChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -1.1, yMax: 1.1);
			//SChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			SChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -1.1, yMax: 1.1);
			//TriggerChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			TriggerChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -1.1, yMax: 1.1);



			TriggerChart.Plot.Clear();
			var vects = this.trigger.OutputHistory.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects.Where(x => !float.IsNaN(x.p1.Y) && !float.IsNaN(x.p2.Y)))
				{
					TriggerChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Red, lineWidth: 4);
				}

				Console.Write("Update chart called\n");
				TriggerChart.Refresh(true);
			}

			OscillatorChart.Plot.Clear();
			vects = this.oscill.OutputHistory.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					OscillatorChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Orange, lineWidth: 2);
				}
				OscillatorChart.Refresh(true);
			}

			RChart.Plot.Clear();
			vects = this.Rgen.OutputHistory.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					RChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Blue, lineWidth: 1);
				}
				RChart.Refresh(true);
			}

			SChart.Plot.Clear();
			vects = this.Sgen.OutputHistory.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					SChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Blue, lineWidth: 1);
				}
				SChart.Refresh(true);
			}
		}
	}
}
