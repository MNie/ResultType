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

#pragma warning disable 8610
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Success<TPayload>) obj);
        }
#pragma warning restore 8610

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<IError>.Default.GetHashCode(_error) * 397);
            }
        }

        public bool Equals(IResult<TPayload> other) =>
            other switch
                {
                Failure<TPayload> x => EqualityComparer<IError>.Default.Equals(_error, x._error),
                _ => false
                };

        public void Deconstruct(out IError error) => error = _error;
    }
}