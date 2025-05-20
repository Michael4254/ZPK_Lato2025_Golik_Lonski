using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace arkanoid_component
{
    public class ScoreEntry
    {
        public string Nickname { get; set; } = "";
        public int Points { get; set; }      // zamiast Level
        public DateTime Date { get; set; }
    }

    public static class RankingManager
    {
        private static readonly string FilePath =
            Path.Combine(Application.UserAppDataPath, "ranking.json");

        public static List<ScoreEntry> LoadScores()
        {
            try
            {
                if (!File.Exists(FilePath)) return new List<ScoreEntry>();
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<ScoreEntry>>(json)
                       ?? new List<ScoreEntry>();
            }
            catch
            {
                return new List<ScoreEntry>();
            }
        }

        public static void AddScore(ScoreEntry entry)
        {
            var list = LoadScores();
            list.Add(entry);
            list = list
                .OrderByDescending(s => s.Points)  // sortuj po Points
                .ThenBy(s => s.Date)
                .Take(20)
                .ToList();
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(list));
        }
    }
}
