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
	/// Interaction logic for JK_Trigger_Page.xaml
	/// </summary>
	public partial class JK_Trigger_Page : Page
	{
		JK_Trigger Trigger;
		ClockGenerator J_Channel;
		ClockGenerator C_Channel;
		ClockGenerator K_Channel;
		public JK_Trigger_Page()
		{
			InitializeComponent();

			Trigger = new JK_Trigger();
			Trigger.J_ChannelInput = J_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };
			Trigger.C_ChannelInput = C_Channel = new ClockGenerator { LowLevelDuration_sec=0.2f,HighLevelDuration_sec=0.8f };
			Trigger.K_ChannelInput = K_Channel = new ClockGenerator { LowLevel_Volt = 1, HighLevel_Volt = 1 };

			new InputToClockGeneratorAutosyncer(J_Channel, J_HighVoltageInput, J_LowVoltageInput, J_HightVoltageDuration, J_LowVoltageDuration, J_Hz);
			new InputToClockGeneratorAutosyncer(C_Channel, C_HighVoltageInput, C_LowVoltageInput, C_HightVoltageDuration, C_LowVoltageDuration, C_Hz);
			new InputToClockGeneratorAutosyncer(K_Channel, K_HighVoltageInput, K_LowVoltageInput, K_HightVoltageDuration, K_LowVoltageDuration, K_Hz);

			var ChartSyncer = new ChartToHistorySyncer(); 
			ChartSyncer.Pairs.Add((J_Chart, J_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((C_Chart, C_Channel.OutputHistory));
			ChartSyncer.Pairs.Add((K_Chart, K_Channel.OutputHistory));
		}
	}
}
