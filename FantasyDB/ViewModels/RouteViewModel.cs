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
    public class RouteViewModel : IViewModelWithId
    {

        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public int? Length { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public string? TravelTime { get; set; } = string.Empty;


        public int? FromLocationId { get; set; }
        public int? ToLocationId { get; set; }


        public string? ToLocationName { get; set; } = string.Empty;
        public string? FromLocationName { get; set; } = string.Empty;

    }
}
