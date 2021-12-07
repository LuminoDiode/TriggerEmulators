using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
    public abstract class ATrigger :ICurrentSource,ITrigger
    {
		public bool IsInClockedMode { get; set; } = false;

		protected event EventHandler? StateChanged;
		public event EventHandler? OutputChanged
		{
			add => StateChanged += value;
			remove => StateChanged -= value;
		}
		protected event EventHandler? StatePendingChange;

		private TRIGGER_STATES _CurrentState;
		public TRIGGER_STATES CurrentState
		{
			get => _CurrentState;
			set
			{
				StatePendingChange?.Invoke(this, EventArgs.Empty);
				_CurrentState = value;
				StateChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public CurrentHistory OutputHistory { get; protected set; }

		public float CurrentLevel_Volt
		{
			get => StateToVolt(this.CurrentState);
		}
		private float StateToVolt(TRIGGER_STATES state) => state switch
		{
			TRIGGER_STATES.Q_IsOne => 1f,
			TRIGGER_STATES.Q_IsZero => 0f,
			TRIGGER_STATES.Q_IsIncorrect => float.NaN,
			_ => throw new NotImplementedException()
		};

		public ATrigger()
		{
			
		}
	}
}
