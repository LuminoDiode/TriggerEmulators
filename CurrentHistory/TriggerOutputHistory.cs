using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Emulators
{
	public class TriggerOutputHistory:AHistory
	{
		public TriggerOutputHistory(RS_Trigger TrackedTrigger):base(TrackedTrigger)
		{

			TrackedTrigger.StateChanged+=PushRecordFromTracked;
		}

		/// <summary>
		/// Returns points for building chart of current history.
		/// </summary>
		/// <param name="OneSecondScale"></param>
		/// <param name="OneVoltScale"></param>
		/// <returns></returns>

		private float StateToVolt(STATES state) => state switch
		{
			STATES.Q_IsOne => 1f,
			STATES.Q_IsZero => 0f,
			STATES.Q_IsIncorrect => float.NaN,
			_ => throw new NotImplementedException()
		};

		private long DiffToTicks(DateTime dt1, DateTime dt2)
			=> dt1.Ticks - dt2.Ticks;
	}
	public class OutputRecord
	{
		public STATES QState;
		public DateTime TimeCreated;
	}
}
