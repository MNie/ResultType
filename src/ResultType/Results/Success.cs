namespace ResultType.Results
{
    using System.Collections.Generic;

    public class Success<TPayload> : IResult<TPayload>
    {
        private readonly TPayload _payload;
        public TPayload Payload => _payload;
        internal Success(TPayload value) => _payload = value;

        public bool Equals(IResult<TPayload>? other) =>
            other switch
                {
                Success<TPayload> x => EqualityComparer<TPayload>.Default.Equals(_payload, x._payload),
                _ => false
                };
    }
}