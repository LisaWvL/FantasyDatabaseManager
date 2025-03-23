using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class CalendarViewModel
    {
        public string Weekdays { get; set; } = "Solrun, Lura Stellis Embra Umbrin Even Auro"; // Store weekdays as JSON
        public string Months { get; set; } = "Aurn, Brol, Chana, Drom, Eice, Fram, Gila, Heno, Irrst, Jart, Kwarm, Lehnd, Jespen"; // Store months as JSON
        public int DaysPerWeek { get; set; } = 7;
        public int MonthsPerYear { get; set; } = 13;
        public int DaysPerYear { get; set; } = 365;
    }
}
