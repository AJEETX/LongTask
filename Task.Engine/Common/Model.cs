using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongTask.Engine.Common
{
    public class LogData
    {
        public int SerialNo { get; set; }
        public int ThreadID { get; set; }
        public int Workers { get; set; }
        public int IOWorker { get; set; }
        public string Time { get; set; }
    }
}
