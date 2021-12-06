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
		ICurrentGenerator oscill, Rgen, Sgen;
		DispatcherTimer updateChartTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500), IsEnabled = true };
		public MainWindow()
		{
			InitializeComponent();

			oscill = trigger.C_ChannelInput = new ClockGenerator { LowLevelDuration_sec = 5f, HighLevelDuration_sec = 5f };
			Rgen = trigger.R_ChannelInput = new ClockGenerator { LowLevelDuration_sec = 1f, HighLevelDuration_sec = 0.2f };
			Sgen = trigger.S_ChannelInput = new ClockGenerator { LowLevel_Volt = 0, HighLevel_Volt = 0 };
			trigger.IsInClocked_Mode = true;
			((ClockGenerator)oscill).ExecuteAsync(new(false));
			((ClockGenerator)Rgen).ExecuteAsync(new(false));
			((ClockGenerator)Sgen).ExecuteAsync(new(false));
			oscill.History.SecondsStored = Rgen.History.SecondsStored = Sgen.History.SecondsStored = trigger.History.SecondsStored = 30;


			updateChartTimer.Tick += UpdateChart;
		}
		public void UpdateChart(object? sender, EventArgs e)
		{
			//OscillatorChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			OscillatorChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -2, yMax: 2);
			//RChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			RChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -2, yMax: 2);
			//SChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			SChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -2, yMax: 2);
			//TriggerChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			TriggerChart.Plot.SetInnerViewLimits(xMin: NowTicksOfTheDay - Timespan15secs, xMax: NowTicksOfTheDay, yMin: -2, yMax: 2);



			TriggerChart.Plot.Clear();
			var vects = this.trigger.History.GetChartLines();
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
			vects = this.oscill.History.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					OscillatorChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Orange, lineWidth: 2);
				}
				OscillatorChart.Refresh(true);
			}

			RChart.Plot.Clear();
			vects = this.Rgen.History.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					RChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Blue, lineWidth: 1);
				}
				RChart.Refresh(true);
			}

			SChart.Plot.Clear();
			vects = this.Sgen.History.GetChartLines();
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
