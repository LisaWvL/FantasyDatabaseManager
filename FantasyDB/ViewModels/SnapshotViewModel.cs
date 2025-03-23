using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using FantasyDB.Services;

namespace FantasyDB.ViewModels
{
    public class SnapshotViewModel : IViewModelWithId
    {

        public int Id { get; set; }
        public string? Book { get; set; } = string.Empty;
        public string? Act { get; set; } = string.Empty;
        public string? Chapter { get; set; } = string.Empty;
        public string? SnapshotName { get; set; } = string.Empty; // ✅  loaded from DB
    }

}
