namespace ResultType.Tests
{
    using ResultType.Results;

    internal class TestError : IError
    {
        public string Message { get; }
        public string MemberName { get; }
        public string FilePath { get; }
        public int Line { get; }
        public TestError(string message, string memberName, string filePath, int line)
        {
            Message = message;
            MemberName = memberName;
            FilePath = filePath;
            Line = line;
        }
    }
}
