namespace ResultType.Results
{
    public interface IError
    {
        string Message { get; }
    }

    public class Error : IError
    {
        public string Message { get; }

        public Error(string msg)
        {
            Message = msg;
        }
    }
}