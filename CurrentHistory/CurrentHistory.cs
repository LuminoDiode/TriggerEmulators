using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Emulators
{
	public class CurrentHistory:AHistory
	{
		public CurrentHistory(ClockGenerator TrackedGenerator) : base(TrackedGenerator)
		{
			
		}
	}
}
