namespace Process.Domain.Utilities
{
    public static class AuditJsonHelper
    {
        private const string KEY = "validationProcessId";

        public static long? ExtractValidationProcessId(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            var keyPosition = FindKeyPosition(json);
            if (keyPosition < 0)
                return null;

            var numberText = ExtractNumber(json, keyPosition);

            return ParseId(numberText);
        }

        private static int FindKeyPosition(string json)
        {
            return json.IndexOf(KEY, StringComparison.OrdinalIgnoreCase);
        }

        private static string ExtractNumber(string json, int keyPosition)
        {
            var colonPosition = json.IndexOf(':', keyPosition);
            if (colonPosition < 0)
                return string.Empty;

            var start = SkipWhitespaces(json, colonPosition + 1);
            var end = ReadDigits(json, start);

            return json[start..end];
        }

        private static int SkipWhitespaces(string json, int position)
        {
            while (position < json.Length && char.IsWhiteSpace(json[position]))
                position++;

            return position;
        }

        private static int ReadDigits(string json, int position)
        {
            var end = position;

            while (end < json.Length && char.IsDigit(json[end]))
                end++;

            return end;
        }

        private static long? ParseId(string number)
        {
            return long.TryParse(number, out var id)
                ? id
                : null;
        }
    }
}
