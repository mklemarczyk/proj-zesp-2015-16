using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators {
	public static class EmulatorLogger {
		private static LinkedList<string> logs;

		static EmulatorLogger( ) {
			logs = new LinkedList<string>( );
		}

		public static void Log(string eventName, string description) {
			var log = string.Format("{0}: {1} - {2}", DateTime.Now, eventName, description);
			logs.AddLast(log);
		}

        public static IEnumerable<string> Logs { get { return logs; } }
	}
}
