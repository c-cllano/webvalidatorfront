namespace Process.Domain.Utilities
{
    public static class ScoreRulesHelper
    {
        private static readonly List<(decimal Min, decimal Max, int Result)> Rules =
        [
            (0.00m, 20.00m, 3),
            (21.00m, 80.00m, 2),
            (81.00m, 100.00m, 1)
        ];

        public static int GetResult(decimal value)
        {
            var rule = Rules.FirstOrDefault(r => value >= r.Min && value <= r.Max);

            if (rule == default)
            {
                throw new ArgumentOutOfRangeException($"El valor {value} no entra en ningún rango definido.");
            }

            return rule.Result;
        }

        public static decimal HomologationScoreMegvi(decimal score)
        {
            return Math.Round(score switch
            {
                100m => 100m,
                > 95m => 99.5m,
                > 90m => 99m,
                > 85m => 98.5m,
                > 80m => 98m,
                _ => score
            }, 2);
        }
    }
}
