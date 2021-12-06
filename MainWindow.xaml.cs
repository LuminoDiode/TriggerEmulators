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
		RS_Trigger trigger = new RS_Trigger();
		ICurrentGenerator oscill, Rgen, Sgen;
		DispatcherTimer updateChartTimer = new DispatcherTimer() { Interval=new TimeSpan(0,0,0,0,100), IsEnabled=true};
		public MainWindow()
		{
			InitializeComponent();


			oscill = trigger.C_ChannelInput;
			trigger.C_ChannelInput = new ClockGenerator { HighLevelDuration_sec = 5, LowLevel_Volt = 5 };
			Rgen = trigger.R_ChannelInput = new ClockGenerator { LowLevelDuration_sec = 3f, HighLevelDuration_sec = 0.1f };
			Sgen = trigger.S_ChannelInput = new ClockGenerator { LowLevel_Volt = 0, HighLevel_Volt = 0 };
			((ClockGenerator)Rgen).ExecuteAsync(new(false));
			((ClockGenerator)Sgen).ExecuteAsync(new(false));
			oscill.History.SecondsStored = Rgen.History.SecondsStored = Sgen.History.SecondsStored = trigger.History.SecondsStored = 15;


			updateChartTimer.Tick += UpdateChart;
		}
		public void UpdateChart(object? sender, EventArgs e)
		{
			//OscillatorChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			OscillatorChart.Plot.SetInnerViewLimits(xMin: DateTime.UtcNow.AddSeconds(-15).Ticks, xMax: DateTime.UtcNow.Ticks, yMin: -2, yMax: 2);
			//RChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			RChart.Plot.SetInnerViewLimits(xMin: DateTime.UtcNow.AddSeconds(-15).Ticks, xMax: DateTime.UtcNow.Ticks, yMin: -2, yMax: 2);
			//SChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			SChart.Plot.SetInnerViewLimits(xMin: DateTime.UtcNow.AddSeconds(-15).Ticks, xMax: DateTime.UtcNow.Ticks, yMin: -2, yMax: 2);
			//TriggerChart.Plot.SetAxisLimits(xMin: 0, xMax: (DateTime.UtcNow - DateTime.UtcNow.AddSeconds(-10)).Ticks);
			TriggerChart.Plot.SetInnerViewLimits(xMin: DateTime.UtcNow.AddSeconds(-15).Ticks, xMax: DateTime.UtcNow.Ticks, yMin: -2, yMax: 2);



			TriggerChart.Plot.Clear();
			var vects = this.trigger.History.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					TriggerChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Red, lineWidth: 5);
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
					OscillatorChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Red, lineWidth: 5);
				}
			}

			RChart.Plot.Clear();
			vects = this.Rgen.History.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					RChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Red, lineWidth: 5);
				}
			}

			SChart.Plot.Clear();
			vects = this.Sgen.History.GetChartLines();
			if (vects.Count() > 0)
			{
				foreach (var vect in vects)
				{
					SChart.Plot.AddLine(vect.p1.X, vect.p1.Y, vect.p2.X, vect.p2.Y, color: System.Drawing.Color.Red, lineWidth: 5);
				}
			}
		}
	}
}
