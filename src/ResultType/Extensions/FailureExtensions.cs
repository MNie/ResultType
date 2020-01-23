namespace ResultType.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Factories;
    using Results;

    public static class FailureExtensions
    {
        public static IResult<TType> ToFailureWithMsg<TType>(this Exception obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailure<TType>(new ErrorWithException(msg, obj, memberName, sourceFilePath, sourceLineNumber));
        
        public static IResult<TType> ToFailure<TType>(this Exception obj, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailure<TType>(new ErrorWithException(obj, memberName, sourceFilePath, sourceLineNumber));
        
        public static IResult<TType> ToFailure<TType>(this string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailure<TType>(msg, memberName, sourceFilePath, sourceLineNumber);
        
        public static Task<IResult<TType>> ToFailureAsync<TType>(this Exception obj, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailureAsync<TType>(new ErrorWithException(obj, memberName, sourceFilePath, sourceLineNumber));

        public static Task<IResult<TType>> ToFailureWithMsgAsync<TType>(this Exception obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailureAsync<TType>(new ErrorWithException(msg, obj, memberName, sourceFilePath, sourceLineNumber));

        public static Task<IResult<TType>> ToFailureAsync<TType>(this string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => ResultFactory.CreateFailureAsync<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        
        
        public static async Task<IResult<TType>> ToFailureAsync<TType>(this Task<Exception> obj, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => await ResultFactory.CreateFailureAsync<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));

        public static async Task<IResult<TType>> ToFailureAsync<TType>(this Task<Exception> obj, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var ex = await obj;
            return await ResultFactory.CreateFailureAsync<TType>(new Error(ex.Message, memberName, sourceFilePath, sourceLineNumber));
        }
        
        public static IResult<TType> ToFailureWhen<TType>(this TType obj, Predicate<TType> predicate, string msg,
            [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => obj.ToSuccessWhen(x => !predicate(x), msg, memberName, sourceFilePath, sourceLineNumber);
        
        
        public static IResult<TType> ToFailureWhen<TType>(this TType obj, Predicate<TType> predicate, IError err,
            [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => obj.ToSuccessWhen(x => !predicate(x), err, memberName, sourceFilePath, sourceLineNumber);
        
        public static bool IsFailure<TType>(this IResult<TType> result) =>
            result switch
            {
                Failure<TType> _ => true,
                _ => false
            };
    }
}