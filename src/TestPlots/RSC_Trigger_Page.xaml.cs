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
	public partial class RSC_Trigger_Page : Page
	{
		RSC_Trigger Trigger;
		ClockGenerator S_Channel;
		ClockGenerator C_Channel;
		ClockGenerator R_Channel;
		public RSC_Trigger_Page()
		{
			InitializeComponent();

			Trigger = new RSC_Trigger();
			Trigger.S_ChannelInput = S_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };
			Trigger.C_ChannelInput = C_Channel = new ClockGenerator { LowLevelDuration_sec = 1.8f, HighLevelDuration_sec = 0.2f };
			Trigger.R_ChannelInput = R_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };

			new InputToClockGeneratorAutosyncer(S_Channel, S_HighVoltageInput, S_LowVoltageInput, S_HightVoltageDuration, S_LowVoltageDuration, S_Hz);
			new InputToClockGeneratorAutosyncer(C_Channel, C_HighVoltageInput, C_LowVoltageInput, C_HightVoltageDuration, C_LowVoltageDuration, C_Hz);
			new InputToClockGeneratorAutosyncer(R_Channel, R_HighVoltageInput, R_LowVoltageInput, R_HightVoltageDuration, R_LowVoltageDuration, R_Hz);

			var ChartSyncer = new ChartToHistoryAutosyncer();
			ChartSyncer.Pairs.Add((JK_TriggerChart, Trigger.OutputHistory));
			ChartSyncer.Pairs.Add((S_Chart, S_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((C_Chart, C_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((R_Chart, R_Channel.OutputHistory));
		}
	}
}
