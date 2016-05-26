using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Logger {
    public static class EmulatorLogger {
        private static LinkedList<string> logs;

        static EmulatorLogger( ) {
            logs = new LinkedList<string>( );
        }

        public static void Log(LogLevel level, EventType type, string message, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) {
            var log = string.Format("[{0}] {1}: {2} - {3}", level, DateTime.Now, type, message);
            logs.AddLast(log);
        }

        public static IEnumerable<string> Logs { get { return logs; } }
    }
}
