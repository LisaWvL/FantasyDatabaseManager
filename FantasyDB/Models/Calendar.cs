using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Weekdays { get; set; } = "Solrun, Lura Stellis Embra Umbrin Even Auro"; // Store weekdays as JSON
        public string Months { get; set; } = "Aurn, Brol, Chana, Drom, Eice, Fram, Gila, Heno, Irrst, Jart, Kwarm, Lehnd, Jespen"; // Store months as JSON
        public int DaysPerWeek { get; set; } = 7;
        public int MonthsPerYear { get; set; } = 13;
        public int DaysPerYear { get; set; } = 365;
    }
}

