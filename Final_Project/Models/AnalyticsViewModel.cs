namespace Final_Project.Models
{
    public class AnalyticsViewModel
    {
        // 1. Leaderboard
        public List<LeaderboardEntry> Leaderboard { get; set; }

        // 2. Topics per Course
        public List<ChartDataPoint> TopicsPerCourse { get; set; }

        // 3. Materials per Type
        public List<ChartDataPoint> MaterialsByType { get; set; }

        // 4. Personal Progress (Last 7 attempts)
        public List<int> PersonalScores { get; set; }
        public List<string> PersonalDates { get; set; }
    }
}

public class LeaderboardEntry
{
    public string Name { get; set; }
    public int TotalScore { get; set; }
}

public class ChartDataPoint
{
    public string Label { get; set; }
    public int Value { get; set; }
}