using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Media;
using System.Drawing;

namespace Emulators
{
	public abstract class AHistory
	{
		protected static long NowTicks => DateTime.UtcNow.Ticks;

		public event EventHandler? HistoryChanged;

		protected Timer HistoryRecordInvoker = new Timer { Interval = 100d, AutoReset = true, Enabled = true };

		public int RecordInterval_msec
		{
			get => (int)HistoryRecordInvoker.Interval;
			set => HistoryRecordInvoker.Interval = value;
		}

		public int RecordsStored { get; set; }
		public int SecondsStored
		{
			get => RecordsStored / RecordInterval_msec;
			set => RecordsStored = value * 1000 / RecordInterval_msec;
		}


		public List<CurrentRecord> Records { get; protected set; } = new();

		protected ICurrentGenerator _TrackedGenerator;
		public AHistory(ICurrentGenerator TrackedGenerator)
		{
			this._TrackedGenerator = TrackedGenerator;
			this._TrackedGenerator.HistoryRecordInvoker += PushRecordFromTracked;
			PushRecordFromTracked(this,EventArgs.Empty);
		}

		protected void PushRecordFromTracked(object? sender, EventArgs e)
		{
			var record = new CurrentRecord
			{
				Voltage_Volt = this._TrackedGenerator.CurrentLevel_Volt,
				TimeCreated = DateTime.UtcNow
			};

			Records.Add(record);

			while ((Records.Count > 2) && ((DateTime.UtcNow - Records[1].TimeCreated).Seconds > SecondsStored))
				Records.RemoveAt(0);

			HistoryChanged?.Invoke(this, EventArgs.Empty);
		}


		public List<(PointF p1, PointF p2)> GetChartLines(float PointIntervalMsec = 100)
		{
			List<(PointF p1, PointF p2)> Out = new List<(PointF p1, PointF p2)>((int)(SecondsStored / PointIntervalMsec + 0.5f));

			for (int i = 0; (i * PointIntervalMsec) < SecondsStored*1000; i++)
			{
				var currRec = GetRecordByTime(DateTime.UtcNow.AddSeconds(-(i * PointIntervalMsec / 1000)));
				var nextRec = GetRecordByTime(DateTime.UtcNow.AddSeconds(-((i + 1) * PointIntervalMsec / 1000)));
				//if (currRec == nextRec) break;

				Out.Add((
					new PointF(currRec.TimeCreated.Ticks, currRec.Voltage_Volt),
					new PointF(nextRec.TimeCreated.Ticks, nextRec.Voltage_Volt)
					));
			}
			return Out;
		}
		protected CurrentRecord GetRecordByTime(DateTime TimeMoment)
		{
			var id = Records.FindIndex(x => (x.TimeCreated - TimeMoment).Ticks < 0);
			return id!=-1 ? Records[id]: Records[Records.Count - 1];
		}
	}
}
