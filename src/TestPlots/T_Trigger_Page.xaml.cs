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

namespace Emulators
{
	/// <summary>
	/// Interaction logic for T_Trigger_Page.xaml
	/// </summary>
	public partial class T_Trigger_Page : Page
	{
		T_Trigger Trigger;
		ClockGenerator T_Channel;
		ClockGenerator C_Channel;
		public T_Trigger_Page()
		{
			InitializeComponent();

			T_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };
			C_Channel = new ClockGenerator { LowLevelDuration_sec = 1.8f, HighLevelDuration_sec = 0.2f };
			Trigger = new T_Trigger(T_Channel,C_Channel);

			new InputToClockGeneratorAutosyncer(T_Channel, T_HighVoltageInput, T_LowVoltageInput, T_HightVoltageDuration, T_LowVoltageDuration, T_Hz);
			new InputToClockGeneratorAutosyncer(C_Channel, C_HighVoltageInput, C_LowVoltageInput, C_HightVoltageDuration, C_LowVoltageDuration, C_Hz);

			var ChartSyncer = new ChartToHistoryAutosyncer();
			ChartSyncer.Pairs.Add((T_TriggerChart, Trigger.OutputHistory));
			ChartSyncer.Pairs.Add((D_Chart, T_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((C_Chart, C_Channel.OutputHistory));
		}
	}
}
