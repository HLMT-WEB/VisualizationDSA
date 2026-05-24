namespace VisualizationDSA.Application.Constants
{
    public static class CacheKeys
    {
        public const string AlgorithmList = "algorithms:list";
        public const string AlgorithmMetadataPrefix = "algorithms:metadata:";
        public const string QuizList = "quizzes:list";
        public const string QuizByIdPrefix = "quizzes:id:";
        public const string QuizByTopicPrefix = "quizzes:topic:";
        public const string BadgeList = "badges:list";
        public const string LeaderboardPrefix = "leaderboard:top:";
    }

    public static class CacheDurations
    {
        public static readonly System.TimeSpan AlgorithmMetadata = System.TimeSpan.FromHours(24);
        public static readonly System.TimeSpan QuizList = System.TimeSpan.FromMinutes(30);
        public static readonly System.TimeSpan BadgeList = System.TimeSpan.FromHours(1);
        public static readonly System.TimeSpan Leaderboard = System.TimeSpan.FromMinutes(5);
    }
}
