using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
{
    public class SceneViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public int? SceneNumber { get; set; } = 0;
        public string? SceneTitle { get; set; } = string.Empty;
        public string? SceneText { get; set; }
        public int? SceneWordCount { get; set; } = 0;
        public string? SceneSummary { get; set; }

        public string? SceneToDo { get; set; }

        public int? ChapterId { get; set; } = 0;
        public string? ChapterTitle { get; set; } = string.Empty;// Readable Name
        public int? ChapterNumber { get; set; } = 0;//Readable Number
    }

}
