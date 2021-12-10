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
using System.Runtime.InteropServices;

namespace Emulators
{
	public partial class RS_Trigger_Page : Page
	{
		RS_Trigger Trigger;
		ClockGenerator R_Channel;
		ClockGenerator S_Channel;
		public RS_Trigger_Page()
		{
			InitializeComponent();

			Trigger = new RS_Trigger();
			Trigger.R_ChannelInput = R_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };
			Trigger.S_ChannelInput =S_Channel = new ClockGenerator { LowLevelDuration_sec = 1.8f, HighLevelDuration_sec = 0.2f };

			new InputToClockGeneratorAutosyncer(R_Channel, R_HighVoltageInput, R_LowVoltageInput, R_HightVoltageDuration, R_LowVoltageDuration, R_Hz);
			new InputToClockGeneratorAutosyncer(S_Channel, S_HighVoltageInput, S_LowVoltageInput, S_HightVoltageDuration, S_LowVoltageDuration, S_Hz);

			var ChartSyncer = new ChartToHistoryAutosyncer();
			ChartSyncer.Pairs.Add((RS_TriggerChart, Trigger.OutputHistory));
			ChartSyncer.Pairs.Add((R_Chart, R_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((S_Chart, S_Channel.OutputHistory));
		}
	}
}
