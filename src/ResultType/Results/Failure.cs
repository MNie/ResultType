namespace ResultType.Results
{
    using System.Collections.Generic;

    public class Failure<TPayload> : IResult<TPayload>
    {
        private readonly IError _error;
        public IError Error => _error;
        
        internal Failure(IError value)
        {
            _error = value;
        }

        public bool Equals(IResult<TPayload>? other) =>
            other switch
                {
                Failure<TPayload> x => EqualityComparer<IError>.Default.Equals(_error, x._error),
                _ => false
                };
    }
}