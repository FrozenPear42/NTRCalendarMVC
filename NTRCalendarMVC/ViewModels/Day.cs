using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTRCalendarMVC.ViewModels
{
    public class Day
    {
        public string Name { get; set; }
        public List<Appointment> Appointments { get; set; }

        public Day() {
            Appointments = new List<Appointment>();
        }
    }
}