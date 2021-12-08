using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emulators;

namespace Emulators
{
	public class RS_Trigger : ATrigger
	{
		public ICurrentSource S_ChannelInput
		{
			get => base.S_ChannelInput;
			set => base.S_ChannelInput = value;
		}
		public ICurrentSource R_ChannelInput
		{
			get => base.R_ChannelInput;
			set => base.R_ChannelInput = value;
		}

		public RS_Trigger()
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
			if (false)
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
			// Состояние непредсказуемо, запрещенная комбинация
			if ((R_ChannelInput.CurrentLevel_Volt > 0 && S_ChannelInput.CurrentLevel_Volt > 0))
			{
				this.CurrentState = TRIGGER_STATES.Q_IsIncorrect;
				return;
			}
			// 1-0 Сохранение текущего состояния
			if (S_ChannelInput.CurrentLevel_Volt > 0)
			{
				return;
			}
			// 0-1 Установка в 0
			if (R_ChannelInput.CurrentLevel_Volt > 0)
			{
				this.CurrentState = TRIGGER_STATES.Q_IsZero;
				return;
			}
			// 0-0 Установка в 1
			else
			{
				this.CurrentState = TRIGGER_STATES.Q_IsOne;
				return;
			}
		}
	}
}
