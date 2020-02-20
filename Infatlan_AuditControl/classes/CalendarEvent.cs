using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infatlan_AuditControl.classes
{
    public class CalendarEvent
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public bool allDay { get; set; }
        public int status { get; set; }
    }
}