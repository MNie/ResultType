namespace ResultType.Results
{
    using System.Collections.Generic;
    using System.Linq;

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
    
    public class AggregateError : IError
    {
        public string Message { get; }
        public IReadOnlyCollection<IError> Errors { get; }

        public AggregateError(IEnumerable<IError> errors) => Errors = errors?.ToList() ?? new List<IError>();

        public Error Flatten => new Error(string.Concat(", ", Errors.Select(x => x.Message)));
    }
}