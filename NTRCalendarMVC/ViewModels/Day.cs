using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTRCalendarMVC.ViewModels
{
    public class Day
    {
        public List<Appointment> Appointments { get; set; }
        public DateTime Date { get; set; }
        public string Name => Date.ToString("dd MMMM");

        public Day() {
            Appointments = new List<Appointment>();
        }
    }
}