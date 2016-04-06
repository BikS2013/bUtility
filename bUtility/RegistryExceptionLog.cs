using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class RegistryExceptionLog
    {
        string Source;
        Func<Exception, string> ToJSON;
        public RegistryExceptionLog( string source, Func<Exception, string> toJson)
        {
            Source = source;
            ToJSON = toJson;
        }


        public void ToInfoLog(string message, EventLogEntryType logType = EventLogEntryType.Information)
        {
            try
            {
                EventLog.WriteEntry(Source,message,logType);
            }
            catch
            {
                throw new Exception("Failed to Log Message: " + message);
            }
        }
        public void ToNotificationLog(string message, EventLogEntryType logType = EventLogEntryType.Warning)
        {
            try
            {
                EventLog.WriteEntry(Source, message, logType);
            }
            catch
            {
                throw new Exception("Failed to Log Message: " + message);
            }
        }
        public void ToNotificationLog(Exception ex, EventLogEntryType logType = EventLogEntryType.Warning)
        {
            try
            {
                EventLog.WriteEntry( Source, ToJSON(ex), logType);
            }
            catch
            {
                ToEventLog(ex.Message, EventLogEntryType.Error);
                throw new Exception("Error Logging Exception", ex);
            }
        }
        public void ToNotificationLog(Exception ex, string message, EventLogEntryType logType = EventLogEntryType.Error)
        {
            try
            {
                EventLog.WriteEntry( Source, message + ": " + ToJSON(ex), logType);
            }
            catch
            {
                ToEventLog(ex.Message, EventLogEntryType.Error);
                throw new Exception("Error Logging Exception with message: " + message, ex);
            }
        }

        public void ToEventLog(string message, EventLogEntryType logType = EventLogEntryType.Error)
        {
            try
            {
                EventLog.WriteEntry( Source, message, logType);
            }
            catch
            {
                throw new Exception("Failed to Log Message: " + message);
            }
        }
        public void ToEventLog(Exception ex, EventLogEntryType logType = EventLogEntryType.Error)
        {
            try
            {
                EventLog.WriteEntry( Source, ToJSON(ex), logType);
            }
            catch
            {
                ToEventLog(ex.Message, EventLogEntryType.Error);
                throw new Exception("Error Logging Exception", ex);
            }
        }
        public void ToEventLog(Exception ex, string message, EventLogEntryType logType = EventLogEntryType.Error)
        {
            try
            {
                EventLog.WriteEntry( Source, message + ": " + ToJSON(ex), logType);
            }
            catch
            {
                ToEventLog(ex.Message, EventLogEntryType.Error);
                throw new Exception("Error Logging Exception with message: " + message, ex);
            }
        }
    }
}
