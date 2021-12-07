using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulators
{
	/// <summary>
	/// An abstract constant current generator.
	///	Default parameters is 1 Volt.
	/// </summary>
	public class ConstantCurrentSource : ICurrentSource, IRunable
	{
		private ClockGenerator _ClockGenerator;

		public bool IsRunning
		{
			get => _ClockGenerator.IsRunning;
			set => _ClockGenerator.IsRunning = value;
		}

		/// <summary>
		/// Invokes when <see cref="CurrentLevel_Volt"/> property changes.
		/// </summary>
		public event EventHandler? LevelChanged;
		public event EventHandler? OutputChanged;

		private float _CurrentLevel_Volt = 1;
		/// <summary>
		/// Generator's voltage
		/// </summary>
		public float CurrentLevel_Volt
		{
			get => _CurrentLevel_Volt;
			set
			{
				_ClockGenerator.LowLevel_Volt = _ClockGenerator.HighLevel_Volt = value;
				OutputChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Hostory of generator's current.
		/// Writes new record when generator's voltage changes.
		/// </summary>
		public CurrentHistory OutputHistory => _ClockGenerator.OutputHistory;
		public ConstantCurrentSource()
		{
			this._ClockGenerator = new ClockGenerator() { Clock_Hz = 1f, LowLevel_Volt = this.CurrentLevel_Volt, HighLevel_Volt = this.CurrentLevel_Volt };
		}
	}
}
