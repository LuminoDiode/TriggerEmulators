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
	/// Interaction logic for D_Trigger_Page.xaml
	/// </summary>
	/// 

	public partial class D_Trigger_Page : Page
	{
		D_Trigger Trigger;
		ClockGenerator D_Channel;
		ClockGenerator C_Channel;

		public D_Trigger_Page()
		{
			InitializeComponent();

			D_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };
			C_Channel = new ClockGenerator { LowLevelDuration_sec = 1.8f, HighLevelDuration_sec = 0.2f };
			Trigger = new D_Trigger(D_Channel,C_Channel);
		
			new InputToClockGeneratorAutosyncer(D_Channel, D_HighVoltageInput, D_LowVoltageInput, D_HightVoltageDuration, D_LowVoltageDuration, D_Hz);
			new InputToClockGeneratorAutosyncer(C_Channel, C_HighVoltageInput, C_LowVoltageInput, C_HightVoltageDuration, C_LowVoltageDuration, C_Hz);

			var ChartSyncer = new ChartToHistoryAutosyncer();
			ChartSyncer.Pairs.Add((D_TriggerChart, Trigger.OutputHistory));
			ChartSyncer.Pairs.Add((D_Chart, D_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((C_Chart, C_Channel.OutputHistory));
		}
	}
}
