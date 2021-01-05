namespace ResultType.Operations
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Factories;
    using Results;

    public static class BindOperations
    {
        [DebuggerStepThrough]
        public static IResult<TOutput> Bind<TInput, TOutput>(this IResult<TInput> result, Func<IResult<TOutput>> toBind) =>
            result switch
            {
                Success<TInput> _ => toBind(),
                Failure<TInput> e => ResultFactory.CreateFailure<TOutput>(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };

        [DebuggerStepThrough]
        public static IResult<TOutput> Bind<TInput, TOutput>(this IResult<TInput> result, Func<TInput, IResult<TOutput>> toBind) =>
            result switch
            {
                Success<TInput> s => toBind(s.Payload),
                Failure<TInput> e => ResultFactory.CreateFailure<TOutput>(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };

        [DebuggerStepThrough]
        public static IResult<TOutput> Bind<TInput, TOutput>(this IResult<TInput> result, Func<IResult<TOutput>> onSuccess, Func<IResult<TOutput>> onFailure) =>
            result switch
            {
                Success<TInput> _ => onSuccess(),
                Failure<TInput> _ => onFailure(),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };
        
        [DebuggerStepThrough]
        public static async Task<IResult<TOutput>> BindAsync<TInput, TOutput>(this IResult<TInput> result, Func<Task<IResult<TOutput>>> toBind) =>
            result switch
            {
                Success<TInput> _ => await toBind().ConfigureAwait(false),
                Failure<TInput> e => ResultFactory.CreateFailure<TOutput>(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };

        [DebuggerStepThrough]
        public static async Task<IResult<TOutput>> BindAsync<TInput, TOutput>(
            this IResult<TInput> result,
            Func<Task<IResult<TOutput>>> onSuccess,
            Func<Task<IResult<TOutput>>> onFailure) =>
            result switch
            {
                Success<TInput> _ => await onSuccess().ConfigureAwait(false),
                Failure<TInput> _ => await onFailure().ConfigureAwait(false),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };

        [DebuggerStepThrough]
        public static async Task<IResult<TOutput>> BindAsync<TInput, TOutput>(this IResult<TInput> result, Func<TInput, Task<IResult<TOutput>>> toBind) =>
            result switch
            {
                Success<TInput> s => await toBind(s.Payload).ConfigureAwait(false),
                Failure<TInput> e => ResultFactory.CreateFailure<TOutput>(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };

        [DebuggerStepThrough]
        public static async Task<IResult<TOutput>> BindAsync<TInput, TOutput>(this Task<IResult<TInput>> result, Func<Task<IResult<TOutput>>> toBind)
        {
            var r = await result.ConfigureAwait(false);
            return r switch
            {
                Success<TInput> _ => await toBind().ConfigureAwait(false),
                Failure<TInput> e => ResultFactory.CreateFailure<TOutput>(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };
        }

        [DebuggerStepThrough]
        public static async Task<IResult<TOutput>> BindAsync<TInput, TOutput>(this Task<IResult<TInput>> result, Func<TInput, Task<IResult<TOutput>>> toBind)
        {
            var r = await result.ConfigureAwait(false);
            return r switch
            {
                Success<TInput> s => await toBind(s.Payload).ConfigureAwait(false),
                Failure<TInput> e => ResultFactory.CreateFailure<TOutput>(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };
        }
        
        [DebuggerStepThrough]
        public static IResult<TOutput> Bind<TInput, TOutput>(this IResult<TInput> result, Func<TInput, IResult<TOutput>> onSuccess, Func<IError, IResult<TOutput>> onFailure) =>
            result switch
            {
                Success<TInput> s => onSuccess(s.Payload),
                Failure<TInput> e => onFailure(e.Error),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };
        
        [DebuggerStepThrough]
        public static Task<IResult<TOutput>> BindAsync<TInput, TOutput>(this IResult<TInput> result, Func<TInput, Task<IResult<TOutput>>> onSuccess, Func<IError, Task<IResult<TOutput>>> onFailure) =>
            result switch
            {
                Success<TInput> s => onSuccess(s.Payload),
                Failure<TInput> e => onFailure(e.Error),
                _ => ResultFactory.CreateFailureAsync<TOutput>("invalid state of a Result")
            };

        [DebuggerStepThrough]
        public static async Task<IResult<TOutput>> BindAsync<TInput, TOutput>(this Task<IResult<TInput>> result,
            Func<TInput, Task<IResult<TOutput>>> onSuccess, Func<IError, Task<IResult<TOutput>>> onFailure)
        {
            var r = await result;
            return r switch
            {
                Success<TInput> s => await onSuccess(s.Payload).ConfigureAwait(false),
                Failure<TInput> e => await onFailure(e.Error).ConfigureAwait(false),
                _ => ResultFactory.CreateFailure<TOutput>("invalid state of a Result")
            };
        }
    }
}
