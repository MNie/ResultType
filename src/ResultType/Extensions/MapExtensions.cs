namespace ResultType.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Results;

    public static class MapExtensions
    {
        public static TOut Map<TIn, TOut>(this IResult<TIn> result, Func<TIn, TOut> onSuccess,
            Func<IError, TOut> onFailure) =>
            result switch
                {
                Success<TIn> s => onSuccess(s.Payload),
                Failure<TIn> e => onFailure(e.Error),
                _ => onFailure(new Error("invalid state of a Result"))
                };

        public static async Task<TOut> MapAsync<TIn, TOut>(this Task<IResult<TIn>> result, Func<TIn, Task<TOut>> onSuccess,
            Func<IError, Task<TOut>> onFailure)
        {
            var r = await result.ConfigureAwait(false);
            return r switch
            {
                Success<TIn> s => await onSuccess(s.Payload).ConfigureAwait(false),
                Failure<TIn> e => await onFailure(e.Error).ConfigureAwait(false),
                _ => await onFailure(new Error("invalid state of a Result"))
            };
        }
        
        public static async Task<TOut> MapAsync<TIn, TOut>(this Task<IResult<TIn>> result, Func<TIn, TOut> onSuccess,
            Func<IError, TOut> onFailure)
        {
            var r = await result.ConfigureAwait(false);
            return r switch
                {
                Success<TIn> s => onSuccess(s.Payload),
                Failure<TIn> e => onFailure(e.Error),
                _ => onFailure(new Error("invalid state of a Result"))
                };
        }
    }
}