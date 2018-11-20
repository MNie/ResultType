namespace ResultType.Operations
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ResultType.Factories;
    using ResultType.Results;

    public static class ResultOperations
    {
        [DebuggerStepThrough]
        public static Result<TOutput> Bind<TInput, TOutput>(this Result<TInput> result, Func<Result<TOutput>> toBind) =>
            result.IsSuccess
                ? toBind()
                : ResultFactory.CreateFailure<TOutput>(result.Error.Message);

        [DebuggerStepThrough]
        public static Result<TOutput> Bind<TInput, TOutput>(this Result<TInput> result, Func<TInput, Result<TOutput>> toBind) =>
            Bind(result, () => toBind(result.Payload));

        [DebuggerStepThrough]
        public static Result<TOutput> Bind<TInput, TOutput>(this Result<TInput> result, Func<Result<TOutput>> onSuccess, Func<Result<TOutput>> onFailure) =>
            result.IsSuccess
                ? onSuccess()
                : onFailure();

        [DebuggerStepThrough]
        public static async Task<Result<TOutput>> BindAsync<TInput, TOutput>(this Result<TInput> result, Func<Task<Result<TOutput>>> toBind) =>
            result.IsSuccess
                ? await toBind().ConfigureAwait(false)
                : ResultFactory.CreateFailure<TOutput>(result.Error.Message);

        [DebuggerStepThrough]
        public static async Task<Result<TOutput>> BindAsync<TInput, TOutput>(
            this Result<TInput> result,
            Func<Task<Result<TOutput>>> onSuccess,
            Func<Task<Result<TOutput>>> onFailure) =>
            result.IsSuccess
                ? await onSuccess().ConfigureAwait(false)
                : await onFailure().ConfigureAwait(false);

        [DebuggerStepThrough]
        public static async Task<Result<TOutput>> BindAsync<TInput, TOutput>(this Result<TInput> result, Func<TInput, Task<Result<TOutput>>> toBind) =>
            await BindAsync(result, async () => await toBind(result.Payload).ConfigureAwait(false)).ConfigureAwait(false);

        [DebuggerStepThrough]
        public static async Task<Result<TOutput>> BindAsync<TInput, TOutput>(this Task<Result<TInput>> result, Func<Task<Result<TOutput>>> toBind)
        {
            var r = await result.ConfigureAwait(false);
            return r.IsSuccess
                ? await toBind().ConfigureAwait(false)
                : ResultFactory.CreateFailure<TOutput>(r.Error.Message);
        }

        [DebuggerStepThrough]
        public static async Task<Result<TOutput>> BindAsync<TInput, TOutput>(this Task<Result<TInput>> result, Func<TInput, Task<Result<TOutput>>> toBind) =>
            await BindAsync(result, async () => await toBind((await result.ConfigureAwait(false)).Payload)).ConfigureAwait(false);
        
        [DebuggerStepThrough]
        public static Result<TOutput> Bind<TInput, TOutput>(this Result<TInput> result, Func<TInput, Result<TOutput>> onSuccess, Func<IError, Result<TOutput>> onFailure) =>
            result.IsSuccess
                ? onSuccess(result.Payload)
                : onFailure(result.Error);
    }
}
