using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	internal class LogicalDelayer: ATrigger
	{
		public int DelayDepth = 1;

		public Queue<float> DelayedOutputValues = new();

		public ICurrentSource InputChannel;
		public ICurrentSource ClockChannel;

		public LogicalDelayer(ICurrentSource InputChannel, ICurrentSource ClockChannel)
		{
			this.InputChannel = InputChannel;
			this.ClockChannel = ClockChannel;
			base.OutputHistory = new CurrentHistory(this);

			ClockChannel.OutputChanged += OnAnyVoltageChange;
		}

		protected override void OnAnyVoltageChange(object? sender, EventArgs e)
		{
			if(sender == this.ClockChannel)
			{
				if (this.ClockChannel.CurrentLevel_Volt > 0)
				{
					CalculateState();
				}
			}
		}
		protected override void CalculateState()
		{
			this.DelayedOutputValues.Enqueue(InputChannel.CurrentLevel_Volt);
			while (this.DelayedOutputValues.Count > DelayDepth)
			{
				base.CurrentState = this.DelayedOutputValues.Dequeue()==0f? 
					TRIGGER_STATES.Q_IsZero:TRIGGER_STATES.Q_IsOne;
			}
		}
	}
}
