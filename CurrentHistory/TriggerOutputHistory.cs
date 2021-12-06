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

		}
	}
}
