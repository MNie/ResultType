namespace ResultType.Results
{
    using System.Collections.Generic;

    public class Success<TPayload> : IResult<TPayload>
    {
        private readonly TPayload _payload;
        public TPayload Payload => _payload;
        internal Success(TPayload value) => _payload = value;

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
                return (EqualityComparer<TPayload>.Default.GetHashCode(_payload) * 397);
            }
        }

        public bool Equals(IResult<TPayload> other) =>
            other switch
                {
                Success<TPayload> x => EqualityComparer<TPayload>.Default.Equals(_payload, x._payload),
                _ => false
                };
    }
}