using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	class T_Trigger : ATrigger
	{
		/* ...Как можно видеть в таблице истинности JK-триггера, 
		 * он переходит в инверсное состояние каждый раз 
		 * при одновременной подаче на входы J и K логической 1. 
		 * Это свойство позволяет создать на базе JK-триггера Т-триггер, 
		 * объединяя входы J и К.
		 */
		private JK_Trigger thisJK;

		public ICurrentSource T_ChannelInput
		{
			get => this.thisJK.J_ChannelInput;
			set => this.thisJK.J_ChannelInput = this.thisJK.K_ChannelInput = value;
		}
		public ICurrentSource C_ChannelInput
		{
			get => this.thisJK.C_ChannelInput;
			set => this.thisJK.C_ChannelInput=value;
		}

		public CurrentHistory OutputHistory => thisJK.OutputHistory;
		public T_Trigger(ICurrentSource T_ChannelInput, ICurrentSource C_ChannelInput) : base()
		{
			thisJK = new JK_Trigger();
			this.T_ChannelInput = T_ChannelInput;
			this.C_ChannelInput = C_ChannelInput;
			this.C_ChannelInput.OutputChanged += this.OnAnyVoltageChange;
		}

		protected override void CalculateState()
		{
			this.CurrentState=thisJK.CurrentState;
		}
		protected override void OnAnyVoltageChange(object? sender, EventArgs e)
		{
			CalculateState();
		}
	}
}
