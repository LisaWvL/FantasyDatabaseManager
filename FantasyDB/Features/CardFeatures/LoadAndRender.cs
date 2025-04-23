//// ✅ Full implementation of LoadAndRender using real data

//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using FantasyDB.Entities._Shared;
//using FantasyDB.Entities;

//namespace FantasyDB.Features
//{
//    public class LoadAndRender
//    {
//        private readonly AppDbContext _context;
//        private readonly CardDesigner _cardDesigner;

//        public LoadAndRender(AppDbContext context, CardDesigner cardDesigner)
//        {
//            _context = context;
//            _cardDesigner = cardDesigner;
//        }

//        public async Task<List<CardRenderResponse>> Load(string entityType, string context)
//        {
//            switch (entityType.ToLower())
//            {
//                case "plotpoint":
//                    var plotPoints = await _context.PlotPoints
//                        .Include(p => p.StartDate)
//                        .Include(p => p.EndDate)
//                        .ToListAsync();
//                    return (await Task.WhenAll(plotPoints.Select(p => _cardDesigner.DesignCard("PlotPoint", p, context)))).ToList();

//                case "event":
//                    var events = await _context.Events
//                        .Include(e => e.StartDate)
//                        .Include(e => e.EndDate)
//                        .ToListAsync();
//                    return (await Task.WhenAll(events.Select(e => _cardDesigner.DesignCard("Event", e, context)))).ToList();

//                case "era":
//                    var eras = await _context.Eras
//                        .Include(e => e.StartDate)
//                        .Include(e => e.EndDate)
//                        .ToListAsync();
//                    return (await Task.WhenAll(eras.Select(e => _cardDesigner.DesignCard("Era", e, context)))).ToList();

//                case "chapter":
//                    var chapters = await _context.Chapters
//                        .Include(c => c.StartDate)
//                        .Include(c => c.EndDate)
//                        .ToListAsync();
//                    return (await Task.WhenAll(chapters.Select(c => _cardDesigner.DesignCard("Chapter", c, context)))).ToList();

//                default:
//                    return new List<CardRenderResponse>();
//            }
//        }
//    }
//}
