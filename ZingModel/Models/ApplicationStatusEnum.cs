using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZingModel.Models
{
    class ApplicationStatusEnum
    {
        
        public static string Open { get { return "Open"; } }
        public static string ReportAsReady { get { return "ReportAsReady"; } }
        public static string Approved { get { return "Approved"; } }
        public static string Rejected { get { return "Rejected"; } }
        public static string Submitted { get { return "Submitted"; } }
        public static string ChangeRequest { get { return "ChangeRequest"; } }
      
    }
}
