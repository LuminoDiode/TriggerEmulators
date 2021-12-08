using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Emulators
{
	class InputToClockGeneratorAutosyncer
	{
		private ClockGenerator _ClockGenerator;

		private TextBox? HighLevel_Volt_TB;
		private TextBox? LowLevel_Volt_TB;
		private TextBox? HightLevelDuration_sec_TB;
		private TextBox? LowLevelDuration_sec_TB;
		private TextBox? Clock_Hz_TB;

		public InputToClockGeneratorAutosyncer(
			ClockGenerator SetableGenerator,
			TextBox? HighLevel_Volt_TB = null,
			TextBox? LowLevel_Volt_TB = null,
			TextBox? HightLevelDuration_sec_TB = null,
			TextBox? LowLevelDuration_sec_TB = null,
			TextBox? Clock_Hz_TB = null)
		{
			this._ClockGenerator = SetableGenerator;
			this.HighLevel_Volt_TB = HighLevel_Volt_TB;
			this.LowLevel_Volt_TB = LowLevel_Volt_TB;
			this.HightLevelDuration_sec_TB = HightLevelDuration_sec_TB;
			this.LowLevelDuration_sec_TB = LowLevelDuration_sec_TB;
			this.Clock_Hz_TB = Clock_Hz_TB;

			if (this.HighLevel_Volt_TB != null)
				this.HighLevel_Volt_TB.KeyDown += SyncByInput;
			if (this.LowLevel_Volt_TB != null)
				this.LowLevel_Volt_TB.KeyDown += SyncByInput;
			if (this.HightLevelDuration_sec_TB != null)
				this.HightLevelDuration_sec_TB.KeyDown += SyncByInput;
			if (this.LowLevelDuration_sec_TB != null)
				this.LowLevelDuration_sec_TB.KeyDown += SyncByInput;
			if (this.Clock_Hz_TB != null)
				this.Clock_Hz_TB.KeyDown += SyncByInput;

			SyncByOutput();
		}

		private void SyncByInput(object sender, EventArgs e)
		{
			try
			{
				if (sender == this.HighLevel_Volt_TB)
					this._ClockGenerator.HighLevel_Volt = float.Parse(this.HighLevel_Volt_TB.Text);
				if (sender == this.LowLevel_Volt_TB)
					this._ClockGenerator.LowLevel_Volt = float.Parse(this.LowLevel_Volt_TB.Text);
				if (sender == this.HightLevelDuration_sec_TB)
					this._ClockGenerator.HighLevelDuration_sec = float.Parse(this.HightLevelDuration_sec_TB.Text);
				if (sender == this.LowLevelDuration_sec_TB)
					this._ClockGenerator.LowLevelDuration_sec = float.Parse(this.LowLevelDuration_sec_TB.Text);
				if (sender == this.Clock_Hz_TB)
					this._ClockGenerator.Clock_Hz = float.Parse(this.Clock_Hz_TB.Text);

				SyncByOutput();
			}
			catch (FormatException)
			{
				return;
			}
		}
		private void SyncByOutput()
		{
			if (this.HighLevel_Volt_TB != null)
				this.HighLevel_Volt_TB.Text = this._ClockGenerator.HighLevel_Volt.ToString("F1");
			if (this.LowLevel_Volt_TB != null)
				this.LowLevel_Volt_TB.Text = this._ClockGenerator.LowLevel_Volt.ToString("F1");
			if (this.HightLevelDuration_sec_TB != null)
				this.HightLevelDuration_sec_TB.Text = this._ClockGenerator.HighLevelDuration_sec.ToString("F1");
			if (this.LowLevelDuration_sec_TB != null)
				this.LowLevelDuration_sec_TB.Text = this._ClockGenerator.LowLevelDuration_sec.ToString("F1");
			if (this.Clock_Hz_TB != null)
				this.Clock_Hz_TB.Text = this._ClockGenerator.Clock_Hz.ToString();
		}
	}
}
