using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	class JK_Trigger : ATrigger
	{
		public ICurrentSource J_ChannelInput
		{
			get => base.S_ChannelInput;
			set => base.S_ChannelInput = value;
		}
		public ICurrentSource C_ChannelInput
		{
			get => base.C_ChannelInput;
			set => base.C_ChannelInput = value;
		}
		public ICurrentSource K_ChannelInput
		{
			get => base.R_ChannelInput;
			set => base.R_ChannelInput = value;

		}

		public JK_Trigger()
		{
			this.S_ChannelInput = new ConstantCurrentSource();
			this.C_ChannelInput = new ClockGenerator();
			this.R_ChannelInput = new ConstantCurrentSource();
			base.OutputHistory = new CurrentHistory(this);

			S_ChannelInput.OutputChanged += OnAnyVoltageChange;
			C_ChannelInput.OutputChanged += OnAnyVoltageChange;
			R_ChannelInput.OutputChanged += OnAnyVoltageChange;
		}

		protected override void OnAnyVoltageChange(object sender, EventArgs e)
		{
			// Режим с синхронизацией
			if (true)
			{
				// сигнал с тактирующего входа
				if (sender == this.C_ChannelInput)
				{
					// сигнал положительной величины
					if (this.C_ChannelInput.CurrentLevel_Volt > 0)
					{
						CalculateState();
					}
				}
			}
			// Режим без синхронизации
			else
			{
				CalculateState();
			}
		}

		protected override void CalculateState()
		{
			// интвертировать состояние
			if ((K_ChannelInput.CurrentLevel_Volt > 0 && J_ChannelInput.CurrentLevel_Volt > 0))
			{
				this.InvertState();
				return;
			}
			// 1-0 Установка в 1
			if (J_ChannelInput.CurrentLevel_Volt > 0)
			{
				this.CurrentState = TRIGGER_STATES.Q_IsOne;
				return;
			}
			// 0-1 Установка в 0
			if (K_ChannelInput.CurrentLevel_Volt > 0)
			{
				this.CurrentState = TRIGGER_STATES.Q_IsZero;
				return;
			}
			// 0-0 Сохранение состояния
			else
			{
				return;
			}
		}
		private void InvertState()
		{
			if (this.CurrentState == TRIGGER_STATES.Q_IsZero)
				this.CurrentState = TRIGGER_STATES.Q_IsOne;
			else
				this.CurrentState = TRIGGER_STATES.Q_IsZero;
		}
	}
}
