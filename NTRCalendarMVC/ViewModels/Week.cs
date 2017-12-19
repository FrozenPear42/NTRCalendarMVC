using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTRCalendarMVC.ViewModels {
    public class Week {
        public int Number { get; set; }
        public int Year { get; set; }
        public List<Day> Days { get; set; }
    }
}