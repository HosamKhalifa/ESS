using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZingModel.Models
{
    class ApplicationTypeEnum
    {
       
        public static string Leave { get { return "Leave"; }}
        public static string Termination { get { return "Termination"; } }
        public static string Resignation { get { return "Resignation"; } }
        public static string Escape { get { return "Escape"; } }
        public static string Death { get { return "Death"; } }
    }
}
