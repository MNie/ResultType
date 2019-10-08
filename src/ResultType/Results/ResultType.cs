namespace ResultType.Results
{
    using System;


    public interface IResult<TPayload> : IEquatable<IResult<TPayload>>
    {
    }
}
