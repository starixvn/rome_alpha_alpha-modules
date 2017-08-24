namespace Alpha.IntegrationTests.Extensions
{
    public static class StringExtension
    {
        public static string StandardizeNewLine(this string value)
            => value.Replace("\r\n", "\n");
    }
}
