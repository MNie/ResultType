namespace ResultType.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Factories;
    using Results;

    public static class ResultExtensions
    {
        public static Result<Unit> Success() 
            => ResultFactory.CreateSuccess();
        
        public static Result<TType> ToSuccess<TType>(this TType obj) 
            => ResultFactory.CreateSuccess<TType>(obj);
        
        public static Result<TType> ToSuccessWhen<TType>(this TType obj, Predicate<TType> predicate, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => 
                predicate(obj) ? ResultFactory.CreateSuccess(obj) : ResultFactory.CreateFailure<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));

        public static Result<TType> ToFailureWhen<TType>(this TType obj, Predicate<TType> predicate, string msg,
            [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => obj.ToSuccessWhen(x => !predicate(x), msg, memberName, sourceFilePath, sourceLineNumber);
        
        public static Result<TType> ToSuccessWhen<TType>(this TType obj, Predicate<TType> predicate, IError err, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => 
                predicate(obj) ? ResultFactory.CreateSuccess(obj) : ResultFactory.CreateFailure<TType>(err);

        public static Result<TType> ToFailureWhen<TType>(this TType obj, Predicate<TType> predicate, IError err,
            [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => obj.ToSuccessWhen(x => !predicate(x), err, memberName, sourceFilePath, sourceLineNumber);
        
        public static Result<TType> ToFailure<TType>(this Exception obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailure<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        
        public static Result<TType> ToFailure<TType>(this string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailure<TType>(msg, memberName, sourceFilePath, sourceLineNumber);
        
        public static Task<Result<TType>> ToSuccessAsync<TType>(this TType obj) 
            => ResultFactory.CreateSuccessAsync<TType>(obj);
        
        public static Task<Result<TType>> ToFailureAsync<TType>(this Exception obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailureAsync<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        
        public static Task<Result<TType>> ToFailureAsync<TType>(this string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
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
        
        public static Result<IEnumerable<T>> Flatten<T>(this IEnumerable<Result<T>> results)
        {
            var resultList = results.ToArray();
            var errors = resultList.Where(x => x.IsFailure).Select(failure => failure.Error).ToList();
            var payloads = resultList.Where(x => x.IsSuccess).Select(success => success.Payload);
            return errors.Any()
                ? ResultFactory.CreateFailure<IEnumerable<T>>(new AggregateError(errors))
                : ResultFactory.CreateSuccess(payloads);
        }
        
        public static async Task<Result<IEnumerable<T>>> FlattenAsync<T>(this IEnumerable<Task<Result<T>>> results)
        {
            var resultList = results.ToList();
            
            await Task.WhenAll(resultList); 

            var errors = resultList.Select(t => t.Result).Where(x => x.IsFailure).Select(failure => failure.Error).ToArray();
            var payloads = resultList.Select(t => t.Result).Where(x => x.IsSuccess).Select(success => success.Payload);
            
            return errors.Any()
                ? ResultFactory.CreateFailure<IEnumerable<T>>(new AggregateError(errors))
                : ResultFactory.CreateSuccess(payloads);
        }
    }
}