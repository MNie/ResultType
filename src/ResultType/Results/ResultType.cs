namespace ResultType.Results
{
    using System;
    using System.Collections.Generic;

    public class Result<TPayload> : IEquatable<Result<TPayload>>
    {
        private readonly TPayload _payload;
        private readonly IError _error;

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public TPayload Payload => IsSuccess ? _payload : throw new InvalidOperationException(_error.Message);
        public IError Error => IsFailure ? _error : throw new InvalidOperationException();

        internal Result(TPayload value)
        {
            _payload = value;
            IsSuccess = true;
        }

        internal Result(IError value)
        {
            _error = value;
            IsSuccess = false;
        }

        public static bool operator ==(Result<TPayload> first, Result<TPayload> second)
        {
            if (first.IsSuccess == second.IsSuccess)
            {
                return first.IsSuccess
                    ? first.Payload.Equals(second.Payload) 
                    : first.Error.Equals(second.Error);
            }
                
            return false;
        }

        public static bool operator !=(Result<TPayload> first, Result<TPayload> second) => !(first == second);

        public bool Equals(Result<TPayload> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TPayload>.Default.Equals(_payload, other._payload) && Equals(_error, other._error) && IsSuccess == other.IsSuccess;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Result<TPayload>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<TPayload>.Default.GetHashCode(_payload);
                hashCode = (hashCode * 397) ^ (_error != null ? _error.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsSuccess.GetHashCode();
                return hashCode;
            }
        }
    }
}
