using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UserModel
{
    public class Logger
    {
        public List<Log> Logs { get; set; }

        public Logger()
        {
            Logs = new List<Log>();
        }
    }
    public class Log
    {
        public LogType? Status { get; set; }
        public readonly DateTime DateTime;
        public Log() 
        {
            DateTime = DateTime.Now;
        }
    }

    public enum LogType
    {
        AddUser,
        PutBot,
        PutUser,
        GetAnswers
    }
}
