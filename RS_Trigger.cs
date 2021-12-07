using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emulators;

namespace Emulators
{
	public enum STATES
	{
		Q_IsIncorrect = -1,
		Q_IsZero = 0,
		Q_IsOne = 1
	}
	public class RS_Trigger:ICurrentGenerator
	{

		
		// Режим синхронизиуемого триггера
		public bool IsInClocked_Mode { get; set; } = false;
		public event EventHandler StateChanged;
		public event EventHandler OutputChanged
		{
			add => StateChanged += value;
			remove => StateChanged -= value;
		}
		public event EventHandler StatePendingChange;

		private ICurrentGenerator _S_ChannelInput;
		public ICurrentGenerator S_ChannelInput
		{
			get => _S_ChannelInput;
			set
			{
				value.OutputChanged += OnAnyVoltageChange;
				_S_ChannelInput = value;
			}
		}
		private ICurrentGenerator _C_ChannelInput;
		public ICurrentGenerator C_ChannelInput
		{
			get => _C_ChannelInput;
			set
			{
				value.OutputChanged += OnAnyVoltageChange;
				_C_ChannelInput = value;
			}
		}
		private ICurrentGenerator _R_ChannelInput;
		public ICurrentGenerator R_ChannelInput
		{
			get => _R_ChannelInput;
			set
			{
				value.OutputChanged += OnAnyVoltageChange;
				_R_ChannelInput = value;
			}
		}

		public float CurrentLevel_Volt
		{
			get => StateToVolt(this.CurrentState);
		}
		private float StateToVolt(STATES state) => state switch
		{
			STATES.Q_IsOne => 1f,
			STATES.Q_IsZero => 0f,
			STATES.Q_IsIncorrect => float.NaN,
			_ => throw new NotImplementedException()
		};

		private STATES _CurrentState;
		public STATES CurrentState
		{
			get => _CurrentState;
			set
			{
				StatePendingChange?.Invoke(this, EventArgs.Empty);
				_CurrentState = value;
				CurrentStateStartedDateTime = DateTime.UtcNow;
				StateChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		public DateTime CurrentStateStartedDateTime { get; private set; }

		public CurrentHistory History { get; private set; }

		public RS_Trigger()
		{
			this.S_ChannelInput = new ConstantCurrentSource();
			this.C_ChannelInput = new ClockGenerator();
			this.R_ChannelInput = new ConstantCurrentSource();
			this.History = new CurrentHistory(this);


			((ClockGenerator)(C_ChannelInput)).ExecuteAsync(new System.Threading.CancellationToken(false));


			S_ChannelInput.OutputChanged += OnAnyVoltageChange;
			C_ChannelInput.OutputChanged += OnAnyVoltageChange;
			R_ChannelInput.OutputChanged += OnAnyVoltageChange;
		}

		public void OnAnyVoltageChange(object sender, EventArgs e)
		{
			// Режим с синхронизацией
			if (this.IsInClocked_Mode)
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
			if (R_ChannelInput.CurrentLevel_Volt > 0 && S_ChannelInput.CurrentLevel_Volt > 0)
			{
				this.CurrentState = STATES.Q_IsIncorrect;
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
				this.CurrentState = STATES.Q_IsZero;
				return;
			}
			// 0-0 Установка в 1
			else
			{
				this.CurrentState = STATES.Q_IsOne;
				return;
			}
		}
		private void InvertState()
		{
			if (this.CurrentState == STATES.Q_IsZero)
				this.CurrentState = STATES.Q_IsOne;
			else
				this.CurrentState = STATES.Q_IsZero;
		}
	}
}
