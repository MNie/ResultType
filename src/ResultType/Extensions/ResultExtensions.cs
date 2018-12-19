namespace ResultType.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Factories;
    using Results;

    public static class ResultExtensions
    {
        public static Result<TType> ToSuccess<TType>(this TType obj) 
            => ResultFactory.CreateSuccess<TType>(obj);
        
        public static Result<TType> ToFailure<TType>(this Exception obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailure<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        
        public static Task<Result<TType>> ToSuccessAsync<TType>(this TType obj) 
            => ResultFactory.CreateSuccessAsync<TType>(obj);
        
        public static Task<Result<TType>> ToFailureAsync<TType>(this Exception obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailureAsync<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        
        public static async Task<Result<TType>> ToSuccessAsync<TType>(this Task<TType> obj) 
            => await ResultFactory.CreateSuccessAsync<TType>(await obj);

        public static async Task<Result<TType>> ToFailureAsync<TType>(this Task<Exception> obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => await ResultFactory.CreateFailureAsync<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));

        public static async Task<Result<TType>> ToFailureAsync<TType>(this Task<Exception> obj, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var ex = await obj;
            return await ResultFactory.CreateFailureAsync<TType>(new Error(ex.Message, memberName, sourceFilePath, sourceLineNumber));
        }
        
        public static TOut Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<IError, TOut> onFailure) => 
            result.IsSuccess
                ? onSuccess(result.Payload)
                : onFailure(result.Error);

        public static async Task<TOut> MapAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> onSuccess,
            Func<IError, Task<TOut>> onFailure)
        {
            var r = await result;
            return r.IsSuccess
                ? await onSuccess(r.Payload)
                : await onFailure(r.Error);
        }
        
        public static async Task<TOut> MapAsync<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> onSuccess,
            Func<IError, TOut> onFailure)
        {
            var r = await result;
            return r.IsSuccess
                ? onSuccess(r.Payload)
                : onFailure(r.Error);
        }
    }
}