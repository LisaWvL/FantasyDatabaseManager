using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyDB.Services
{
    public interface IViewModelWithId
    {
        int Id { get; set; }
    }
}
