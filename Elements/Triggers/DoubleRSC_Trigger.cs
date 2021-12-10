using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	class DoubleRSC_Trigger:ATrigger
	{
		RSC_Trigger FirstTrigger;
		RSC_Trigger SecondTrigger;

		public ICurrentSource S_ChannelInput
		{
			get => base.S_ChannelInput;
			set => base.S_ChannelInput = value;
		}
		public ICurrentSource C_ChannelInput
		{
			get => base.C_ChannelInput;
			set => base.C_ChannelInput = value;
		}
		public ICurrentSource R_ChannelInput
		{
			get => base.R_ChannelInput;
			set => base.R_ChannelInput = value;
		}

		public DoubleRSC_Trigger()
		{
			FirstTrigger = new RSC_Trigger();
			SecondTrigger = new RSC_Trigger();

			this.FirstTrigger.S_ChannelInput = this.S_ChannelInput;
			this.FirstTrigger.C_ChannelInput= this.C_ChannelInput;
			this.FirstTrigger.R_ChannelInput= this.R_ChannelInput;

			this.SecondTrigger.S_ChannelInput = FirstTrigger.Q_Channel_Output;
			this.FirstTrigger.C_ChannelInput = new LogicalInvertor(this.C_ChannelInput);
			this.SecondTrigger.R_ChannelInput = FirstTrigger.InvQ_Channel_Input;
		}
		protected override void CalculateState()
		{
			this.CurrentState = SecondTrigger.CurrentState;
		}
		protected override void OnAnyVoltageChange(object senrder, EventArgs e)
		{

		}
	}
}
