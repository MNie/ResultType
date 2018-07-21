namespace ResultType.Tests
{
    using ResultType.Results;

    internal class TestError : IError
    {
        public string Message { get; }
        public TestError(string message) => Message = message;
    }
}
