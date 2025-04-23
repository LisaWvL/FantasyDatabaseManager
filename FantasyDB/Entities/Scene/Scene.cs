using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
{
    public class Scene
    {
        public int Id { get; set; }
        public int? SceneNumber { get; set; } = 0;
        public string? SceneTitle { get; set; } = string.Empty;
        public string? SceneText { get; set; }
        public int? SceneWordCount { get; set; } = 0;
        public string? SceneSummary { get; set; }
        public string? SceneToDo { get; set; }

        public int? ChapterId { get; set; }
        public virtual Chapter? Chapter { get; set; } = null!;
    }

}
