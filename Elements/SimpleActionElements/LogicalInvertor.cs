using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
    class LogicalInvertor : ICurrentSource
    {
		public event EventHandler OutputChanged;

		private ICurrentSource _InputChannel;
		public ICurrentSource InputChannel
		{
			get=> _InputChannel;
			set
			{
				_InputChannel = value;
				value.OutputChanged += OnAnyVoltageChange;
			}
		}

		private float _CurrentLevel_Volt;
		public float CurrentLevel_Volt
		{
			get => _CurrentLevel_Volt;
			set
			{
				_CurrentLevel_Volt = value;
				OutputChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public CurrentHistory OutputHistory { get; }
		public LogicalInvertor(ICurrentSource InputChannel)
		{
			this.InputChannel = InputChannel;
			this.InputChannel.OutputChanged += OnAnyVoltageChange;
			this.OutputHistory = new CurrentHistory(this);
		}

		private void OnAnyVoltageChange(object? sender, EventArgs e)
		{
			CalculateState();
		}

		private void CalculateState()
		{
			if (this.CurrentLevel_Volt == 0f) 
				CurrentLevel_Volt = 1;
			else 
				this.CurrentLevel_Volt = -(this.InputChannel.CurrentLevel_Volt);
		}
	}
}
