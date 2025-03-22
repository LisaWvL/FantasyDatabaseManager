using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class SnapshotViewModel
    {
        public string? Book { get; set; } = string.Empty;
        public string? Act { get; set; } = string.Empty;
        public string? Chapter { get; set; } = string.Empty;
        public string? SnapshotName { get; set; } = string.Empty; // ✅  loaded from DB
    }

}
