namespace ResultType.Results
{
    public interface IError
    {
        string Message { get; }
        string MemberName { get; }
        string FilePath { get; }
        int Line { get; }
    }

    public class Error : IError
    {
        public string Message { get; }
        public string MemberName { get; }
        public string FilePath { get; }
        public int Line { get; }

        public Error(string msg, string memberName, string filePath, int line)
        {
            Message = msg;
            MemberName = memberName;
            FilePath = filePath;
            Line = line;
        }
    }
}