using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emulators;

namespace Emulators
{
	public enum TRIGGER_STATES
	{
		Q_IsIncorrect = -1,
		Q_IsZero = 0,
		Q_IsOne = 1
	}
	public class RS_Trigger: ATrigger, ICurrentSource
	{

		private ICurrentSource _S_ChannelInput;
		public ICurrentSource S_ChannelInput
		{
			get => _S_ChannelInput;
			set
			{
				value.OutputChanged += OnAnyVoltageChange;
				_S_ChannelInput = value;
			}
		}
		private ICurrentSource _C_ChannelInput;
		public ICurrentSource C_ChannelInput
		{
			get => _C_ChannelInput;
			set
			{
				value.OutputChanged += OnAnyVoltageChange;
				_C_ChannelInput = value;
			}
		}
		private ICurrentSource _R_ChannelInput;
		public ICurrentSource R_ChannelInput
		{
			get => _R_ChannelInput;
			set
			{
				value.OutputChanged += OnAnyVoltageChange;
				_R_ChannelInput = value;
			}
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

		public void OnAnyVoltageChange(object sender, EventArgs e)
		{
			// Режим с синхронизацией
			if (this.IsInClockedMode)
			{
				// есть сигнал с тактирующего входа
				if (this.C_ChannelInput.CurrentLevel_Volt>0)
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

		public void CalculateState()
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
		private void InvertState()
		{
			if (this.CurrentState == TRIGGER_STATES.Q_IsZero)
				this.CurrentState = TRIGGER_STATES.Q_IsOne;
			else
				this.CurrentState = TRIGGER_STATES.Q_IsZero;
		}
	}
}
